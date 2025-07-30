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
                if (servicio == null)
                    throw new Exception("Servicio no encontrado");

                servicio.ser_tipo_factura = "Cancelado";

                _novusnetPROContext.SServicio.Update(servicio);
                await _novusnetPROContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cancelar servicio", ex);
            }
        }


        public async Task<List<SServicoDTO>> BuscarServiciosPorCriterioDTO(string criterio, string busqueda)
        {
            try
            {
                var query = _novusnetPROContext.SServicio
                    .Include(s => s.fk_clienteNavigation)
                    .AsQueryable();

                string criterioLower = criterio?.ToLower();

                switch (criterioLower)
                {
                    case "nombre":
                    case "nombreservicio":
                        query = query.Where(s => s.ser_nombre.Contains(busqueda));
                        break;

                    case "descripcion":
                        query = query.Where(s => s.ser_descripcion.Contains(busqueda));
                        break;

                    case "precio":
                        if (decimal.TryParse(busqueda, out decimal precio))
                        {
                            query = query.Where(s => s.ser_precio == precio);
                        }
                        break;

                    case "tipofactura":
                    case "factura":
                        query = query.Where(s => s.ser_tipo_factura.Contains(busqueda));
                        break;

                    case "cliente":
                    case "fkcliente":
                        if (int.TryParse(busqueda, out int clienteId))
                        {
                            query = query.Where(s => s.fk_cliente == clienteId);
                        }
                        break;

                    case "nombrecliente":
                    case "clientenombre":
                        query = query.Where(s => s.fk_clienteNavigation.cli_nombre.Contains(busqueda));
                        break;

                    case "apellidocliente":
                    case "clienteapellido":
                        query = query.Where(s => s.fk_clienteNavigation.cli_apellido.Contains(busqueda));
                        break;

                    case "cedulacliente":
                    case "clientecedula":
                        query = query.Where(s => s.fk_clienteNavigation.cli_cedula.Contains(busqueda));
                        break;

                    case "requierematerial":
                    case "material":
                        if (int.TryParse(busqueda, out int requiereMaterial))
                        {
                            query = query.Where(s => s.ser_requiere_material == requiereMaterial);
                        }
                        break;

                    case "id":
                    case "servicioid":
                    case "pkservicio":
                        if (int.TryParse(busqueda, out int servicioId))
                        {
                            query = query.Where(s => s.pk_servicio == servicioId);
                        }
                        break;
                }

                var lista = await query.ToListAsync();

                return lista.Select(s => new SServicoDTO
                {
                    pk_servicio = s.pk_servicio,
                    ser_nombre = s.ser_nombre,
                    ser_precio = s.ser_precio,
                    ser_descripcion = s.ser_descripcion,
                    ser_requiere_material = s.ser_requiere_material,
                    ser_tipo_factura = s.ser_tipo_factura,
                    fk_cliente = s.fk_cliente,

                    ClienteId = s.fk_clienteNavigation.pk_cliente,
                    CedulaCliente = s.fk_clienteNavigation.cli_cedula,
                    NombreCliente = s.fk_clienteNavigation.cli_nombre,
                    ApellidoCliente = s.fk_clienteNavigation.cli_apellido,
                    TelefonoCliente = s.fk_clienteNavigation.cli_telefono

                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar servicios con datos de cliente", ex);
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

      


        public async Task<List<SServicoDTO>> ObtenerServiciosConDatosClienteAsync()
        {
            try
            {
                return await _novusnetPROContext.SServicio
                    .Include(s => s.fk_clienteNavigation)
                    .Select(s => new SServicoDTO
                    {
                        pk_servicio = s.pk_servicio,
                        ser_nombre = s.ser_nombre,
                        ser_precio = s.ser_precio,
                        ser_descripcion = s.ser_descripcion,
                        ser_requiere_material = s.ser_requiere_material,
                        ser_tipo_factura = s.ser_tipo_factura,
                        fk_cliente = s.fk_cliente,

                        // Datos del cliente
                        ClienteId = s.fk_clienteNavigation.pk_cliente,
                        CedulaCliente = s.fk_clienteNavigation.cli_cedula,
                        NombreCliente = s.fk_clienteNavigation.cli_nombre,
                        ApellidoCliente = s.fk_clienteNavigation.cli_apellido,
                        TelefonoCliente = s.fk_clienteNavigation.cli_telefono
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener servicios con datos de cliente", ex);
            }
        }

    }
}