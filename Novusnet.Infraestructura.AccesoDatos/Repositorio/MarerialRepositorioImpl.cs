using Microsoft.EntityFrameworkCore;
using Novusnet.Dominio.Modelo.Abstracciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Infraestructura.AccesoDatos.Repositorio
{
    public class MaterialRepositorioImpl : RepositorioImpl<Material>, IMaterialRepositorio
    {
        private readonly NovusnetPROContext _novusnetPROContext;

        public MaterialRepositorioImpl(NovusnetPROContext dBContext) : base(dBContext)
        {
            _novusnetPROContext = dBContext;
        }

        // MÉTODOS CRUD BÁSICOS
        public async Task MaterialAddAsync(Material entidad)
        {
            try
            {
                await _novusnetPROContext.Material.AddAsync(entidad);
                await _novusnetPROContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear material", ex);
            }
        }

        public new async Task<List<Material>> MaterialGetAllAsync()
        {
            try
            {
                return await _novusnetPROContext.Material.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar materiales", ex);
            }
        }

        public async Task<Material> MaterialGetByIdAsync(int pk_material)
        {
            try
            {
                return await _novusnetPROContext.Material.FindAsync(pk_material);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener material con ID {pk_material}", ex);
            }
        }

        public async Task MaterialUpdateAsync(Material entidad)
        {
            try
            {
                var materialExistente = await _novusnetPROContext.Material.FindAsync(entidad.pk_material);

                if (materialExistente == null)
                    throw new Exception("Material no encontrado.");

                // Actualizar los campos
                materialExistente.ma_codigo = entidad.ma_codigo;
                materialExistente.ma_nombre = entidad.ma_nombre;
                materialExistente.ma_descripcion = entidad.ma_descripcion;
                materialExistente.ma_precio_unitario = entidad.ma_precio_unitario;
                materialExistente.ma_stock_actual = entidad.ma_stock_actual;
                materialExistente.ma_stock_minimo = entidad.ma_stock_minimo;
                materialExistente.ma_duracion = entidad.ma_duracion;

                await _novusnetPROContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar material", ex);
            }
        }

        public async Task MaterialDeleteAsync(int pk_material)
        {
            try
            {
                Console.WriteLine($"=== REPOSITORIO: Eliminación lógica del material {pk_material} ===");

                var material = await _novusnetPROContext.Material.FindAsync(pk_material);
                if (material != null)
                {
                    Console.WriteLine($"Material encontrado:");
                    Console.WriteLine($"  - ID: {material.pk_material}");
                    Console.WriteLine($"  - Código: {material.ma_codigo}");
                    Console.WriteLine($"  - Nombre: {material.ma_nombre}");
                    Console.WriteLine($"  - Stock actual: {material.ma_stock_actual}");
                    Console.WriteLine($"  - Stock mínimo: {material.ma_stock_minimo}");

                    // ELIMINACIÓN LÓGICA: Cambiar stock actual a 0
                    int stockAnterior = material.ma_stock_actual ?? 0;
                    material.ma_stock_actual = 0;

                    // También puedes cambiar el stock mínimo a 0 si quieres marcarlo como "eliminado"
                    // material.ma_stock_minimo = 0;

                    Console.WriteLine($"Cambiando stock de {stockAnterior} a 0...");

                    // Marcar como modificado y guardar
                    _novusnetPROContext.Material.Update(material);
                    await _novusnetPROContext.SaveChangesAsync();

                    Console.WriteLine($"✅ ELIMINACIÓN LÓGICA EXITOSA:");
                    Console.WriteLine($"  - Material ID: {pk_material}");
                    Console.WriteLine($"  - Stock anterior: {stockAnterior}");
                    Console.WriteLine($"  - Stock actual: {material.ma_stock_actual}");
                    Console.WriteLine($"  - El material sigue existiendo en la BD pero con stock 0");
                }
                else
                {
                    Console.WriteLine($"❌ Material con ID {pk_material} no encontrado en la base de datos");
                    throw new Exception($"Material con ID {pk_material} no encontrado.");
                }
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"❌ Error de base de datos al actualizar material {pk_material}:");
                Console.WriteLine($"   Mensaje: {dbEx.Message}");
                if (dbEx.InnerException != null)
                {
                    Console.WriteLine($"   Error interno: {dbEx.InnerException.Message}");
                }
                throw new Exception("Error de base de datos al eliminar material", dbEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error general al eliminar material {pk_material}:");
                Console.WriteLine($"   Mensaje: {ex.Message}");
                Console.WriteLine($"   Stack trace: {ex.StackTrace}");
                throw new Exception("Error al eliminar material", ex);
            }
        }

        // MÉTODO DE BÚSQUEDA POR CRITERIOS
        public async Task<List<Material>> BuscarMaterialesPorCriterio(string criterio, string busqueda)
        {
            try
            {
                List<Material> listaMateriales = new List<Material>();

                // Convertir criterio a minúsculas para hacer la comparación case-insensitive
                string criterioLower = criterio?.ToLower();

                switch (criterioLower)
                {
                    case "codigo":
                    case "materialcodigo":
                        listaMateriales = await _novusnetPROContext.Material
                            .Where(m => m.ma_codigo != null && m.ma_codigo.Contains(busqueda))
                            .ToListAsync();
                        break;

                    case "nombre":
                    case "nombrematerial":
                        listaMateriales = await _novusnetPROContext.Material
                            .Where(m => m.ma_nombre != null && m.ma_nombre.Contains(busqueda))
                            .ToListAsync();
                        break;

                    case "descripcion":
                        listaMateriales = await _novusnetPROContext.Material
                            .Where(m => m.ma_descripcion != null && m.ma_descripcion.Contains(busqueda))
                            .ToListAsync();
                        break;

                    case "precio":
                    case "preciounitario":
                        // Para búsqueda por precio, intentar convertir la búsqueda a decimal
                        if (decimal.TryParse(busqueda, out decimal precio))
                        {
                            listaMateriales = await _novusnetPROContext.Material
                                .Where(m => m.ma_precio_unitario == precio)
                                .ToListAsync();
                        }
                        break;

                    case "stockactual":
                    case "stock":
                        // Para búsqueda por stock actual
                        if (int.TryParse(busqueda, out int stock))
                        {
                            listaMateriales = await _novusnetPROContext.Material
                                .Where(m => m.ma_stock_actual == stock)
                                .ToListAsync();
                        }
                        break;

                    case "stockminimo":
                    case "minimo":
                        // Para búsqueda por stock mínimo
                        if (int.TryParse(busqueda, out int stockMinimo))
                        {
                            listaMateriales = await _novusnetPROContext.Material
                                .Where(m => m.ma_stock_minimo == stockMinimo)
                                .ToListAsync();
                        }
                        break;

                    case "duracion":
                        listaMateriales = await _novusnetPROContext.Material
                            .Where(m => m.ma_duracion != null && m.ma_duracion.Contains(busqueda))
                            .ToListAsync();
                        break;

                    case "id":
                    case "materialid":
                    case "pkmaterial":
                        // Para búsqueda por ID, intentar convertir la búsqueda a entero
                        if (int.TryParse(busqueda, out int materialId))
                        {
                            listaMateriales = await _novusnetPROContext.Material
                                .Where(m => m.pk_material == materialId)
                                .ToListAsync();
                        }
                        break;

                    default:
                        // Si no se especifica criterio válido, devolver todos los materiales
                        listaMateriales = await _novusnetPROContext.Material.ToListAsync();
                        break;
                }

                return listaMateriales;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener materiales por parámetro: " + ex.Message, ex);
            }
        }

        // MÉTODOS ESPECÍFICOS PARA MATERIAL
        public async Task<List<Material>> ListarMaterialStock()
        {
            try
            {
                var resultado = from tmMaterial in _novusnetPROContext.Material
                                where tmMaterial.ma_stock_actual > 1
                                select tmMaterial;
                return await resultado.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error Listar Materiales con Stock:" + ex.Message);
            }
        }

        public async Task<List<Material>> ListarMaterialesBajoStock()
        {
            try
            {
                return await _novusnetPROContext.Material
                    .Where(m => m.ma_stock_actual <= m.ma_stock_minimo)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar materiales con stock bajo", ex);
            }
        }

        public async Task<bool> ActualizarStock(int pk_material, int nuevoStock)
        {
            try
            {
                var material = await _novusnetPROContext.Material.FindAsync(pk_material);
                if (material != null)
                {
                    material.ma_stock_actual = nuevoStock;
                    await _novusnetPROContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar stock del material", ex);
            }
        }

        public async Task<bool> ReducirStock(int pk_material, int cantidad)
        {
            try
            {
                var material = await _novusnetPROContext.Material.FindAsync(pk_material);
                if (material != null && material.ma_stock_actual >= cantidad)
                {
                    material.ma_stock_actual -= cantidad;
                    await _novusnetPROContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al reducir stock del material", ex);
            }
        }

        public async Task<bool> AumentarStock(int pk_material, int cantidad)
        {
            try
            {
                var material = await _novusnetPROContext.Material.FindAsync(pk_material);
                if (material != null)
                {
                    material.ma_stock_actual += cantidad;
                    await _novusnetPROContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al aumentar stock del material", ex);
            }
        }

        // MÉTODO OBSOLETO - MANTENER POR COMPATIBILIDAD
        public Task ObtenerPorIdAsync(int pk_material)
        {
            // Este método se mantiene para compatibilidad pero se recomienda usar MaterialGetByIdAsync
            return Task.FromResult(MaterialGetByIdAsync(pk_material));
        }
    }
}