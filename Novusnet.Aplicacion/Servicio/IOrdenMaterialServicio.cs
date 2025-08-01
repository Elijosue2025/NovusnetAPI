using Novusnet.Aplicacion.DTO.DTOS;
using Novusnet.Infraestructura.AccesoDatos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Novusnet.Aplicacion.Servicio
{
    public interface IOrdenMaterialServicio
    {
        Task OrdenMaterialAddAsync(Orden_Material entidad);
        Task<List<Orden_Material>> OrdenMaterialGetAllAsync();
        Task<Orden_Material> OrdenMaterialGetByIdAsync(int pk_orden_material);
        Task OrdenMaterialUpdateAsync(Orden_Material entidad);
        Task OrdenMaterialDeleteAsync(int pk_orden_material);
        Task Orden(int pk_orden_material); // puedes reemplazar con un nombre más específico si lo decides

        Task<List<OrdenMaterialDTO>> ObtenerOrdenesTrabajoCompletasConMaterialesAsync();
        Task<List<OrdenMaterialDTO>> FiltrarOrdenesTrabajoPorCriteriosAsync(string criterio, string busqueda);
    }
}
