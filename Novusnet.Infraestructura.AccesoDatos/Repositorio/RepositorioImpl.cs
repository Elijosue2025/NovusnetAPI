using Microsoft.EntityFrameworkCore;
using Novusnet.Dominio.Modelo.Abstracciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Infraestructura.AccesoDatos.Repositorio
{
    public class RepositorioImpl<T> : IRepositorio<T> where T : class
    {
        private readonly NovusnetPROContext _dBcontext;
        private readonly DbSet<T> _dbSet;

        public RepositorioImpl(NovusnetPROContext dBcontext)
        {
            _dBcontext = dBcontext;
            _dbSet = _dBcontext.Set<T>();
        }
        public async Task AddAsync(T entidad)
        {
            try
            {
                await _dbSet.AddAsync(entidad);//insertar un registro todas las tablas
                await _dBcontext.SaveChangesAsync();//guardar los cambios en la base de datos
            }
            catch (Exception ex)
            {
                throw new Exception("Error: no se pudo ingresar datos," + ex.Message);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var entidad = await GetByIdAsync(id);// buscar el registro por id
                _dbSet.Remove(entidad);// eliminar el registro
                await _dBcontext.SaveChangesAsync();// 
            }
            catch (Exception ex)
            {
                throw new Exception("Error: no se pudo eliminar datos," + ex.Message);
            }
        }
        public async Task<IEnumerable<Empleado>> EmpleadoGetAllAsync()
        {
            try
            {
                var empleados = await _dBcontext.Empleado.ToListAsync();
                return empleados;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar empleados", ex);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {

                return await _dbSet.ToListAsync();// obtener todos los registros de la tabla

            }
            catch (Exception ex)
            {
                throw new Exception("Error: no se pudo recuperar datos," + ex.Message);
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            try
            {

                return await _dbSet.FindAsync(id);// buscar un registro por id siempre que sea numeros

            }
            catch (Exception ex)
            {
                throw new Exception("Error: no se pudo listar datos," + ex.Message);
            }
        }

        public async Task UpdateAsync(T entidad)
        {
            try
            {
                _dbSet.Update(entidad);// actualizar un registro en la tabla
                await _dBcontext.SaveChangesAsync();//guardar los cambios en la base de datos
            }
            catch (Exception ex)
            {
                throw new Exception("Error: no se pudo actualizar datos," + ex.Message);
            }
        }


    }
}
