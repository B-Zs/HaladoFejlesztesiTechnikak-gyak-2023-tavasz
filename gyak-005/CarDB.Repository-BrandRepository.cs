using CarDB.Data;
using CarDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDB.Repository
{
    public class BrandRepository : Repository<Brand>, IRepository<Brand>
    {
        public BrandRepository(CarDbContext ctx) : base(ctx)
        {

        }

        public override Brand Read(int id) {
            var b = ctx.Brand.FirstOrDefault(
                b => b.Id == id
            );
            if (b != null)
            {
                return b;
            }

            throw new Exception("No such brand");

        }
        public override void Update(Brand entity)
        {
            var brandToUpdate = Read(entity.Id);
            brandToUpdate.Name = entity.Name;
            ctx.SaveChanges();
        }
    }
}
