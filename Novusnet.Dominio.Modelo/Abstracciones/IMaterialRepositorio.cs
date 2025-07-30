using Novusnet.Infraestructura.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Dominio.Modelo.Abstracciones
{
    public interface IMaterialRepositorio : IRepositorio<Material>
    {
        // Métodos CRUD específicos
        Task MaterialAddAsync(Material entidad);
        Task<List<Material>> MaterialGetAllAsync();
        Task<Material> MaterialGetByIdAsync(int pk_material);
        Task MaterialUpdateAsync(Material entidad);
        Task MaterialDeleteAsync(int pk_material);

        // Métodos de búsqueda
        Task<List<Material>> BuscarMaterialesPorCriterio(string criterio, string busqueda);

        // Métodos específicos de negocio
        Task<List<Material>> ListarMaterialStock();
        Task<List<Material>> ListarMaterialesBajoStock();
        Task<bool> ActualizarStock(int pk_material, int nuevoStock);
        Task<bool> ReducirStock(int pk_material, int cantidad);
        Task<bool> AumentarStock(int pk_material, int cantidad);
    }
}
