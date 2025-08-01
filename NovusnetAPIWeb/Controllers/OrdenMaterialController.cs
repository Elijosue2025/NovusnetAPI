using Microsoft.AspNetCore.Mvc;
using Novusnet.Aplicacion.DTO.DTOS;
using Novusnet.Aplicacion.Servicio;
using Novusnet.Infraestructura.AccesoDatos;

namespace NovusnetAPIWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdenMaterialController : ControllerBase
    {
        private readonly IOrdenMaterialServicio _ordenMaterialServicio;

        public OrdenMaterialController(IOrdenMaterialServicio ordenMaterialServicio)
        {
            _ordenMaterialServicio = ordenMaterialServicio;
        }

        [HttpGet("ordenes/completas")]
        public async Task<IActionResult> ObtenerOrdenesTrabajoCompletas()
        {
            try
            {
                var ordenes = await _ordenMaterialServicio.ObtenerOrdenesTrabajoCompletasConMaterialesAsync();
                return Ok(new { message = "Órdenes obtenidas exitosamente", data = ordenes, count = ordenes.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpGet("buscarPorCriterio")]
        public async Task<IActionResult> BuscarOrdenesPorCriterio([FromQuery] string criterio, [FromQuery] string busqueda)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(criterio) || string.IsNullOrWhiteSpace(busqueda))
                    return BadRequest("Los parámetros 'criterio' y 'busqueda' son requeridos.");

                var resultado = await _ordenMaterialServicio.FiltrarOrdenesTrabajoPorCriteriosAsync(criterio, busqueda);

                return Ok(new
                {
                    message = $"Búsqueda completada. Se encontraron {resultado.Count} resultado(s)",
                    criterio,
                    busqueda,
                    data = resultado,
                    count = resultado.Count
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarOrdenMaterial([FromBody] Orden_Material orden)
        {
            try
            {
                await _ordenMaterialServicio.OrdenMaterialAddAsync(orden);
                return Ok(new { message = "Orden de material registrada exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al registrar orden de material", error = ex.Message });
            }
        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> ActualizarOrdenMaterial([FromBody] Orden_Material orden)
        {
            try
            {
                await _ordenMaterialServicio.OrdenMaterialUpdateAsync(orden);
                return Ok(new { message = "Orden de material actualizada exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar orden de material", error = ex.Message });
            }
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> EliminarOrdenMaterial(int id)
        {
            try
            {
                await _ordenMaterialServicio.OrdenMaterialDeleteAsync(id);
                return Ok(new { message = "Estado de orden actualizado a devuelto/cancelado." });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("no encontrada"))
                    return NotFound(new { message = ex.Message });

                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerOrdenMaterialPorId(int id)
        {
            try
            {
                var orden = await _ordenMaterialServicio.OrdenMaterialGetByIdAsync(id);
                if (orden == null)
                    return NotFound(new { message = "Orden no encontrada." });

                return Ok(orden);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener orden de material", error = ex.Message });
            }
        }

        [HttpGet("listar")]
        public async Task<IActionResult> ListarOrdenesMaterial()
        {
            try
            {
                var ordenes = await _ordenMaterialServicio.OrdenMaterialGetAllAsync();
                return Ok(new { message = "Órdenes de material obtenidas exitosamente", data = ordenes, count = ordenes.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al listar órdenes de material", error = ex.Message });
            }
        }
    }
}
