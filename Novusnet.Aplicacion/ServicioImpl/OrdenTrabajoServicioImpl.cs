using Novusnet.Aplicacion.DTO.DTOS;
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
    public class OrdenTrabajoServicioImpl : IOrdenTrabajoServicio
    {
        private IOrdenTrabajoRepositorio _ordenTrabajoRepositorio;
        private readonly NovusnetPROContext _dBContext;

        public OrdenTrabajoServicioImpl(NovusnetPROContext dBContext)
        {
            this._dBContext = dBContext;
            _ordenTrabajoRepositorio = new OrdenTrabajoRepositorioImpl(_dBContext);
        }

        // ===============================
        // MÉTODOS CRUD BÁSICOS (ORIGINALES)
        // ===============================

        public async Task OrdenTrabajoAddAsync(Orden_Trabajo entidad)
        {
            await _ordenTrabajoRepositorio.AddAsync(entidad);
        }

        public async Task OrdenTrabajoDeleteAsync(int entidad)
        {
            await _ordenTrabajoRepositorio.DeleteAsync(entidad);
        }

        public async Task<IEnumerable<Orden_Trabajo>> OrdenTrabajoGetAllAsync()
        {
            return await _ordenTrabajoRepositorio.GetAllAsync();
        }

        public async Task<Orden_Trabajo> OrdenTrabajoGetByIdAsync(int id)
        {
            return await _ordenTrabajoRepositorio.GetByIdAsync(id);
        }

        public async Task OrdenTrabajoUpdateAsync(Orden_Trabajo entidad)
        {
            await _ordenTrabajoRepositorio.UpdateAsync(entidad);
        }

        // ===============================
        // MÉTODOS ESPECÍFICOS MEJORADOS
        // ===============================

        public async Task CrearOrdenTrabajoAsync(Orden_Trabajo nuevaOrden)
        {
            try
            {
                await _ordenTrabajoRepositorio.CrearOrdenTrabajoAsync(nuevaOrden);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en servicio al crear orden de trabajo", ex);
            }
        }

        public async Task<List<Orden_Trabajo>> ObtenerTodasOrdenesAsync()
        {
            try
            {
                return await _ordenTrabajoRepositorio.ObtenerTodasOrdenesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en servicio al obtener todas las órdenes", ex);
            }
        }

        public async Task<Orden_Trabajo> ObtenerOrdenPorIdAsync(int id)
        {
            try
            {
                return await _ordenTrabajoRepositorio.ObtenerOrdenPorIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al obtener orden ID {id}", ex);
            }
        }

        public async Task ActualizarOrdenTrabajoAsync(Orden_Trabajo orden)
        {
            try
            {
                await _ordenTrabajoRepositorio.ActualizarOrdenTrabajoAsync(orden);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en servicio al actualizar orden", ex);
            }
        }

        public async Task EliminarOrdenTrabajoAsync(int id)
        {
            try
            {
                await _ordenTrabajoRepositorio.EliminarOrdenTrabajoAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en servicio al cancelar orden", ex);
            }
        }

        // ===============================
        // MÉTODOS CON DTO COMPLETO
        // ===============================

        public async Task<List<OrdenTrabajo>> ObtenerOrdenesTrabajoCompletasAsync()
        {
            try
            {
                return await _ordenTrabajoRepositorio.ObtenerOrdenesTrabajoCompletasAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en servicio al obtener órdenes completas", ex);
            }
        }

        public async Task<OrdenTrabajo> ObtenerOrdenCompletaPorIdAsync(int id)
        {
            try
            {
                return await _ordenTrabajoRepositorio.ObtenerOrdenCompletaPorIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en servicio al obtener orden completa ID {id}", ex);
            }
        }

        public async Task<List<OrdenTrabajo>> BuscarOrdenesTrabajosPorCriterioAsync(string criterio, string busqueda)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(criterio) || string.IsNullOrWhiteSpace(busqueda))
                {
                    throw new ArgumentException("El criterio y la búsqueda son requeridos");
                }

                return await _ordenTrabajoRepositorio.BuscarOrdenesTrabajosPorCriterioAsync(criterio, busqueda);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en servicio al buscar órdenes por criterio", ex);
            }
        }

        // ===============================
        // MÉTODOS AUXILIARES PARA FORMULARIOS
        // ===============================

        public async Task<List<object>> ObtenerEmpleadosParaFormularioAsync()
        {
            try
            {
                return await _ordenTrabajoRepositorio.ObtenerEmpleadosParaFormularioAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en servicio al obtener empleados para formulario", ex);
            }
        }

        public async Task<List<object>> ObtenerServiciosParaFormularioAsync()
        {
            try
            {
                return await _ordenTrabajoRepositorio.ObtenerServiciosParaFormularioAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en servicio al obtener servicios para formulario", ex);
            }
        }

        // ===============================
        // MÉTODO DE ESTADÍSTICAS
        // ===============================

        public async Task<Dictionary<string, int>> ObtenerEstadisticasOrdenesAsync()
        {
            try
            {
                return await _ordenTrabajoRepositorio.ObtenerEstadisticasOrdenesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en servicio al obtener estadísticas", ex);
            }
        }
    }
}