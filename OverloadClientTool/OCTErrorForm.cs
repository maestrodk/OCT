using System;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Principal;
using System.Diagnostics;
using System.IO.Compression;

using OverloadClientTool;

namespace OverloadClientApplication
{
    public partial class OCTErrorForm : Form
    {
        public OCTErrorForm(string message)
        {
            InitializeComponent();
            ExceptionMessageText.Text = message;
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(ExceptionMessageText.Text);
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string parameters = "?subject=" + Uri.EscapeDataString("OCT crash report") + "&body=" + Uri.EscapeDataString(ExceptionMessageText.Text);
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "mailto:" + linkLabel1.Text + parameters;
            proc.Start();
        }

        private void CopyEmailButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(linkLabel1.Text);
        }
    }
}
