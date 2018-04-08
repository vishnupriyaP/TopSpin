using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TopSpin2.Models;
using FluentNHibernate.Mapping;

namespace TopSpin2.Mappings
{
    public class MatchesMapping: ClassMap<Match>
    {
        public MatchesMapping()
        {
            Id(x => x.MatchID);
            Map(x => x.SetOne1)
                .Nullable();
            Map(x => x.SetOne2);
            Map(x => x.SetTwo1);
            Map(x => x.SetTwo2);
            Map(x => x.SetThree1);
            Map(x => x.SetThree2);
            References(x => x.Player1)
                .Column("Player1ID");
            References(x => x.Player2)
                .Column("Player2ID");
            References(x => x.Event)
                .Column("EventID");
            References(x => x.Winner)
                .Column("WinnerID");

            Table("Match");
        }
    }
}