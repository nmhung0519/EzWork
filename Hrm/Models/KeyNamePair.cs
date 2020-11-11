using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hrm.Models
{
    public class KeyNamePair
    {
        public int ID { get; set; }

        public string Value { get; set; }

        public KeyNamePair() { }

        public KeyNamePair(int id, string value)
        {
            ID = id;
            Value = value;
        }
    }
}