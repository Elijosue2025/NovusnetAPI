using Novusnet.Aplicacion.DTO.DTOS;
using Novusnet.Infraestructura.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Dominio.Modelo.Abstracciones
{
    public interface IOrdenMaterialRepositorio : IRepositorio<Orden_Material>
    {
        public Task OrdenMaterialAddAsync(Orden_Material entidad);
        public Task<List<Orden_Material>> OrdenMaterialGetAllAsync();
        public Task<Orden_Material> OrdenMaterialGetByIdAsync(int pk_orden_material);
        public Task OrdenMaterialUpdateAsync(Orden_Material entidad);
        public Task OrdenMaterialDeleteAsync(int pk_orden_material);
        public  Task<List<OrdenMaterialDTO>> ObtenerOrdenesTrabajoCompletasConMaterialesAsync();
        // public  Task OrdenMaterialDeleteAsync(int pk_orden_material);
        public  Task<List<OrdenMaterialDTO>> FiltrarOrdenesTrabajoPorCriteriosAsync(string criterio, string valor);






    }
}
