using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TopSpin2.Models;
using System.Diagnostics;


namespace TopSpin2.Controllers
{
    public class MatchController : Controller
    {
        private const string DbFile = "TopSpin";
        ISessionFactory SessionFactory = HomeController.GetFactory(); //CreateSessionFactory();

        // GET: Match
        public ActionResult Index(string countryName, string searchString)
        {
            var session = SessionFactory.OpenSession();

            using (session)
            {

               /* var matchList = new List<int>(); //to store all the retrieved matches
                //retrieve matches from the database
                var countryQry = session.Query<Match>().OrderBy(Match => Match.MatchID)
                    .Select(Match => new
                    {
                        Match.MatchID
                    }
                );
                //insert each result of the query into the matchList
                foreach (var val in countryQry)
                {
                    matchList.Add(val.MatchID);
                }


                ViewBag.countryName = new SelectList(matchList);*/
                
                
                //if user wants to fiter by country
                IQueryable<Match> matches = session.Query<Match>()
                    .Fetch(m => m.Event)
                    .Fetch(m => m.Player1)
                    .Fetch(m => m.Player2)
                    .Fetch(m => m.Winner);
                if(!String.IsNullOrEmpty(searchString))
                {
                    matches = matches.Where(s => s.Event.Country.Contains(searchString));
                   

                }

                if(!string.IsNullOrEmpty(countryName))
                {
                    matches = matches.Where(s => s.Event.Country == countryName);
                }
               
            
                return View(matches.ToList());
            }
        }

        // GET: Match/Details/5
        public ActionResult Details(int? id)
        {
            using (var session = SessionFactory.OpenSession())
            {
                if(id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                }

                //gives players profiles
                var match = session.Get<Match>(id);

                var players = session.Query<Player>();
                players = players.Where(s => s.PlayerName == match.Player1.PlayerName);
                //players.Add(match.Player1);
                //players.Add(match.Player2);
            }
                return View(); 
        }

        // GET: Match/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Match/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind (Include ="MatchID, Player1, Player2, Event, SetOne1, SetOne2, SetTwo1, SetTwo2, SetThree1, SetThree2, Winner")] Match match)
        {
            using (var session = SessionFactory.OpenSession())
            {
                var matchToAdd = new Match();

                if (ModelState.IsValid)
                {
                
                    matchToAdd.MatchID = match.MatchID;
                    

                    //creating a new match should also create a new event 
                    //set event object of the new match
                    Event eventToAdd = session.Query<Event>().Where(e => e.Tournament == match.Event.Tournament).FirstOrDefault();
                    matchToAdd.Event = eventToAdd;

                    //set player1 info
                    Player p1 = session.Query<Player>().Where(p => p.PlayerName == match.Player1.PlayerName).FirstOrDefault();
                    matchToAdd.Player1 = p1;

                    //set player2 info
                    Player player2 = session.Query<Player>().Where(s => s.PlayerName == match.Player2.PlayerName).FirstOrDefault();
                    matchToAdd.Player2 = player2;

                    match.Player1 = matchToAdd.Player1;
                    match.Player2 = matchToAdd.Player2;
                    
                    //Event eventInfo = session.Query<Event>().Where(s => s.Tournament == match.Event.Tournament).FirstOrDefault();
                    //set score info
                    matchToAdd.SetOne1 = match.SetOne1;
                    matchToAdd.SetOne2 = match.SetOne2;
                    matchToAdd.SetTwo1 = match.SetTwo1;
                    matchToAdd.SetTwo2 = match.SetTwo2;
                    matchToAdd.SetThree1 = match.SetThree1;
                    matchToAdd.SetThree2 = match.SetThree2;

                    int p1S = 0;
                    int p2S = 0;
                    Player winner;

                    if(matchToAdd.SetOne1 > matchToAdd.SetOne2)
                    {
                        p1S++;
                    }
                    else //if(matchToAdd.SetOne2 > matchToAdd.SetOne1)
                    {
                        p2S++;
                    }

                    if (matchToAdd.SetTwo1 > matchToAdd.SetTwo2)
                    {
                        p1S++;
                    }
                    else //if (matchToAdd.SetTwo2 > matchToAdd.SetTwo1)
                    {
                        p2S++;
                    }

                    if(matchToAdd.SetThree1 > matchToAdd.SetThree2 && p1S <= 1 && p2S <= 1)
                    {
                        p1S++;
                    }
                    else // if (matchToAdd.SetThree2 > matchToAdd.SetThree1 && p1S <= 1 && p2S <= 1)
                    {
                        p2S++;
                    }

                    //player2 is the winner
                    if(p1S < p2S)
                    {
                        //winner = session.Query<Player>().Where(s => s.PlayerID == match.Player2.PlayerID).FirstOrDefault();
                        winner = matchToAdd.Player2;

                    }
                    //player1 is the winner
                    else
                    {
                        //winner = session.Query<Player>().Where(s => s.PlayerID == match.Player1.PlayerID).FirstOrDefault();
                        winner = matchToAdd.Player1;

                    }


                   // Player winner = session.Query<Player>().Where(s => s.PlayerName == match.Winner.PlayerName).FirstOrDefault();
                    matchToAdd.Winner = winner;

                    session.Save(matchToAdd);

                    return RedirectToAction("Index");
                }

                return View(match);
            }

        }

