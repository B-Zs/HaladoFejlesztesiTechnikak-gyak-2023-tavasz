using System;

namespace Validation.Classes
{

}

namespace HFT_het04_validation
{
    class Person
    {
        //[Validation.Classes.MaxLength(20)]
        public string Name { get; set; }

        //[Validation.Classes.Range(50, 300)]
        public int Height { get; set; }
    }

    internal class Program
    {

        static void Main(string[] args)
        {
            Person validPerson = new Person()
            {
                Name = "Alex Fergusson",
                Height = 190
            };

            //Person invalidPerson1 = new Person()
            //{
            //    Name = "012345678901234567891",
            //    Height = 190
            //};

            //Person invalidPerson2 = new Person()
            //{
            //    Name = "Alex Fergusson",
            //    Height = 22
            //};

            //Validation.Classes.Validator validator = new Validation.Classes.Validator();
            //Console.WriteLine("1st person: " + validator.Validate(validPerson));
            //Console.WriteLine("2nd person: " + validator.Validate(invalidPerson1));
            //Console.WriteLine("3rd person: " + validator.Validate(invalidPerson2));
            //Console.ReadLine();
        }
    }
}
