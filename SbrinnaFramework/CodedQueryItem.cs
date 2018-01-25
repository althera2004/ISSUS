using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SbrinnaCoreFramework
{
    public class CodedQueryItem
    {
        public string Key { get; set; }
        public object Value { get; set; }

        public CodedQueryItem(string key, object value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}
