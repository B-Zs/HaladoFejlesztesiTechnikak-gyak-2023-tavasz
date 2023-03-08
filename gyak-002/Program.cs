using System;
using System.Xml.Linq;

namespace Westeros
{
    static class Operations
    {
        public static void ToConsole<T>(this IEnumerable<T> input, string header)
        {
            Console.WriteLine($"************* {header} ************");
            foreach (var item in input) Console.WriteLine(item);
            Console.WriteLine($"************* {header} ************");
            Console.ReadLine();
        }
    }


    internal class Program
    {

        static void Main(string[] args)
        {
            XDocument doc = XDocument.Load("02_war_of_westeros.xml");

            var names = doc.Descendants("name").Select(x => x.Value);
            names.ToConsole("Elokeszuletek");

            // Q1 = How many houses participated?
            var distinctHouses = doc.
                Descendants("house").
                Select(node => node.Value).
                Distinct();
            Console.WriteLine($"TOTAL: {distinctHouses.Count()}");
            distinctHouses.ToConsole("Q1");

            // Q2 = List the battles with the "ambush" type
            var ambushBattle = doc.Descendants("battle")
                .Where(b => b.Element("type")?.Value == "ambush")
                .Descendants("name").Select(x => x.Value);

            ambushBattle.ToConsole("Q2-megoldasMethodSyntax");

            
            var ambushBattle2 = from battleNode in doc.Descendants("battle")
                     where battleNode.Element("type")?.Value == "ambush"
                     select battleNode.Element("name")?.Value;
            ambushBattle2.ToConsole("Q2-megoldasQuerySyntax");
        }

    }
}