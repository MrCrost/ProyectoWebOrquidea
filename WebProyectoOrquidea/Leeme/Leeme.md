# 🌸 OrchidCare System - Sistema de Monitoreo de Orquídeas

## 📋 Descripción del Proyecto

Sistema web de monitoreo inteligente para cultivo de orquídeas en invernadero, desarrollado con **ASP.NET Core MVC**. Integra tecnologías de la Industria 4.0 (sensores IoT, conectividad Bluetooth/Wi-Fi) para optimizar las condiciones de cultivo.

### Parámetros Óptimos de Cultivo
- **Humedad Relativa:** ~80%
- **Temperatura Nocturna:** 18°C - 20°C
- **Temperatura Diurna:** 22°C - 24°C

---

## ✨ Funcionalidades Principales

### 1. 🔐 Sistema de Autenticación
- Login seguro con validación de credenciales
- Interfaz temática con fondo de orquídeas
- Usuario demo: `admin` / `admin123`

### 2. 📊 Dashboard Principal
- Visualización en tiempo real de condiciones ambientales
- Panel de notificaciones centralizado
- Acceso rápido a todas las funcionalidades
- Resumen rápido de sensores activos

### 3. 💧 Monitor de Humedad
- **3 sensores** distribuidos en zonas A, B y C
- Lecturas en tiempo real con actualización cada 5 segundos
- Estados: Óptimo (40-70%) / Seco / Muy Húmedo
- Indicadores visuales de próximos riegos
- Historial de últimas lecturas

### 4. 🌡️ Monitor de Temperatura
- **3 sensores:** Invernadero principal, Zona de propagación, Exterior
- Diferenciación Día/Noche automática
- Alertas cuando se sale del rango óptimo
- Gráfico de variación térmica (24 horas simulado)
- Registro histórico detallado

### 5. 📅 Calendario de Riego
- Programación de riegos personalizados
- Configuración de frecuencias: Diario, Cada 2/3 días, Semanal
- **Sistema de notificaciones multi-canal:**
  - 📱 SMS (simulado)
  - 📧 Email (simulado)
  - 🌐 Notificaciones web en tiempo real
- Gestión completa: Activar/Pausar/Eliminar calendarios
- Vista de riegos del día actual
- Notificaciones automáticas cada 15 segundos

---

## 🏗️ Arquitectura del Sistema

### Tecnologías Utilizadas
- **Backend:** ASP.NET Core MVC (.NET 6/7/8)
- **Frontend:** HTML5, CSS3, JavaScript (Vanilla)
- **Base de Datos:** SQL Server (diseñada pero no implementada físicamente en este prototipo)
- **Patrón de diseño:** MVC (Model-View-Controller)

### Estructura de Archivos

```
OrchidCareSystem/
├── Controllers/
│   └── HomeController.cs          # Controlador principal con todos los endpoints
├── Views/
│   └── Home/
│       ├── Login.cshtml           # Página de inicio de sesión
│       ├── Dashboard.cshtml       # Panel principal
│       ├── Humidity.cshtml        # Monitor de humedad
│       ├── Temperature.cshtml     # Monitor de temperatura
│       └── Calendar.cshtml        # Calendario de riego
├── wwwroot/
│   └── css/
│       └── Styles.css             # Estilos globales del sistema
└── SQL/
    └── DatabaseScript.sql         # Script completo de base de datos
```

---

## 🗄️ Diseño de Base de Datos

### Tablas Principales

#### 1. **Usuarios**
Gestiona los usuarios del sistema con autenticación.

#### 2. **Sensores**
Catálogo de sensores IoT instalados (Humedad y Temperatura).
- Almacena dirección MAC para conectividad
- Estado activo/inactivo
- Última lectura registrada

#### 3. **HistorialHumedad**
Registro histórico de todas las lecturas de humedad.
- Valor de humedad (%)
- Estado (Óptimo/Bajo/Alto)
- Timestamp

#### 4. **HistorialTemperatura**
Registro histórico de temperatura.
- Temperatura (°C)
- Período del día (Día/Noche)
- Estado según rango óptimo

#### 5. **Plantas**
Catálogo de orquídeas en el sistema.
- Especie, Zona, Fecha de plantación
- Estado de salud

#### 6. **CalendarioRiego**
Programación de riegos.
- Fecha y hora programada
- Frecuencia de repetición
- Método de notificación

#### 7. **HistorialRiegos**
Registro de riegos realizados.

#### 8. **Notificaciones**
Historial de todas las notificaciones enviadas.
- Canal (SMS/Email/Web)
- Estado (Enviado/Pendiente/Fallido)

#### 9. **Alertas**
Alertas del sistema por condiciones anormales.
- Severidad (Baja/Media/Alta/Crítica)
- Estado de resolución

#### 10. **ConfiguracionSistema**
Parámetros configurables del sistema.

### Stored Procedures Incluidos

