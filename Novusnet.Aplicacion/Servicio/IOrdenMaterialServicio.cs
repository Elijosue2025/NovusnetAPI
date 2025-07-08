using Novusnet.Infraestructura.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Aplicacion.Servicio
{
    public interface IOrdenMaterialServicio
    {

        [OperationContract]

        Task OrdenMaterialAddAsync(Orden_Material entidad); //metdo para insertar

        [OperationContract]
        Task OrdenMaterialUpdateAsync(Orden_Material entidad); //metodo para actualizar 

        [OperationContract]
        Task OrdenMaterialDeleteAsync(int entidad);//metdo para eliminar

        [OperationContract]
        Task<IEnumerable<Orden_Material>> OrdenMaterialGetAllAsync(); //metodo lista de todos los registros (select * from)

        [OperationContract]
        Task<Orden_Material> OrdenMaterialGetByIdAsync(int id); //metodo para buscar un registro por id (select * from where id = @id)

        [OperationContract]

        Task<List<Orden_Material>> ListarOrdenMaterialActivos();
    }
}
