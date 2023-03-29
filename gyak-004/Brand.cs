

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cars_Database_Application
{
    public class Brand
    {

        //Egy Brand táblát reprezentáló osztályt, amely tartalmaz:

        //egy Id nevű intet, ami elsődleges kulcs és aminek az értéke automatikusan növekszik az insertekkel
        //egy legfeljebb 100 hosszúságú stringet, melynek megadása kötelező, és a neve az, hogy Name
        //egy olyan Cars nevű ICollection típusú tagváltozót, amely nem kerül bele az adatbázisba, de az egy-a-többhöz virtuális mappeléshez használható
        //egy default konstruktort, amely HashSet konkrét típussal példányosítja az üres Cars tagváltozót

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        [NotMapped]
        public virtual ICollection<Car> Cars { get; set; }

        public Brand()
        {
            Cars = new HashSet<Car>();
        }


    }

}
