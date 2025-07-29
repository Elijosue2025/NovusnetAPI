using Novusnet.Infraestructura.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Aplicacion.Servicio
{
    public interface ILogingServicio
    {
        Task<Logging> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Logging>> ObtenerTodosAsync();
        Task<int> CrearLogginAsync(Logging entidad);
        Task ActualizarAsync(Logging entidad);
        Task EliminarAsync(int id);
        Task<bool> ValidarLoginAsync(string usuario, string password);
    }
}
