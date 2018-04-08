using FluentNHibernate.Cfg;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TopSpin2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private static ISessionFactory sessionFactory = CreateSessionFactory();

        public static ISessionFactory GetFactory()
        {
            return sessionFactory; 

        }

        /*ISessionFactory sessionFactory = CreateSessionFactory();

        public static ISessionFactory sessionFactory
        {
            get { }
        }*/
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