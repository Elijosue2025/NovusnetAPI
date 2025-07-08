using Novusnet.Aplicacion.Servicio;
using Novusnet.Dominio.Modelo.Abstracciones;
using Novusnet.Infraestructura.AccesoDatos;
using Novusnet.Infraestructura.AccesoDatos.Repositorio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Novusnet.Aplicacion.ServicioImpl
{
    public class ServicioImpl : IServicio
    {
        private readonly IServicioRepositorio _servicioRepositorio;
        private readonly NovusnetPROContext _dBContext;

        public ServicioImpl()
        {
        }

        public ServicioImpl(NovusnetPROContext dBContext)
        {
            this._dBContext = dBContext;
            _servicioRepositorio = new ServicioRepositorioImpl(_dBContext);


        }

        public Task<List<SServicio>> ListarServicioActivos()
        {
            return _servicioRepositorio.ListarServiciosActivos();

        }

        public async Task ServicioAddAsync(SServicio entidad)
        {
            await _servicioRepositorio.AddAsync(entidad);
        }

        public async Task ServicioDeleteAsync(int entidad)
        {
            await _servicioRepositorio.DeleteAsync(entidad);
        }

        public Task<IEnumerable<SServicio>> ServicioGetAllAsync()
        {
            return _servicioRepositorio.GetAllAsync();
        }

        public Task<SServicio> ServicioGetByIdAsync(int id)
        {
            return _servicioRepositorio.GetByIdAsync(id);
        }

        public async Task ServicioUpdateAsync(SServicio entidad)
        {
            await _servicioRepositorio.UpdateAsync(entidad);
        }
    }
}

