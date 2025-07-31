using Novusnet.Aplicacion.DTO.DTOS;
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
        // ===============================
        // MÉTODOS CRUD BÁSICOS (ORIGINALES)
        // ===============================
        // MÉTODOS CRUD BÁSICOS (ORIGINALES)
        [OperationContract]
        Task OrdenTrabajoAddAsync(Orden_Trabajo entidad);
        [OperationContract]
        Task OrdenTrabajoUpdateAsync(Orden_Trabajo entidad);
        [OperationContract]
        Task OrdenTrabajoDeleteAsync(int entidad);
        [OperationContract]
        Task<IEnumerable<Orden_Trabajo>> OrdenTrabajoGetAllAsync();
        [OperationContract]
        Task<Orden_Trabajo> OrdenTrabajoGetByIdAsync(int id);

        // MÉTODOS ESPECÍFICOS MEJORADOS
        [OperationContract]
        Task CrearOrdenTrabajoAsync(Orden_Trabajo nuevaOrden);
        [OperationContract]
        Task<List<Orden_Trabajo>> ObtenerTodasOrdenesAsync();
        [OperationContract]
        Task<Orden_Trabajo> ObtenerOrdenPorIdAsync(int id);
        [OperationContract]
        Task ActualizarOrdenTrabajoAsync(Orden_Trabajo orden);
        [OperationContract]
        Task EliminarOrdenTrabajoAsync(int id);

        // MÉTODOS CON DTO COMPLETO
        [OperationContract]
        Task<List<OrdenTrabajo>> ObtenerOrdenesTrabajoCompletasAsync();
        [OperationContract]
        Task<OrdenTrabajo> ObtenerOrdenCompletaPorIdAsync(int id);
        [OperationContract]
        Task<List<OrdenTrabajo>> BuscarOrdenesTrabajosPorCriterioAsync(string criterio, string busqueda);

        // MÉTODOS AUXILIARES PARA FORMULARIOS
        [OperationContract]
        Task<List<object>> ObtenerEmpleadosParaFormularioAsync();
        [OperationContract]
        Task<List<object>> ObtenerServiciosParaFormularioAsync();

        // MÉTODO DE ESTADÍSTICAS
        [OperationContract]
        Task<Dictionary<string, int>> ObtenerEstadisticasOrdenesAsync();
    }
}
