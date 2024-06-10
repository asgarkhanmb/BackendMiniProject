using System.ComponentModel.DataAnnotations.Schema;

namespace BackendMiniProject.Models
{
    public class InstructorSocial
    {
        public int Id { get; set; }
        public Instructor Instructor { get; set; }

        [ForeignKey(nameof(Instructor))]
        public int InstructorId { get; set; }

        public Social Social { get; set; }
        [ForeignKey(nameof(Social))]
        public int SocialId { get; set; }
        public string SocialLink { get; set; }
    }
}
