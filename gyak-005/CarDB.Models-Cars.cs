using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDB.Models
{
    [Table("cars")]
    public class Car
    {
        //egy Id nevű intet, ami elsődleges kulcs, aminek az értéke automatikusan növekszik az insertekkel, és amelynek adatbázisban levő oszlopneve az, hogy "car_id"
        //egy legfeljebb 100 hosszúságú stringet, melynek megadása kötelező, és a neve az, hogy Model
        //egy BasePrice nevű, int típusú adattagot, melynek megadása nem kötelező
        //egy virtuális Brand típusú tagváltozót, amely az adatbázisban nem kap helyet, azonban egy egy-a-többhöz mappeléshez használható.
        //egy BrandId nevű int típusú idegen kulcsot a Brand táblára

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("car_id", TypeName = "int")]
        public int Id { get; set; }

        [StringLength(100)]
        [Required]
        public string Model { get; set; }

        public int? BasePrice { get; set; }

        public virtual Brand Brand { get; set; }

        [ForeignKey(nameof(Brand))]
        public int BrandId { get; set; }

    }
}
