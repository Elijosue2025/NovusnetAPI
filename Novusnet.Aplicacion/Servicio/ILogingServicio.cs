using Novusnet.Aplicacion.DTO.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Aplicacion.Servicio
{
    public interface ILogingServicio
    {
        Task<LoggingDTO> ObtenerPorIdAsync(int id);
        Task<LoggingDTO> ObtenerPorUsuarioAsync(string usuario);
        Task<IEnumerable<LoggingDTO>> ObtenerTodosAsync();
        Task<int> CrearAsync(LoggingDTO logging);
        Task ActualizarAsync(LoggingDTO logging);
        Task EliminarAsync(int id);
        Task<bool> ValidarLoginAsync(string usuario, string password);
    }
}
