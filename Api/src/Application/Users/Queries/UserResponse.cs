using Application.Mappings;
using Domain.Entities;

namespace Application.Users.Queries
{
    public class UserResponse : IMapFrom<User>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public virtual ICollection<UserPreference> UserPreferences { get; set; }
    }
}