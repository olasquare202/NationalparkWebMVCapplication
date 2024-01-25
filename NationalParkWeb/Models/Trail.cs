using System.ComponentModel.DataAnnotations;

namespace NationalParkWeb.Models
{
    public class Trail
    {
        public int Id { get; set; } 
        [Required]
        public string Name { get; set; }
        [Required]
        public double Distance { get; set; }
        [Required]
        public double Elevation { get; set; }

        public enum DifficultyType { Easy, Moderate, Difficult, Expert }
        public DifficultyType Difficulty { get; set; }
        [Required]
        public int NationalParkId { get; set; }//bcos d Trail is associted with NationalPark

        public NationalPark NationalPark { get; set; }
    }
}
