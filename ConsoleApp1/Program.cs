using Animals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly a = Assembly.LoadFrom(@"C:\Users\assaftayouri\source\repos\DuckTorrent\Dog.dll");
            Type[] types = a.GetTypes();
            if (types.Length > 0)
            {
                AnimalAttribute animal = (AnimalAttribute)Attribute.GetCustomAttribute(types[0], typeof(AnimalAttribute));
                if (animal == null)
                {
                    MessageBox.Show("Unknown DLL");
                }
                else
                {
                    if (animal.Kind.Equals("Dog"))
                    {
                        object[] parameters = { "DogiDogo", "Black" };
                        object obj = Activator.CreateInstance(types[0], parameters);
                        MethodInfo m = types[0].GetMethod("Print");
                        Console.WriteLine(m.Invoke(obj, null));
                    }

                    else if (animal.Kind.Equals("Duck"))
                    {
                        object[] parameters = { "DonaldDuck", "White" };
                        object obj = Activator.CreateInstance(types[0], parameters);
                        MethodInfo m = types[0].GetMethod("Print");
                        Console.WriteLine(m.Invoke(obj, null));
                    }
                }
            }
        }
    }
}
