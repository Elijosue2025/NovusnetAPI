using Microsoft.EntityFrameworkCore;
using Novusnet.Aplicacion.DTO.DTOS;
using Novusnet.Dominio.Modelo.Abstracciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Infraestructura.AccesoDatos.Repositorio
{
    public class ClienteRepositorioImpl : RepositorioImpl<Cliente>, IClienteRepositorio
    {
        private readonly NovusnetPROContext _novusnetPROContext;

        public ClienteRepositorioImpl(NovusnetPROContext dBContext) : base(dBContext)
        {
            _novusnetPROContext = dBContext;
        }

        public Task<List<Cliente>> ListarClientesActivos()
        {
            
                try
                {
                    var Resultado = from tmCliente in _novusnetPROContext.Cliente
                                    where tmCliente.cli_activo == 1
                                    select tmCliente;
                    return Resultado.ToListAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error Listar Clientes Activos:" + ex.Message);

                }
            
        }


        public async Task<IEnumerable<Cliente>> ClienteGetAllAsync()
        {
            return await _novusnetPROContext.Cliente.ToListAsync();
        }

        public async Task<Cliente> ClienteGetByIdAsync(int id)
        {
             
            try
            {
                return await _novusnetPROContext.Cliente.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener empleado con ID {id}", ex);
            }
        }

        public async Task ClienteUpdateAsync(Cliente entidad)
        {
            var clienteExistente = await _novusnetPROContext.Cliente.FindAsync(entidad.pk_cliente);
            if (clienteExistente == null)
                throw new Exception("Cliente no encontrado.");

            clienteExistente.cli_cedula = entidad.cli_cedula;
            clienteExistente.cli_nombre = entidad.cli_nombre;
            clienteExistente.cli_apellido = entidad.cli_apellido;
            clienteExistente.cli_telefono = entidad.cli_telefono;
            clienteExistente.cli_email = entidad.cli_email;
            clienteExistente.cli_direccion = entidad.cli_direccion;
            clienteExistente.cli_referencia_ubicacion = entidad.cli_referencia_ubicacion;
            clienteExistente.cli_fecha_registro = entidad.cli_fecha_registro;
            clienteExistente.cli_activo = entidad.cli_activo;

            await _novusnetPROContext.SaveChangesAsync();
        }

        

        public async Task<ClienteDTO> ObtenerClientePorCedula(string cedula)
        {
            try
            {
                return await _novusnetPROContext.Cliente
                    .Where(c => c.cli_cedula == cedula)
                    .Select(c => new ClienteDTO
                    {
                        pk_cliente = c.pk_cliente,
                        cli_nombre = c.cli_nombre,
                        cli_apellido = c.cli_apellido,
                        cli_cedula = c.cli_cedula,
                        cli_telefono = c.cli_telefono,
                        cli_email = c.cli_email,
                        cli_direccion = c.cli_direccion,
                        cli_referencia_ubicacion = c.cli_referencia_ubicacion,
                        cli_fecha_registro = c.cli_fecha_registro,
                        cli_activo = c.cli_activo
                    }).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar cliente por cédula", ex);
            }
        }

        public async Task<List<ClienteDTO>> ClientesPorEstado(bool activo)
        {
            int estado = activo ? 1 : 0;

            return await _novusnetPROContext.Cliente
                .Where(c => c.cli_activo == estado)
                .Select(c => new ClienteDTO
                {
                    pk_cliente = c.pk_cliente,
                    cli_nombre = c.cli_nombre,
                    cli_apellido = c.cli_apellido,
                    cli_email = c.cli_email,
                    cli_telefono = c.cli_telefono,
                    cli_cedula = c.cli_cedula,
                    cli_direccion = c.cli_direccion,
                    cli_activo = c.cli_activo,
                    cli_fecha_registro = c.cli_fecha_registro
                }).ToListAsync();
        }

        Task IClienteRepositorio.ClienteAddAsync(Cliente entidad)
        {
            throw new NotImplementedException();
        }

        Task IClienteRepositorio.ClienteDeleteAsync(int entidad)
        {
            throw new NotImplementedException();
        }
    }

}
