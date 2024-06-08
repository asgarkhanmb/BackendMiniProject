using System.ComponentModel.DataAnnotations;

namespace BackendMiniProject.Models
{
    public class About : BaseEntity
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        [Required]
        [StringLength(800)]
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
