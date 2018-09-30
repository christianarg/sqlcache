using SqlCaching.Caching;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace WebApplicationSqlCache.Controllers
{
    public class HomeController : Controller
    {
        static SqlCache sqlCache = new SqlCache(ConfigurationManager.ConnectionStrings["SqlCache"].ConnectionString);
        static int i = 0;
        public ActionResult Index(int id = -1)
        {
            ViewBag.Count = sqlCache.GetCount();
            Foo bar = null;
            if (id != -1)
            {
                bar = sqlCache[$"key{id}"] as Foo;
            }
            return View(bar);
        }

        public ActionResult SetCacheAbsolute()
        {
            var currentId = i++;
            sqlCache.Set($"key{currentId}", new Foo { Bar = $"quito{currentId}" }, DateTimeOffset.Now.AddMinutes(5));

            return RedirectToHome();
        }

        public ActionResult SetCacheSliding()
        {
            var currentId = i++;

            sqlCache.Set($"key{currentId}", new Foo { Bar = $"quito{currentId}" }, new System.Runtime.Caching.CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(5) });

            return RedirectToHome();
        }

        public ActionResult FlushCache()
        {
            sqlCache.Flush();
            return RedirectToHome();
        }

        private ActionResult RedirectToHome()
        {
            return RedirectToAction(nameof(Index));
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
    }

    public class Foo
    {
        public string Bar { get; set; }
    }
}