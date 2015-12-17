using MainClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DEMO_MVC.Controllers
{
    public class CoursController : Controller
    {
        //
        // GET: /Cours/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sort(String sortBy)
        {
            if (Session["Cour_SortBy"] == null)
            {
                Session["Cour_SortBy"] = sortBy;
                Session["Cour_SortOrder"] = "ASC";
            }
            else
            {
                if ((String)Session["Cour_SortBy"] == sortBy)
                {
                    if ((String)Session["Cour_sortOrder"] == "ASC")
                        Session["Cour_SortOrder"] = "DESC";
                    else
                        Session["Cour_SortOrder"] = "ASC";
                }
                else
                {
                    Session["Cour_SortBy"] = sortBy;
                    Session["Cour_SortOrder"] = "ASC";
                }
            }
            return RedirectToAction("Lister", "Cours");
        }

        public ActionResult Lister()
        {
            Cours cours = new Cours(Session["Main_BD"]);

            String orderBy = "";
            if (Session["Cour_SortBy"] != null)
                orderBy = (String)Session["Cour_SortBy"] + " " + (String)Session["Cour_SortOrder"];

            cours.SelectAll(orderBy);
            return View(cours.ToList());
        }

        public ActionResult Ajouter()
        {
            return View(new Cour());
        }

        [HttpPost]
        public ActionResult Ajouter(MainClass.Cour cour)
        {
            if (ModelState.IsValid)
            {
                Cours cours = new Cours(Session["Main_BD"]);
                cours.cour.CodeCours = Request["CodeCours"];
                cours.cour.NomCours = Request["NomCours"];
                cours.cour.Ponderation = Request["Ponderation"];
                cours.Insert();
                return RedirectToAction("Lister", "Cours");
            }
            return View(cour);
        }

        public ActionResult Effacer(String Id)
        {
            Cours cours = new Cours(Session["Main_BD"]);
            cours.DeleteRecordByID(Id);
            return RedirectToAction("Lister", "Cours");
        }


        public ActionResult Editer(String Id)
        {
            Cours cours = new Cours(Session["Main_BD"]);
            if (cours.SelectByID(Id))
                return View(cours.cour);
            else
                return RedirectToAction("Lister", "Cours");
        }

        [HttpPost]
        public ActionResult Editer(MainClass.Cour cour)
        {
            Cours cours = new Cours(Session["Main_BD"]);
            if (ModelState.IsValid)
            {
                if (cours.SelectByID(cour.Id))
                {
                    cours.cour.CodeCours = Request["CodeCours"];
                    cours.cour.NomCours = Request["NomCours"];
                    cours.cour.Ponderation = Request["Ponderation"];
                    cours.Update();
                    return RedirectToAction("Lister", "Cours");
                }
            }
            return View(cour);
        }
	}
}