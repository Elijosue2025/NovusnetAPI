using Novusnet.Infraestructura.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Aplicacion.Servicio
{
    public interface IEmpleadoServicio
    {
        [OperationContract]

        Task EmpleadoAddAsync(Empleado entidad); //metdo para insertar

        [OperationContract]
        Task EmpleadoUpdateAsync(Cliente entidad); //metodo para actualizar 

        [OperationContract]
        Task EmpleadoDeleteAsync(int entidad);//metdo para eliminar

        [OperationContract]
        Task<IEnumerable<Cliente>> EmpleadoGetAllAsync(); //metodo lista de todos los registros (select * from)

        [OperationContract]
        Task<Cliente> EmpleadoGetByIdAsync(int id); //metodo para buscar un registro por id (select * from where id = @id)

        [OperationContract]

        Task<List<Cliente>> ListarEmpleadoRoll();
    }
}
