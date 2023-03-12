using Application.Mappings;
using Domain.Entities;

namespace Application.SolutionsPreferences.Queries
{
    public class SolutionPreferenceResponse : IMapFrom<SolutionPreference>
    {
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
