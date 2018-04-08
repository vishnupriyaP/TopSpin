using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TopSpin2.Models;

namespace TopSpin2.Controllers
{
    public class CreatePlayerController : Controller
    {
        ISessionFactory sessionFactory = HomeController.GetFactory();

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreatePlayerVM playerInfo)
        {
            using (var session = sessionFactory.OpenSession())
            {
                string dir = @"C:\Users\Vishnupriya\source\repos\TopSpin2.0\TopSpin2.0\images\";
                var playerToAdd = new Player();

                if (ModelState.IsValid)
                {
                    playerToAdd.PlayerID = playerInfo.Player.PlayerID;
                    playerToAdd.PlayerName = playerInfo.Player.PlayerName;
                    playerToAdd.Country = playerInfo.Player.Country;
                    playerToAdd.Age = playerInfo.Player.Age;
                    playerToAdd.Height = playerInfo.Player.Height;

                    if (playerInfo.PhotoFile != null && playerInfo.PhotoFile.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(playerInfo.PhotoFile.FileName);
                        playerInfo.PhotoFile.SaveAs(Path.Combine(dir, fileName));
                    }
                    else
                    {
                        var fileName = "test.png";
                        playerInfo.PhotoFile.SaveAs(Path.Combine(dir, fileName));
                    }
                    playerToAdd.FilePath = Path.Combine(dir, playerInfo.PhotoFile.FileName);


                   

                    session.Save(playerToAdd);
                    return RedirectToAction("Index", "Player");
                }

                return View(playerInfo);
            }

        }

      /*  public ActionResult Upload(HttpPostedFileBase PhotoFile)
        {
            string dir = @"C:\Users\Vishnupriya\source\repos\TopSpin2.0\TopSpin2.0\images\";
            if(PhotoFile != null && PhotoFile.ContentLength > 0)
            {
                var fileName = Path.GetFileName(PhotoFile.FileName);
                PhotoFile.SaveAs(Path.Combine(dir, fileName));
            }

            return RedirectToAction("Index");
        }*/
    }
}