using Application.Mappings;
using Domain.Entities;

namespace Application.GlobalPreferences.Queries
{
    public class GlobalPreferenceResponse : IMapFrom<GlobalPreference>
    {
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}