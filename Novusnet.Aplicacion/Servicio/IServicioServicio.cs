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
    public interface IServicioServicio
    {
        [OperationContract]
        Task IServicioServicioAddAsync(IServicioServicio entidad); //metdo para insertar
        [OperationContract]
        Task IServicioServicioUpdateAsync(IServicioServicio entidad); //metodo para actualizar 
        [OperationContract]
        Task IServicioServicioDeleteAsync(int entidad);//metdo para eliminar
        [OperationContract]
        Task<IEnumerable<IServicioServicio>> IServicioServicioGetAllAsync(); //metodo lista de todos los registros (select * from)
        [OperationContract]
        Task<IServicioServicio> IServicioServicioGetByIdAsync(int id); //metodo para buscar un registro por id (select * from where id = @id)


    }
}
