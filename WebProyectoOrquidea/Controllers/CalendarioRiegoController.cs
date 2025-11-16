using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebProyectoOrquidea.Controllers
{
    public class CalendarioRiegoController : Controller
    {
        // GET: CalendarioRiegoController
        public ActionResult Calendar()
        {
            return View();
        }

        // GET: CalendarioRiegoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CalendarioRiegoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CalendarioRiegoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CalendarioRiegoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CalendarioRiegoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CalendarioRiegoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CalendarioRiegoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
