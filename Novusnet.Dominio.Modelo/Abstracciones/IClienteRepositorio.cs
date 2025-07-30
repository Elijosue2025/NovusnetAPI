using Novusnet.Aplicacion.DTO.DTOS;
using Novusnet.Infraestructura.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Dominio.Modelo.Abstracciones
{

    public interface IClienteRepositorio : IRepositorio<Cliente>
    {
        Task<List<Cliente>> ListarClientesActivos();
        Task ClienteAddAsync(Cliente entidad);
        Task ClienteDeleteAsync(int entidad);
        Task<IEnumerable<Cliente>> ClienteGetAllAsync();
        Task<Cliente> ClienteGetByIdAsync(int id);
        Task ClienteUpdateAsync(Cliente entidad);
       
       
       

    }
}
