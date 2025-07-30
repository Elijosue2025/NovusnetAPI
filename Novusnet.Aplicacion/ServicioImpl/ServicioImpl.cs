using Novusnet.Aplicacion.DTO.DTOS;
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

        public async Task ServicioUpdateAsync(SServicio entidad)
        {
            await _servicioRepositorio.ServicioUpdateAsync(entidad);
        }

        public async Task ServicioDeleteAsync(int pk_servicio)
        {
            await _servicioRepositorio.ServicioDeleteAsync(pk_servicio);
        }

        public async Task<SServicio> ServicioGetByIdAsync(int pk_servicio)
        {
            return await _servicioRepositorio.ServicioGetByIdAsync(pk_servicio);
        }

        // MÉTODOS PARA OBTENER SERVICIOS CON DATOS DE CLIENTE
        public async Task<List<SServicoDTO>> ObtenerServiciosConDatosClienteAsync()
        {
            return await _servicioRepositorio.ObtenerServiciosConDatosClienteAsync();
        }

        public async Task<List<SServicio>> ListarServiciosConDetallesCliente()
        {
            return await _servicioRepositorio.ListarServiciosConDetallesCliente();
        }

        // MÉTODO DE BÚSQUEDA AVANZADA
        public async Task<List<SServicoDTO>> BuscarServiciosPorCriterioDTO(string criterio, string busqueda)
        {
            return await _servicioRepositorio.BuscarServiciosPorCriterioDTO(criterio, busqueda);
        }
    }
}