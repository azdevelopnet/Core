using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace Pagination
{
    public class Datum
    {
        public string code { get; set; }
        public string codingSystem { get; set; }
        public string type { get; set; }
        public string name { get; set; }
    }

    public class Metadata
    {
        public string db_published_date { get; set; }
        public int elements_per_page { get; set; }
        public string current_url { get; set; }
        public string next_page_url { get; set; }
        public int total_elements { get; set; }
        public int total_pages { get; set; }
        public int current_page { get; set; }
        public string previous_page { get; set; }
        public string previous_page_url { get; set; }
        public int next_page { get; set; }
    }

    public class RootObject
    {
        public List<Datum> data { get; set; }
        public Metadata metadata { get; set; }
    }
}
