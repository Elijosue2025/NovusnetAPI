using Microsoft.EntityFrameworkCore;
using Novusnet.Dominio.Modelo.Abstracciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novusnet.Infraestructura.AccesoDatos.Repositorio
{
    public class MarerialRepositorioImpl : RepositorioImpl<Material>, IMaterialRepositorio
    {
        private readonly NovusnetPROContext _novusnetPROContext;

        public MarerialRepositorioImpl(NovusnetPROContext dBContext) : base(dBContext)
        {
            _novusnetPROContext = dBContext;

        }




        public Task<List<Material>> ListarMaterialStock()
        {
            try
            {
                var Resultado = from tmMaterial in _novusnetPROContext.Material
                                where tmMaterial.ma_stock_actual > 1
                                select tmMaterial;
                return Resultado.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error Listar Clientes Activos:" + ex.Message);

            }
        }


    }
}
