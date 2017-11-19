using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SbrinnaCoreFramework
{
    public class UniqueSessionData
    {
        public Guid Token { get; set; }
        public int UserId { get; set; }
        public string IP { get; set; }
        public DateTime LastConnection { get; set; }
    }
}
