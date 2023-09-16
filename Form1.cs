using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkypeLogger
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public void cmd(string strCmdText)
        {
            //System.Diagnostics.Process.Start("CMD.exe"," /c " +  strCmdText/*);*/
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                PowerShellInstance.AddScript(strCmdText);
                Collection<PSObject> PSOutput = PowerShellInstance.Invoke();
                foreach (PSObject outputItem in PSOutput)
                {
                }
            }
            /*
            ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", strCmdText)
            {
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process proc = new Process())
            {
                proc.StartInfo = procStartInfo;
                proc.Start();

                string output = proc.StandardOutput.ReadToEnd();

                if (string.IsNullOrEmpty(output))
                    output = proc.StandardError.ReadToEnd();

                this.Hide();
            }
            */

        }
        public string Lastlog;
        public void Changelog()
        {


            try
            {
                Lastlog = File.ReadAllLines(Environment.CurrentDirectory + "\\SkypeLog.txt").Last();
                string[] h = Lastlog.Split(',');


                string[] us = h[8].Split('$');
                string[] ug = us[1].Split(';');
                string[] ub = h[2].Split(' ');
                string message = h[7];
                string Timestamp = ub[1].Replace(".", ":");
                string User = ug[0];
                
                string TSmsgFormat = "<" + Timestamp + ">" + " " + h[5] + ":" + " " + message;
               
          
                if (!File.Exists(Environment.CurrentDirectory + "\\LogTSFormat.txt") ||File.ReadAllLines(Environment.CurrentDirectory + "\\LogTSFormat.txt").Last().Replace(Environment.NewLine, "") != TSmsgFormat)
                {


                    File.AppendAllText( Environment.CurrentDirectory + "\\LogTSFormat.txt", Environment.NewLine + TSmsgFormat);

                }
            }
            catch(Exception e)
            {
               
            }
        }
        public void Makelog()
        {

            cmd(Environment.CurrentDirectory + "\\SkypeLogView.exe /UseTimeRange 1 /FromTime \"" + DateTime.Now.ToShortDateString().Replace(".", "-").Replace(",", "-").Replace(";", "-").Replace(":", "-") + "\" /scomma " + Environment.CurrentDirectory + "\\SkypeLog.txt");
            Thread.Sleep(10);
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Makelog();
            Changelog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }
    }

}
