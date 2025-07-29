using Microsoft.EntityFrameworkCore;
using Novusnet.Aplicacion.DTO.DTOS;
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
    public class LogigServicioImpl : ILogingServicio 
    {
        private readonly NovusnetPROContext _dBContext;
        private readonly LogingRepositorioImpl _logingRepositorio;

        public LogigServicioImpl(NovusnetPROContext dBContext)
        {
            _dBContext = dBContext;
            _logingRepositorio = new LogingRepositorioImpl(_dBContext);


        }

        public async Task ActualizarAsync(Logging entidad)
        {
            await _logingRepositorio.ActualizarAsync(entidad);
        }

        public async Task<int> CrearLogginAsync(Logging entidad)
        {
            return await _logingRepositorio.CrearLogginAsync(entidad);
        }

        public async Task EliminarAsync(int id)
        {
            await _logingRepositorio.EliminarAsync(id);
        }

        public async Task<Logging> ObtenerPorIdAsync(int id)
        {
           return await _logingRepositorio.ObtenerPorIdAsync(id);
        }

        public async Task<IEnumerable<Logging>> ObtenerTodosAsync()
        {
            return await _logingRepositorio.ObtenerTodosAsync();
          //  return await _context.Logging.ToListAsync();

        }


        public async Task<bool> ValidarLoginAsync(string usuario, string password)
        {
            return await _logingRepositorio.ValidarLoginAsync(usuario, password);
        }
    }
}
