using FluentNHibernate.Cfg;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TopSpin2.Controllers;

namespace TopSpin2.Models
{
    public class Player 
    {
        public virtual int PlayerID { get; set; }
        public virtual string PlayerName { get; set; }
        public virtual string Country { get; set; }
        public virtual string Height { get; set; }
        public virtual int Age { get; set; }
        public virtual int ATPPoints { get
            {
                return CalculatePoints(PlayerID);
            }
        }
        public virtual IList<Match> Matches { get; set; }
        public virtual string FilePath { get; set; }
        //public virtual string FileName { get; set; }
        public virtual string FileName { get; set; }

        ISessionFactory SessionFactory = HomeController.GetFactory(); //CreateSessionFactory();


        public Player()
        {
            Matches = new List<Match>();
        }
        public virtual void AddMatch(Match match)
        {
            Matches.Add(match);
        }

        
        //CalculatePoints method here
        public virtual int CalculatePoints(int id)
        {

            var session = SessionFactory.OpenSession();


            /*** METHOD #1: SQL QUERY ***/

            /*var matches = new List<Match>();
            int points = 0;
            String query = "SELECT * FROM Match WHERE WinnerID = " + id;
            var q =  session.CreateSQLQuery(query);
            matches = (List <Match>) q.list(); 
            //perform sum using sql as well
            foreach(var m in matches)
            {
                points += m.Event.MatchType;
            }*/
            

            /*** METHOD #2: C# ***/

            //list of match objects
            var matchesWon = session.Query<Match>();

            //points gained
            var player = session.Get<Player>(id);

            //int pointsToAdd = player.ATPPoints;
            int pointsToAdd = 0;

            //identify all matches where match.winner.Id == id and add these to the matches list
            matchesWon = matchesWon.Where(x => x.Winner.PlayerID == id);

            //for each match, retieve the points it is worth
            foreach (var m in matchesWon)
            {
                pointsToAdd += m.Event.MatchType;
            }

            return pointsToAdd;
        }


        //only have this in one spot
        private static ISessionFactory CreateSessionFactory()
        {

            return Fluently.Configure()

                .Database(FluentNHibernate.Cfg.Db.MsSqlConfiguration.MsSql2012
                .ConnectionString(c => c.FromConnectionStringWithKey("connectionStringKey")).ShowSql())

                .Mappings(m =>
                    m.FluentMappings.AddFromAssemblyOf<PlayerController>())
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
        }

        private static void BuildSchema(NHibernate.Cfg.Configuration config)
        {
            // delete the existing db on each run
            /*if (File.Exists(DbFile))
                File.Delete(DbFile);*/

            // this NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
            //new SchemaExport(config)
            //  .Create(false, true);
        }
    }

    
}