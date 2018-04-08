using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TopSpin2.Models;
using System.Net;
using System.IO;
using System.Web;
using Microsoft.Web.Infrastructure;
using System.Web.Razor;
using System.Web.WebPages.Deployment;
using System.Web.WebPages.Razor;

namespace TopSpin2.Controllers
{
    public class PlayerController : Controller
    {

        private const string DbFile = "TopSpin";
        //omprove by creating a base controller
        ISessionFactory SessionFactory = HomeController.GetFactory(); //CreateSessionFactory();

        // GET: Player
        public ActionResult Index(string playerName, string searchString, string countryName, string searchString2)
        {
            var session = SessionFactory.OpenSession();
            
            using (session)
            {
                /*var nameList = new List<int>();
                var nameQry = session.Query<Player>().OrderBy(Player => Player.ATPPoints)
                    .Select(Player => new
                    {
                        Player.ATPPoints,
                    }
                );

                foreach(var player in nameQry)
                {
                    nameList.Add(player.ATPPoints);
                }
                ViewBag.playerName = new SelectList(nameList);*/

                var players = session.Query<Player>();//.OrderByDescending(s => s.ATPPoints);

                           
                if(!String.IsNullOrEmpty(searchString))
                {
                    //players = players.Where(s => s.PlayerName.Contains(searchString)).OrderBy(s => s.ATPPoints);
                    players = players.Where(x => x.PlayerName.Contains(searchString));//.OrderByDescending(s => s.ATPPoints);
                    
                }
                
                if(!string.IsNullOrEmpty(playerName))
                {
                    players = players.Where(x => x.PlayerName == playerName);//.OrderByDescending(s => s.ATPPoints);
                }

                if (!String.IsNullOrEmpty(searchString2))
                {
                    //players = players.Where(s => s.PlayerName.Contains(searchString)).OrderBy(s => s.ATPPoints);
                    players = players.Where(x => x.Country.Contains(searchString2));//.OrderByDescending(s => s.ATPPoints);

                }

                if (!string.IsNullOrEmpty(countryName))
                {
                    players = players.Where(x => x.Country == countryName);//.OrderByDescending(s => s.ATPPoints);
                }

                //players = players.OrderByDescending(s => s.ATPPoints); // avoid making two copies
                return View(players.ToList().OrderByDescending(s => s.ATPPoints));
            }
        }

        public ActionResult Details(int? id)
        {
            using (var session = SessionFactory.OpenSession())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                //var player = session.Get<Player>(id);

                //get matches where the match.player1ID == id
                //add: eager loading - players
                var matches = session.Query<Match>();
                matches = matches.Where(s => s.Player1.PlayerID  == id || s.Player2.PlayerID == id)//.Where(s => s.Player2.PlayerID == id)
                    .Fetch(p => p.Player1)
                    .Fetch(p => p.Player2)
                    .Fetch(p => p.Winner)
                    .Fetch(p => p.Event);

                /*if (player == null)
                {
                    return HttpNotFound();
                }*/
                return View(matches.ToList());
            }


        }

       /* public ActionResult Create()
        {
            return View();
        }*/

        /*[HttpPost]
        [ValidateAntiForgeryToken]
        //change parameter to type ViewModel (HttpPostedFileBase)
        public ActionResult Create([Bind(Include = "PlayerID,PlayerName,Country,Age,Height,ATPPoints, FilePath")] Player player)
        {
            using (var session = SessionFactory.OpenSession())
            {
                var playerToAdd = new Player();

                if(ModelState.IsValid)
                {
                    playerToAdd.PlayerID = player.PlayerID;
                    playerToAdd.PlayerName = player.PlayerName;
                    playerToAdd.Country = player.Country;
                    playerToAdd.Age = player.Age;
                    playerToAdd.Height = player.Height;
                    //playerToAdd.ATPPoints = player.ATPPoints;
                   // playerToAdd.ATPPoints = player.CalculatePoints(player.PlayerID);

                    //use viewModel
                    playerToAdd.FilePath = player.FilePath;
                    session.Save(playerToAdd);
                    return RedirectToAction("Index");
                }

                return View(player);
            }
        }*/

        public ActionResult EditP(int? id)
        {
            using (var session = SessionFactory.OpenSession())
            {
                var player = session.Get<Player>(id);
                return View(player);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditP([Bind(Include = "PlayerID, PlayerName, Country, Height, Age, FilePath, FileName")] Player player)
        {
            try
            {
                using (var session = SessionFactory.OpenSession())
                {
                    var playerToChange = session.Get<Player>(player.PlayerID);

                    player.PlayerID = playerToChange.PlayerID;
                    playerToChange.PlayerName = player.PlayerName;
                    playerToChange.Country = player.Country;
                    playerToChange.Height = player.Height;
                    playerToChange.Age = player.Age;
                   

                    using (var transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(playerToChange);
                        transaction.Commit();
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View(player);
            }
        }

        public ActionResult Delete(int? id)
        {
           
            using (var session = SessionFactory.OpenSession())
            {
                var player = session.Get<Player>(id);
                return View(player);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
           
            using (var session = SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var player = session.Get<Player>(id);
                    session.BeginTransaction();
                    session.Delete(player);
                    transaction.Commit();
                }

                return RedirectToAction("Index"); 

            }
        }
        
        //open img - returns ActionResult: return img contents
        public ActionResult GetPhoto(int id)
        {
            string fileName = "";
            var session = SessionFactory.OpenSession();
            var filePath = session.Get<Player>(id).FilePath;
            Console.WriteLine(filePath);
            if(Path.GetFileName(filePath) == null)
            {
                fileName = "test.png";
            }
            else
            {
                fileName = Path.GetFileName(filePath);
            }

            return File(filePath, "images/png");

        }
       




    }
}