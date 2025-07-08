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
        [OperationContract]

        Task ServicioAddAsync(SServicio entidad); //metdo para insertar

        [OperationContract]
        Task ServicioUpdateAsync(SServicio entidad); //metodo para actualizar 

        [OperationContract]
        Task ServicioDeleteAsync(int entidad);//metdo para eliminar

        [OperationContract]
        Task<IEnumerable<SServicio>> ServicioGetAllAsync(); //metodo lista de todos los registros (select * from)

        [OperationContract]
        Task<SServicio> ServicioGetByIdAsync(int id); //metodo para buscar un registro por id (select * from where id = @id)

        [OperationContract]

        Task<List<SServicio>> ListarServicioActivos();
    }
}
