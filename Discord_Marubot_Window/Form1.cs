using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Discord_Marubot_Window
{

    public partial class Form1 : Form 
    {
        public TextBox Log;
        public ProgressBar Pgb;
        public ListBox list;
        public TextBox domainbox;
        public TextBox titlebox;
        public ListBox perm;

        data Database;

        public Form1()
        {
            InitializeComponent();
            Log = Logbox;
            Pgb = progressBar1;
            Pgb.Minimum = 0;
            Pgb.Maximum = 100;
            Pgb.Step = 4;
            Pgb.Value = 0;

            list = list_list;
            domainbox = tb_domain;
            titlebox = tb_title;
            perm = list_perm;



        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private int select;


        private void btn_perm_input_Click(object sender, EventArgs e)
        {
            perm.Items.Add(tb_perm.Text);
        }

        private void btn_perm_del_Click(object sender, EventArgs e)
        {
            perm.Items.RemoveAt(select);
        }

        private void list_perm_SelectedIndexChanged(object sender, EventArgs e)
        {
            select = list_perm.SelectedIndex;
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.ExitThread();
            Environment.Exit(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Database.filesave();
        }

        public void GetDB(data DB)
        {
            Database = DB;
        }
    }
}
