using Microsoft.AspNetCore.Mvc;
using Novusnet.Aplicacion.Servicio;
using Novusnet.Aplicacion.DTO.DTOS;
using Novusnet.Infraestructura.AccesoDatos;

namespace NovusnetAPIWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialController : ControllerBase
    {
        private IMaterialesServicio _materialServicio;

        public MaterialController(IMaterialesServicio materialServicio)
        {
            _materialServicio = materialServicio;
        }

        [HttpGet("ListaMateriales")]
        public Task<IEnumerable<Material>> ListarMateriales()
        {
            return _materialServicio.MaterialGetAllAsync();
        }

        [HttpPost("CrearMaterial")]
        public async Task<IActionResult> MaterialAddAsync([FromBody] Material nuevoMaterial)
        {
            try
            {
                await _materialServicio.MaterialAddAsync(nuevoMaterial);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Error Interno");
            }
        }

        [HttpDelete("EliminarMaterial/{pk_material}")]
        public async Task<IActionResult> EliminarMaterial(int pk_material)
        {
            try
            {
                Console.WriteLine($"=== API: Eliminación lógica del material {pk_material} ===");

                if (pk_material <= 0)
                {
                    Console.WriteLine($"❌ Error: ID inválido - {pk_material}");
                    return BadRequest(new
                    {
                        error = "ID de material inválido",
                        pk_material = pk_material
                    });
                }

                // Verificar que el material existe
                var materialExistente = await _materialServicio.MaterialGetByIdAsync(pk_material);
                if (materialExistente == null)
                {
                    Console.WriteLine($"❌ Error: Material {pk_material} no encontrado");
                    return NotFound(new
                    {
                        error = $"Material con ID {pk_material} no encontrado"
                    });
                }

                Console.WriteLine($"✅ Material encontrado:");
                Console.WriteLine($"   - Código: {materialExistente.ma_codigo}");
                Console.WriteLine($"   - Nombre: {materialExistente.ma_nombre}");
                Console.WriteLine($"   - Stock actual: {materialExistente.ma_stock_actual}");

                // Verificar si ya está eliminado lógicamente
                if (materialExistente.ma_stock_actual == 0)
                {
                    Console.WriteLine($"⚠️ Advertencia: Material {pk_material} ya está eliminado (stock = 0)");
                    return Ok(new
                    {
                        message = "Material ya estaba eliminado",
                        id = pk_material,
                        warning = "El material ya tenía stock 0",
                        action = "no_action_needed"
                    });
                }

                // Guardar stock anterior para el response
                int stockAnterior = materialExistente.ma_stock_actual ?? 0;

                // Proceder con la eliminación lógica
                Console.WriteLine($"🔄 Procediendo con eliminación lógica (stock {stockAnterior} → 0)...");
                await _materialServicio.MaterialDeleteAsync(pk_material);

                Console.WriteLine($"✅ Material {pk_material} eliminado exitosamente");

                return Ok(new
                {
                    message = "Material eliminado exitosamente",
                    id = pk_material,
                    codigo = materialExistente.ma_codigo,
                    nombre = materialExistente.ma_nombre,
                    action = "eliminacion_logica",
                    stock_anterior = stockAnterior,
                    stock_actual = 0,
                    nota = "El material sigue existiendo en la base de datos pero con stock 0"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al eliminar material {pk_material}:");
                Console.WriteLine($"   Mensaje: {ex.Message}");
                Console.WriteLine($"   Stack trace: {ex.StackTrace}");

                return StatusCode(500, new
                {
                    error = "Error al eliminar material",
                    details = ex.Message,
                    pk_material = pk_material,
                    timestamp = DateTime.Now
                });
            }
        }

        [HttpPut("ActualizarMaterial/{id}")]
        public async Task<IActionResult> ActualizarMaterial(int id, [FromBody] Material material)
        {
            try
            {
                if (id != material.pk_material)
                    return BadRequest("El ID del path no coincide con el del cuerpo.");

                await _materialServicio.MaterialUpdateAsync(material);
                return Ok(new { mensaje = "Material actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al actualizar material", details = ex.Message });
            }
        }

        // NUEVOS MÉTODOS ADICIONALES

        [HttpGet("BuscarPorCriterio")]
        public async Task<IActionResult> BuscarMaterialesPorCriterio([FromQuery] string criterio, [FromQuery] string busqueda)
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
                var materiales = await _materialServicio.BuscarMaterialesPorCriterio(criterio, busqueda);

                // Verificar si se encontraron resultados
                if (materiales == null || !materiales.Any())
                {
                    return Ok(new
                    {
                        message = "No se encontraron materiales con los criterios especificados",
                        data = new List<object>(),
                        count = 0
                    });
                }

                // Retornar resultados exitosos
                return Ok(new
                {
                    message = "Búsqueda realizada exitosamente",
                    data = materiales,
                    count = materiales.Count
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
                Console.WriteLine($"Error al buscar materiales: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");

                return StatusCode(500, new
                {
                    error = "Error interno del servidor al buscar materiales",
                    details = ex.Message
                });
            }
        }

        [HttpGet("ObtenerMaterial/{id}")]
        public async Task<IActionResult> GetMaterial(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("ID de material inválido");
                }

                var material = await _materialServicio.MaterialGetByIdAsync(id);

                if (material == null)
                {
                    return NotFound($"Material con ID {id} no encontrado");
                }

                return Ok(material);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { error = "Error al obtener material", details = ex.Message });
            }
        }

        // MÉTODOS ESPECÍFICOS DE GESTIÓN DE INVENTARIO

        [HttpGet("ListaMaterialesConStock")]
        public async Task<IActionResult> ListarMaterialesConStock()
        {
            try
            {
                var materiales = await _materialServicio.ListarMaterialStock();

                if (materiales == null || !materiales.Any())
                {
                    return Ok(new
                    {
                        message = "No se encontraron materiales con stock disponible",
                        data = new List<object>(),
                        count = 0
                    });
                }

                return Ok(new
                {
                    message = "Materiales con stock obtenidos exitosamente",
                    data = materiales,
                    count = materiales.Count
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { error = "Error al obtener materiales con stock", details = ex.Message });
            }
        }

        [HttpGet("ListaMaterialesBajoStock")]
        public async Task<IActionResult> ListarMaterialesBajoStock()
        {
            try
            {
                var materiales = await _materialServicio.ListarMaterialesBajoStock();

                if (materiales == null || !materiales.Any())
                {
                    return Ok(new
                    {
                        message = "No se encontraron materiales con stock bajo",
                        data = new List<object>(),
                        count = 0
                    });
                }

                return Ok(new
                {
                    message = "Materiales con stock bajo obtenidos exitosamente",
                    data = materiales,
                    count = materiales.Count
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { error = "Error al obtener materiales con stock bajo", details = ex.Message });
            }
        }

        [HttpPatch("ActualizarStock/{pk_material}")]
        public async Task<IActionResult> ActualizarStock(int pk_material, [FromBody] int nuevoStock)
        {
            try
            {
                if (pk_material <= 0)
                {
                    return BadRequest("ID de material inválido");
                }

                if (nuevoStock < 0)
                {
                    return BadRequest("El stock no puede ser negativo");
                }

                var resultado = await _materialServicio.ActualizarStock(pk_material, nuevoStock);

                if (!resultado)
                {
                    return NotFound($"Material con ID {pk_material} no encontrado");
                }

                return Ok(new
                {
                    mensaje = "Stock actualizado correctamente",
                    materialId = pk_material,
                    nuevoStock = nuevoStock
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { error = "Error al actualizar stock", details = ex.Message });
            }
        }

        [HttpPatch("ReducirStock/{pk_material}")]
        public async Task<IActionResult> ReducirStock(int pk_material, [FromBody] int cantidad)
        {
            try
            {
                if (pk_material <= 0)
                {
                    return BadRequest("ID de material inválido");
                }

                if (cantidad <= 0)
                {
                    return BadRequest("La cantidad debe ser mayor a 0");
                }

                var resultado = await _materialServicio.ReducirStock(pk_material, cantidad);

                if (!resultado)
                {
                    return BadRequest($"No se pudo reducir el stock. Verifique que el material existe y tiene stock suficiente");
                }

                return Ok(new
                {
                    mensaje = "Stock reducido correctamente",
                    materialId = pk_material,
                    cantidadReducida = cantidad
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { error = "Error al reducir stock", details = ex.Message });
            }
        }

        [HttpPatch("AumentarStock/{pk_material}")]
        public async Task<IActionResult> AumentarStock(int pk_material, [FromBody] int cantidad)
        {
            try
            {
                if (pk_material <= 0)
                {
                    return BadRequest("ID de material inválido");
                }

                if (cantidad <= 0)
                {
                    return BadRequest("La cantidad debe ser mayor a 0");
                }

                var resultado = await _materialServicio.AumentarStock(pk_material, cantidad);

                if (!resultado)
                {
                    return NotFound($"Material con ID {pk_material} no encontrado");
                }

                return Ok(new
                {
                    mensaje = "Stock aumentado correctamente",
                    materialId = pk_material,
                    cantidadAumentada = cantidad
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { error = "Error al aumentar stock", details = ex.Message });
            }
        }
    }
}