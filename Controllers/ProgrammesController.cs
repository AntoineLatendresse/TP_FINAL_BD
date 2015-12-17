using MainClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DEMO_MVC.Controllers
{
    public class ProgrammesController : Controller
    {
        //
        // GET: /Programmes/
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Sort(String sortBy)
        {
            if (Session["Programme_SortBy"] == null)
            {
                Session["Programme_SortBy"] = sortBy;
                Session["Programme_SortOrder"] = "ASC";
            }
            else
            {
                if ((String)Session["Programme_SortBy"] == sortBy)
                {
                    if ((String)Session["Programme_SortOrder"] == "ASC")
                        Session["Programme_SortOrder"] = "DESC";
                    else
                        Session["Programme_SortOrder"] = "ASC";
                }
                else
                {
                    Session["Programme_SortBy"] = sortBy;
                    Session["Programme_SortOrder"] = "ASC";
                }
            }
            return RedirectToAction("Lister", "Programmes");
        }

        public ActionResult Lister()
        {
            Programmes programmes = new Programmes(Session["Main_BD"]);

            String orderBy = "";
            if (Session["Programme_SortBy"] != null)
                orderBy = (String)Session["Programme_SortBy"] + " " + (String)Session["Programme_SortOrder"];

            programmes.SelectAll(orderBy);
            return View(programmes.ToList());
        }

        public ActionResult Ajouter()
        {
            return View(new Programme());
        }

        [HttpPost]
        public ActionResult Ajouter(MainClass.Programme programme)
        {
            if (ModelState.IsValid)
            {
                Programmes programmes = new Programmes(Session["Main_BD"]);
                programmes.programme.CodeProgramme = Request["CodeProgramme"];
                programmes.programme.NomProgramme = Request["NomProgramme"];
                programmes.programme.UpLoadPoster(Request);
                programmes.Insert();
                return RedirectToAction("Lister", "Programmes");
            }
            return View(programme);
        }

        public ActionResult Effacer(String Id)
        {
            Programmes programmes = new Programmes(Session["Main_BD"]);
            programmes.DeleteRecordByID(Id);
            return RedirectToAction("Lister", "Programmes");
        }

        public ActionResult Editer(String Id)
        {
            Programmes programmes = new Programmes(Session["Main_BD"]);
            if (programmes.SelectByID(Id))
                return View(programmes.programme);
            else
                return RedirectToAction("Lister", "Programmes");
        }

        [HttpPost]
        public ActionResult Editer(MainClass.Programme programme)
        {
            Programmes programmes = new Programmes(Session["Main_BD"]);
            if (ModelState.IsValid)
            {
                if (programmes.SelectByID(programme.Id))
                {
                    programmes.programme.CodeProgramme = Request["CodeProgramme"];
                    programmes.programme.NomProgramme = Request["NomProgramme"];
                    programmes.programme.UpLoadPoster(Request);
                    programmes.Update();
                    return RedirectToAction("Lister", "Programmes");
                }
            }
            return View(programme);
        }

        public ActionResult Details(String Id)
        {
            Programmes programmes = new Programmes(Session["Main_BD"]);
            if (programmes.SelectByID(Id))
                return View(programmes.programme);
            else
                return RedirectToAction("Lister", "Programmes");
        }
	}
}