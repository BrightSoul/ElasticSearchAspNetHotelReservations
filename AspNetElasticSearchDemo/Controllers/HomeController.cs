using AspNetElasticSearchDemo.Models.DTOs;
using AspNetElasticSearchDemo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AspNetElasticSearchDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Suggest(string text)
        {
            var indexManager = new ElasticSearchIndexManager();
            var result = indexManager.Suggest(text);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Search(DateTime arrival, int nights, decimal? maxPrice = null, string text = null, string[] terms = null, int offset = 0)
        {
            var indexManager = new ElasticSearchIndexManager();
            var result = indexManager.Search(
                arrival,
                nights,
                maxPrice,
                terms == null ? null : terms.Select(t => TermFilterDTO.FromString(t)),
                text,
                offset);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        
    }
}