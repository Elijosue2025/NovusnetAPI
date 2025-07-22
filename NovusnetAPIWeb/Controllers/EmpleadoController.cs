using Microsoft.AspNetCore.Mvc;
using Novusnet.Aplicacion.Servicio;
using Novusnet.Infraestructura.AccesoDatos;

namespace NovusnetAPIWeb.Controllers
{
    [ApiController]
    [Route("/")]

    public class EmpleadoController
    {
        private IEmpleadoServicio _empleadoServicio;

        public EmpleadoController(IEmpleadoServicio empleadoServicio) {
            _empleadoServicio = empleadoServicio;

        }
        [HttpGet("ListaEmpleados")]

        public Task<IEnumerable<Empleado>> ListarEmpleadosActivos()
        {
            return _empleadoServicio.EmpleadoGetAllAsync();
           // return Ok(empleados); // Esto retorna JSON automáticamente

        }
    }
}
