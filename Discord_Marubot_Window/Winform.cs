using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Marubot_Window
{
    public class Winform
    {
        Form1 form;
        public Winform()
        {
            this.form = Program.form;
        }
        public void WinformPermadd(string text)
        {
            if (form.InvokeRequired)
                form.BeginInvoke(new Action(() => form.perm.Items.Add(text)));
            else
                form.perm.Items.Add(text);
        }
        public void WinformPermremove(string text)
        {
            if (form.InvokeRequired)
                form.BeginInvoke(new Action(() => form.perm.Items.RemoveAt(form.perm.FindString(text))));
            else
            {
                form.perm.Items.RemoveAt(form.perm.FindString(text));
            }
        }
        public int WinformPermfind(string text)
        {
            int output = 0;
            if (form.InvokeRequired)
                form.BeginInvoke(new Action(() => output = form.perm.FindString(text)));
            else
            {
                form.perm.FindString(text);
            }
            return output;
        }
        public void WinformPerclear()
        {
            if (form.InvokeRequired)
                form.BeginInvoke(new Action(() => form.perm.Items.Clear()));
            else
            {
                form.perm.Items.Clear();
            }
        }
        public void WinformLog(string text)
        {
            if (form.InvokeRequired)
            {
                form.BeginInvoke(new Action(() => form.Log.AppendText(text + "\n")));
            }else
            {
                form.Log.AppendText(text + "\n");
            }
        }

        public void Inputlist(string text)
        {
            if (form.InvokeRequired)
            {
                form.BeginInvoke(new Action(() => form.list.Items.Add(text)));
            }
            else
            {
                form.list.Items.Add(text);
            }
        }

        public void Removelist(int At)
        {
            if (form.InvokeRequired)
            {
                form.BeginInvoke(new Action(() => form.list.Items.RemoveAt(At)));
            }else
            {
                form.list.Items.RemoveAt(At);
            }
        }
        public void Removelistall()
        {
            if (form.InvokeRequired)
            {
                form.BeginInvoke(new Action(() => form.list.Items.Clear()));
            }else
            {
                form.list.Items.Clear();
            }
        }

        public void WinformOld(string title, string domain)
        {
            if (form.InvokeRequired)
            {
                form.BeginInvoke(new Action(() => form.titlebox.ResetText()));
                form.BeginInvoke(new Action(() => form.domainbox.ResetText()));
                try
                {
                    title = title.Replace("	", "");

                    form.BeginInvoke(new Action(() => form.titlebox.AppendText(title)));
                    form.BeginInvoke(new Action(() => form.domainbox.AppendText(domain)));
                }
                catch
                {

                
                    }
            }
            else
            {
                form.titlebox.ResetText();
                form.domainbox.ResetText();
                form.titlebox.AppendText(title);
                form.domainbox.AppendText(domain);
            }
        }
        public string WinformGetOld()
        {
            string txt = "";


            //txt += form.titlebox.Text+ "\r\n" +form.domainbox.Text + "\r\n"+ "이거봄";
            txt += form.domainbox.Text;


            return txt;

        }


        public void WinformProg(data DataBase)
        {
            if (form.InvokeRequired)
            {
                if (form.Pgb.Value < 100)
                {
                    form.BeginInvoke(new Action(() => form.Pgb.PerformStep()));
                }
                else
                {
                    form.BeginInvoke(new Action(() => form.Pgb.Value = 0));
                    form.BeginInvoke(new Action(() => form.Log.Clear()));
                    DataBase.filesave();
                }
            }else
            {
                if (form.Pgb.Value < 100)
                {
                   form.Pgb.PerformStep();
                }
                else
                {
                   form.Pgb.Value = 0;
                    form.Log.Clear();
                    DataBase.filesave();

                    //DataBase.backup();
                }

            }
        }
        public List<string> getpermlist()
        {

            List<string> permlist = new List<string> { };
            for (int i = 0; i < form.perm.Items.Count; i++)
                permlist.Add(form.perm.Items[i].ToString());
            return permlist;
        }
        public bool checkPerm(string id)
        {
            bool ok = false;

            List<string> perm = getpermlist();
            int length = perm.Count();

            for (int i = 0; i < length; i++)
            {
                if (perm[i] == id)
                    ok = true;
            }

            return ok;
        }



    }
}
