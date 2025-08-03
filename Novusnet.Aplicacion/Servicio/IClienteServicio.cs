using Novusnet.Aplicacion.DTO.DTOS;
using Novusnet.Dominio.Modelo.Abstracciones;
using Novusnet.Infraestructura.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Aplicacion.Servicio
{

    [ServiceContract]
    public interface IClienteServicio
    {


        [OperationContract]
        Task ClienteAddAsync(Cliente entidad); //metdo para insertar

        [OperationContract]
        Task ClienteUpdateAsync(Cliente entidad);

        [OperationContract]
        Task ClienteDeleteAsync(int entidad);

        [OperationContract]
        Task<IEnumerable<Cliente>> ClienteGetAllAsync();

        [OperationContract]
        Task<Cliente> ClienteGetByIdAsync(int id);

        [OperationContract]
        Task<List<Cliente>> ClientesPorEstado(bool activo);



        [OperationContract]
        Task<bool> CambiarEstadoCliente(int id, bool nuevoEstado);

        [OperationContract]
        Task<List<Cliente>> BuscarClientesPorCriterio(string criterio, string busqueda);
    }
}
