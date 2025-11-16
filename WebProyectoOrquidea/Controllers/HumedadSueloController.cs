using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebProyectoOrquidea.Controllers
{
    public class HumedadSueloController : Controller
    {
        // GET: HumedadSueloController
        public ActionResult HumiditySoil()
        {
            return View();
        }

        // GET: HumedadSueloController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HumedadSueloController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HumedadSueloController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(HumiditySoil));
            }
            catch
            {
                return View();
            }
        }

        // GET: HumedadSueloController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HumedadSueloController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(HumiditySoil));
            }
            catch
            {
                return View();
            }
        }

        // GET: HumedadSueloController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HumedadSueloController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(HumiditySoil));
            }
            catch
            {
                return View();
            }
        }


        // GET: Obtener historial de datos
        [HttpGet]
        public IActionResult GetHistoricalData(DateTime startDate, DateTime endDate, string sensorType)
        {
            // AQUÍ SE CONECTARÍA CON LA BASE DE DATOS
            // Consulta de ejemplo:
            /*
            var query = @"SELECT * FROM HistorialSensores 
                         WHERE FechaRegistro BETWEEN @StartDate AND @EndDate 
                         AND TipoSensor = @SensorType
                         ORDER BY FechaRegistro DESC";
            */

            return Json(new { success = true, data = new List<object>() });
        }
    }
}
