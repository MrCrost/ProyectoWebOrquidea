using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrchidCareSystem.Controllers;
using WebProyectoOrquidea.Models;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;

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
            //Json En caso de modelo nulo
            if (model is null)
                return Json(new { success = false, message = "Datos vacíos" });

            // Json En caso de errores
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Modelo inválido", errors = ModelState });

            // Ajustes por defecto
            if (model.EstadoNotificacion == false)
                model.EstadoNotificacion = true;

            // Llamada al método del modelo que inserta en BD
            var repo = new CalendarioRiego();
            await repo.AgregarCalendario(model);

            // Si el guardado fue exitoso, redirigimos a la vista Calendar para recargar la página.
            return RedirectToAction(nameof(Calendar));
        }

        // GET: Obtener calendarios en JSON para que lo use el JS
        [HttpGet]
        public async Task<IActionResult> MostrarCalendario()
        {
            try
            {
                var repo = new CalendarioRiego();
                var list = await repo.GetCalendario();
                
                return Json(list);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message, data = new List<CalendarioRiego>() });
            }
        }

        // POST: Elimiar Alerta de Riego
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BorrarAlerta(int id, IFormCollection collection)
        {
            // Llamada al método del modelo que inserta en BD
            var repo = new CalendarioRiego();
            await repo.EliminarCalendario(id);

            // Si el guardado fue exitoso, redirigimos a la vista Calendar para recargar la página.
            return RedirectToAction(nameof(Calendar));
        }

        // POST: Modificar Estado de notificacion de la Alerta de Riego
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstadoNotificacion(int IdCalendarioRiego, bool EstadoNotificacion)
        {
            if (IdCalendarioRiego <= 0)
                return RedirectToAction(nameof(Calendar));

            try
            {
                var repo = new CalendarioRiego();
                await repo.ModificarCalendario(new CalendarioRiego
                {
                    IdCalendarioRiego = IdCalendarioRiego,
                    EstadoNotificacion = EstadoNotificacion
                });

                return RedirectToAction(nameof(Calendar));
            }
            catch (Exception)
            {
                // opcional: registrar el error
                return RedirectToAction(nameof(Calendar));
            }
        }

    }
}
