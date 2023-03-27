using System;
using System.Reflection;

namespace Validation.Classes
{
    [AttributeUsage(AttributeTargets.Property)]
    // a fenti sor nelkul barmire, pl. osztalyra vagy konstruktorra is
    // alkalmazni lehetne...
    public class MaxLengthAttribute : Attribute
    {
        public int _maxLength { get; }

        public MaxLengthAttribute(int maxlen)
        {
            _maxLength = maxlen;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    // a fenti sor nelkul barmire, pl. osztalyra vagy konstruktorra is
    // alkalmazni lehetne...
    public class RangeAttribute : Attribute
    {
        public int _minValue { get; }
        public int _maxValue { get; }

        public RangeAttribute(int minval, int maxval)
        {
            _minValue = minval;
            _maxValue = maxval;
        }
    }

    interface IValidation
    {
        public bool Validate(object instance, PropertyInfo propInfo);
    }

    public class RangeValidation : IValidation
    {
        RangeAttribute range;

        public RangeValidation(RangeAttribute range)
        {
            this.range = range;
        }

        public bool Validate(object instance, PropertyInfo propInfo)
        {
            int val = (int)propInfo.GetValue(instance);
            if (val > range._minValue && val < range._maxValue)
            {
                return true;
            }
            return false;
        }
    }

    internal class MaxLengthValidation : IValidation
    {
        MaxLengthAttribute maxLength;

        public MaxLengthValidation(MaxLengthAttribute maxLength)
        {
            this.maxLength = maxLength;
        }

        public bool Validate(object instance, PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType == typeof(string))
            {
                var value = (string)propertyInfo.GetValue(instance);
                return value.Length <= maxLength._maxLength;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }

    //IF not factory:
    // - ugly, repeated code, hard to find new variable names,
    // - no real possibility of generalizing the Validator, we want to be type safe in the methods  
    // ---> let a factory take care of the instantiations & casts,
    // only the factory will have to know the full list of the concrete validator attribute types in our code.
    // Other solutions are also possible 
    // (e.g. the attribute knows how to validate, BUT that solution breaks layering, makes logic appear on the data level)
    internal class ValidationFactory
    {
        public IValidation GetValidation(Attribute attribute)
        {
            if (attribute is MaxLengthAttribute)
            {
                return new MaxLengthValidation((MaxLengthAttribute)attribute);
            }

            if (attribute is RangeAttribute)
            {
                return new RangeValidation((RangeAttribute)attribute);
            }

            return null;
        }
    }

    public class Validator
    {
        public bool Validate(object instance)
        {
            ValidationFactory validationFactory = new ValidationFactory();

            foreach (var prop in instance.GetType().GetProperties())
            {
                //Console.WriteLine($"Property: {prop.Name}");

                foreach (var attr in prop.GetCustomAttributes(false))
                {
                    // elony: Validator osztaly igy fix marad, akkor is ha tobbfele validation-t hozunk letre kesobb
                    IValidation validation = validationFactory.GetValidation((System.Attribute)attr);
                    if (validation?.Validate(instance, prop) == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}

namespace HFT_het04_validation
{

    class Person
    {
        [Validation.Classes.MaxLength(20)]
        public string Name { get; set; }

        [Validation.Classes.Range(50, 300)]
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

            Person invalidPerson1 = new Person()
            {
                Name = "012345678901234567891",
                Height = 190
            };

            Person invalidPerson2 = new Person()
            {
                Name = "Alex Fergusson",
                Height = 22
            };

            Validation.Classes.Validator validator = new Validation.Classes.Validator();
            // validator.Validate() elso korben irja ki az objektum osszes
            // tulajdonsagat, es azok osszes attributumat!
            Console.WriteLine("1st person: " + validator.Validate(validPerson));
            Console.WriteLine("2nd person: " + validator.Validate(invalidPerson1));
            Console.WriteLine("3rd person: " + validator.Validate(invalidPerson2));
            Console.ReadLine();
        }
    }
}
