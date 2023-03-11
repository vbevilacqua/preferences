namespace Domain.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Solution
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public virtual ICollection<SolutionPreference> SolutionPreferences { get; set; } = new List<SolutionPreference>();
    }
}