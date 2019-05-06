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
        private Theme theme;

        private object suspendLogUpdate = new object();

        public ListViewLogger(ListView listView, Theme theme)
        {
            this.theme = theme;

            listView.Scrollable = true;
            listView.View = View.Details;
            listView.HeaderStyle = ColumnHeaderStyle.None;
            listView.BorderStyle = BorderStyle.None;
            (listView.Parent as Panel).BorderStyle = BorderStyle.FixedSingle;

            // This disables the horizontal scroll bar.
            listView.Columns[0].Width = listView.Width - 4 - SystemInformation.VerticalScrollBarWidth;

            listView.ItemSelectionChanged += ItemSelectionChanged;
            this.listView = listView;
        }


        private void ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            e.Item.Selected = false;
            e.Item.Focused = false;
        }

        public void SetTheme(Theme theme)
        {
            lock (suspendLogUpdate)
            {
                this.theme = theme;
                

                (listView.Parent as Panel).BackColor = theme.BackColor;
                DrawBorder();
            }
        }

        private void DrawBorder()
        {
            listView.BackColor = theme.ControlBackColor;
            listView.ForeColor = theme.ControlForeColor;

            if (theme.Name == "Dark")
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

                    listView.ForeColor = theme.ForeColor;

                    ListViewItem item = new ListViewItem(DateTime.Now.ToString("HH:mm:ss") + " " + text);
                    item.Font = listViewFont;
                    listView.Items.Add(item);

                    DrawBorder();

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