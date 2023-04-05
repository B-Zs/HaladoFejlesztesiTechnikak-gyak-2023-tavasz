using CarDB.Data;
using CarDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDB.Repository
{
    public class CarRepository : Repository<Car>, IRepository<Car>
    {
        public CarRepository(CarDbContext ctx) : base(ctx)
        {

        }

        public override Car Read(int id)
        {
            var b = ctx.Car.FirstOrDefault(
                b => b.Id == id
            );
            if (b != null)
            {
                return b;
            }

            throw new Exception("No such car");

        }
        public override void Update(Car entity)
        {
            var carToUpdate = Read(entity.Id);
            carToUpdate.Model = entity.Model;
            carToUpdate.BasePrice = entity.BasePrice;
            carToUpdate.BrandId = entity.BrandId;
            ctx.SaveChanges();
        }
    }
}
