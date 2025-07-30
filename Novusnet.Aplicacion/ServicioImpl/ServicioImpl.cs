using Novusnet.Aplicacion.Servicio;
using Novusnet.Dominio.Modelo.Abstracciones;
using Novusnet.Infraestructura.AccesoDatos;
using Novusnet.Infraestructura.AccesoDatos.Repositorio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Novusnet.Aplicacion.ServicioImpl
{
    public class ServicioImpl : IServicio
    {
        private readonly IServicioRepositorio _servicioRepositorio;
        private readonly NovusnetPROContext _dBContext;

        public ServicioImpl(NovusnetPROContext dBContext)
        {
            this._dBContext = dBContext;
            _servicioRepositorio = new ServicioRepositorioImpl(_dBContext);
        }

        // MÉTODOS CRUD BÁSICOS
        public async Task ServicioAddAsync(SServicio entidad)
        {
            await _servicioRepositorio.ServicioAddAsync(entidad);
        }

        public async Task ServicioDeleteAsync(int entidad)
        {
            await _servicioRepositorio.ServicioDeleteAsync(entidad);
        }

        public async Task<IEnumerable<SServicio>> ServicioGetAllAsync()
        {
            return await _servicioRepositorio.ServicioGetAllAsync();
        }

        public Task<SServicio> ServicioGetByIdAsync(int id)
        {
            return _servicioRepositorio.ServicioGetByIdAsync(id);
        }

        public async Task ServicioUpdateAsync(SServicio entidad)
        {
            await _servicioRepositorio.ServicioUpdateAsync(entidad);
        }

        // MÉTODOS ESPECÍFICOS DE NEGOCIO
        public Task<List<SServicio>> ListarServiciosConMateriales()
        {
            return _servicioRepositorio.ListarServiciosConMateriales();
        }

        public Task<List<SServicio>> BuscarServiciosPorCriterio(string criterio, string busqueda)
        {
            return _servicioRepositorio.BuscarServiciosPorCriterio(criterio, busqueda);
        }

        public Task<List<SServicio>> ListarServiciosSinMateriales()
        {
            return _servicioRepositorio.ListarServiciosSinMateriales();
        }

        public Task<List<SServicio>> ListarServiciosPorCliente(int fk_cliente)
        {
            return _servicioRepositorio.ListarServiciosPorCliente(fk_cliente);
        }

        public Task<List<SServicio>> ListarServiciosPorTipoFactura(string tipoFactura)
        {
            return _servicioRepositorio.ListarServiciosPorTipoFactura(tipoFactura);
        }

        public Task<bool> CambiarRequiereMaterial(int pk_servicio, bool requiereMaterial)
        {
            return _servicioRepositorio.CambiarRequiereMaterial(pk_servicio, requiereMaterial);
        }
    }
}