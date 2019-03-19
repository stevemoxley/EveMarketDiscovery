using EveAccountant.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EveAccountantWeb.Controllers
{
    public class SSOController : Controller
    {
        // GET: SSO
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AuthUrl()
        {
            return Redirect(AuthenticationManager.GetAuthenticationUrl());
        }

        public ActionResult Load()
        {
            ViewBag.NewCount = AuthenticationManager.LoadTransactionsAndJournals();
            return View();
        }

        public async Task<ActionResult> PostAuth(string code)
        {
            var result = await AuthenticationManager.PostAuthCheck(code);
            ViewBag.CharacterName = AuthenticationManager.CharacterInfo.CharacterName;
            return View();
        }
    }
}