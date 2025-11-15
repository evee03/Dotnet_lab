using System.ComponentModel.DataAnnotations;

namespace Lab6.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tytu³ jest wymagany.")]
        [MaxLength(50, ErrorMessage = "Tytu³ mo¿e mieæ maksymalnie 50 znaków.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Opis jest wymagany.")]
        [UIHint("LongText")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Ocena jest wymagana.")]
        [Range(1, 5, ErrorMessage = "Ocena filmu musi byæ liczb¹ pomiêdzy 1 a 5.")]
        [UIHint("Stars")]
        public int Rating { get; set; }

        public string? TrailerLink { get; set; }
        public Genre Genre { get; set; }
    }
}
