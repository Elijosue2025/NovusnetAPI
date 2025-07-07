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

        async Task IClienteServicio.ClienteDeleteAsync(int entidad)
        {

            await _clienteRepositorio.DeleteAsync(entidad);
        }

        async Task<IEnumerable<Cliente>> IClienteServicio.ClienteGetAllAsync()
        {
            return await _clienteRepositorio.GetAllAsync();
        }

        async Task<Cliente> IClienteServicio.ClienteGetByIdAsync(int id)
        {
            return await _clienteRepositorio.GetByIdAsync(id);
        }

        async Task IClienteServicio.ClienteUpdateAsync(Cliente entidad)
        {
            await _clienteRepositorio.UpdateAsync(entidad);
        }

        private IClienteRepositorio Get_clienteRepositorio()
        {
            return _clienteRepositorio;
        }

        

        public Task<List<Cliente>> ListarClientesActivos()
        {
            return _clienteRepositorio.ListarClientesActivos();
        }
    }
}
