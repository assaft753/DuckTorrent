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


    [Animal("Dog")]
    public class Dog
    {
        public String Name { get; set; }
        public string Color { get; set; }

        public Dog(string name, string color)
        {
            Name = name;
            Color = color;
        }
        public Dog() { }

        public string Print()
        {
            return "The Dog's Name Is: " + this.Name + " And His Color Is: " + this.Color;
        }
    }

    [Animal("Duck")]
    public class Duck
    {
        public String Name { get; set; }
        public string Color { get; set; }

        public Duck(string name, string color)
        {
            Name = name;
            Color = color;
        }
        public Duck() { }

        public string Print()
        {
            return "The Duck's Name Is: " + this.Name + " And His Color Is: " + this.Color;
        }
    }
}
