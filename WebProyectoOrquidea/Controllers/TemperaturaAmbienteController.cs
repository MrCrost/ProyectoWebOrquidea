using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebProyectoOrquidea.Models;

namespace WebProyectoOrquidea.Controllers
{
    public class TemperaturaAmbienteController : Controller
    {
        // GET: TemperaturaAmbienteController
        public ActionResult Temperature()
        {
            return View();
        }

        // GET: TemperaturaAmbienteController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TemperaturaAmbienteController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TemperaturaAmbienteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Temperature));
            }
            catch
            {
                return View();
            }
        }

        // GET: TemperaturaAmbienteController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TemperaturaAmbienteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Temperature));
            }
            catch
            {
                return View();
            }
        }

        // GET: TemperaturaAmbienteController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TemperaturaAmbienteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Temperature));
            }
            catch
            {
                return View();
            }
        }


        // POST: Agregar datos Sensor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarDatosSensorEHistorialTA(int idSensor, double temperature, DateTime date, TimeSpan time, string periodo, string estado)
        {
            try
            {
                
                

                double? humedad = null;

                var rValoresSensor = new ValoresSensor();
                await rValoresSensor.AgregarValoresEHistorialTA(idSensor, temperature /10, humedad, date, time, periodo, estado);

                return Json(new { success = true, message = "Valores agregados" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Obtener Historial en JSON para que lo use el JS
        [HttpGet]
        public async Task<IActionResult> MostrarHistorial()
        {
            try
            {
                var repo = new RegistroHistoricoTA();
                var list = await repo.GetRegistroHistoricoTA();

                return Json(list);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message, data = new List<RegistroHistoricoTA>() });
            }
        }


    }
}
