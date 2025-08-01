using Novusnet.Aplicacion.DTO.DTOS;
using Novusnet.Aplicacion.Servicio;
using Novusnet.Dominio.Modelo.Abstracciones;
using Novusnet.Infraestructura.AccesoDatos;
using Novusnet.Infraestructura.AccesoDatos.Repositorio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Novusnet.Aplicacion.ServicioImpl
{
    public class OrdenMaterialServicioImpl : IOrdenMaterialServicio
    {
        private readonly NovusnetPROContext _dBContext;
        private readonly IOrdenMaterialRepositorio _ordenMaterialRepositorio;

        public OrdenMaterialServicioImpl(NovusnetPROContext dBContext)
        {
            _dBContext = dBContext;
            _ordenMaterialRepositorio = new OrdenMaterialRepositorioImpl(_dBContext);
        }

        public async Task<List<OrdenMaterialDTO>> FiltrarOrdenesTrabajoPorCriteriosAsync(string criterio, string busqueda)
        {
            return await _ordenMaterialRepositorio.FiltrarOrdenesTrabajoPorCriteriosAsync(criterio, busqueda);
        }

        public async Task<List<OrdenMaterialDTO>> ObtenerOrdenesTrabajoCompletasConMaterialesAsync()
        {
            return await _ordenMaterialRepositorio.ObtenerOrdenesTrabajoCompletasConMaterialesAsync();
        }

        public async Task Orden(int pk_orden_material)
        {
            // Método reservado si deseas extender funcionalidad adicional.
            await Task.CompletedTask;
        }

        public async Task OrdenMaterialAddAsync(Orden_Material entidad)
        {
            await _ordenMaterialRepositorio.OrdenMaterialAddAsync(entidad);
        }

        public async Task OrdenMaterialDeleteAsync(int pk_orden_material)
        {
            await _ordenMaterialRepositorio.OrdenMaterialDeleteAsync(pk_orden_material);
        }

        public async Task<List<Orden_Material>> OrdenMaterialGetAllAsync()
        {
            return await _ordenMaterialRepositorio.OrdenMaterialGetAllAsync();
        }

        public async Task<Orden_Material> OrdenMaterialGetByIdAsync(int pk_orden_material)
        {
            return await _ordenMaterialRepositorio.OrdenMaterialGetByIdAsync(pk_orden_material);
        }

        public async Task OrdenMaterialUpdateAsync(Orden_Material entidad)
        {
            await _ordenMaterialRepositorio.OrdenMaterialUpdateAsync(entidad);
        }
    }
}

