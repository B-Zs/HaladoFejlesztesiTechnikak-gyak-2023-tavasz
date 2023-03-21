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

            //Q3.Hány olyan csata volt, ahol a védekező sereg győzött, és volt híres fogoly ?
            var battlesWithDefWinAndMajorCaptures = from battleNode in doc.Descendants("battle")
                                                    where battleNode.Element("outcome")?.Value == "defender" &&
                                                           //(int)battleNode.Element("majorcapture") > 0
                                                           int.TryParse(battleNode.Element("majorcapture")?.Value, out var defWin) &&
                                                           defWin > 0
                                                    // battleNode.Element("majorcapture")?.Value != "0"
                                                    select battleNode.Element("name")?.Value;
            Console.WriteLine($"{battlesWithDefWinAndMajorCaptures.Count()} esetben nyert vedekezo csapat es volt majorcature");
            battlesWithDefWinAndMajorCaptures.ToConsole("3. feladat");

            var battlesWithDefWinAndMajorCaptures2 = doc.Descendants("battle")
                .Where(battle => battle.Element("outcome")?.Value == "defender" && battle.Element("majorcapture")?.Value != "0")
                .Descendants("name").Select(n => n.Value);
            battlesWithDefWinAndMajorCaptures2.ToConsole("3. feladat method syntax-szal");

            // Q4 = How many battles were won by the Stark house ?
            var q4 = from battleNode in doc.Descendants("battle")
                     let whoWon = battleNode.Element("outcome")?.Value
                     let winnerHouses = battleNode.Element(whoWon).Elements("house").Select(x => x.Value)
                     where winnerHouses.Contains("Stark")
                     select new
                     {
                         BattleName = battleNode.Element("name").Value,
                         Outcome = whoWon,
                         Houses = String.Join("; ", winnerHouses)
                     };
            Console.WriteLine(q4.Count());
            q4.ToConsole("Q4");

            // Q5 = Which battles had more than 2 participating houses? attacker+defender houses
            var q5 = from battleNode in doc.Descendants("battle")
                     let attackerHouses = battleNode.Element("attacker").Elements("house").Count()
                     let defenderHouses = battleNode.Element("defender").Elements("house").Count()
                     let sumHouses1 = attackerHouses + defenderHouses
                     let sumHouses2 = battleNode.Descendants("house").Count()
                     let sumHouses3 = battleNode.Descendants("house").Select(x => x.Value).Distinct().Count()
                     where sumHouses3 > 2
                     orderby sumHouses3 descending
                     select new { BattleName = battleNode.Element("name").Value, NumHouses = sumHouses3, Region = battleNode.Element("region").Value };
            q5.ToConsole("Q5");

            // Q6 = Which are the 3 most violent regions? groupby
            var q6 = from battleNode in doc.Descendants("battle")
                     group battleNode by battleNode.Element("region").Value into grp
                     let cnt = grp.Count()
                     orderby cnt descending
                     select new { Region = grp.Key, Count = cnt };
            q6.Take(3).ToConsole("Q6");
            var top3Counts = q6.Select(x => x.Count).Distinct().Take(3);
            q6.Where(grp => top3Counts.Contains(grp.Count)).ToConsole("Q6 - alter");

            // Q7 = Which one is the most violent region?
            Console.WriteLine("Q7 = " + q6.FirstOrDefault());
            Console.ReadLine();

            // Q8 = In the 3 most violent region, which battles had more than 2 participating houses? (Q5 join Q6)
            var q8 = from battle in q5
                     join region in q6.Take(3) on battle.Region equals region.Region
                     select new { battle, region };
            q8.ToConsole("Q8");

            
            // Q9 = List the houses ordered descending by the number of battles won! ... from+from+groupby ... SelectMany()
            var q9 = from battleNode in doc.Descendants("battle")
                     let whoWon = battleNode.Element("outcome").Value
                     let winnerHouses = battleNode.Element(whoWon).Elements("house").Select(x => x.Value)
                     from house in winnerHouses
                     group house by house into grp
                     let winCount = grp.Count()
                     orderby winCount descending
                     select new { House = grp.Key, winCount };
            q9.ToConsole("Q9");

            // Q10 = Which battle had the biggest known army? where
            var q10 = from battleNode in doc.Descendants("battle")
                      let maxSize = doc.Descendants("size").Max(x => (int)x)
                      let currentSizes = battleNode.Descendants("size").Select(x => (int)x)
                      // let attackerSize = int.Parse(battleNode.Element("attacker").Element("size")?.Value ?? "0")
                      where currentSizes.Contains(maxSize)
                      select new
                      {
                          BattleName = battleNode.Element("name").Value,
                          Sizes = String.Join("; ", currentSizes),
                          MaxSize = maxSize
                      };
            q10.ToConsole("Q10");

            // Q11 = List the three commanders who attacked the most often! groupby
            var q11 = from attacker in doc.Descendants("attacker")
                      from commander in attacker.Descendants("commander")
                      group commander by commander.Value into grp
                      let attackCount = grp.Count()
                      where attackCount != 1
                      orderby attackCount descending, grp.Key
                      select new { AttackerCommanderName = grp.Key, Count = attackCount };
            q11.ToConsole("Q11");
        }

    }
}
