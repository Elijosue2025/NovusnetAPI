using Microsoft.EntityFrameworkCore;
using Novusnet.Aplicacion.Servicio;
using Novusnet.Aplicacion.ServicioImpl;
using Novusnet.Infraestructura.AccesoDatos;

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

        [Test]
        public async Task TestInsertarCliente()
        {
            var cliente = new Cliente
            {
                cli_cedula = "000000003",
                cli_nombre = "Katya",
                cli_apellido = "Valladares",
                cli_telefono = "0000000000",
                cli_email = "test@example.com",
                cli_direccion = "TestDireccion",
                cli_referencia_ubicacion = "Referencia X",
                cli_fecha_registro = new DateTime(2025, 7, 13),
                cli_activo = 1
            };

            await _ClienteServicio.ClienteAddAsync(cliente);

            Assert.Pass("Cliente insertado correctamente.");
        }

        [TearDown]
        public void DespuesTest()
        {
            _novusnetPROContext.Dispose();
        }

    }
}
