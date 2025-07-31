using Novusnet.Aplicacion.DTO.DTOS;
using Novusnet.Infraestructura.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Dominio.Modelo.Abstracciones
{
    public interface IOrdenTrabajoRepositorio : IRepositorio<Orden_Trabajo>
    {
        // MÉTODOS CRUD BÁSICOS
        Task CrearOrdenTrabajoAsync(Orden_Trabajo orden);
        Task<List<Orden_Trabajo>> ObtenerTodasOrdenesAsync();
        Task<Orden_Trabajo> ObtenerOrdenPorIdAsync(int id);
        Task ActualizarOrdenTrabajoAsync(Orden_Trabajo orden);
        Task EliminarOrdenTrabajoAsync(int id);

        // MÉTODOS CON DTO COMPLETO
        Task<List<OrdenTrabajo>> ObtenerOrdenesTrabajoCompletasAsync();
        Task<OrdenTrabajo> ObtenerOrdenCompletaPorIdAsync(int id);
        Task<List<OrdenTrabajo>> BuscarOrdenesTrabajosPorCriterioAsync(string criterio, string busqueda);

        // MÉTODOS AUXILIARES PARA FORMULARIOS
        Task<List<object>> ObtenerEmpleadosParaFormularioAsync();
        Task<List<object>> ObtenerServiciosParaFormularioAsync();

        // MÉTODO DE ESTADÍSTICAS
        Task<Dictionary<string, int>> ObtenerEstadisticasOrdenesAsync();

    }
}
