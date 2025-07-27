using Novusnet.Infraestructura.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Dominio.Modelo.Abstracciones
{
    public interface ILogingRepositorio : IRepositorio<Logging>
    {
        Task<Logging> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Logging>> ObtenerPorEmpleadoAsync(int empleadoId);
        Task<IEnumerable<Logging>> ObtenerTodosAsync();
        Task<int> CrearLogginAsync(Logging logging);
        Task ActualizarAsync(Logging logging);
        Task EliminarAsync(int id);
    }
}
