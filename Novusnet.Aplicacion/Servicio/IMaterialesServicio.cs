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
    public interface IMaterialesServicio
    {

        [OperationContract]

        Task MaterialAddAsync(Material entidad); //metdo para insertar

        [OperationContract]
        Task MaterialUpdateAsync(Material entidad); //metodo para actualizar 

        [OperationContract]
        Task MaterialDeleteAsync(int entidad);//metdo para eliminar

        [OperationContract]
        Task<IEnumerable<Material>> MaterialGetAllAsync(); //metodo lista de todos los registros (select * from)

        [OperationContract]
        Task<Material> MaterialGetByIdAsync(int id); //metodo para buscar un registro por id (select * from where id = @id)

        [OperationContract]

        Task<List<Material>> ListarMaterialStock();


    
     }
}
