using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebProyectoOrquidea.Controllers
{
    public class HumedadAmbienteController : Controller
    {
        // GET: HumedadAmbienteController
        public ActionResult Humidity()
        {
            return View();
        }

        // GET: HumedadAmbienteController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HumedadAmbienteController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HumedadAmbienteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Humidity));
            }
            catch
            {
                return View();
            }
        }

        // GET: HumedadAmbienteController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HumedadAmbienteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Humidity));
            }
            catch
            {
                return View();
            }
        }

        // GET: HumedadAmbienteController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HumedadAmbienteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Humidity));
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
