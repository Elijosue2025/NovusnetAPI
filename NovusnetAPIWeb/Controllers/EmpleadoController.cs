using Microsoft.AspNetCore.Mvc;
using Novusnet.Aplicacion.Servicio;
using Novusnet.Infraestructura.AccesoDatos;


namespace NovusnetAPIWeb.Controllers
{
    [ApiController]
    [Route("/")]

    public class EmpleadoController : ControllerBase
    {
        private IEmpleadoServicio _empleadoServicio;
      //  private IEmpleadoServicio _empleadoServicio;


        public EmpleadoController(IEmpleadoServicio empleadoServicio) {
            _empleadoServicio = empleadoServicio;

        }
        [HttpGet("ListaEmpleados")]

        public Task<IEnumerable<Empleado>> ListarEmpleadosActivos()
        {
            return _empleadoServicio.EmpleadoGetAllAsync();
           // return Ok(empleados); // Esto retorna JSON automáticamente

        }

        [HttpPost("CrearEmpleado")]
        public async Task<IActionResult> EmpleadoAddAsync([FromBody] Empleado nuevoEmpleado)
        {
            try
            {
                await _empleadoServicio.EmpleadoAddAsync(nuevoEmpleado);
                return Ok(); // Sin Results.
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Error Interno"); // Sin Results.
            }
        }

        

        [HttpDelete("EliminarEmpleado/{pk_Empleado}")]
        public async Task<IActionResult> EliminarEmpleado(int pk_Empleado)
        {
            try
            {
                if (pk_Empleado <= 0)
                {
                    return BadRequest("ID de empleado inválido");
                }

                // ✅ CORRECCIÓN: Remover los asteriscos
                var empleadoExistente = await _empleadoServicio.EmpleadoGetByIdAsync(pk_Empleado);
                if (empleadoExistente == null)
                {
                    return NotFound($"Empleado con ID {pk_Empleado} no encontrado");
                }

                await _empleadoServicio.EmpleadoDeleteAsync(pk_Empleado);
                return Ok(new { message = "Empleado eliminado exitosamente", id = pk_Empleado });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar empleado: {ex.Message}");
                return StatusCode(500, new { error = "Error al eliminar empleado", details = ex.Message });
            }
        }

        [HttpGet("ListarPorRol")]
        public async Task<IActionResult> ListarEmpleadosPorRol()
        {
            try
            {
                var empleados = await _empleadoServicio.ListarEmpleadoRoll();
                return Ok(empleados);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Error al listar empleados por rol");
            }
        }
        [HttpPut("ActualizarEmpleado/{id}")]
        public async Task<IActionResult> ActualizarEmpleado(int id, [FromBody] Empleado empleado)
        {
            try
            {
                if (id != empleado.pk_Empleado)
                    return BadRequest("El ID del path no coincide con el del cuerpo.");

                await _empleadoServicio.EmpleadoUpdateAsync(empleado);
                return Ok(new { mensaje = "Empleado actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al actualizar empleado", details = ex.Message });
            }
        }


    }
}
