using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrchidCareSystem.Controllers;
using WebProyectoOrquidea.Models;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

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
                return RedirectToAction(nameof(Calendar));
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
                return RedirectToAction(nameof(Calendar));
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
                return RedirectToAction(nameof(Calendar));
            }
            catch
            {
                return View();
            }
        }



        // POST: Enviar notificación
        [HttpPost]
        public IActionResult SendNotification(NotificationModel model)
        {
            try
            {
                // AQUÍ SE INTEGRARÍAN LOS SERVICIOS DE NOTIFICACIÓN

                // 1. Envío por SMS (usando servicio como Twilio, etc.)
                if (model.SendBySMS)
                {
                    // SendSMSNotification(model.PhoneNumber, model.Message);
                }

                // 2. Envío por Email (usando SMTP o servicio como SendGrid)
                if (model.SendByEmail)
                {
                    // SendEmailNotification(model.Email, model.Subject, model.Message);
                }

                // 3. Notificación Web (Push notification)
                if (model.SendByWeb)
                {
                    // SendWebPushNotification(model.Message);
                }

                return Json(new { success = true, message = "Notificación enviada" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        // POST: Guardar Calendario de Riego
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveWateringSchedule(CalendarioRiego model)
        {
            try
            {
                if (model is null)
                    return Json(new { success = false, message = "Datos vacíos" });

                if (!ModelState.IsValid)
                    return Json(new { success = false, message = "Modelo inválido", errors = ModelState });
                                
                if (model.EstadoNotificacion == 0)
                    model.EstadoNotificacion = 1; 

                // Llamada al método del modelo que inserta en BD
                var newId = await model.AgregarCalendario(model);

                return Json(new { success = true, message = "Calendario guardado exitosamente", id = newId });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
