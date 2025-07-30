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
    public interface IMaterialesServicio
    {


        // Métodos CRUD básicos
        [OperationContract]
        Task MaterialAddAsync(Material entidad);

        [OperationContract]
        Task MaterialUpdateAsync(Material entidad);

        [OperationContract]
        Task MaterialDeleteAsync(int entidad);

        [OperationContract]
        Task<IEnumerable<Material>> MaterialGetAllAsync();

        [OperationContract]
        Task<Material> MaterialGetByIdAsync(int id);

        // Métodos de búsqueda
        [OperationContract]
        Task<List<Material>> BuscarMaterialesPorCriterio(string criterio, string busqueda);

        // Métodos específicos de negocio
        [OperationContract]
        Task<List<Material>> ListarMaterialStock();

        [OperationContract]
        Task<List<Material>> ListarMaterialesBajoStock();

        [OperationContract]
        Task<bool> ActualizarStock(int pk_material, int nuevoStock);

        [OperationContract]
        Task<bool> ReducirStock(int pk_material, int cantidad);

        [OperationContract]
        Task<bool> AumentarStock(int pk_material, int cantidad);



    }
}
