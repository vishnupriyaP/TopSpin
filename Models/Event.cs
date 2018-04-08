using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TopSpin2.Models
{
    public class Event
    {
        public virtual int EventID { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string Tournament{ get; set; }
        public virtual string City { get; set; }
        public virtual string Country { get; set; }
        public virtual int MatchType { get; set; }
        public virtual IList<Match> MatchesScheduled { get; set; }

        public Event()
        {
            MatchesScheduled = new List<Match>();
        }

        public virtual void ScheduleMatch(Match match)
        {
            MatchesScheduled.Add(match);
        }


    }
}