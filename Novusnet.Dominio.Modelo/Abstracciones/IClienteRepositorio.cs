using Novusnet.Infraestructura.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Dominio.Modelo.Abstracciones
{
    public interface IClienteRepositorio: IRepositorio<Cliente>
    {
        Task<List<Cliente>> ListarClientesActivos();

    }
}
