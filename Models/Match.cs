using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TopSpin2.Models
{
    public class Match
    {
        public virtual int MatchID { get; set; }
        public virtual Player Player1 { get; set; } //Add Reference in MatchesMapping
        public virtual Player Player2 { get; set; }
        public virtual Event Event { get; set; }
        public virtual int? SetOne1 { get; set; }
        public virtual int? SetOne2 { get; set; }
        public virtual int? SetTwo1 { get; set; }
        public virtual int? SetTwo2 { get; set; }
        public virtual int? SetThree1 { get; set; }
        public virtual int? SetThree2 { get; set; }
        public virtual Player Winner { get; set; }
    }
}