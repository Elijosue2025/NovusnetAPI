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
    public class OrdenTrabajoServicioImpl : IOrdenTrabajoServicio

    {
        private IOrdenTrabajoRepositorio _ordenTrabajoRepositorio;
        private readonly NovusnetPROContext _dBContext;

        public OrdenTrabajoServicioImpl(NovusnetPROContext dBContext)
        {

            this._dBContext = dBContext;
            _ordenTrabajoRepositorio = new OrdenTrabajoRepositorioImpl(_dBContext);

        }

        public async Task OrdenTrabajoAddAsync(Orden_Trabajo entidad)
        {
            await _ordenTrabajoRepositorio.AddAsync(entidad);

        }

        public async Task OrdenTrabajoDeleteAsync(int entidad)
        {
            await _ordenTrabajoRepositorio.DeleteAsync(entidad);


        }

        public async Task<IEnumerable<Orden_Trabajo>> OrdenTrabajoGetAllAsync()
        {
            return await _ordenTrabajoRepositorio.GetAllAsync();

        }

        public async Task<Orden_Trabajo> OrdenTrabajoGetByIdAsync(int id)
        {
            return await _ordenTrabajoRepositorio.GetByIdAsync(id);

        }

        public async Task OrdenTrabajoUpdateAsync(Orden_Trabajo entidad)
        {
            await _ordenTrabajoRepositorio.UpdateAsync(entidad);

        }
    }
}
