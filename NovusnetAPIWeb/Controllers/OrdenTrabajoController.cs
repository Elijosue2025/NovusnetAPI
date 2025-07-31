using Microsoft.AspNetCore.Mvc;
using Novusnet.Aplicacion.Servicio;
using Novusnet.Infraestructura.AccesoDatos;
using Novusnet.Aplicacion.DTO.DTOS;

namespace NovusnetAPIWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdenTrabajoController : ControllerBase
    {
        private readonly IOrdenTrabajoServicio _ordenTrabajoServicio;

        public OrdenTrabajoController(IOrdenTrabajoServicio ordenTrabajoServicio)
        {
            _ordenTrabajoServicio = ordenTrabajoServicio;
        }

        // ===============================
        // CRUD BÁSICO
        // ===============================

        [HttpPost("crear")]
        public async Task<IActionResult> CrearOrden([FromBody] Orden_Trabajo orden)
        {
            try
            {
                await _ordenTrabajoServicio.CrearOrdenTrabajoAsync(orden);
                return Ok("Orden de trabajo creada correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("todas")]
        public async Task<IActionResult> ObtenerTodas()
        {
            try
            {
                var ordenes = await _ordenTrabajoServicio.ObtenerTodasOrdenesAsync();
                return Ok(ordenes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var orden = await _ordenTrabajoServicio.ObtenerOrdenPorIdAsync(id);
                if (orden == null) return NotFound();
                return Ok(orden);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> Actualizar([FromBody] Orden_Trabajo orden)
        {
            try
            {
                await _ordenTrabajoServicio.ActualizarOrdenTrabajoAsync(orden);
                return Ok("Orden actualizada correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("cancelar/{id}")]
        public async Task<IActionResult> CancelarOrden(int id)
        {
            try
            {
                await _ordenTrabajoServicio.EliminarOrdenTrabajoAsync(id);
                return Ok("Orden cancelada correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ===============================
        // CON DTO COMPLETO
        // ===============================

        [HttpGet("completas")]
        public async Task<IActionResult> ObtenerCompletas()
        {
            try
            {
                var lista = await _ordenTrabajoServicio.ObtenerOrdenesTrabajoCompletasAsync();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("completa/{id}")]
        public async Task<IActionResult> ObtenerCompletaPorId(int id)
        {
            try
            {
                var dto = await _ordenTrabajoServicio.ObtenerOrdenCompletaPorIdAsync(id);
                if (dto == null) return NotFound();
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar([FromQuery] string criterio, [FromQuery] string busqueda)
        {
            try
            {
                var resultados = await _ordenTrabajoServicio.BuscarOrdenesTrabajosPorCriterioAsync(criterio, busqueda);
                return Ok(resultados);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ===============================
        // FORMULARIOS / COMBOS
        // ===============================

        [HttpGet("formulario/empleados")]
        public async Task<IActionResult> EmpleadosParaFormulario()
        {
            try
            {
                var empleados = await _ordenTrabajoServicio.ObtenerEmpleadosParaFormularioAsync();
                return Ok(empleados);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("formulario/servicios")]
        public async Task<IActionResult> ServiciosParaFormulario()
        {
            try
            {
                var servicios = await _ordenTrabajoServicio.ObtenerServiciosParaFormularioAsync();
                return Ok(servicios);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ===============================
        // ESTADÍSTICAS
        // ===============================

        [HttpGet("estadisticas")]
        public async Task<IActionResult> Estadisticas()
        {
            try
            {
                var stats = await _ordenTrabajoServicio.ObtenerEstadisticasOrdenesAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
