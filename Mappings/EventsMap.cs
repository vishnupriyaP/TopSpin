using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TopSpin2.Models;
using FluentNHibernate.Mapping;

namespace TopSpin2.Mappings
{
    public class EventsMap : ClassMap<Event>
    {
        public EventsMap()
        {
            Id(x => x.EventID);
            Map(x => x.Date);
            Map(x => x.Tournament);
            Map(x => x.City);
            Map(x => x.Country);
            Map(x => x.MatchType);
            HasMany(x => x.MatchesScheduled);
            Table("Event");
        }
    }
}