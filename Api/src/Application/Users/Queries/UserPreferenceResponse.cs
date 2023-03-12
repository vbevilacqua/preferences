using Application.Mappings;
using Domain.Entities;

namespace Application.Users.Queries
{
    public class UserPreferenceResponse : IMapFrom<User>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public virtual User User { get; set; }
        public virtual Solution Solution { get; set; }
    }
}