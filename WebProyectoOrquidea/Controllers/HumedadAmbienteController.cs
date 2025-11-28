using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Xml.Linq;
using WebProyectoOrquidea.ConexionDB;
using WebProyectoOrquidea.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

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


        // GET: Obtener Historial en JSON para que lo use el JS
        [HttpGet]
        public async Task<IActionResult> MostrarHistorial()
        {
            try
            {
                var repo = new RegistroHistoricoHA();
                var list = await repo.GetRegistroHistoricoHA();
                
                return Json(list);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message, data = new List<RegistroHistoricoHA>() });
            }
        }

        // POST: Agregar datos Sensor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarHistorial(RegistroHistoricoHA ha)
        {
            try
            {
                
                // Llamada al método del modelo que inserta en BD
                var historial = new RegistroHistoricoHA();
                await historial.AgregarRegistroHistoricoHA(ha);

                return Json(new { success = true, message = "Valores agregados" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        // POST: Agregar datos Sensor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarDatosSensor(int idSensor, double? humidity)
        {
            try
            {
                double? humidityResult = humidity / 10;
                double? temperature = null;
                // Llamada al método del modelo que inserta en BD
                var rValoresSensor = new ValoresSensor();
                await rValoresSensor.AgregarValoresSensor(idSensor, temperature, humidityResult);                

                return Json(new { success = true, message = "Valores agregados"});
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSensor()
        {
            try
            {
                var repo = new Sensor();
                var list = await repo.GetSensor();

                return Json(list);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message, data = new List<Sensor>() });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetValoresSensor()
        {
            try
            {
                var repo = new ValoresSensor();
                var list = await repo.GetValoresSensor();

                return Json(list);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message, data = new List<ValoresSensor>() });
            }
        }
    }
}