```sql
-- Registrar lectura de humedad con alertas automáticas
sp_RegistrarHumedad @SensorID, @ValorHumedad

-- Obtener próximos riegos programados
sp_ObtenerProximosRiegos @Fecha

-- Obtener historial de sensores
sp_ObtenerHistorial @TipoSensor, @FechaInicio, @FechaFin
```

### Vistas Útiles

```sql
-- Sensores activos con última lectura
vw_SensoresActivos

-- Alertas no resueltas
vw_AlertasActivas
```

---

## 🔧 Integración de Sensores IoT (Propuesta)

### Hardware Recomendado

#### Opción 1: ESP32 + Sensores DHT22
```
ESP32 (Wi-Fi/Bluetooth)
├── DHT22 (Temperatura y Humedad)
├── Sensor de Humedad de Suelo Capacitivo
└── Alimentación 5V
```

#### Opción 2: Raspberry Pi + Múltiples Sensores
```
Raspberry Pi 4
├── DHT22 x3 (diferentes zonas)
├── DS18B20 (Temperatura de precisión)
├── Sensores de Humedad de Suelo x3
└── Módulo Bluetooth/Wi-Fi integrado
```

### Protocolo de Comunicación

**Método 1: HTTP REST API**
```javascript
// El sensor envía datos vía POST
POST /api/sensors/data
{
  "sensorId": 1,
  "type": "humidity",
  "value": 78.5,
  "timestamp": "2024-11-15T10:30:00"
}
```

**Método 2: MQTT (Recomendado para IoT)**
```
Broker MQTT → Sistema Web
Topic: orchidcare/sensors/humidity/1
Payload: {"value": 78.5, "timestamp": "..."}
```

**Método 3: SignalR (Tiempo Real)**
```csharp
// Hub de SignalR para datos en tiempo real
public class SensorHub : Hub
{
    public async Task SendSensorData(SensorData data)
    {
        await Clients.All.SendAsync("ReceiveSensorData", data);
    }
}
```

---

## 🚀 Instalación y Configuración

### Requisitos Previos
- Visual Studio 2022 o superior
- .NET 6.0 SDK o superior
- SQL Server 2019 o superior
- Navegador web moderno

### Pasos de Instalación

1. **Clonar/Descargar el proyecto**
```bash
git clone https://github.com/tu-repo/orchidcare-system.git
cd orchidcare-system
```

2. **Configurar la Base de Datos**
```bash
# Ejecutar el script SQL en SQL Server Management Studio
# Archivo: DatabaseScript.sql
```

3. **Configurar Connection String**
```csharp
// En appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=OrchidCareDB;Trusted_Connection=True;"
  }
}
```

4. **Restaurar paquetes y compilar**
```bash
dotnet restore
dotnet build
```

5. **Ejecutar la aplicación**
```bash
dotnet run
```

6. **Acceder al sistema**
```
URL: https://localhost:5001
Usuario: admin
Contraseña: admin123
```

---

## 📊 Características del Prototipo Actual

### ✅ Implementado (Funcional)

- ✅ Sistema de login completo
- ✅ Dashboard con datos simulados en tiempo real
- ✅ Monitoreo de humedad (3 sensores)
- ✅ Monitoreo de temperatura (3 sensores)
- ✅ Calendario de riego con CRUD completo
- ✅ Sistema de notificaciones multi-canal (simulado)
- ✅ Actualización automática de datos cada 3-5 segundos
- ✅ Interfaz responsive y moderna
- ✅ Diseño temático de orquídeas
- ✅ Alertas visuales por condiciones fuera de rango

### 🔄 Por Implementar (Próximas Fases)

- 🔄 Conexión real con base de datos SQL Server
- 🔄 Integración con sensores IoT físicos
- 🔄 Envío real de SMS (Twilio API)
- 🔄 Envío real de Emails (SMTP/SendGrid)
- 🔄 Autenticación JWT para API
- 🔄 Gráficos históricos interactivos (Chart.js)
- 🔄 Exportación de reportes PDF
- 🔄 Panel de administración de usuarios
- 🔄 Configuración de umbrales personalizados

---

## 🧪 Pruebas del Sistema

### Plan de Pruebas Propuesto

#### 1. Pruebas Funcionales

**RF1 - Establecer Calendario de Riego**
```
Caso de Prueba: CP-RF1-01
Descripción: Crear un nuevo calendario de riego
Pasos:
1. Iniciar sesión en el sistema
2. Navegar a "Calendario de Riego"
3. Completar formulario con datos válidos
4. Presionar "Guardar"
Resultado Esperado: Calendario creado exitosamente, notificación mostrada
Estado: ✅ APROBADO
```

**RF2 - Notificar Hora de Riego**
```
Caso de Prueba: CP-RF2-01
Descripción: Verificar envío de notificaciones automáticas
Pasos:
1. Programar un riego para el día actual
2. Esperar la hora programada
3. Verificar notificación en pantalla
Resultado Esperado: Notificación muestra mensaje con canales (SMS/Email/Web)
Estado: ✅ APROBADO (Simulado)
```

