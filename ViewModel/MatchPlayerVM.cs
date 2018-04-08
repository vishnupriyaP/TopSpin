using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TopSpin2.Models;

namespace TopSpin2
{
    public class MatchPlayerVM
    {
        public virtual IList<Match> Matches { get; set; }
        public virtual IList<string> countries { get; set; }
        public virtual Match matchFound { get; set; }
        public string countrySelected;
        
    }
}