using EveAccountant.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EveAccountantWeb.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            var report = AccountingProvider.LoadReport();
            return View(report);
        }
    }
}