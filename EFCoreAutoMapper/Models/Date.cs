using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreAutoMapper.Models
{
    public class Date 
    {
        public Date() { }

        public Date(DateTime date)
        {
            Value = date;
        }

        public Date(DateTime? date)
        {
            Value = date.Value;
        }

        public DateTime Value { get; set; }
        public string Formatted
        {
            get
            {
                return $"{Value.ToString("MM/dd/yyyy")}  ({(DateTime.Now - Value).Days})";
            }
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
