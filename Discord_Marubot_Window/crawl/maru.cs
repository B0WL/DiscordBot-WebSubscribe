using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Marubot_Window
{
    public class maru
    {
        string title;
        string domain;

        public maru(string t, string d)
        {
            title = t;
            domain = d;
        }

        public string gettitle()
        {
            return this.title;
        }
        public string getdomain()
        {
            return this.domain;
        }


    }
}
