using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Films.Models;
using System.IO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Films.Controllers
{
    public class HomeController : Controller
    {
        private Entities db = new Entities();
       
        public ActionResult Index()
        {
            var model = new FilmsViewModel();
            model.Films = db.Film.ToList();
            ViewData["CurrentUser"] = User.Identity.GetUserId();

            return View(model);
        }

        public ActionResult AddFilm()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFilm([Bind(Include = "Id,Name,Description,Year,Producer,Poster")] Film newFilm, HttpPostedFileBase filmPoster)
        {
            if (ModelState.IsValid)
            {
                if (filmPoster != null)
                {
                    newFilm.Poster = new byte[filmPoster.ContentLength];
                    filmPoster.InputStream.Read(newFilm.Poster, 0, filmPoster.ContentLength);
                }

                newFilm.Creator = Guid.Parse(User.Identity.GetUserId());
                db.Film.Add(newFilm);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult DeleteFilm(int Id)
        {
            Film film = db.Film.Where(f => f.Id == Id).FirstOrDefault();
            db.Film.Remove(film);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult EditFilm(int id)
        {
            Film film = db.Film.Find(id);
            film.Name = (film.Name ?? "").Trim();
            film.Producer = (film.Producer ?? "").Trim();
            return View(film);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFilm([Bind(Include = "Id,Name,Description,Year,Producer,Poster")] Film f, HttpPostedFileBase filmPoster)
        {
            Film film = db.Film.Find(f.Id);

            if (ModelState.IsValid)
            {
                film.Name = f.Name;
                film.Description = f.Description;
                film.Year = f.Year;
                film.Producer = f.Producer;
                if (filmPoster != null)
                {
                    film.Poster = new byte[filmPoster.ContentLength];
                    filmPoster.InputStream.Read(film.Poster, 0, filmPoster.ContentLength);
                }

                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }

        public ActionResult ViewFilm(int id)
        {
            Film film = db.Film.Find(id);
            ViewData["CurrentUser"] = User.Identity.GetUserId();
            return View(film);
        }

    }
}