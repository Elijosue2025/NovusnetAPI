using Novusnet.Aplicacion.Servicio;
using Novusnet.Dominio.Modelo.Abstracciones;
using Novusnet.Infraestructura.AccesoDatos;
using Novusnet.Infraestructura.AccesoDatos.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Aplicacion.ServicioImpl
{
    public class MaterialesServicioImpl : IMaterialesServicio
    {
        private readonly NovusnetPROContext _dBContext;


        private IMaterialRepositorio _materialRepositorio;


        public MaterialesServicioImpl(NovusnetPROContext novusnetPROContext)
        {
            this._dBContext = novusnetPROContext;
            _materialRepositorio = new MarerialRepositorioImpl(novusnetPROContext);


        }

        public async Task MaterialAddAsync(Material entidad)
        {
            await _materialRepositorio.AddAsync(entidad);
        }

        public async Task MaterialUpdateAsync(Material entidad)
        {
            await _materialRepositorio.UpdateAsync(entidad);
        }

        public async Task MaterialDeleteAsync(int entidad)
        {
            await _materialRepositorio.DeleteAsync(entidad);
        }

        public Task<IEnumerable<Material>> MaterialGetAllAsync()
        {

            return _materialRepositorio.GetAllAsync();
        }

        public Task<Material> MaterialGetByIdAsync(int id)
        {
            return _materialRepositorio.GetByIdAsync(id);
        }

        public Task<List<Material>> ListarMaterialStock()
        {
            // return _clienteRepositorio.ListarClientesActivos();
            return _materialRepositorio.ListarMaterialStock();
        }
    }
}

