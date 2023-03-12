using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class GlobalPreference
    {
        [Key]
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}