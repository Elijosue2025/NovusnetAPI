using Microsoft.EntityFrameworkCore;
using Novusnet.Aplicacion.Servicio;
using Novusnet.Aplicacion.ServicioImpl;
using Novusnet.Infraestructura.AccesoDatos;
using System;

namespace TestNovusnet
{
    public class Tests
    {
        private NovusnetPROContext _novusnetPROContext;

        private IClienteServicio _ClienteServicio;

        [SetUp]
        public void Setup()

        {
            var options = new DbContextOptionsBuilder<NovusnetPROContext>()
            .UseSqlServer("Data Source=EJCF;Initial Catalog=NovusnetPRO;Integrated Security=True;TrustServerCertificate=True")
            .Options;
            _novusnetPROContext = new NovusnetPROContext(options);
            _ClienteServicio = new ClienteServicoImpl(_novusnetPROContext);
        }

        public async Task TestInsertarCliente()
        {
            // Arrange
            var cliente = new Cliente
            {
                cli_cedula = "0000000001",
                cli_nombre = "TestNombre",
                cli_apellido = "TestApellido",
                cli_telefono = "0000000000",
                cli_email = "test@example.com",
                cli_direccion = "TestDireccion",
                cli_referencia_ubicacion = "Referencia X",
                cli_fecha_registro = new DateTime(2025, 6, 12),
                cli_activo = 1 // activo
            };
            await _ClienteServicio.ClienteAddAsync(cliente);



            
        }
        [TearDown]
        public void DespuesTest()
        {
            _novusnetPROContext.Dispose();
        }



    }
}