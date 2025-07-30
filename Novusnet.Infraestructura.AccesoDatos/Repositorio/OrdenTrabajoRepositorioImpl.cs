using Microsoft.EntityFrameworkCore;
using Novusnet.Dominio.Modelo.Abstracciones;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Novusnet.Infraestructura.AccesoDatos.Repositorio
{
    public class OrdenTrabajoRepositorioImpl : RepositorioImpl<Orden_Trabajo>, IOrdenTrabajoRepositorio
    {
        private readonly NovusnetPROContext _context;

        public OrdenTrabajoRepositorioImpl(NovusnetPROContext context) : base(context)
        {
            _context = context;
        }

        public async Task CrearOrdenTrabajoAsync(Orden_Trabajo orden)
        {
            try
            {
                orden.otra_fecha_registro = DateTime.Now; // Fecha automática
                await _context.Orden_Trabajo.AddAsync(orden);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear orden de trabajo", ex);
            }
        }

        public async Task<List<Orden_Trabajo>> ObtenerTodasOrdenesAsync()
        {
            try
            {
                return await _context.Orden_Trabajo.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener órdenes", ex);
            }
        }

        public async Task<Orden_Trabajo> ObtenerOrdenPorIdAsync(int id)
        {
            try
            {
                return await _context.Orden_Trabajo.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener orden ID {id}", ex);
            }
        }

        public async Task ActualizarOrdenTrabajoAsync(Orden_Trabajo orden)
        {
            try
            {
                var existente = await _context.Orden_Trabajo.FindAsync(orden.pk_orden_trabajo);
                if (existente == null) throw new Exception("Orden no encontrada");

                // Campos actualizables (excepto fecha_registro)
                existente.otra_codigo = orden.otra_codigo;
                existente.otra_descripcion = orden.otra_descripcion;
                existente.otra_fecha_programada = orden.otra_fecha_programada;
                existente.otra_tiempo_estimado = orden.otra_tiempo_estimado;
                existente.otra_estado = orden.otra_estado;
                existente.otra_prioridad = orden.otra_prioridad;
                existente.fk_Empleado = orden.fk_Empleado;
                existente.fk_servicio = orden.fk_servicio;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar orden", ex);
            }
        }

        public async Task EliminarOrdenTrabajoAsync(int id)
        {
            try
            {
                var orden = await _context.Orden_Trabajo.FindAsync(id);
                if (orden != null)
                {
                    _context.Orden_Trabajo.Remove(orden);
                    await _context.SaveChangesAsync();
                }
                else throw new Exception("Orden no encontrada");
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar orden", ex);
            }
        }
    }
}
