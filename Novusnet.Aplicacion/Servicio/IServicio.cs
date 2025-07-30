using Novusnet.Aplicacion.DTO.DTOS;
using Novusnet.Dominio.Modelo.Abstracciones;
using Novusnet.Infraestructura.AccesoDatos;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Novusnet.Aplicacion.Servicio
{
    [ServiceContract]
    public interface IServicio
    {
        // MÉTODOS CRUD BÁSICOS
        [OperationContract]
        Task ServicioAddAsync(SServicio entidad);

        [OperationContract]
        Task ServicioUpdateAsync(SServicio entidad);

        [OperationContract]
        Task ServicioDeleteAsync(int pk_servicio);

        [OperationContract]
        Task<SServicio> ServicioGetByIdAsync(int pk_servicio);

        // MÉTODOS PARA OBTENER SERVICIOS CON DATOS DE CLIENTE
        [OperationContract]
        Task<List<SServicoDTO>> ObtenerServiciosConDatosClienteAsync();

        [OperationContract]
        Task<List<SServicio>> ListarServiciosConDetallesCliente();

        // MÉTODO DE BÚSQUEDA AVANZADA
        [OperationContract]
        Task<List<SServicoDTO>> BuscarServiciosPorCriterioDTO(string criterio, string busqueda);
    }
}