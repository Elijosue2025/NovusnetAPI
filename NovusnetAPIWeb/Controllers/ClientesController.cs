using Microsoft.AspNetCore.Mvc;
using Novusnet.Aplicacion.Servicio;
using Novusnet.Aplicacion.DTO.DTOS;
using Novusnet.Infraestructura.AccesoDatos;

namespace NovusnetAPIWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteServicio _clienteServicio;

        public ClientesController(IClienteServicio clienteServicio)
        {
            _clienteServicio = clienteServicio;
        }

        // GET: /ListaClientes
        [HttpGet("ListaClientes")]
        public async Task<IActionResult> ListarClientes()
        {
            try
            {
                var clientes = await _clienteServicio.ClienteGetAllAsync();
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener clientes", detalles = ex.Message });
            }
        }

        // GET: /ListaClientesActivos
        [HttpGet("ListaClientesActivos")]
        public async Task<IActionResult> ClientesPorEstado(bool activo )
        {
            try
            {
                var clientes = await _clienteServicio.ClientesPorEstado(activo);
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener clientes activos", detalles = ex.Message });
            }
        }

        // GET: /ClientePorId/{id}
        [HttpGet("ClientePorId/{id}")]
        public async Task<IActionResult> ObtenerClientePorId(int id)
        {
            try
            {
                var cliente = await _clienteServicio.ClienteGetByIdAsync(id);
                if (cliente == null)
                    return NotFound($"Cliente con ID {id} no encontrado");
                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener cliente", detalles = ex.Message });
            }
        }

    

        // GET: /ClientesPorEstado/{activo}
        [HttpGet("ClientesPorEstado/{activo}")]
        public async Task<IActionResult> ObtenerClientesPorEstado(bool activo)
        {
            try
            {
                var clientes = await _clienteServicio.ClientesPorEstado(activo);
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener clientes por estado", detalles = ex.Message });
            }
        }

        // GET: /BuscarClientes/{criterio}/{busqueda}
        [HttpGet("BuscarClientes/{criterio}/{busqueda}")]
        public async Task<IActionResult> BuscarClientesPorCriterio(string criterio, string busqueda)
        {
            try
            {
                var clientes = await _clienteServicio.BuscarClientesPorCriterio(criterio, busqueda);
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al buscar clientes", detalles = ex.Message });
            }
        }

        // POST: /CrearCliente
        [HttpPost("CrearCliente")]
        public async Task<IActionResult> CrearCliente([FromBody] Cliente nuevoCliente)
        {
            try
            {
                if (nuevoCliente == null)
                    return BadRequest("Los datos del cliente son requeridos");

                await _clienteServicio.ClienteAddAsync(nuevoCliente);
                return Ok(new { mensaje = "Cliente creado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al crear cliente", detalles = ex.Message });
            }
        }

        // PUT: /ActualizarCliente/{id}
        [HttpPut("ActualizarCliente/{id}")]
        public async Task<IActionResult> ActualizarCliente(int id, [FromBody] Cliente cliente)
        {
            try
            {
                if (cliente == null)
                    return BadRequest("Los datos del cliente son requeridos");

                if (id != cliente.pk_cliente)
                    return BadRequest("El ID del path no coincide con el del cuerpo.");

                await _clienteServicio.ClienteUpdateAsync(cliente);
                return Ok(new { mensaje = "Cliente actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al actualizar cliente", detalles = ex.Message });
            }
        }

        // DELETE: /EliminarCliente/{id}
        [HttpDelete("EliminarCliente/{id}")]
        public async Task<IActionResult> EliminarCliente(int id)
        {
            try
            {
                var clienteExistente = await _clienteServicio.ClienteGetByIdAsync(id);
                if (clienteExistente == null)
                    return NotFound($"Cliente con ID {id} no encontrado");

                await _clienteServicio.ClienteDeleteAsync(id);
                return Ok(new { mensaje = "Cliente eliminado exitosamente", id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al eliminar cliente", detalles = ex.Message });
            }
        }

        // PUT: /CambiarEstadoCliente/{id}/{nuevoEstado}
        [HttpPut("CambiarEstadoCliente/{id}/{nuevoEstado}")]
        public async Task<IActionResult> CambiarEstadoCliente(int id, bool nuevoEstado)
        {
            try
            {
                var resultado = await _clienteServicio.CambiarEstadoCliente(id, nuevoEstado);
                if (!resultado)
                    return NotFound($"Cliente con ID {id} no encontrado");

                return Ok(new { mensaje = "Estado del cliente cambiado exitosamente", id, nuevoEstado });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al cambiar estado del cliente", detalles = ex.Message });
            }
        }
    }
}