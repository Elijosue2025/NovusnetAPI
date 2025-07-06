using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Dominio.Modelo.Abstracciones
{

    public interface IRepositorio <T> where T : class
    {
        Task AddAsync(T entidad); //metdo para insertar
        Task UpdateAsync(T entidad); //metodo para actualizar 
        Task DeleteAsync(int entidad);//metdo para eliminar
        Task<IEnumerable<T>> GetAllAsync(); //metodo lista de todos los registros (select * from)
        Task<T> GetByIdAsync(int id); //metodo para buscar un registro por id (select * from where id = @id)
    }
}
