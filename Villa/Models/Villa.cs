using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Villa.Models
{
    public class Villa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        public string Details { get; set; }

        public int Rate { get; set; }
        public string Amenity { get; set; }
        public int Occupancy { get; set; }
        public int SqFt { get; set; }

        public string ImageURL { get; set; }

        public DateTime CreatedData { get; set; }
        public DateTime UpdatedData { get; set; }
    }
}
