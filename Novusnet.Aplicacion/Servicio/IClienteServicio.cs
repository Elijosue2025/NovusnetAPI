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
        Task ClienteUpdateAsync(Cliente entidad); //metodo para actualizar 
        [OperationContract]
        Task ClienteDeleteAsync(int entidad);//metdo para eliminar
        [OperationContract]
        Task<IEnumerable<Cliente>> ClienteGetAllAsync(); //metodo lista de todos los registros (select * from)
        [OperationContract]
        Task<Cliente> ClienteGetByIdAsync(int id); //metodo para buscar un registro por id (select * from where id = @id)

    }
}
