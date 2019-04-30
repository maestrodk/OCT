using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace OverloadClientTool
{
    public class ListViewLogger
    {
        public enum Level : int
        {
            Info = 0,
            Verbose = 1,
            Warning = 2,
            Error = 3
        };

        private Font listViewFont = new Font("Tahoma", 8.25f, FontStyle.Regular);

        private ListView listView;
        private bool isDarkTheme = false;

        private Color darkBackColor;
        private Color lightBackColor;

        private object suspendLogUpdate = new object();

        public ListViewLogger(ListView listView, Color darkBackColor, Color lightBackColor, bool dark)
        {
            listView.Scrollable = true;
            listView.View = View.Details;
            listView.HeaderStyle = ColumnHeaderStyle.None;
            listView.BorderStyle = BorderStyle.None;
            (listView.Parent as Panel).BorderStyle = BorderStyle.FixedSingle;

            // This disables the horizontal scroll bar.
            listView.Columns[0].Width = listView.Width - 4 - SystemInformation.VerticalScrollBarWidth;

            listView.ItemSelectionChanged += ItemSelectionChanged;
            this.listView = listView;

            SetThemeBackgroundColors(dark, darkBackColor, lightBackColor);
        }


        private void ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            e.Item.Selected = false;
            e.Item.Focused = false;
        }

        public void SetThemeBackgroundColors(bool dark, Color darkBackColor, Color lightBackColor)
        {
            lock (suspendLogUpdate)
            {
                this.isDarkTheme = dark;
                this.darkBackColor = darkBackColor;
                this.lightBackColor = lightBackColor;

                listView.BackColor = (dark) ? darkBackColor : lightBackColor;

                (listView.Parent as Panel).BackColor = (dark) ? Color.White : Color.FromArgb(128, 0, 0);

                DrawBorder(dark);
            }
        }

        private void DrawBorder(bool dark)
        {
            listView.BackColor = (dark) ? darkBackColor : lightBackColor;

            if (dark)
            {
                listView.Dock = DockStyle.None;
                listView.Location = new Point(1, 1);
                listView.Size = new Size(listView.Parent.ClientRectangle.Width - 2, listView.Parent.ClientRectangle.Height - 2);
            }
            else
            {
                listView.Dock = DockStyle.Fill;
            }
        }

        private void LogText(Level level, string text)
        {
            if ((listView == null) || (text == null)) return;

            if (listView.InvokeRequired)
            {
                listView.BeginInvoke(new Action(delegate { LogText(level, text); }));
                return;
            }
            else
            {
                lock (suspendLogUpdate)
                {
                    listView.Columns[0].Width = listView.Width - 4 - SystemInformation.VerticalScrollBarWidth;

                    listView.ForeColor = (isDarkTheme) ? Color.LightGray : Color.FromArgb(32, 32, 32);

                    ListViewItem item = new ListViewItem(DateTime.Now.ToString("HH:mm:ss") + " " + text);
                    item.Font = listViewFont;
                    listView.Items.Add(item);

                    DrawBorder(isDarkTheme);

                    // Autoscroll to bottom.
                    if (listView.Items.Count > 3) listView.TopItem = listView.Items[listView.Items.Count - 3];
                }
            }
        }

        public void InfoLogMessage(string text)
        {
            LogText(Level.Info, text);
        }

        public void VerboseLogMessage(string text)
        {
            LogText(Level.Verbose, text);
        }

        public void WarningLogMessage(string text)
        {
            LogText(Level.Warning, text);
        }

        public void ErrorLogMessage(string text)
        {
            LogText(Level.Error, text);
        }
   }
}
