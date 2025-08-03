using Microsoft.EntityFrameworkCore;
using Novusnet.Aplicacion.DTO.DTOS;
using Novusnet.Dominio.Modelo.Abstracciones;

namespace Novusnet.Infraestructura.AccesoDatos.Repositorio
{
    public class ClienteRepositorioImpl : RepositorioImpl<Cliente>, IClienteRepositorio
    {
        private readonly NovusnetPROContext _novusnetPROContext;

        public ClienteRepositorioImpl(NovusnetPROContext dBContext) : base(dBContext)
        {
            _novusnetPROContext = dBContext;

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



        public async Task<List<Cliente>> ClientesPorEstado(bool activo)
        {
            int estado = activo ? 1 : 0;

            return await _novusnetPROContext.Cliente
                .Where(c => c.cli_activo == estado)
                .Select(c => new Cliente
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

        async Task IClienteRepositorio.ClienteAddAsync(Cliente entidad)
        {
            await _novusnetPROContext.Cliente.AddAsync(entidad);
            await _novusnetPROContext.SaveChangesAsync();
            // await _novusnetPROContext.ClienteAddAsync();
        }
        // MÉTODO CORREGIDO - Debe retornar Task en lugar de Task<bool>
        public async Task ClienteDeleteAsync(int entidad)
        {
            try
            {
                var cliente = await _novusnetPROContext.Cliente.FindAsync(entidad);
                if (cliente == null)
                {
                    throw new ArgumentException($"No se encontró el cliente con ID: {entidad}");
                }

                cliente.cli_activo = 0;
                _novusnetPROContext.Cliente.Update(cliente);
                await _novusnetPROContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar cliente", ex);
            }
        }

        public async Task<List<Cliente>> BuscarClientesPorCriterio(string criterio, string busqueda)
        {
            try
            {
                List<Cliente> listaClientes = new List<Cliente>();
                string criterioLower = criterio?.ToLower();

                switch (criterioLower)
                {
                    case "nombre":
                    case "nombrecliente":
                        listaClientes = await _novusnetPROContext.Cliente
                            .Where(c => c.cli_nombre != null && c.cli_nombre.Contains(busqueda))
                            .ToListAsync();
                        break;

                    case "apellido":
                    case "apellidocliente":
                        listaClientes = await _novusnetPROContext.Cliente
                            .Where(c => c.cli_apellido != null && c.cli_apellido.Contains(busqueda))
                            .ToListAsync();
                        break;

                    case "cedula":
                    case "clientecedula":
                        listaClientes = await _novusnetPROContext.Cliente
                            .Where(c => c.cli_cedula != null && c.cli_cedula.Contains(busqueda))
                            .ToListAsync();
                        break;

                    case "direccion":
                        listaClientes = await _novusnetPROContext.Cliente
                            .Where(c => c.cli_direccion != null && c.cli_direccion.Contains(busqueda))
                            .ToListAsync();
                        break;

                    case "telefono":
                        listaClientes = await _novusnetPROContext.Cliente
                            .Where(c => c.cli_telefono != null && c.cli_telefono.Contains(busqueda))
                            .ToListAsync();
                        break;

                    case "email":
                    case "correo":
                        listaClientes = await _novusnetPROContext.Cliente
                            .Where(c => c.cli_email != null && c.cli_email.Contains(busqueda))
                            .ToListAsync();
                        break;

                    case "referencia":
                    case "referenciaubicacion":
                        listaClientes = await _novusnetPROContext.Cliente
                            .Where(c => c.cli_referencia_ubicacion != null && c.cli_referencia_ubicacion.Contains(busqueda))
                            .ToListAsync();
                        break;

                    case "id":
                    case "clienteid":
                    case "pkcliente":
                        if (int.TryParse(busqueda, out int clienteId))
                        {
                            listaClientes = await _novusnetPROContext.Cliente
                                .Where(c => c.pk_cliente == clienteId)
                                .ToListAsync();
                        }
                        break;

                    case "activo":
                        if (int.TryParse(busqueda, out int estadoActivo))
                        {
                            listaClientes = await _novusnetPROContext.Cliente
                                .Where(c => c.cli_activo == estadoActivo)
                                .ToListAsync();
                        }
                        break;

                    case "fecha":
                    case "fecharegistro":
                        if (DateTime.TryParse(busqueda, out DateTime fechaBusqueda))
                        {
                            listaClientes = await _novusnetPROContext.Cliente
                                .Where(c => c.cli_fecha_registro.HasValue &&
                                           c.cli_fecha_registro.Value.Date == fechaBusqueda.Date)
                                .ToListAsync();
                        }
                        break;

                    default:
                        listaClientes = await _novusnetPROContext.Cliente
                            .Where(c => c.cli_activo == 1)
                            .ToListAsync();
                        break;
                }

                return listaClientes;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener clientes por parámetro: " + ex.Message, ex);
            }
        }

        public async Task<bool> CambiarEstadoCliente(int pk_cliente, bool nuevoEstado)
        {
            try
            {
                var cliente = await _novusnetPROContext.Cliente.FindAsync(pk_cliente);
                if (cliente != null)
                {
                    cliente.cli_activo = 0; // Como especificaste, siempre pone 0
                    await _novusnetPROContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cambiar estado del cliente", ex);
            }
        }

       
    }
}