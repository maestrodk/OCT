using System;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace OverloadClientTool
{
    public enum Level : int
    {
        Critical = 0,
        Error = 1,
        Warning = 2,
        Info = 3,
        Verbose = 4,
        Debug = 5
    };

    public sealed class ListBoxLog : IDisposable
    {
        // For message formats see FormatALogEventMessage().
        private const string DefaualtMessageFormat = "{8}"; // "{2} {4} {8}"; // "{0} [{5}] : {8}";

        private const int DefaultLinesInTextBox = 1000;

        private bool disposed;
        private ListBox listBox;
        private string messageFormat;
        private readonly int maxEntriesInListBox;
        private bool canAddLogEntry;

        private void OnHandleCreated(object sender, EventArgs e)
        {
            canAddLogEntry = true;
        }

        private void OnHandleDestroyed(object sender, EventArgs e)
        {
            canAddLogEntry = false;
        }

        private bool isDarkTheme = false;

        private Color darkBackColor;
        private Color lightBackColor;

        public void SetDarkTheme(bool dark, Color darkBackColor, Color lightBackColor)
        {
            this.isDarkTheme = dark;
            this.darkBackColor = darkBackColor;
            this.lightBackColor = lightBackColor;
        }

        private void DrawItemHandler(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                ListBox listBox = ((ListBox)sender);

                if (listBox.Focused == false) listBox.ClearSelected();

                if (isDarkTheme) listBox.BackColor = darkBackColor;
                else listBox.BackColor = lightBackColor;

                e.DrawBackground();

                //e.DrawFocusRectangle();
                ControlPaint.DrawBorder(Graphics.FromHwnd(listBox.Handle), listBox.ClientRectangle, (isDarkTheme) ? Color.White : Color.Black, ButtonBorderStyle.Solid);

                LogEvent logEvent = listBox.Items[e.Index] as LogEvent;

                // SafeGuard against wrong configuration of list box
                if (logEvent == null) logEvent = new LogEvent(Level.Critical, listBox.Items[e.Index].ToString());

                Color color = Color.Black;

                switch (logEvent.Level)
                {
                    case Level.Critical:
                        color = Color.White;
                        break;

                    case Level.Error:
                        color = (isDarkTheme) ? Color.FromArgb(224, 96, 96) : Color.Red;
                        break;

                    case Level.Warning:
                        color = (isDarkTheme) ? Color.Goldenrod : Color.DarkOrange;
                        break;

                    case Level.Info:
                        color = (isDarkTheme) ? Color.LightGray : Color.FromArgb(32, 32, 32);
                        break;

                    case Level.Verbose:
                        color = (isDarkTheme) ? Color.LightGreen : Color.DarkGreen;
                        break;

                    default:
                        // Black.
                        break;
                }

                if (logEvent.Level == Level.Critical) e.Graphics.FillRectangle(new SolidBrush(Color.Red), e.Bounds);

                e.Graphics.DrawString(FormatALogEventMessage(logEvent, messageFormat), new Font("Lucida Console", 8.25f, FontStyle.Regular), new SolidBrush(color), e.Bounds);
            }
        }

        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            if ((e.Modifiers == Keys.Control) && (e.KeyCode == Keys.C)) CopyToClipboard();
        }

        private void CopyMenuOnClickHandler(object sender, EventArgs e)
        {
            CopyToClipboard();
        }

        private void CopyMenuPopupHandler(object sender, EventArgs e)
        {
            ContextMenu menu = sender as ContextMenu;
            if (menu != null) menu.MenuItems[0].Enabled = (listBox.SelectedItems.Count > 0);
        }

        private class LogEvent
        {
            public LogEvent(Level level, string message)
            {
                EventTime = DateTime.Now;
                Level = level;
                Message = message;
            }

            public readonly DateTime EventTime;
            public readonly Level Level;
            public readonly string Message;
        }

        private void WriteEvent(LogEvent logEvent)
        {
            if ((logEvent != null) && (canAddLogEntry)) listBox.BeginInvoke(new AddALogEntryDelegate(AddALogEntry), logEvent);
        }

        private delegate void AddALogEntryDelegate(object item);

        private void AddALogEntry(object item)
        {
            listBox.Items.Add(item);
            if (listBox.Items.Count > maxEntriesInListBox) listBox.Items.RemoveAt(0);
            if (!Paused) listBox.TopIndex = listBox.Items.Count - 1;
        }

        private string LevelName(Level level)
        {
            switch (level)
            {
                case Level.Critical:
                    return "Critical";

                case Level.Error:
                    return "Error";

                case Level.Warning:
                    return "Warning";

                case Level.Info:
                    return "Info";

                case Level.Verbose:
                    return "Verbose";

                case Level.Debug:
                    return "Debug";

                default:
                    return string.Format("<value={0}>", (int)level);
            }
        }

        private string FormatALogEventMessage(LogEvent logEvent, string messageFormat)
        {
            string message = logEvent.Message;
            if (message == null) { message = "<NULL>"; }

            return string.Format(messageFormat,
                /* {0} */ logEvent.EventTime.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                /* {1} */ logEvent.EventTime.ToString("yyyy-MM-dd HH:mm:ss"),
                /* {2} */ logEvent.EventTime.ToString("yyyy-MM-dd"),
                /* {3} */ logEvent.EventTime.ToString("HH:mm:ss.fff"),
                /* {4} */ logEvent.EventTime.ToString("HH:mm:ss"),

                /* {5} */ LevelName(logEvent.Level)[0],
                /* {6} */ LevelName(logEvent.Level),
                /* {7} */ (int)logEvent.Level,

                /* {8} */ message);
        }

        private void CopyToClipboard()
        {
            if (listBox.SelectedItems.Count > 0)
            {
                StringBuilder selectedItemsAsRTFText = new StringBuilder();
                selectedItemsAsRTFText.AppendLine(@"{\rtf1\ansi\deff0{\fonttbl{\f0\fcharset0 Courier;}}");
                selectedItemsAsRTFText.AppendLine(@"{\colortbl;\red255\green255\blue255;\red255\green0\blue0;\red218\green165\blue32;\red0\green128\blue0;\red0\green0\blue255;\red0\green0\blue0}");

                foreach (LogEvent logEvent in listBox.SelectedItems)
                {
                    selectedItemsAsRTFText.AppendFormat(@"{{\f0\fs16\chshdng0\chcbpat{0}\cb{0}\cf{1} ", (logEvent.Level == Level.Critical) ? 2 : 1, (logEvent.Level == Level.Critical) ? 1 : ((int)logEvent.Level > 5) ? 6 : ((int)logEvent.Level) + 1);
                    selectedItemsAsRTFText.Append(FormatALogEventMessage(logEvent, messageFormat));
                    selectedItemsAsRTFText.AppendLine(@"\par}");
                }

                selectedItemsAsRTFText.AppendLine(@"}");
                System.Diagnostics.Debug.WriteLine(selectedItemsAsRTFText.ToString());
                Clipboard.SetData(DataFormats.Rtf, selectedItemsAsRTFText.ToString());
            }

        }

        public ListBoxLog(ListBox listBox) : this(listBox, DefaualtMessageFormat, DefaultLinesInTextBox) { }

        public ListBoxLog(ListBox listBox, string messageFormat) : this(listBox, messageFormat, DefaultLinesInTextBox) { }

        public ListBoxLog(ListBox listBox, string messageFormat, int maxLinesInListbox)
        {
            this.disposed = false;

            this.listBox = listBox;
            this.messageFormat = messageFormat;
            this.maxEntriesInListBox = maxLinesInListbox;

            Paused = false;

            canAddLogEntry = listBox.IsHandleCreated;

            //this.listBox.SelectionMode = SelectionMode.MultiExtended;
            this.listBox.SelectionMode = SelectionMode.None;
            this.listBox.Enabled = false;

            this.listBox.HandleCreated += OnHandleCreated;
            this.listBox.HandleDestroyed += OnHandleDestroyed;

            this.listBox.DrawItem += DrawItemHandler;

            
            // this.listBox.KeyDown += KeyDownHandler;

            // MenuItem[] menuItems = new MenuItem[] { new MenuItem("Copy", new EventHandler(CopyMenuOnClickHandler)) };

            // this.listBox.ContextMenu = new ContextMenu(menuItems);
            // this.listBox.ContextMenu.Popup += new EventHandler(CopyMenuPopupHandler);

            this.listBox.DrawMode = DrawMode.OwnerDrawFixed;
        }

        public void Log(string message) { Log(Level.Debug, message); }

        public void Log(string format, params object[] args) { Log(Level.Debug, (format == null) ? null : string.Format(format, args)); }

        public void Log(Level level, string format, params object[] args) { Log(level, (format == null) ? null : string.Format(format, args)); }

        public void Log(Level level, string message)
        {
            WriteEvent(new LogEvent(level, message));
        }

        public bool Paused { get; set; }

        ~ListBoxLog()
        {
            if (!disposed)
            {
                Dispose(false);
                disposed = true;
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
                disposed = true;
            }
        }

        private void Dispose(bool disposing)
        {
            if (listBox != null)
            {
                canAddLogEntry = false;

                listBox.HandleCreated -= OnHandleCreated;
                listBox.HandleCreated -= OnHandleDestroyed;

                listBox.DrawItem -= DrawItemHandler;
                //listBox.KeyDown -= KeyDownHandler;

                //listBox.ContextMenu.MenuItems.Clear();
                //listBox.ContextMenu.Popup -= CopyMenuPopupHandler;
                listBox.ContextMenu = null;

                listBox.Items.Clear();
                listBox.DrawMode = DrawMode.Normal;
                listBox = null;
            }
        }
    }
}
