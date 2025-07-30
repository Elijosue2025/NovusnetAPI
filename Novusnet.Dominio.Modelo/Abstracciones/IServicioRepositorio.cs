using Novusnet.Aplicacion.DTO.DTOS;
using Novusnet.Dominio.Modelo.Abstracciones;
using Novusnet.Infraestructura.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Novusnet.Dominio.Modelo.Abstracciones
{
    public interface IServicioRepositorio : IRepositorio<SServicio>
    {
        // MÉTODOS CRUD BÁSICOS
        Task ServicioAddAsync(SServicio entidad);
        Task<SServicio> ServicioGetByIdAsync(int pk_servicio);
        Task ServicioUpdateAsync(SServicio entidad);
        Task ServicioDeleteAsync(int pk_servicio);

        // MÉTODOS PARA OBTENER SERVICIOS CON DATOS DE CLIENTE
        Task<List<SServicoDTO>> ObtenerServiciosConDatosClienteAsync();
        Task<List<SServicio>> ListarServiciosConDetallesCliente();

        // MÉTODO DE BÚSQUEDA AVANZADA
        Task<List<SServicoDTO>> BuscarServiciosPorCriterioDTO(string criterio, string busqueda);
    }
}