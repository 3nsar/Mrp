using System;
using System.Collections.Generic;
using System.Text;

namespace Mrp.Models
{
    public class MediaModel
    {
        public Guid ID { get; set; } = Guid.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string MediaType { get; set; } = string.Empty;
        public int ReleaseYear { get; set; }
        public string Genre { get; set; } = string.Empty;
        public int AgeRestriction { get; set; }

        public double AverageScore { get; set; }

        public int FavoriteCount { get; set; }
    }
}
