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
    public interface IOrdenTrabajoServicio
    {
        [OperationContract]
        Task OrdenTrabajoAddAsync(Orden_Trabajo entidad); //metdo para insertar
        [OperationContract]
        Task OrdenTrabajoUpdateAsync(Orden_Trabajo entidad); //metodo para actualizar 
        [OperationContract]
        Task OrdenTrabajoDeleteAsync(int entidad);//metdo para eliminar
        [OperationContract]
        Task<IEnumerable<Orden_Trabajo>> OrdenTrabajoGetAllAsync(); //metodo lista de todos los registros (select * from)
        [OperationContract]
        Task<Orden_Trabajo> OrdenTrabajoGetByIdAsync(int id); //metodo para buscar un registro por id (select * from where id = @id)

    }
}
