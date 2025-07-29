using Microsoft.AspNetCore.Mvc;
using Novusnet.Aplicacion.Servicio;
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
            var clientes = await _clienteServicio.ClienteGetAllAsync();
            return Ok(clientes);
        }

        // GET: /ListaClientesActivos
        [HttpGet("ListaClientesActivos")]
        public async Task<IActionResult> ListarClientesActivos()
        {
            var clientes = await _clienteServicio.ListarClientesActivos();
            return Ok(clientes);
        }



       

        // GET: /ClientePorId/{id}
        [HttpGet("ClientePorId/{id}")]
        public async Task<IActionResult> ObtenerClientePorId(int id)
        {
            var cliente = await _clienteServicio.ClienteGetByIdAsync(id);
            if (cliente == null)
                return NotFound($"Cliente con ID {id} no encontrado");
            return Ok(cliente);
        }

        // POST: /CrearCliente
        [HttpPost("CrearCliente")]
        public async Task<IActionResult> CrearCliente([FromBody] Cliente nuevoCliente)
        {
            try
            {
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
    }
}
