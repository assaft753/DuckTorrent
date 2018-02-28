using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animals
{
    public class AnimalAttribute : Attribute
    {
        public string Kind { get; set; }

        public AnimalAttribute(string kind)
        {
            Kind = kind;
        }
    }
}
