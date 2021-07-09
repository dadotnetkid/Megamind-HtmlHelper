using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HtmlExtensions.IEnumerableHelpers;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public static List<Test> list;

        public HomeController()
        {
            list = new List<Test>();
            /*for (var i = 0; i <= 100; i++)
            {*/
                list.Add(new Test
                {
                    FirstName = "aaaa",
                    MiddleName="gggg",
                    LastName = "bbbb",
                    BirthDate = new DateTime(2021,5,28).ToString()
                });
                list.Add(new Test
                {
                    FirstName = "cccc",
                    MiddleName = "hhhh",
                    LastName = "ddddd",
                    BirthDate = new DateTime(2021, 5, 28).ToString()
                });
                list.Add(new Test
                {
                    FirstName = "eeee",
                    MiddleName = "iiiii",
                    LastName = "ffff",
                    BirthDate = new DateTime(2021, 5, 28).ToString()
                });
                list.Add(new Test
                {
                    FirstName = "kkkk",
                    MiddleName = "jjjjj",
                    LastName = "llllll",
                    BirthDate = new DateTime(2021, 5, 28).ToString()
                });
            /*}*/

        }
        public ActionResult Index()
        {
            return View(list);
        }

        public PartialViewResult ListDataTablePartial()
        {
            return PartialView(list.ToDataTable());
        }
        public JsonResult GetTestData()
        {
          return Json(list.ToDataTable(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSelectData()
        {
            return Json(list.ToSelect(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(Test test)
        {
            return Json(new {OK = true}, JsonRequestBehavior.AllowGet);
        }
    }

    public class Test
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string  BirthDate { get; set; }
    }
}