        public ActionResult Edit(int? id)
        {
            using (var session = SessionFactory.OpenSession())
            {

                var match = session.Get<Match>(id);
                    

                return View(match);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MatchID, Player1, Player2, SetOne1, SetOne2, SetTwo1, SetTwo2, SetThree1, SetThree2, Winner")] Match match)
        {
            int i = 0; 

            try
            {
                using (var session = SessionFactory.OpenSession())

                {
                    var matchToEdit = session.Get<Match>(match.MatchID);

                    match.MatchID = matchToEdit.MatchID;

                    match.Player1 = matchToEdit.Player1;
                    match.Player2 = matchToEdit.Player2;


                    matchToEdit.SetOne1 = match.SetOne1;
                    matchToEdit.SetOne2 = match.SetOne2;

                    matchToEdit.SetTwo1 = match.SetTwo1;
                    matchToEdit.SetTwo2 = match.SetTwo2;

                    matchToEdit.SetThree1 = match.SetThree1;
                    matchToEdit.SetThree2 = match.SetThree2;

                    int p1Score = 0;
                    int p2Score = 0;
                    Player winner;

                    if (matchToEdit.SetOne1 > matchToEdit.SetOne2)
                    {
                        p1Score++;
                    }
                    else if (matchToEdit.SetOne2 > matchToEdit.SetOne1)
                    {
                        p2Score++;
                    }

                    if (matchToEdit.SetTwo1 > matchToEdit.SetTwo2)
                    {
                        p1Score++;
                    }
                    else if (matchToEdit.SetTwo2 > matchToEdit.SetTwo1)
                    {
                        p2Score++;
                    }

                    if (matchToEdit.SetThree1 > matchToEdit.SetThree2 && p1Score <= 1 && p2Score <= 1)
                    {
                        p1Score++;
                    }
                    else if (matchToEdit.SetThree2 > matchToEdit.SetThree1 && p1Score <= 1 && p2Score <= 1)
                    {
                        p2Score++;
                    }

                    //player2 is the winner
                    if (p1Score < p2Score)
                    {
                        // winner = session.Query<Player>().Where(s => s.PlayerID == match.Player2.PlayerID).FirstOrDefault();
                        matchToEdit.Winner = matchToEdit.Player2;
                        i = i + 1;

                    }
                    //player1 is the winner
                    else
                    {
                        //winner = session.Query<Player>().Where(s => s.PlayerID == match.Player1.PlayerID).FirstOrDefault();
                        matchToEdit.Winner = matchToEdit.Player1;

                    }
                    //Winner, update points for winner 
                    //matchToEdit.Winner = winner;
                    match.Winner = matchToEdit.Winner;
                    //matchToEdit.Winner.ATPPoints += winner.CalculatePoints(winner.PlayerID);

                    session.SaveOrUpdate(matchToEdit);


                    using (var transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(matchToEdit);
                        transaction.Commit();
                    }
                }
                return RedirectToAction("Index");
            }

            catch
            {
                //Console.WriteLine("Error - match edit");
                return View(match);
            }

        }

        // GET: Match/Delete/5
        public ActionResult Delete(int? id)
        {
            using (var session = SessionFactory.OpenSession())
            {
                var match = session.Get<Match>(id);
                return View(match);
            }
        }

        // POST: Match/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (var session = SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var match = session.Get<Match>(id);
                    session.BeginTransaction();
                    session.Delete(match);
                    transaction.Commit();
                }

                return RedirectToAction("Index");

            }
        }

     
    }
}
