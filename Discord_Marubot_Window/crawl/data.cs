using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using System.IO;

namespace Discord_Marubot_Window
{
    
    public class data 
    {
        public List<string> list = new List<string>();
        public List<string> user = new List<string>();
        public List<string> server = new List<string>();

        String old_title;
        String old_domain;
        
        Form1 form;
        Winform wf;
        public data()
        {
            form = Program.form;
            wf = Program.wf;

            fileload();
            wf.WinformLog("@" + "DB 로드");
            
        }

        String direction = Directory.GetCurrentDirectory()+@"\data";
        public void fileload()
        {
            list.Clear();
            user.Clear();
            server.Clear();
            wf.WinformPerclear();
            string[] dataValue;
            string[] titleValue;
            string[] userValue;
            string[] serverValue;
            string[] permValue;

            string[] listValue;


            DirectoryInfo di = new DirectoryInfo(direction);
            if (!di.Exists)
                di.Create();

            try
            {
                dataValue = File.ReadAllLines(direction + @"\data.txt");
                old_title = dataValue[0];
                old_domain = dataValue[1];
            }
            catch (FileNotFoundException){
                dataValue = new string[0];
                old_title = null;
                old_domain = null;
            }

            try { listValue = File.ReadAllLines(direction + @"\datalist.txt"); }
            catch (FileNotFoundException) { listValue = new string[0]; }

            try{titleValue = File.ReadAllLines(direction + @"\title.txt");}
            catch (FileNotFoundException){titleValue = new string[0];}

            try{userValue = File.ReadAllLines(direction + @"\user.txt");}
            catch (FileNotFoundException){userValue = new string[0];}

            try{serverValue = File.ReadAllLines(direction + @"\server.txt");}
            catch (FileNotFoundException){serverValue = new string[0];}

            try{permValue = File.ReadAllLines(direction + @"\perm.txt");}
            catch(FileNotFoundException){permValue = new string[0];}



            wf.Removelistall();
            for(int i = 0; i < titleValue.Length; i++)
                listadd(titleValue[i], userValue[i],serverValue[i]);
            for(int i = 0; i < permValue.Length; i++)
                wf.WinformPermadd(permValue[i]);
            for (int i = 0; i < listValue.Length; i++)
                oldlist.Add(listValue[i]);

            
            wf.WinformLog("@리스트 로드");
            filesave();
        }
        public void filesave()
        {
            File.Delete(direction + @"\data.txt");
            if (old_title != null)
                File.AppendAllText(direction + @"\data.txt", old_title+ "\n");
            else
                File.AppendAllText(direction + @"\data.txt", "-" + "\n");
            if (old_domain != null)
                File.AppendAllText(direction + @"\data.txt", old_domain + "\n");
            else
                File.AppendAllText(direction + @"\data.txt", "-" + "\n");

            File.Delete(direction + @"\datalist.txt");
            File.AppendAllLines(direction + @"\datalist.txt", oldlist);

            File.Delete(direction + @"\title.txt");
            File.AppendAllLines(direction + @"\title.txt", list);
            File.Delete(direction + @"\user.txt");
            File.AppendAllLines(direction + @"\user.txt", user);
            File.Delete(direction + @"\server.txt");
            File.AppendAllLines(direction + @"\server.txt", server);
            File.Delete(direction + @"\perm.txt");
            File.AppendAllLines(direction + @"\perm.txt", wf.getpermlist());

            //Console.WriteLine("@리스트 저장");
            wf.WinformLog("@" + "리스트 저장");
        }


        public void listadd(string title, string user, string server)
        {
            this.list.Add(title);
            this.user.Add(user);
            this.server.Add(server);
            //Console.WriteLine("@리스트에 "+title+"을 추가합니다");
            wf.WinformLog("@" + "리스트 추가: <" + title + ">, <" + user+ ">, <" + server + ">");
            wf.Inputlist(title + ":"+user+":"+server);
        }
        public bool listfind(string title)
        {
            if (list.Contains(title))
                return true;
            else
                return false;
        }
        public bool listdel(string title,string user ,string server)
        {
            int length = list.Count();
            for (int i = 0; i < length; i++)
                    if (this.user[i] == user|| user == "전체"||user=="강제")
                    {
                        if (list[i] == title || title == "전체")
                        {
                            wf.WinformLog("@" +
                                "리스트 제거: <" + title + ">, <" + user + ">, <" + server + ">");
                            list.RemoveAt(i);
                            this.user.RemoveAt(i);
                            this.server.RemoveAt(i);
                            wf.Removelist(i);

                            if (title != "전체")
                            {
                                return true;
                            }
                            length--;
                            i--;
                        }
                    }
            if (title == "전체")
                return true;
            else
                return false;
            //Console.WriteLine("@리스트에서 "+title+"를 제거합니다");
        }

        public List<string> getlist()
        {
            return this.list;
        }
        public List<string> getuser()
        {
            return this.user;
        }
        public List<string> getserver()
        {
            return this.server;
        }
        public void setold(String title, String domain)
        {
            this.old_title = title;
            this.old_domain = domain;
        }
        public string[] getold()
        {
            string[] old= new string[2];
            old[0] = this.old_title;
            old[1] = this.old_domain;
            return old;
        }

        List<string> oldlist = new List<string>();

        public void setoldlist(List<maru> list)
        {
            oldlist.Clear();
            for(int i = 0; i < list.Count; i++)
            {
                oldlist.Add(list[i].getdomain());
            }
        }
        public List<string> getoldlist()
        {
            return this.oldlist;
        }
        
        
        public string getdirection()
        {
            return direction;
        }

        public void backup()
        {
            string tmp;
            tmp = direction 
                + @"\" + DateTime.Now.ToString("yyyyMMdd") 
                + @"\" + DateTime.Now.ToString("HHmm");

            DirectoryInfo di = new DirectoryInfo(tmp);
            if (!di.Exists)
                di.Create();

            FileInfo datafile = new FileInfo(direction + @"\data.txt");
            datafile.CopyTo(tmp + @"\data.txt", true);

            FileInfo titlefile = new FileInfo(direction + @"\title.txt");
            titlefile.CopyTo(tmp + @"\title.txt", true);

            FileInfo userfile = new FileInfo(direction + @"\user.txt");
            userfile.CopyTo(tmp + @"\user.txt", true);

            FileInfo serverfile = new FileInfo(direction + @"\server.txt");
            serverfile.CopyTo(tmp + @"\server.txt", true);

            FileInfo permfile = new FileInfo(direction + @"\perm.txt");
            serverfile.CopyTo(tmp + @"\perm.txt", true);
            wf.WinformLog("@" + "백업");
        }

    }
}
