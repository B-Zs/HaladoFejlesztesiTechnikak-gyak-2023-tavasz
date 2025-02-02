﻿using CarDB.Data;
using CarDB.Models;
using CarDB.Repository;

namespace Cars_Database_Application
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

    class Program
    {
        static void Main(string[] args)
        {
            CarDbContext ctx = new CarDbContext();
            ctx.Brand.Select(x => x.Name).ToConsole("BRANDS");

            BrandRepository brep = new BrandRepository(ctx);
            brep.Update(new Brand() { Id = 1, Name = "XYZ" });

            ctx.Brand.Select(x => x.Name).ToConsole("BRANDS");
        }
    }
}