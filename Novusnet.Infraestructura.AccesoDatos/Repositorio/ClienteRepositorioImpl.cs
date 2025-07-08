using Microsoft.EntityFrameworkCore;
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
            throw new NotImplementedException();
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
        }

    }

}
