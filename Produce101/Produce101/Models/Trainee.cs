using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Produce101.Models
{
    public class Trainee
    {
        public string Result { get; set; }

        #region Origin
        public string weight { get; set; }
        public int count { get; set; }
        public string clips { get; set; }
        #region Result
        public string result1 { set { Result = value; } }
        public string result2 { set { Result = value; } }
        public string result3 { set { Result = value; } }
        public string result4 { set { Result = value; } }
        public string result5 { set { Result = value; } }
        public string result6 { set { Result = value; } }
        public string result7 { set { Result = value; } }
        public string result8 { set { Result = value; } }
        #endregion
        public string aWord { get; set; }
        public string height { get; set; }
        public int page { get; set; }
        public string name { get; set; }
        public string age { get; set; }
        public string profileVod { get; set; }
        public int limit { get; set; }
        public string rdt { get; set; }
        public string agencyNm { get; set; }
        public string udt { get; set; }
        public string bType { get; set; }
        public string picUrl { get; set; }
        public string hobby { get; set; }
        public string agency { get; set; }
        public string k { get; set; }
        public string isOpen { get; set; }
        public string eName { get; set; }
        public string photos { get; set; }
        public string nick { get; set; }
        public string traineePeriod { get; set; }
        public int seq { get; set; }
        public string specialty { get; set; }
        public string eliminated { get; set; }
        #endregion
    }
}
