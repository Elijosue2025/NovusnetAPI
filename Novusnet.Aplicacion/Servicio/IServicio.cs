using Novusnet.Dominio.Modelo.Abstracciones;
using Novusnet.Infraestructura.AccesoDatos;
using Novusnet.Infraestructura.AccesoDatos.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Aplicacion.Servicio
{
    [ServiceContract]
    public interface IServicio

    {
        // Métodos CRUD básicos
        [OperationContract]
        Task ServicioAddAsync(SServicio entidad);

        [OperationContract]
        Task ServicioUpdateAsync(SServicio entidad);

        [OperationContract]
        Task ServicioDeleteAsync(int entidad);

        [OperationContract]
        Task<IEnumerable<SServicio>> ServicioGetAllAsync();

        [OperationContract]
        Task<SServicio> ServicioGetByIdAsync(int id);

        // Métodos de búsqueda
        [OperationContract]
        Task<List<SServicio>> BuscarServiciosPorCriterio(string criterio, string busqueda);

        // Métodos específicos de negocio
        [OperationContract]
        Task<List<SServicio>> ListarServiciosConMateriales();

        [OperationContract]
        Task<List<SServicio>> ListarServiciosSinMateriales();

        [OperationContract]
        Task<List<SServicio>> ListarServiciosPorCliente(int fk_cliente);

        [OperationContract]
        Task<List<SServicio>> ListarServiciosPorTipoFactura(string tipoFactura);

        [OperationContract]
        Task<bool> CambiarRequiereMaterial(int pk_servicio, bool requiereMaterial);

    }
}
