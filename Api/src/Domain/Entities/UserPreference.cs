using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class UserPreference
    {
        public string Name { get; set; } = string.Empty;

        public virtual Int32 UserId { get; set; }

        public virtual Int32 SolutionId { get; set; }

        public string Value { get; set; } = string.Empty;

        public bool IsActive { get; set; }

    }
}