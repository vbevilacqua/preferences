namespace Application.Solutions.Queries
{
    using System;
    using System.Collections.Generic;
    using Application.Mappings;
    using Application.SolutionsPreferences.Queries;
    using Domain.Entities;

    public class SolutionResponse : IMapFrom<Solution>
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        public IEnumerable<SolutionPreferenceResponse> SolutionPreferences { get; set; } = new List<SolutionPreferenceResponse>();
    }
}
