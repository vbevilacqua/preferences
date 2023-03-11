namespace Application.Users.Queries
{
    using System;
    using System.Collections.Generic;
    using Mappings;
    using Domain.Entities;
    
    public class UserResponse : IMapFrom<User>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public virtual ICollection<UserPreference> UserPreferences { get; set; }
    }
}