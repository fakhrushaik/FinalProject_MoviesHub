using System.ComponentModel.DataAnnotations;

namespace MoviesHub.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required, StringLength(120)]
        [Display(Name = "Title")]
        public string Title { get; set; } = string.Empty;

        [StringLength(80)]
        [Display(Name = "Director")]
        public string? Director { get; set; }

        [Range(1900, 2100)]
        [Display(Name = "Year")]
        public int Year { get; set; }

        [StringLength(40)]
        [Display(Name = "Genre")]
        public string? Genre { get; set; }

        [Range(1, 500)]
        [Display(Name = "Runtime (mins)")]
        public int RuntimeMins { get; set; }

        [Range(1, 10)]
        [Display(Name = "Rating (1–10)")]
        public int Rating { get; set; }

        [StringLength(1000)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Url, StringLength(300)]
        [Display(Name = "Poster URL")]
        public string? PosterUrl { get; set; }

        [Display(Name = "Top pick")]
        public bool IsTopPick { get; set; } 
    }
}
