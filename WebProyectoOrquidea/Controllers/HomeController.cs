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


        // GET: Obtener datos de sensores (API para actualización en tiempo real)
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

    //------------------ Eliminar lo de abajo cuando se agregue la base de datos ------------------------------------------------------------------------------

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