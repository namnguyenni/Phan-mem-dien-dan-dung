using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace form_menu
{
    public class LienceKey
    {
        public string ID_PC;
        public string date;

        public LienceKey() { }
        public LienceKey(string a, string b)
        {
            this.ID_PC = a;
            this.date = b;
        }
        public string GetIDPC() { return ID_PC; }
        public string GetDate() { return date; }
        public void SetIDPC(string IDPC)
        {
            this.ID_PC = IDPC;
        }
        public void SetDate()
        {
            date = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
            
        }
    }
}
