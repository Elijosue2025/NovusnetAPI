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
    internal class EmpleadoServicioImpl : IEmpleadoServicio
    {
        private IEmpleadoServicio _empleadoServicio;

        private readonly NovusnetPROContext _dBContext;

        public EmpleadoServicioImpl(NovusnetPROContext dBContext)
        {
            this._dBContext = dBContext;
            _empleadoServicio = new EmpleadoServicioImpl(_dBContext);


        }

        public async Task EmpleadoAddAsync(Empleado entidad)
        {

            await _empleadoServicio.EmpleadoAddAsync(entidad);
        }

        public async Task EmpleadoUpdateAsync(Cliente entidad)
        {
            await _empleadoServicio.EmpleadoUpdateAsync(entidad);

        }

        public async Task EmpleadoDeleteAsync(int entidad)
        {
            await _empleadoServicio.EmpleadoDeleteAsync(entidad);
        }

        public Task<IEnumerable<Cliente>> EmpleadoGetAllAsync()
        {
            return _empleadoServicio.EmpleadoGetAllAsync();
        }

        public Task<Cliente> EmpleadoGetByIdAsync(int id)
        {
            return _empleadoServicio.EmpleadoGetByIdAsync(id);
        }

        public Task<List<Cliente>> ListarEmpleadoRoll()
        {
            return _empleadoServicio.ListarEmpleadoRoll();
        }
    }
}
