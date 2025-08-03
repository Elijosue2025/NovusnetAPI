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
    public class ClienteServicoImpl : IClienteServicio

    {
        private IClienteRepositorio _clienteRepositorio;

        private readonly NovusnetPROContext _dBContext;

        public ClienteServicoImpl(NovusnetPROContext dBContext)
        {
            this._dBContext = dBContext;
            _clienteRepositorio = new ClienteRepositorioImpl(_dBContext);


        }

        public async Task ClienteAddAsync(Cliente entidad)
        {
            await _clienteRepositorio.AddAsync(entidad);


        }

        public async Task ClienteUpdateAsync(Cliente entidad)
        {
            await _clienteRepositorio.UpdateAsync(entidad);
        }

        public async Task ClienteDeleteAsync(int entidad)
        {
            await _clienteRepositorio.DeleteAsync(entidad);
        }

        public async Task<IEnumerable<Cliente>> ClienteGetAllAsync()
        {
            return await _clienteRepositorio.GetAllAsync(); 
        }

        public async Task<Cliente> ClienteGetByIdAsync(int id)
        {
           return await _clienteRepositorio.GetByIdAsync(id);
        }

     

        public async Task<bool> CambiarEstadoCliente(int id, bool nuevoEstado)
        {
            return await _clienteRepositorio.CambiarEstadoCliente(id, nuevoEstado);
        }

        public async Task<List<Cliente>> BuscarClientesPorCriterio(string criterio, string busqueda)
        {
            return await _clienteRepositorio.BuscarClientesPorCriterio(criterio, busqueda);
        }

        public async Task<List<Cliente>> ClientesPorEstado(bool activo)
        {
            return await _clienteRepositorio.ClientesPorEstado(activo);
        }
    }
}
