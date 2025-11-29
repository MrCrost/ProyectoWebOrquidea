using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebProyectoOrquidea.Models;

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

        // GET: Filtro de historial 
        [HttpGet]
        public async Task<IActionResult> Filtro(int? idSensor, string zona, DateTime fechaInicial, DateTime fechaFinal, TimeSpan horaInicial, TimeSpan horaFinal)
        {
            try
            {
                var list = new List<RegistroHistoricoHS>();
                var repo = new RegistroHistoricoHS();

                if (idSensor.HasValue)
                {
                    list = await repo.GetFiltroIdSensor(idSensor.Value, fechaInicial, fechaFinal, horaInicial, horaFinal);
                }
                else if (!string.IsNullOrWhiteSpace(zona))
                {
                    list = await repo.GetFiltroZona(zona, fechaInicial, fechaFinal, horaInicial, horaFinal);
                }
                else
                {
                    list = await repo.GetFiltroFechaHora(fechaInicial, fechaFinal, horaInicial, horaFinal);
                }



                return Json(list);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message, data = new List<RegistroHistoricoHS>() });
            }
        }

        // POST: Agregar datos Sensor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarDatosSensorEHistorialHS(int idSensor, double humedad, DateTime date, TimeSpan time, string estado)
        {
            try
            {
                double? temperature = null;

                var rValoresSensor = new ValoresSensor();
                await rValoresSensor.AgregarValoresEHistorialHS(idSensor, temperature , humedad / 10, date, time, estado);

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
                var repo = new RegistroHistoricoHS();
                var list = await repo.GetRegistroHistoricoHS();

                return Json(list);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message, data = new List<RegistroHistoricoHS>() });
            }
        }
    }
}
