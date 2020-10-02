using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Anti_Process_Freeze
{
    public partial class Form1 : Form
    {
        public static Process[] process = null;
        public static int Errors = 0;
        public static Thread thread = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            if(bunifuFlatButton2.Text.CompareTo("Stop") == 0)
            {
                Reset();
                new Notification($"Thread Finalized!").ShowDialog();
                return;
            }
            string processname = null;
            if (OnlyNumbers(bunifuMaterialTextbox1.Text))
            {
                if (!ProcessExists(int.Parse(bunifuMaterialTextbox1.Text)))
                {
                    new Notification($"Process: {bunifuMaterialTextbox1.Text}\nis invalid").ShowDialog();
                    return;
                }
                processname = Process.GetProcessById(int.Parse(bunifuMaterialTextbox1.Text)).ProcessName;
            }
            else
                processname = bunifuMaterialTextbox1.Text;
            if (!OnlyNumbers(bunifuMaterialTextbox2.Text) || !OnlyNumbers(bunifuMaterialTextbox3.Text))
            {
                new Notification($"Use only numbers\non Delay/Confirmation").ShowDialog();
                return;
            }
                process = Process.GetProcessesByName(processname);
            if(process.Length == 0)
            {
                new Notification($"Process: {bunifuMaterialTextbox1.Text}\nnot founded").ShowDialog();
                return;
            }
            bunifuCustomLabel6.Text = "Starting...";
            ButtonStatus(false);
            bunifuFlatButton2.Enabled = false;
            bunifuFlatButton2.Text = "Stop";
            Errors = 0;
            if (thread != null && thread.IsAlive)
                thread.Abort();
            thread = new Thread(CheckFreeze);
            thread.Start();
            bunifuCustomLabel6.Text = "Checking Process...";
            bunifuCustomLabel6.ForeColor = System.Drawing.Color.Green;
        }

        private static bool OnlyNumbers(string text)
        {
            foreach(char chr in text)
                if (chr < 0x30 || chr > 0x39)
                    return false;
            return true;
        }

        private bool ProcessExists(int id)
        {
            return Process.GetProcesses().Any(x => x.Id == id);
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            if (thread != null && thread.IsAlive)
                thread.Abort();
            Application.Exit();
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void CheckFreeze()
        {
            if(bunifuFlatButton2.InvokeRequired)
                bunifuFlatButton2.Invoke(new Action(() => { bunifuFlatButton2.Enabled = true; }));
            else
                bunifuFlatButton2.Enabled = true;
            for (; ; )
            {
                if (bunifuFlatButton2.Text.CompareTo("Start") == 0)
                    break;
                Thread.Sleep(int.Parse(bunifuMaterialTextbox2.Text));
                try
                {
                    if (process.Length == 0)
                        break;
                    if (!process[0].Responding)
                        Errors++;
                    if (Errors == int.Parse(bunifuMaterialTextbox3.Text))
                    {
                        new Notification($"Processo {process[0].ProcessName}\nFinalizado!").ShowDialog();
                        process[0].Kill();
                        Reset();
                        break;
                    }
                }
                catch {}
            }
            ButtonStatus(true);
            Reset();
        }

        private void Reset()
        {
            if (bunifuFlatButton2.InvokeRequired)
            {
                bunifuFlatButton2.Invoke(new Action(() => { bunifuFlatButton2.Text = "Start"; }));
                bunifuCustomLabel6.Invoke(new Action(() => { bunifuCustomLabel6.Text = "Waiting to Start..."; }));
                bunifuCustomLabel6.Invoke(new Action(() => { bunifuCustomLabel6.ForeColor = System.Drawing.Color.White; }));
            }
            else
            {
                bunifuFlatButton2.Text = "Start";
                bunifuCustomLabel6.Text = "Waiting to Start...";
                bunifuCustomLabel6.ForeColor = System.Drawing.Color.White;
            }
        }

        private void ButtonStatus(bool status)
        {
            if (bunifuMaterialTextbox1.InvokeRequired)
            {
                bunifuMaterialTextbox1.Invoke(new Action(() => { bunifuMaterialTextbox1.Enabled = status; }));
                bunifuMaterialTextbox2.Invoke(new Action(() => { bunifuMaterialTextbox2.Enabled = status; }));
                bunifuMaterialTextbox3.Invoke(new Action(() => { bunifuMaterialTextbox3.Enabled = status; }));
            }
            else
            {
                bunifuMaterialTextbox1.Enabled = status;
                bunifuMaterialTextbox2.Enabled = status;
                bunifuMaterialTextbox3.Enabled = status;
            }
        }

    }
}
