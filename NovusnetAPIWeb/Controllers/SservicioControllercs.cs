using Microsoft.AspNetCore.Mvc;
using Novusnet.Aplicacion.Servicio;
using Novusnet.Aplicacion.DTO.DTOS;
using Novusnet.Infraestructura.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovusnetAPIWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicioController : ControllerBase
    {
        private readonly IServicio _servicioServicio;

        public ServicioController(IServicio servicioServicio)
        {
            _servicioServicio = servicioServicio;
        }

        // MÉTODOS CRUD BÁSICOS

        /// <summary>
        /// Crear un nuevo servicio
        /// </summary>
        /// <param name="servicio">Datos del servicio a crear</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPost("CrearServicio")]
        public async Task<IActionResult> CrearServicio([FromBody] SServicio servicio)
        {
            try
            {
                if (servicio == null)
                    return BadRequest("Los datos del servicio son requeridos.");

                await _servicioServicio.ServicioAddAsync(servicio);

                return Ok(new { message = "Servicio creado exitosamente", data = servicio });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }


        /// <summary>
        /// Obtener un servicio por ID con datos del cliente
        /// </summary>
        /// <param name="id">ID del servicio</param>
        /// <returns>Servicio encontrado</returns>
        [HttpGet("ObtenerPorId/{id}")]
        public async Task<IActionResult> ObtenerServicioPorId(int id)
        {
            try
            {
                var servicio = await _servicioServicio.ServicioGetByIdAsync(id);

                if (servicio == null)
                    return NotFound(new { message = $"No se encontró el servicio con ID {id}" });

                return Ok(new { message = "Servicio encontrado", data = servicio });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Actualizar un servicio existente
        /// </summary>
        /// <param name="id">ID del servicio a actualizar</param>
        /// <param name="servicio">Nuevos datos del servicio</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPut("ActualizarServicio/{id}")]
        public async Task<IActionResult> ActualizarServicio(int id, [FromBody] SServicio servicio)
        {
            try
            {
                if (servicio == null)
                    return BadRequest("Los datos del servicio son requeridos.");

                if (id != servicio.pk_servicio)
                    return BadRequest("El ID del parámetro no coincide con el ID del servicio.");

                await _servicioServicio.ServicioUpdateAsync(servicio);
                return Ok(new { message = "Servicio actualizado exitosamente", data = servicio });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("no encontrado"))
                    return NotFound(new { message = ex.Message });

                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Eliminar un servicio
        /// </summary>
        /// <param name="id">ID del servicio a eliminar</param>
        /// <returns>Resultado de la operación</returns>
        [HttpDelete("EliminarServicio/{id}")]
        public async Task<IActionResult> EliminarServicio(int id)
        {
            try
            {
                await _servicioServicio.ServicioDeleteAsync(id);
                return Ok(new { message = "Servicio eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("no encontrado"))
                    return NotFound(new { message = ex.Message });

                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        // MÉTODOS PARA OBTENER SERVICIOS CON DATOS DE CLIENTE

        /// <summary>
        /// Obtener todos los servicios con datos básicos del cliente (DTO)
        /// </summary>
        /// <returns>Lista de servicios con datos del cliente</returns>
        [HttpGet("ServicioDatosCliente")]
        public async Task<IActionResult> ObtenerServiciosConDatosCliente()
        {
            try
            {
                var servicios = await _servicioServicio.ObtenerServiciosConDatosClienteAsync();
                return Ok(new { message = "Servicios obtenidos exitosamente", data = servicios, count = servicios.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtener todos los servicios con detalles completos del cliente
        /// </summary>
        /// <returns>Lista de servicios con detalles completos del cliente</returns>
        [HttpGet("ClientesConDetalles")]
        public async Task<IActionResult> ListarServiciosConDetallesCliente()
        {
            try
            {
                var servicios = await _servicioServicio.ListarServiciosConDetallesCliente();
                return Ok(new { message = "Servicios con detalles obtenidos exitosamente", data = servicios, count = servicios.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        // MÉTODO DE BÚSQUEDA AVANZADA

        /// <summary>
        /// Buscar servicios por criterio específico
        /// </summary>
        /// <param name="criterio">Criterio de búsqueda (nombre, descripcion, precio, cliente, etc.)</param>
        /// <param name="busqueda">Valor a buscar</param>
        /// <returns>Lista de servicios que coinciden con el criterio</returns>
        [HttpGet("buscarPorCriterio")]

        public async Task<IActionResult> BuscarServiciosPorCriterio([FromQuery] string criterio, [FromQuery] string busqueda)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(criterio) || string.IsNullOrWhiteSpace(busqueda))
                    return BadRequest("Los parámetros 'criterio' y 'busqueda' son requeridos.");

                var servicios = await _servicioServicio.BuscarServiciosPorCriterioDTO(criterio, busqueda);

                return Ok(new
                {
                    message = $"Búsqueda completada. Se encontraron {servicios.Count} resultado(s)",
                    criterio = criterio,
                    busqueda = busqueda,
                    data = servicios,
                    count = servicios.Count
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }
    }
}