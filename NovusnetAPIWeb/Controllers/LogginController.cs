using Microsoft.AspNetCore.Mvc;
using Novusnet.Aplicacion.Servicio;
using Novusnet.Infraestructura.AccesoDatos;
using System.Threading.Tasks;

namespace NovusnetAPIWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogginController : ControllerBase
    {
        private readonly ILogingServicio _logginServicio;

        public LogginController(ILogingServicio logginServicio)
        {
            _logginServicio = logginServicio;
        }

        // GET: api/loggin
        [HttpGet("ListaTodos")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _logginServicio.ObtenerTodosAsync();
            return Ok(result);
        }

        // GET: api/loggin/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _logginServicio.ObtenerPorIdAsync(id);
            if (result == null)
                return NotFound($"No se encontró el registro con ID {id}");
            return Ok(result);
        }

        // POST: api/loggin
        [HttpPost("Crear Usuario")]
        public async Task<IActionResult> Create([FromBody] Logging model)
        {
            if (model == null)
                return BadRequest("Datos inválidos.");

            var id = await _logginServicio.CrearLogginAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = id }, model);
        }

        // PUT: api/loggin/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Logging model)
        {
            if (model == null || model.pk_logging != id)
                return BadRequest("Datos inconsistentes.");

            try
            {
                await _logginServicio.ActualizarAsync(model);
                return Ok("Actualización exitosa.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/loggin/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _logginServicio.EliminarAsync(id);
                return Ok("Eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: api/loggin/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Usuario) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Usuario y contraseña son requeridos.");

            bool isValid = await _logginServicio.ValidarLoginAsync(request.Usuario, request.Password);

            if (!isValid)
                return Unauthorized("Credenciales incorrectas.");

            return Ok("Login exitoso.");
        }
    }

    // Clase auxiliar para la petición de login
    public class LoginRequest
    {
        public string Usuario { get; set; }
        public string Password { get; set; }
    }
}

