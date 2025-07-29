using Microsoft.EntityFrameworkCore;
using Novusnet.Dominio.Modelo.Abstracciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Infraestructura.AccesoDatos.Repositorio
{
    public class LogingRepositorioImpl: RepositorioImpl<Logging>, ILogingRepositorio
    {
        private readonly NovusnetPROContext _novusnetPROContext;
        // private readonly Empleado _empleado;
        //  private IEmpleadoRepositorio _empleadoRepositorio;

        public LogingRepositorioImpl(NovusnetPROContext dBContext) : base(dBContext)
        {
            _novusnetPROContext = dBContext;

        }

        public async Task ActualizarAsync(Logging logging)
        {
            var entity = await _novusnetPROContext.Logging.FindAsync(logging.pk_logging);
            if (entity != null)
            {
                entity.log_user = logging.log_user;
                entity.log_password = logging.log_password;
                entity.fk_Empleado = logging.fk_Empleado;

                _novusnetPROContext.Logging.Update(entity);
                await _novusnetPROContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"No se encontró el registro con ID {logging.pk_logging}");
            }
        }


        public async Task<int> CrearLogginAsync(Logging logging)
        {
            try
            {
                // Verificar si el empleado existe
                var empleado = await _novusnetPROContext.Empleado
                    .FirstOrDefaultAsync(e => e.pk_Empleado == logging.fk_Empleado);

                if (empleado == null)
                {
                    throw new Exception("No se encontró el empleado especificado.");
                }

                // Insertar el nuevo logging
                _novusnetPROContext.Logging.Add(logging);
                await _novusnetPROContext.SaveChangesAsync();

                return logging.pk_logging;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear nuevo registro de logging", ex);
            }
        }



        public async Task EliminarAsync(int id)
        {
            var entity = await _novusnetPROContext.Logging.FindAsync(id);
            if (entity != null)
            {
                _novusnetPROContext.Logging.Remove(entity);
                await _novusnetPROContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"No se encontró el registro con ID {id}");
            }
        }

       

        public async Task<Logging> ObtenerPorIdAsync(int id)
        {
            try
            {
                return await _novusnetPROContext.Logging.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear nuevo registro de logging", ex);
            }


        }




        public async Task<IEnumerable<Logging>> ObtenerTodosAsync()
        {
            try
            {
                return await _novusnetPROContext.Logging.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar registros de logging", ex);
            }
        }
        public async Task<bool> ValidarLoginAsync(string usuario, string password)
        {
            return await _novusnetPROContext.Logging
                .AnyAsync(l => l.log_user == usuario && l.log_password == password);
        }


    }
}
