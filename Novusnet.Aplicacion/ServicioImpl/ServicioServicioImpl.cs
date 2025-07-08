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
    public class ServicioServicioImpl : IServicioServicio
    {
        private readonly NovusnetPROContext _dBContext;


        private IServicioServicio _servicioRepositorio;


        public ServicioServicioImpl(NovusnetPROContext novusnetPROContext)
        {
            this._dBContext = novusnetPROContext;
            _servicioRepositorio = new ServicioRepositorioImpl(novusnetPROContext);


        }

        public async Task IServicioServicioAddAsync(IServicioServicio entidad)
        {
            await _servicioRepositorio.AddAsync(entidad);
        }

        public async Task IServicioServicioDeleteAsync(int entidad)
        {
            await _servicioRepositorio.DeleteAsync(entidad);    
        }

        public async Task<IEnumerable<IServicioServicio>> IServicioServicioGetAllAsync()
        {
            await _servicioRepositorio.GetAllAsync();   
        }

        public async Task<IServicioServicio> IServicioServicioGetByIdAsync(int id)
        {
            return await _servicioRepositorio.GetByIdAsync(id);
        }

        public Task IServicioServicioUpdateAsync(IServicioServicio entidad)
        {
            return _servicioRepositorio.UpdateAsync(entidad);
        }
    }
}
