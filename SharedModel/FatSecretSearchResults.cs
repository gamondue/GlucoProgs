using System.Collections.Generic;

namespace GlucoMan
{
    /// <summary>
    /// One page of a FatSecret food search
    /// </summary>
    public class FatSecretSearchResults
    {
        public List<FatSecretFood> Foods { get; set; } = new List<FatSecretFood>();
        public int TotalResults { get; set; }
        public int PageNumber { get; set; }     // zero-based
        public int PageSize { get; set; }

        public bool HasMorePages => Foods.Count > 0
            && (PageNumber + 1) * PageSize < TotalResults;
    }
}
