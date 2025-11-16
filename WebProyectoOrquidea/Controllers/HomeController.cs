using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace OrchidCareSystem.Controllers
{
    public class HomeController : Controller
    {
        // GET: Login Page
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login Authentication
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username == "admin" && password == "admin")
            {
                HttpContext.Session.SetString("User", username);
                return RedirectToAction("Dashboard");
            }

            ViewBag.ErrorMessage = "Usuario o contraseña incorrectos";
            return View();
        }

        // GET: Dashboard Principal
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("User") == null)
                return RedirectToAction("Login");

            return View();
        }

        // GET: Monitor de Humedad
        public IActionResult Humidity()
        {
            // Aquí se obtendría:
            // - Datos actuales de sensores de humedad desde la BD
            // - Historial de lecturas
            // - Alertas activas
            
            return View();
        }

        // GET: Monitor de Temperatura
        public IActionResult Temperature()
        {
            // Aquí se obtendría:
            // - Datos actuales de sensores de temperatura desde la BD
            // - Historial de lecturas
            // - Gráficos de variación térmica
            
            return View();
        }

        // GET: Calendario de Riego
        public IActionResult Calendar()
        {
            // Aquí se obtendría:
            // - Lista de calendarios programados desde la BD
            // - Riegos pendientes del día
            // - Historial de notificaciones enviadas
            
            return View();
        }

        // POST: Guardar Calendario de Riego
        [HttpPost]
        public IActionResult SaveWateringSchedule(WateringScheduleModel model)
        {
            try
            {
                // AQUÍ SE CONECTARÍA CON LA BASE DE DATOS
                // Ejemplo de lo que se haría:
                
                /*
                using (var connection = new SqlConnection(connectionString))
                {
                    var query = @"INSERT INTO CalendarioRiego 
                                  (NombrePlanta, Zona, FechaRiego, HoraRiego, Frecuencia, MetodoNotificacion, Estado, FechaCreacion)
                                  VALUES 
                                  (@NombrePlanta, @Zona, @FechaRiego, @HoraRiego, @Frecuencia, @MetodoNotificacion, 1, GETDATE())";
                    
                    connection.Execute(query, model);
                }
                */
                
                return Json(new { success = true, message = "Calendario guardado exitosamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Obtener datos de sensores (API endpoint para actualización en tiempo real)
        [HttpGet]
        public IActionResult GetSensorData()
        {
            // En producción, esto obtendría datos reales de los sensores IoT
            // conectados vía Bluetooth/Wi-Fi y almacenados en la BD
            
            var data = new
            {
                humidity = new
                {
                    sensor1 = GetRandomHumidity(),
                    sensor2 = GetRandomHumidity(),
                    sensor3 = GetRandomHumidity()
                },
                temperature = new
                {
                    greenhouse = GetRandomTemperature(),
                    propagation = GetRandomTemperature(),
                    exterior = GetRandomTemperature()
                },
                timestamp = DateTime.Now
            };

            return Json(data);
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

                // Guardar registro en BD
                /*
                var query = @"INSERT INTO HistorialNotificaciones 
                             (TipoNotificacion, Mensaje, FechaEnvio, Estado)
                             VALUES (@Tipo, @Mensaje, GETDATE(), 'Enviado')";
                */

                return Json(new { success = true, message = "Notificación enviada" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Métodos auxiliares para simular datos de sensores
        private double GetRandomHumidity()
        {
            Random random = new Random();
            return Math.Round(random.NextDouble() * 40 + 40, 1); // 40-80%
        }

        private double GetRandomTemperature()
        {
            Random random = new Random();
            return Math.Round(random.NextDouble() * 8 + 17, 1); // 17-25°C
        }
    }

    // Modelos de datos
    public class WateringScheduleModel
    {
        public string PlantName { get; set; }
        public string Zone { get; set; }
        public DateTime WateringDate { get; set; }
        public TimeSpan WateringTime { get; set; }
        public string Frequency { get; set; }
        public string NotificationMethod { get; set; }
    }

    public class NotificationModel
    {
        public string Message { get; set; }
        public string Subject { get; set; }
        public bool SendBySMS { get; set; }
        public bool SendByEmail { get; set; }
        public bool SendByWeb { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }

    public class SensorDataModel
    {
        public int SensorId { get; set; }
        public double Value { get; set; }
        public string SensorType { get; set; } // "Humidity " o "Temperature"
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }
    }
}