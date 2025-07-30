using Novusnet.Aplicacion.Servicio;
using Novusnet.Dominio.Modelo.Abstracciones;
using Novusnet.Infraestructura.AccesoDatos;
using Novusnet.Infraestructura.AccesoDatos.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Aplicacion.ServicioImpl
{
    public class MaterialesServicioImpl : IMaterialesServicio
    {
        private readonly NovusnetPROContext _dBContext;
        private IMaterialRepositorio _materialRepositorio;

        public MaterialesServicioImpl(NovusnetPROContext novusnetPROContext)
        {
            this._dBContext = novusnetPROContext;
            _materialRepositorio = new MaterialRepositorioImpl(novusnetPROContext); // Corregido nombre de clase
        }

        // Métodos CRUD básicos
        public async Task MaterialAddAsync(Material entidad)
        {
            try
            {
                await _materialRepositorio.MaterialAddAsync(entidad);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en servicio al crear material", ex);
            }
        }

        public async Task MaterialUpdateAsync(Material entidad)
        {
            try
            {
                await _materialRepositorio.MaterialUpdateAsync(entidad);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en servicio al actualizar material", ex);
            }
        }

        public async Task MaterialDeleteAsync(int entidad)
        {
            try
            {
                await _materialRepositorio.MaterialDeleteAsync(entidad);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en servicio al eliminar material", ex);
            }
        }

        public async Task<IEnumerable<Material>> MaterialGetAllAsync()
        {
            try
            {
                var materiales = await _materialRepositorio.MaterialGetAllAsync();
                return materiales;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en servicio al obtener todos los materiales", ex);
            }
        }

        public async Task<Material> MaterialGetByIdAsync(int id)
        {
            try
            {
                return await _materialRepositorio.MaterialGetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al obtener material con ID {id}", ex);
            }
        }

        // Métodos de búsqueda
        public async Task<List<Material>> BuscarMaterialesPorCriterio(string criterio, string busqueda)
        {
            try
            {
                return await _materialRepositorio.BuscarMaterialesPorCriterio(criterio, busqueda);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en servicio al buscar materiales por criterio", ex);
            }
        }

        // Métodos específicos de negocio
        public async Task<List<Material>> ListarMaterialStock()
        {
            try
            {
                return await _materialRepositorio.ListarMaterialStock();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en servicio al listar materiales con stock", ex);
            }
        }

        public async Task<List<Material>> ListarMaterialesBajoStock()
        {
            try
            {
                return await _materialRepositorio.ListarMaterialesBajoStock();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en servicio al listar materiales con stock bajo", ex);
            }
        }

        public async Task<bool> ActualizarStock(int pk_material, int nuevoStock)
        {
            try
            {
                return await _materialRepositorio.ActualizarStock(pk_material, nuevoStock);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en servicio al actualizar stock", ex);
            }
        }

        public async Task<bool> ReducirStock(int pk_material, int cantidad)
        {
            try
            {
                return await _materialRepositorio.ReducirStock(pk_material, cantidad);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en servicio al reducir stock", ex);
            }
        }

        public async Task<bool> AumentarStock(int pk_material, int cantidad)
        {
            try
            {
                return await _materialRepositorio.AumentarStock(pk_material, cantidad);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en servicio al aumentar stock", ex);
            }
        }

        // Métodos alternativos usando la interfaz base (para compatibilidad)
        public async Task MaterialAddAsyncBase(Material entidad)
        {
            await _materialRepositorio.AddAsync(entidad);
        }

        public async Task MaterialUpdateAsyncBase(Material entidad)
        {
            await _materialRepositorio.UpdateAsync(entidad);
        }

        public async Task MaterialDeleteAsyncBase(int entidad)
        {
            await _materialRepositorio.DeleteAsync(entidad);
        }

        public Task<IEnumerable<Material>> MaterialGetAllAsyncBase()
        {
            return _materialRepositorio.GetAllAsync();
        }

        public Task<Material> MaterialGetByIdAsyncBase(int id)
        {
            return _materialRepositorio.GetByIdAsync(id);
        }
    }
}
