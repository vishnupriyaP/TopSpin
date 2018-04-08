using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TopSpin2.Models;
using FluentNHibernate.Mapping;

namespace TopSpin2.Mappings
{
    public class PlayersMap : ClassMap<Player>
    {
        public PlayersMap()
        {
            Id(x => x.PlayerID);
            Map(x => x.PlayerName);
            Map(x => x.Country);
            Map(x => x.Height);
            Map(x => x.Age);
            //Map(x => x.ATPPoints);
            Map(x => x.FilePath);
            //Map(x => x.FileName);
            HasMany(x => x.Matches);
            Table("Players");
        }

    }
}