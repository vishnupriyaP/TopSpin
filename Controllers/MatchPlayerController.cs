using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TopSpin2.Models;

namespace TopSpin2.Controllers
{
    public class MatchPlayerController : Controller
    {
        ISessionFactory SessionFactory = HomeController.GetFactory(); //CreateSessionFactory();

        // GET: MatchPlayer/Create
        public ActionResult Details(int? id)
        {
            
            MatchPlayerVM matchPlayerVM = new MatchPlayerVM();
            using (var session = SessionFactory.OpenSession())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                matchPlayerVM.matchFound = session.Query<Match>().Where(x => x.MatchID == id)
                    .Fetch(m => m.Event)
                    .Fetch(m => m.Player1)
                    .Fetch(m => m.Player2)
                    .Fetch(m => m.Winner)
                    .First();
      
            }

            return View(matchPlayerVM);
        }

       
        
    }
}
