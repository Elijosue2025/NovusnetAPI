using Microsoft.EntityFrameworkCore;
using Novusnet.Dominio.Modelo.Abstracciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Infraestructura.AccesoDatos.Repositorio
{
    public class ServicioRepositorioImpl : RepositorioImpl<SServicio>, IServicioRepositorio
    {
        private readonly NovusnetPROContext _novusnetPROContext;

        public ServicioRepositorioImpl(NovusnetPROContext dBContext) : base(dBContext)
        {
            _novusnetPROContext = dBContext;
        }

        // MÉTODOS CRUD BÁSICOS
        public async Task ServicioAddAsync(SServicio entidad)
        {
            try
            {
                await _novusnetPROContext.SServicio.AddAsync(entidad);
                await _novusnetPROContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear servicio", ex);
            }
        }

        public new async Task<List<SServicio>> ServicioGetAllAsync()
        {
            try
            {
                return await _novusnetPROContext.SServicio
                    .Include(s => s.fk_clienteNavigation)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar servicios", ex);
            }
        }

        public async Task<SServicio> ServicioGetByIdAsync(int pk_servicio)
        {
            try
            {
                return await _novusnetPROContext.SServicio
                    .Include(s => s.fk_clienteNavigation)
                    .FirstOrDefaultAsync(s => s.pk_servicio == pk_servicio);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener servicio con ID {pk_servicio}", ex);
            }
        }

        public async Task ServicioUpdateAsync(SServicio entidad)
        {
            try
            {
                var servicioExistente = await _novusnetPROContext.SServicio.FindAsync(entidad.pk_servicio);

                if (servicioExistente == null)
                    throw new Exception("Servicio no encontrado.");

                // Actualizar los campos
                servicioExistente.ser_nombre = entidad.ser_nombre;
                servicioExistente.ser_precio = entidad.ser_precio;
                servicioExistente.ser_descripcion = entidad.ser_descripcion;
                servicioExistente.ser_requiere_material = entidad.ser_requiere_material;
                servicioExistente.ser_tipo_factura = entidad.ser_tipo_factura;
                servicioExistente.fk_cliente = entidad.fk_cliente;

                await _novusnetPROContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar servicio", ex);
            }
        }

        public async Task ServicioDeleteAsync(int pk_servicio)
        {
            try
            {
                var servicio = await _novusnetPROContext.SServicio.FindAsync(pk_servicio);
                if (servicio != null)
                {
                    _novusnetPROContext.SServicio.Remove(servicio);
                    await _novusnetPROContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Servicio no encontrado.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar servicio", ex);
            }
        }

        // MÉTODO DE BÚSQUEDA POR CRITERIOS
        public async Task<List<SServicio>> BuscarServiciosPorCriterio(string criterio, string busqueda)
        {
            try
            {
                List<SServicio> listaServicios = new List<SServicio>();

                // Convertir criterio a minúsculas para hacer la comparación case-insensitive
                string criterioLower = criterio?.ToLower();

                switch (criterioLower)
                {
                    case "nombre":
                    case "nombreservicio":
                        listaServicios = await _novusnetPROContext.SServicio
                            .Include(s => s.fk_clienteNavigation)
                            .Where(s => s.ser_nombre != null && s.ser_nombre.Contains(busqueda))
                            .ToListAsync();
                        break;

                    case "descripcion":
                        listaServicios = await _novusnetPROContext.SServicio
                            .Include(s => s.fk_clienteNavigation)
                            .Where(s => s.ser_descripcion != null && s.ser_descripcion.Contains(busqueda))
                            .ToListAsync();
                        break;

                    case "precio":
                        // Para búsqueda por precio, intentar convertir la búsqueda a decimal
                        if (decimal.TryParse(busqueda, out decimal precio))
                        {
                            listaServicios = await _novusnetPROContext.SServicio
                                .Include(s => s.fk_clienteNavigation)
                                .Where(s => s.ser_precio == precio)
                                .ToListAsync();
                        }
                        break;

                    case "tipofactura":
                    case "factura":
                        listaServicios = await _novusnetPROContext.SServicio
                            .Include(s => s.fk_clienteNavigation)
                            .Where(s => s.ser_tipo_factura != null && s.ser_tipo_factura.Contains(busqueda))
                            .ToListAsync();
                        break;

                    case "cliente":
                    case "fkcliente":
                        if (int.TryParse(busqueda, out int clienteId))
                        {
                            listaServicios = await _novusnetPROContext.SServicio
                                .Include(s => s.fk_clienteNavigation)
                                .Where(s => s.fk_cliente == clienteId)
                                .ToListAsync();
                        }
                        break;

                    case "nombrecliente":
                    case "clientenombre":
                        listaServicios = await _novusnetPROContext.SServicio
                            .Include(s => s.fk_clienteNavigation)
                            .Where(s => s.fk_clienteNavigation != null &&
                                   s.fk_clienteNavigation.cli_nombre.Contains(busqueda))
                            .ToListAsync();
                        break;

                    case "apellidocliente":
                    case "clienteapellido":
                        listaServicios = await _novusnetPROContext.SServicio
                            .Include(s => s.fk_clienteNavigation)
                            .Where(s => s.fk_clienteNavigation != null &&
                                   s.fk_clienteNavigation.cli_apellido.Contains(busqueda))
                            .ToListAsync();
                        break;

                    case "cedulacliente":
                    case "clientecedula":
                        listaServicios = await _novusnetPROContext.SServicio
                            .Include(s => s.fk_clienteNavigation)
                            .Where(s => s.fk_clienteNavigation != null &&
                                   s.fk_clienteNavigation.cli_cedula.Contains(busqueda))
                            .ToListAsync();
                        break;

                    case "requierematerial":
                    case "material":
                        if (int.TryParse(busqueda, out int requiereMaterial))
                        {
                            listaServicios = await _novusnetPROContext.SServicio
                                .Include(s => s.fk_clienteNavigation)
                                .Where(s => s.ser_requiere_material == requiereMaterial)
                                .ToListAsync();
                        }
                        break;

                    case "id":
                    case "servicioid":
                    case "pkservicio":
                        // Para búsqueda por ID, intentar convertir la búsqueda a entero
                        if (int.TryParse(busqueda, out int servicioId))
                        {
                            listaServicios = await _novusnetPROContext.SServicio
                                .Include(s => s.fk_clienteNavigation)
                                .Where(s => s.pk_servicio == servicioId)
                                .ToListAsync();
                        }
                        break;

                    default:
                        // Si no se especifica criterio válido, devolver todos los servicios
                        listaServicios = await _novusnetPROContext.SServicio
                            .Include(s => s.fk_clienteNavigation)
                            .ToListAsync();
                        break;
                }

                return listaServicios;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener servicios por parámetro: " + ex.Message, ex);
            }
        }

        // MÉTODOS ESPECÍFICOS PARA SERVICIO
        public async Task<List<SServicio>> ListarServiciosConMateriales()
        {
            try
            {
                var resultado = from tmSservicio in _novusnetPROContext.SServicio
                                where tmSservicio.ser_requiere_material == 1
                                select tmSservicio;
                return await resultado.Include(s => s.fk_clienteNavigation).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar servicios que requieren materiales: " + ex.Message, ex);
            }
        }

        public async Task<List<SServicio>> ListarServiciosSinMateriales()
        {
            try
            {
                return await _novusnetPROContext.SServicio
                    .Include(s => s.fk_clienteNavigation)
                    .Where(s => s.ser_requiere_material == 0 || s.ser_requiere_material == null)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar servicios que no requieren materiales", ex);
            }
        }

        public async Task<List<SServicio>> ListarServiciosPorCliente(int fk_cliente)
        {
            try
            {
                return await _novusnetPROContext.SServicio
                    .Include(s => s.fk_clienteNavigation)
                    .Where(s => s.fk_cliente == fk_cliente)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar servicios del cliente {fk_cliente}", ex);
            }
        }

        public async Task<List<SServicio>> ListarServiciosPorTipoFactura(string tipoFactura)
        {
            try
            {
                return await _novusnetPROContext.SServicio
                    .Include(s => s.fk_clienteNavigation)
                    .Where(s => s.ser_tipo_factura != null && s.ser_tipo_factura.Equals(tipoFactura))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar servicios por tipo de factura {tipoFactura}", ex);
            }
        }

        public async Task<bool> CambiarRequiereMaterial(int pk_servicio, bool requiereMaterial)
        {
            try
            {
                var servicio = await _novusnetPROContext.SServicio.FindAsync(pk_servicio);
                if (servicio != null)
                {
                    servicio.ser_requiere_material = requiereMaterial ? 1 : 0;
                    await _novusnetPROContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cambiar el estado de requiere material del servicio", ex);
            }
        }

        // NUEVOS MÉTODOS PARA CONSULTAR CLIENTES QUE TIENEN SERVICIOS
        public async Task<List<Cliente>> ListarClientesConServicios()
        {
            try
            {
                return await _novusnetPROContext.Cliente
                    .Include(c => c.SServicio)
                    .Where(c => c.SServicio.Any())
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar clientes con servicios", ex);
            }
        }

        public async Task<List<Cliente>> ListarClientesSinServicios()
        {
            try
            {
                return await _novusnetPROContext.Cliente
                    .Include(c => c.SServicio)
                    .Where(c => !c.SServicio.Any())
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar clientes sin servicios", ex);
            }
        }

        public async Task<Cliente> ObtenerClienteConServicios(int pk_cliente)
        {
            try
            {
                return await _novusnetPROContext.Cliente
                    .Include(c => c.SServicio)
                    .FirstOrDefaultAsync(c => c.pk_cliente == pk_cliente);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener cliente con ID {pk_cliente} y sus servicios", ex);
            }
        }

        public async Task<int> ContarServiciosPorCliente(int pk_cliente)
        {
            try
            {
                return await _novusnetPROContext.SServicio
                    .CountAsync(s => s.fk_cliente == pk_cliente);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al contar servicios del cliente {pk_cliente}", ex);
            }
        }

        public async Task<List<Cliente>> BuscarClientesPorNombreConServicios(string nombre)
        {
            try
            {
                return await _novusnetPROContext.Cliente
                    .Include(c => c.SServicio)
                    .Where(c => c.cli_nombre.Contains(nombre) && c.SServicio.Any())
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al buscar clientes por nombre '{nombre}' con servicios", ex);
            }
        }

        public async Task<List<SServicio>> ListarServiciosConDetallesCliente()
        {
            try
            {
                return await _novusnetPROContext.SServicio
                    .Include(s => s.fk_clienteNavigation)
                    .Select(s => new SServicio
                    {
                        pk_servicio = s.pk_servicio,
                        ser_nombre = s.ser_nombre,
                        ser_precio = s.ser_precio,
                        ser_descripcion = s.ser_descripcion,
                        ser_requiere_material = s.ser_requiere_material,
                        ser_tipo_factura = s.ser_tipo_factura,
                        fk_cliente = s.fk_cliente,
                        fk_clienteNavigation = new Cliente
                        {
                            pk_cliente = s.fk_clienteNavigation.pk_cliente,
                            cli_cedula = s.fk_clienteNavigation.cli_cedula,
                            cli_nombre = s.fk_clienteNavigation.cli_nombre,
                            cli_apellido = s.fk_clienteNavigation.cli_apellido,
                            cli_telefono = s.fk_clienteNavigation.cli_telefono,
                            cli_email = s.fk_clienteNavigation.cli_email,
                            cli_direccion = s.fk_clienteNavigation.cli_direccion,
                            cli_referencia_ubicacion = s.fk_clienteNavigation.cli_referencia_ubicacion,
                            cli_fecha_registro = s.fk_clienteNavigation.cli_fecha_registro,
                            cli_activo = s.fk_clienteNavigation.cli_activo
                        }
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar servicios con detalles completos del cliente", ex);
            }
        }

        public async Task<Dictionary<Cliente, List<SServicio>>> ObtenerClientesConSusServicios()
        {
            try
            {
                var clientesConServicios = await _novusnetPROContext.Cliente
                    .Include(c => c.SServicio)
                    .Where(c => c.SServicio.Any())
                    .ToDictionaryAsync(c => c, c => c.SServicio.ToList());

                return clientesConServicios;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener diccionario de clientes con sus servicios", ex);
            }
        }

        // MÉTODO OBSOLETO - MANTENER POR COMPATIBILIDAD
        public Task ObtenerPorIdAsync(int pk_servicio)
        {
            // Este método se mantiene para compatibilidad pero se recomienda usar ServicioGetByIdAsync
            return Task.FromResult(ServicioGetByIdAsync(pk_servicio));
        }
    }
}