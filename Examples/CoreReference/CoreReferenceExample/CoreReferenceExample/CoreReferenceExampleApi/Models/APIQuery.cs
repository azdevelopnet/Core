using System;
namespace CoreReferenceExampleApi.Models
{
    public class APIQuery
    {
        public int? PageIndex;
        public int? PageSize;
        public string Filter { get; set; }
        public string Search { get; set; }
        public string Sort { get; set; }
        public bool IncludeAll { get; set; }
    }
}
