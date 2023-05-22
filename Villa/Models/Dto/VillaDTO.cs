using System.ComponentModel.DataAnnotations;

namespace Villa.Models.Dto
{
    public class VillaDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public int SqFt { get; set; }
        public int Occupancy { get; set; }
    }
}
