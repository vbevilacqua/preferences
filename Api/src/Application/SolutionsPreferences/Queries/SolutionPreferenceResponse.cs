namespace Application.SolutionsPreferences.Queries
{
    using Application.Mappings;
    using Domain.Entities;

    public class SolutionPreferenceResponse : IMapFrom<SolutionPreference>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
