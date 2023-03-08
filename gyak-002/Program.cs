using System;
using System.Xml.Linq;

namespace Westeros
{
    internal class Program
    {
        static void Main(string[] args)
        {
            XDocument doc = XDocument.Load("02_war_of_westeros.xml");

            var names = doc.Descendants("name").Select(x => x.Value).ToList();
            foreach (var name in names)
            {
                Console.WriteLine(name);
            }
        }

    }
}