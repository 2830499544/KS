using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{

    public class LoginJsonEF
    {
        public int status { get; set; }
        public string message { get; set; }
        public LoginJsonEF_Data data { get; set; }
    }

    public class LoginJsonEF_Data
    {
        public string UserName { get; set; }
        public string Url { get; set; }
        public bool IsCheckStudentCourse { get; set; }
    }

}
