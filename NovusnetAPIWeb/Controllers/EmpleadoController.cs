using Microsoft.AspNetCore.Mvc;
using Novusnet.Aplicacion.Servicio;
using Novusnet.Aplicacion.DTO.DTOS;
using Novusnet.Infraestructura.AccesoDatos;

namespace NovusnetAPIWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadoController : ControllerBase
    {
        private IEmpleadoServicio _empleadoServicio;

        public EmpleadoController(IEmpleadoServicio empleadoServicio)
        {
            _empleadoServicio = empleadoServicio;
        }

        [HttpGet("ListaEmpleados")]
        public Task<IEnumerable<Empleado>> ListarEmpleadosActivos()
        {
            return _empleadoServicio.EmpleadoGetAllAsync();
        }

        [HttpPost("CrearEmpleado")]
        public async Task<IActionResult> EmpleadoAddAsync([FromBody] Empleado nuevoEmpleado)
        {
            try
            {
                await _empleadoServicio.EmpleadoAddAsync(nuevoEmpleado);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Error Interno");
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

        // NUEVOS MÉTODOS ADICIONALES

        [HttpGet("BuscarPorCriterio")]
        public async Task<IActionResult> BuscarEmpleadosPorCriterio([FromQuery] string criterio, [FromQuery] string busqueda)
        {
            try
            {
                // Validación de parámetros de entrada
                if (string.IsNullOrWhiteSpace(criterio))
                {
                    return BadRequest(new { error = "El criterio de búsqueda es requerido" });
                }

                if (string.IsNullOrWhiteSpace(busqueda))
                {
                    return BadRequest(new { error = "El término de búsqueda es requerido" });
                }

                // Trimear los parámetros para eliminar espacios
                criterio = criterio.Trim();
                busqueda = busqueda.Trim();

                // Llamar al servicio
                var empleados = await _empleadoServicio.BuscarEmpleadosPorCriterio(criterio, busqueda);

                // Verificar si se encontraron resultados
                if (empleados == null || !empleados.Any())
                {
                    return Ok(new
                    {
                        message = "No se encontraron empleados con los criterios especificados",
                        data = new List<object>(),
                        count = 0
                    });
                }

                // Retornar resultados exitosos
                return Ok(new
                {
                    message = "Búsqueda realizada exitosamente",
                    data = empleados,
                    count = empleados.Count
                });
            }
            catch (ArgumentException argEx)
            {
                // Manejar errores de argumentos específicos
                return BadRequest(new { error = "Parámetros inválidos", details = argEx.Message });
            }
            catch (Exception ex)
            {
                // Log del error (considera usar un logger como ILogger en lugar de Console.WriteLine)
                Console.WriteLine($"Error al buscar empleados: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");

                return StatusCode(500, new
                {
                    error = "Error interno del servidor al buscar empleados",
                    details = ex.Message
                });
            }
        }

     
        [HttpPatch("CambiarEstado/{pk_Empleado}")]
        public async Task<IActionResult> CambiarEstadoEmpleado(int pk_Empleado, [FromBody] bool nuevoEstado)
        {
            try
            {
                if (pk_Empleado <= 0)
                {
                    return BadRequest("ID de empleado inválido");
                }

                var resultado = await _empleadoServicio.CambiarEstadoEmpleado(pk_Empleado, nuevoEstado);

                if (!resultado)
                {
                    return NotFound($"Empleado con ID {pk_Empleado} no encontrado");
                }

                return Ok(new
                {
                    mensaje = "Estado del empleado actualizado correctamente",
                    empleadoId = pk_Empleado,
                    nuevoEstado = nuevoEstado ? "Activo" : "Inactivo"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { error = "Error al cambiar estado del empleado", details = ex.Message });
            }
        }



        [HttpGet("ObtenerEmpleado/{id}")]
        public async Task<IActionResult> GetEmpleado(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("ID de empleado inválido");
                }

                var empleado = await _empleadoServicio.EmpleadoGetByIdAsync(id);

                if (empleado == null)
                {
                    return NotFound($"Empleado con ID {id} no encontrado");
                }

                return Ok(empleado);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { error = "Error al obtener empleado", details = ex.Message });
            }
        }



    }
}
