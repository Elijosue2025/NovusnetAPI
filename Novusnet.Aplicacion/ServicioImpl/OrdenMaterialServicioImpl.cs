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
    public class OrdenMaterialServicioImpl : IOrdenMaterialRepositorio
    {
        private IOrdenMaterialRepositorio _ordenMaterialRepositorio;

        private readonly NovusnetPROContext _dBContext;

        public OrdenMaterialServicioImpl(NovusnetPROContext dBContext)
        {
            this._dBContext = dBContext;
            _ordenMaterialRepositorio = new OrdenMaterialServicioImpl(_dBContext);


        }

        public async Task AddAsync(Orden_Material entidad)
        {
            await _ordenMaterialRepositorio.AddAsync(entidad);
        }

        public async Task UpdateAsync(Orden_Material entidad)
        {
            await _ordenMaterialRepositorio.UpdateAsync(entidad);
        }

        public async Task DeleteAsync(int entidad)
        {
            await _ordenMaterialRepositorio.DeleteAsync(entidad);
        }

        public Task<IEnumerable<Orden_Material>> GetAllAsync()
        {
            return _ordenMaterialRepositorio.GetAllAsync();
        }

        public Task<Orden_Material> GetByIdAsync(int id)
        {
            return _ordenMaterialRepositorio.GetByIdAsync(id);
        }
    }
}
