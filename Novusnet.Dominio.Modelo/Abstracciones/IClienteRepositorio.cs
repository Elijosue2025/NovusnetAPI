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
         Task ClienteUpdateAsync(Cliente entidad);
        Task<List<Cliente>> ClientesPorEstado(bool activo);


         Task ClienteAddAsync(Cliente entidad);
         Task<List<Cliente>> BuscarClientesPorCriterio(string criterio, string busqueda);
         Task<bool> CambiarEstadoCliente(int pk_cliente, bool nuevoEstado);
    }
}
