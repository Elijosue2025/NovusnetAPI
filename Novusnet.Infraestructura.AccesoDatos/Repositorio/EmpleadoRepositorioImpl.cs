using Microsoft.EntityFrameworkCore;
using Novusnet.Aplicacion.DTO.DTOS;
using Novusnet.Dominio.Modelo.Abstracciones;

namespace Novusnet.Infraestructura.AccesoDatos.Repositorio
{
    public class EmpleadoRepositorioImpl : RepositorioImpl<Empleado>, IEmpleadoRepositorio
    {
        private readonly NovusnetPROContext _novusnetPROContext;

        public EmpleadoRepositorioImpl(NovusnetPROContext dBContext) : base(dBContext)
        {
            _novusnetPROContext = dBContext;
        }

        // MÉTODOS CRUD BÁSICOS
        public async Task EmpleadoAddAsync(Empleado entidad)
        {
            try
            {
                await _novusnetPROContext.Empleado.AddAsync(entidad);
                await _novusnetPROContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear empleado", ex);
            }
        }

        public new async Task<List<Empleado>> EmpleadoGetAllAsync()
        {
            try
            {
                return await _novusnetPROContext.Empleado.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar empleados", ex);
            }
        }

        public async Task<Empleado> EmpleadoGetByIdAsync(int pk_Empleado)
        {
            try
            {
                return await _novusnetPROContext.Empleado.FindAsync(pk_Empleado);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener empleado con ID {pk_Empleado}", ex);
            }
        }

        public async Task EmpleadoUpdateAsync(Empleado entidad)
        {
            try
            {
                var empleadoExistente = await _novusnetPROContext.Empleado.FindAsync(entidad.pk_Empleado);

                if (empleadoExistente == null)
                    throw new Exception("Empleado no encontrado.");

                // Actualizar los campos
                empleadoExistente.emp_roll = entidad.emp_roll;
                empleadoExistente.emp_nombre = entidad.emp_nombre;
                empleadoExistente.emp_apellido = entidad.emp_apellido;
                empleadoExistente.emp_cedula = entidad.emp_cedula;
                empleadoExistente.emp_direccion = entidad.emp_direccion;
                empleadoExistente.emp_telefono = entidad.emp_telefono;
                empleadoExistente.emp_email = entidad.emp_email;
                empleadoExistente.emp_fecha_registro = entidad.emp_fecha_registro;
                empleadoExistente.emp_activo = entidad.emp_activo;

                await _novusnetPROContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar empleado", ex);
            }
        }

        public async Task EmpleadoDeleteAsync(int pk_Empleado)
        {
            try
            {
                var empleado = await _novusnetPROContext.Empleado.FindAsync(pk_Empleado);
                if (empleado != null)
                {
                    _novusnetPROContext.Empleado.Remove(empleado);
                    await _novusnetPROContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Empleado no encontrado.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar empleado", ex);
            }
        }



        public async Task<List<Empleado>> BuscarEmpleadosPorCriterio(string criterio, string busqueda)
        {
            try
            {
                List<Empleado> listaEmpleados = new List<Empleado>();

                // Convertir criterio a minúsculas para hacer la comparación case-insensitive
                string criterioLower = criterio?.ToLower(); // Aquí estaba el error de sintaxis

                switch (criterioLower)
                {
                    case "nombre":
                    case "nombreempleado":
                        listaEmpleados = await _novusnetPROContext.Empleado
                            .Where(e => e.emp_nombre != null && e.emp_nombre.Contains(busqueda))
                            .ToListAsync();
                        break;

                    case "apellido":
                    case "apellidoempleado":
                        listaEmpleados = await _novusnetPROContext.Empleado
                            .Where(e => e.emp_apellido != null && e.emp_apellido.Contains(busqueda))
                            .ToListAsync();
                        break;

                    case "cedula":
                    case "empleadocedula":
                        listaEmpleados = await _novusnetPROContext.Empleado
                            .Where(e => e.emp_cedula != null && e.emp_cedula.Contains(busqueda))
                            .ToListAsync();
                        break;

                    case "direccion":
                        listaEmpleados = await _novusnetPROContext.Empleado
                            .Where(e => e.emp_direccion.Contains(busqueda))
                            .ToListAsync();
                        break;

                    case "telefono":
                        listaEmpleados = await _novusnetPROContext.Empleado
                            .Where(e => e.emp_telefono.Contains(busqueda))
                            .ToListAsync();
                        break;

                    case "email":
                    case "correo":
                        listaEmpleados = await _novusnetPROContext.Empleado
                            .Where(e => e.emp_email.Contains(busqueda))
                            .ToListAsync();
                        break;

                    case "roll":
                    case "rol":
                        listaEmpleados = await _novusnetPROContext.Empleado
                            .Where(e => e.emp_roll.Contains(busqueda))
                            .ToListAsync();
                        break;

                    case "id":
                    case "empleadoid":
                    case "pkempleado":
                        // Para búsqueda por ID, intentar convertir la búsqueda a entero
                        if (int.TryParse(busqueda, out int empleadoId))
                        {
                            listaEmpleados = await _novusnetPROContext.Empleado
                                .Where(e => e.pk_Empleado == empleadoId)
                                .ToListAsync();
                        }
                        break;

                    case "activo":
                        // Para búsqueda por estado activo (0 o 1)
                        if (int.TryParse(busqueda, out int estadoActivo))
                        {
                            listaEmpleados = await _novusnetPROContext.Empleado
                                .Where(e => e.emp_activo == estadoActivo)
                                .ToListAsync();
                        }
                        break;

                    default:
                        // Si no se especifica criterio válido, devolver todos los empleados activos
                        listaEmpleados = await _novusnetPROContext.Empleado
                            .Where(e => e.emp_activo == 1)
                            .ToListAsync();
                        break;
                }

                return listaEmpleados;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener empleados por parámetro: " + ex.Message, ex);
            }
        }

        public async Task<bool> CambiarEstadoEmpleado(int pk_Empleado, bool nuevoEstado)
        {
            try
            {
                var empleado = await _novusnetPROContext.Empleado.FindAsync(pk_Empleado);
                if (empleado != null)
                {
                    empleado.emp_activo = 0;


                    await _novusnetPROContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cambiar estado del empleado", ex);
            }
        }


        

        // MÉTODO OBSOLETO - MANTENER POR COMPATIBILIDAD
        public Task ObtenerPorIdAsync(int pk_Empleado)
        {
            // Este método se mantiene para compatibilidad pero se recomienda usar EmpleadoGetByIdAsync
            return Task.FromResult(EmpleadoGetByIdAsync(pk_Empleado));
        }

     
    }
}