using Novusnet.Infraestructura.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Dominio.Modelo.Abstracciones
{
    public interface IServicioRepositorio : IRepositorio<SServicio>
    {
        // Métodos CRUD específicos
        Task ServicioAddAsync(SServicio entidad);
        Task<List<SServicio>> ServicioGetAllAsync();
        Task<SServicio> ServicioGetByIdAsync(int pk_servicio);
        Task ServicioUpdateAsync(SServicio entidad);
        Task ServicioDeleteAsync(int pk_servicio);

        // Métodos de búsqueda
        Task<List<SServicio>> BuscarServiciosPorCriterio(string criterio, string busqueda);

        // Métodos específicos de negocio
        Task<List<SServicio>> ListarServiciosConMateriales();
        Task<List<SServicio>> ListarServiciosSinMateriales();
        Task<List<SServicio>> ListarServiciosPorCliente(int fk_cliente);
        Task<List<SServicio>> ListarServiciosPorTipoFactura(string tipoFactura);
        Task<bool> CambiarRequiereMaterial(int pk_servicio, bool requiereMaterial);
    }
}
