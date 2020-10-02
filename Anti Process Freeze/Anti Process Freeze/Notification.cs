using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Anti_Process_Freeze
{
    public partial class Notification : Form
    {
        public static string NewText = null;
        public Notification(string text)
        {
            InitializeComponent();
            NewText = text;
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Notification_Load(object sender, EventArgs e)
        {
            bunifuCustomLabel2.Text = NewText;
        }
    }
}