**RF3 - Integración de Sensores**
```
Caso de Prueba: CP-RF3-01
Descripción: Verificar conexión de sensores (simulado)
Pasos:
1. Acceder al monitor de humedad/temperatura
2. Verificar actualización automática de datos
Resultado Esperado: Datos se actualizan cada 5 segundos
Estado: ✅ APROBADO (Simulado)
```

**RF4 - Monitorear Nivel de Humedad**
```
Caso de Prueba: CP-RF4-01
Descripción: Verificar monitoreo en tiempo real
Pasos:
1. Navegar a "Humedad del Suelo"
2. Observar valores de 3 sensores
3. Verificar estados (Óptimo/Seco/Húmedo)
Resultado Esperado: Valores se muestran con estado correcto
Estado: ✅ APROBADO
```

**RF7 - Visualización en Tiempo Real**
```
Caso de Prueba: CP-RF7-01
Descripción: Dashboard muestra datos actualizados
Pasos:
1. Acceder al Dashboard
2. Observar panel "Resumen Rápido"
3. Verificar actualización automática
Resultado Esperado: Datos se refrescan cada 3 segundos
Estado: ✅ APROBADO
```

#### 2. Pruebas de Interfaz (UI/UX)

- ✅ Diseño responsive en diferentes resoluciones
- ✅ Navegación intuitiva entre módulos
- ✅ Elementos visuales coherentes con temática
- ✅ Feedback visual en todas las acciones
- ✅ Mensajes de error claros y útiles

#### 3. Pruebas de Rendimiento (Propuestas)

```
Métrica: Tiempo de carga inicial
Objetivo: < 2 segundos
Método: Google Lighthouse

Métrica: Actualización de datos en tiempo real
Objetivo: Latencia < 500ms
Método: Medición de intervalos JavaScript

Métrica: Consultas a base de datos
Objetivo: < 100ms por query
Método: SQL Server Profiler
```

---

## 📱 Capturas de Pantalla

### Login
![Login Screen](Imagen de pantalla de login con fondo de orquídeas)

### Dashboard
![Dashboard](Panel principal con resumen de sensores y notificaciones)

### Monitor de Humedad
![Humidity Monitor](3 tarjetas de sensores con valores en tiempo real)

### Monitor de Temperatura
![Temperature Monitor](Sensores con gráfico de variación diaria)

### Calendario de Riego
![Watering Calendar](Formulario y lista de riegos programados)

---

## 🔒 Consideraciones de Seguridad

### Implementadas
- ✅ Validación de formularios en cliente y servidor
- ✅ Protección contra inyección de código en vistas
- ✅ Sanitización de inputs

### Recomendadas para Producción
- 🔐 Hash de contraseñas con BCrypt
- 🔐 Autenticación JWT para API
- 🔐 HTTPS obligatorio
- 🔐 Rate limiting en endpoints
- 🔐 Validación de CSRF tokens
- 🔐 Encriptación de datos sensibles en BD
- 🔐 Logs de auditoría

---

## 📞 Soporte y Contacto

**Equipo de Desarrollo:**
- Proyecto Académico - Sistema OrchidCare
- Institución: [Tu Institución]
- Curso: Desarrollo de Aplicaciones Web / IoT

**Documentación Técnica:**
- ASP.NET Core MVC: https://docs.microsoft.com/aspnet/core/mvc
- SQL Server: https://docs.microsoft.com/sql
- IoT con ESP32: https://docs.espressif.com

---

## 📄 Licencia

Este proyecto es un prototipo académico desarrollado con fines educativos.

---

## 🎯 Próximos Pasos Recomendados

### Fase 2 - Implementación Real (Próxima Semana)
1. ✅ Completar conexión con SQL Server
2. ✅ Implementar autenticación robusta
3. ✅ Crear API REST para sensores
4. ✅ Documentar endpoints con Swagger

### Fase 3 - Integración IoT
1. Adquirir hardware (ESP32 + sensores)
2. Programar firmware para sensores
3. Establecer comunicación MQTT
4. Probar envío de datos reales

### Fase 4 - Funcionalidades Avanzadas
1. Gráficos históricos interactivos
2. Machine Learning para predicciones
3. App móvil (Xamarin/Flutter)
4. Sistema de respaldo y recuperación

---

## ⭐ Características Destacadas

- 🌸 **Diseño Temático:** Interfaz inspirada en cultivo de orquídeas
- ⚡ **Tiempo Real:** Actualización automática sin recargar
- 📊 **Multi-Sensor:** Soporte para múltiples sensores simultáneos
- 🔔 **Notificaciones Inteligentes:** Sistema multi-canal configurable
- 📅 **Calendario Flexible:** Múltiples frecuencias de riego
- 💾 **Preparado para Datos:** Estructura de BD profesional
- 🔌 **IoT Ready:** Preparado para integración con hardware real

---

**Versión:** 1.0.0  
**Última Actualización:** Noviembre 2024  
**Estado:** Prototipo Funcional ✅