using System;
using System.Web.Mvc;
using BibliotecaUPC.Controllers;
using BibliotecaUPC.Models;
using NUnit.Framework;

namespace BibliotecaUPC.Tests.Controllers
{
    [TestFixture]
    public class UsuariosControllerTest
    {

        [Test]
        public void registrar_nuevo_usuario_mensaje_correcto()
        {
            UsuariosController controller = new UsuariosController();
            Usuario usuario = InicializarUsuario("Javier", "García Rodríguez", new DateTime(1981, 08, 12), "12345667A");

            String result = controller.nuevoUsuario(usuario);

            Assert.AreEqual("Usuario registrado con éxito", result);
            controller.eliminarUsuario(usuario.dni);
        }

        [Test]
        public void registrar_nuevo_usuario_en_bbdd()
        {
            UsuariosController controller = new UsuariosController();
            Usuario usuario = InicializarUsuario("Javier", "García Rodríguez", new DateTime(1981, 08, 12), "12345667A");

            var originalStrength = controller.Usuarios.Count;

            controller.nuevoUsuario(usuario);

            Assert.IsTrue(controller.Usuarios.Count > originalStrength, "No se ha añadido el elemento a la lista");

            controller.eliminarUsuario(usuario.dni);
            Assert.IsTrue(controller.Usuarios.Count == originalStrength, "pasa algo");
        }

        [Test]
        public void registrar_nuevo_usuario_duplicado()
        {
            UsuariosController controller = new UsuariosController();
            Usuario usuario1 = InicializarUsuario("Javier", "García Rodríguez", new DateTime(1981, 08, 12), "12345667A");
            Usuario usuario2 = InicializarUsuario("Elena", "Sánchez Ruiz", new DateTime(1971, 10, 21), "12345667A");


            String result = controller.nuevoUsuario(usuario1);
            result = controller.nuevoUsuario(usuario2);

            Assert.AreEqual("Usuario existente", result);
            controller.eliminarUsuario(usuario1.dni);
        }

        [Test]
        public void registrar_nuevo_usuario_sin_nombre_informado()
        {
            UsuariosController controller = new UsuariosController();
            Models.Usuario usuario = new Models.Usuario();
            usuario.apellidos = "García Rodríguez";
            usuario.fechaNacimiento = new DateTime(1971, 10, 21);
            usuario.dni = "12345667A";

            var originalStrength = controller.Usuarios.Count;

            String result = controller.nuevoUsuario(usuario);
            Assert.AreEqual("El nombre no puede ser nulo", result);
            Assert.IsTrue(controller.Usuarios.Count == originalStrength);
            controller.eliminarUsuario(usuario.dni);
        }

        [Test]
        public void registrar_nuevo_usuario_con_nombre_vacio()
        {
            UsuariosController controller = new UsuariosController();
            Models.Usuario usuario = new Models.Usuario();
            usuario.nombre = String.Empty;
            usuario.apellidos = "García Rodríguez";
            usuario.fechaNacimiento = new DateTime(1971, 10, 21);
            usuario.dni = "12345667A";

            var originalStrength = controller.Usuarios.Count;

            String result = controller.nuevoUsuario(usuario);
            Assert.AreEqual("El nombre no puede estar vacío", result);
            Assert.IsTrue(controller.Usuarios.Count == originalStrength);
            controller.eliminarUsuario(usuario.dni);
        }

        [Test]
        public void registrar_nuevo_usuario_sin_apellidos_informados()
        {
            UsuariosController controller = new UsuariosController();
            Models.Usuario usuario = new Models.Usuario();
            usuario.nombre = "Javier";
            usuario.fechaNacimiento = new DateTime(1971, 10, 21);
            usuario.dni = "12345667A";

            var originalStrength = controller.Usuarios.Count;

            String result = controller.nuevoUsuario(usuario);
            Assert.AreEqual("El apellido no puede ser nulo", result);
            Assert.IsTrue(controller.Usuarios.Count == originalStrength);
            controller.eliminarUsuario(usuario.dni);
        }

        [Test]
        public void registrar_nuevo_usuario_con_apellidos_vacio()
        {
            UsuariosController controller = new UsuariosController();
            Models.Usuario usuario = new Models.Usuario();
            usuario.nombre = "Javier";
            usuario.apellidos = String.Empty;
            usuario.fechaNacimiento = new DateTime(1971, 10, 21);
            usuario.dni = "12345667A";

            var originalStrength = controller.Usuarios.Count;

            String result = controller.nuevoUsuario(usuario);
            Assert.AreEqual("El apellido no puede estar vacío", result);
            Assert.IsTrue(controller.Usuarios.Count == originalStrength);
            controller.eliminarUsuario(usuario.dni);
        }

        [Test]
        public void registrar_nuevo_usuario_sin_fechaNacimiento_informada()
        {
            UsuariosController controller = new UsuariosController();
            Models.Usuario usuario = new Models.Usuario();
            usuario.nombre = "Javier";
            usuario.apellidos = "García Rodríguez";
            usuario.dni = "12345667A";

            var originalStrength = controller.Usuarios.Count;

            String result = controller.nuevoUsuario(usuario);
            Assert.AreEqual("La fecha de nacimiento no puede ser nula", result);
            Assert.IsTrue(controller.Usuarios.Count == originalStrength);
            controller.eliminarUsuario(usuario.dni);
        }

        [Test]
        public void registrar_nuevo_usuario_sin_dni_informado()
        {
            UsuariosController controller = new UsuariosController();
            Usuario usuario = InicializarUsuario("Javier", "García Rodríguez", new DateTime(1981, 08, 12), null);
            var originalStrength = controller.Usuarios.Count;

            String result = controller.nuevoUsuario(usuario);
            Assert.AreEqual("El dni no puede ser nulo", result);
            Assert.IsTrue(controller.Usuarios.Count == originalStrength);
            controller.eliminarUsuario(usuario.dni);
        }

        [Test]
        public void registrar_nuevo_usuario_con_dni_vacio()
        {
            UsuariosController controller = new UsuariosController();
            Usuario usuario = InicializarUsuario("Javier", "García Rodríguez", new DateTime(1981, 08, 12), String.Empty);
            var originalStrength = controller.Usuarios.Count;

            String result = controller.nuevoUsuario(usuario);
            Assert.AreEqual("El dni no puede estar vacío", result);
            Assert.IsTrue(controller.Usuarios.Count == originalStrength);
            controller.eliminarUsuario(usuario.dni);
        }

        [Test]
        public void registrar_nuevo_usuario_longitud_dni_incorrecta()
        {
            UsuariosController controller = new UsuariosController();
            Usuario usuario = InicializarUsuario("Javier", "García Rodríguez", new DateTime(1981, 08, 12), "1234564");
            var originalStrength = controller.Usuarios.Count;

            String result = controller.nuevoUsuario(usuario);
            Assert.AreEqual("La longitud del dni no es correcta", result);
            Assert.IsTrue(controller.Usuarios.Count == originalStrength);
            controller.eliminarUsuario(usuario.dni);
        }

        [Test]
        public void registrar_nuevo_usuario_dni_letra_no_valida()
        {
            UsuariosController controller = new UsuariosController();
            Usuario usuario = InicializarUsuario("Javier", "García Rodríguez", new DateTime(1981, 08, 12), "12345687A");
            var originalStrength = controller.Usuarios.Count;

            String result = controller.nuevoUsuario(usuario);
            Assert.AreEqual("La letra del dni no es correcta", result);
            Assert.IsTrue(controller.Usuarios.Count == originalStrength);
            controller.eliminarUsuario(usuario.dni);
        }

        [Test]
        public void registrar_nuevo_usuario_dni_formato_no_valida()
        {
            UsuariosController controller = new UsuariosController();
            Usuario usuario = InicializarUsuario("Javier", "García Rodríguez", new DateTime(1981, 08, 12), "1234568A7");
            var originalStrength = controller.Usuarios.Count;

            String result = controller.nuevoUsuario(usuario);
            Assert.AreEqual("El formato del dni no es correcto", result);
            Assert.IsTrue(controller.Usuarios.Count == originalStrength);
            controller.eliminarUsuario(usuario.dni);
        }

        private Usuario InicializarUsuario(String nombre, String apellido, DateTime fechaNacimiento, String dni)
        {
            Models.Usuario usuario = new Models.Usuario();
            usuario.nombre = nombre;
            usuario.apellidos = apellido;
            usuario.fechaNacimiento = fechaNacimiento;
            usuario.dni = dni;
            return usuario;
        }

        [Test]
        public void eliminar_usuario_por_dni_no_existente()
        {
            UsuariosController controller = new UsuariosController();
            Usuario usuario = InicializarUsuario("Javier", "García Rodríguez", new DateTime(1981, 08, 12), "1234");

            String result = controller.eliminarUsuario(usuario.dni);

            Assert.AreEqual("Usuario no se puede eliminar porque no existe en la base de datos", result);
        }

        [Test]
        public void eliminar_usuario_bbdd()
        {
            UsuariosController controller = new UsuariosController();
            Usuario usuario = InicializarUsuario("Javier", "García Rodríguez", new DateTime(1981, 08, 12), "12345667A");

            var originalStrength = controller.Usuarios.Count;

            controller.nuevoUsuario(usuario);

            Assert.IsTrue(controller.Usuarios.Count > originalStrength, "No se ha añadido el elemento a la lista");

            String result = controller.eliminarUsuario(usuario.dni);
            Assert.IsTrue(controller.Usuarios.Count == originalStrength, "pasa algo");
            Assert.AreEqual("Usuario eliminado correctamente", result);
        }

        [Test]
        public void comprobar_get_valor_enum_por_descripcion()
        {
            string message = "Usuario eliminado correctamente";
            ResultCodes result = Utils.Utils.GetValueFromDescription<ResultCodes>(message);
            Assert.AreEqual(result, ResultCodes.EDS_USUARIO_DELETED_CORRECT);
        }

        [Test]
        public void get_descripcion_enum_sin_atributo()
        {
            UsuariosController controller = new UsuariosController();
            string result = controller.ResultCodeDescription(ResultCodes.EDS_ERROR);
            Assert.AreEqual(result, ResultCodes.EDS_ERROR.ToString());
        }

        [Test]
        public void get_usuario_detalles_vista()
        {
            UsuariosController controller = new UsuariosController();

            Usuario usuario = InicializarUsuario("Javier", "García Rodríguez", new DateTime(1981, 08, 12), "12345667A");
            controller.nuevoUsuario(usuario);

            var result = controller.Details("12345667A") as PartialViewResult;
            var usuarioVista = (Usuario)result.ViewData.Model;

            controller.eliminarUsuario(usuario.dni);

            Assert.AreEqual("Javier", usuarioVista.nombre);
            Assert.AreEqual("García Rodríguez", usuarioVista.apellidos);
            Assert.AreEqual(new DateTime(1981, 08, 12), usuarioVista.fechaNacimiento);
        }

        [Test]
        public void get_usuarios_detalles_no_existente()
        {
            UsuariosController controller = new UsuariosController();

            var result = (RedirectToRouteResult)controller.Details("12345667A");
            Assert.AreEqual("Index", result.RouteValues["action"]);

        }

        [Test]
        public void get_usuarios_detalle_vista_editar()
        {
            UsuariosController controller = new UsuariosController();

            Usuario usuario = InicializarUsuario("Javier", "García Rodríguez", new DateTime(1981, 08, 12), "12345667A");
            controller.nuevoUsuario(usuario);

            var result = controller.Edit(usuario.id) as ViewResult;
            var usuarioVista = (Usuario)result.ViewData.Model;

            controller.eliminarUsuario(usuario.dni);

            Assert.AreEqual("Javier", usuarioVista.nombre);
            Assert.AreEqual("García Rodríguez", usuarioVista.apellidos);
            Assert.AreEqual(new DateTime(1981, 08, 12), usuarioVista.fechaNacimiento);
        }

        [Test]
        public void get_usuarios_detalle_vista_editar_no_existente()
        {
            UsuariosController controller = new UsuariosController();

            var result = (RedirectToRouteResult)controller.Edit(-1);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [Test]
        public void editar_usuario_bbdd()
        {
            UsuariosController controller = new UsuariosController();
            Usuario usuario = InicializarUsuario("Javier", "García Rodríguez", new DateTime(1981, 08, 12), "12345667A");
            Console.WriteLine("id: " + usuario.id);

            controller.nuevoUsuario(usuario);

            usuario.nombre = "Nuevo nombre";
            usuario.apellidos = "Nuevos apellidos";
            usuario.fechaNacimiento = new DateTime(1990, 01, 01);
            usuario.dni = "19806538L";
            Console.WriteLine("id: " + usuario.id);

            String result = controller.editarUsuario(usuario);
            Assert.AreEqual("Usuario modificado correctamente", result);

            controller.eliminarUsuario(usuario.dni);
        }

        [Test]
        public void editar_usuario_con_nombre_vacio()
        {
            UsuariosController controller = new UsuariosController();
            Usuario usuario = InicializarUsuario("Javier", "García Rodríguez", new DateTime(1981, 08, 12), "12345667A");
            Console.WriteLine("id: " + usuario.id);

            controller.nuevoUsuario(usuario);

            usuario.nombre = String.Empty;

            String result = controller.editarUsuario(usuario);
            Assert.AreEqual("El nombre no puede estar vacío", result);

            controller.eliminarUsuario(usuario.dni);
        }

        [Test]
        public void editar_usuario_sin_nombre_informado()
        {
            UsuariosController controller = new UsuariosController();
            Usuario usuario = InicializarUsuario("Javier", "García Rodríguez", new DateTime(1981, 08, 12), "12345667A");
            Console.WriteLine("id: " + usuario.id);

            controller.nuevoUsuario(usuario);

            usuario.nombre = null;

            String result = controller.editarUsuario(usuario);
            Assert.AreEqual("El nombre no puede ser nulo", result);

            controller.eliminarUsuario(usuario.dni);
        }

        [Test]
        public void editar_usuario_con_apellido_vacio()
        {
            UsuariosController controller = new UsuariosController();
            Usuario usuario = InicializarUsuario("Javier", "García Rodríguez", new DateTime(1981, 08, 12), "12345667A");
            Console.WriteLine("id: " + usuario.id);

            controller.nuevoUsuario(usuario);

            usuario.apellidos = String.Empty;

            String result = controller.editarUsuario(usuario);
            Assert.AreEqual("El apellido no puede estar vacío", result);

            controller.eliminarUsuario(usuario.dni);
        }

        [Test]
        public void editar_usuario_sin_apellido_informado()
        {
            UsuariosController controller = new UsuariosController();
            Usuario usuario = InicializarUsuario("Javier", "García Rodríguez", new DateTime(1981, 08, 12), "12345667A");
            Console.WriteLine("id: " + usuario.id);

            controller.nuevoUsuario(usuario);

            usuario.apellidos = null;

            String result = controller.editarUsuario(usuario);
            Assert.AreEqual("El apellido no puede ser nulo", result);

            controller.eliminarUsuario(usuario.dni);
        }

        [Test]
        public void editar_usuario_sin_dni_informado()
        {
            UsuariosController controller = new UsuariosController();
            Usuario usuario = InicializarUsuario("Javier", "García Rodríguez", new DateTime(1981, 08, 12), "12345667A");
            Console.WriteLine("id: " + usuario.id);

            controller.nuevoUsuario(usuario);

            usuario.dni = null;

            String result = controller.editarUsuario(usuario);
            Assert.AreEqual("El dni no puede ser nulo", result);
            controller.eliminarUsuario("12345667A");
        }

        [Test]
        public void editar_usuario_con_dni_vacio()
        {
            UsuariosController controller = new UsuariosController();
            Usuario usuario = InicializarUsuario("Javier", "García Rodríguez", new DateTime(1981, 08, 12), "12345667A");
            Console.WriteLine("id: " + usuario.id);

            controller.nuevoUsuario(usuario);

            usuario.dni = String.Empty;

            String result = controller.editarUsuario(usuario);
            Assert.AreEqual("El dni no puede estar vacío", result);
            controller.eliminarUsuario("12345667A");
        }

        [Test]
        public void editar_usuario_longitud_dni_incorrecta()
        {
            UsuariosController controller = new UsuariosController();
            Usuario usuario = InicializarUsuario("Javier", "García Rodríguez", new DateTime(1981, 08, 12), "12345667A");
            Console.WriteLine("id: " + usuario.id);

            controller.nuevoUsuario(usuario);

            usuario.dni = "1234";

            String result = controller.editarUsuario(usuario);
            Assert.AreEqual("La longitud del dni no es correcta", result);
            controller.eliminarUsuario("12345667A");
        }

        [Test]
        public void editar_usuario_dni_letra_no_valida()
        {
            UsuariosController controller = new UsuariosController();
            Usuario usuario = InicializarUsuario("Javier", "García Rodríguez", new DateTime(1981, 08, 12), "12345667A");
            Console.WriteLine("id: " + usuario.id);

            controller.nuevoUsuario(usuario);

            usuario.dni = "12345678A";

            String result = controller.editarUsuario(usuario);
            Assert.AreEqual("La letra del dni no es correcta", result);
            controller.eliminarUsuario("12345667A");
        }

        [Test]
        public void editar_usuario_dni_formato_no_valida()
        {
            UsuariosController controller = new UsuariosController();
            Usuario usuario = InicializarUsuario("Javier", "García Rodríguez", new DateTime(1981, 08, 12), "12345667A");
            Console.WriteLine("id: " + usuario.id);

            controller.nuevoUsuario(usuario);

            usuario.dni = "123A5678A";

            String result = controller.editarUsuario(usuario);
            Assert.AreEqual("El formato del dni no es correcto", result);
            controller.eliminarUsuario("12345667A");
        }

        [Test]
        public void editar_usuario_dni_duplicado()
        {
            UsuariosController controller = new UsuariosController();
            Usuario usuario = InicializarUsuario("Javier", "García Rodríguez", new DateTime(1981, 08, 12), "12345667A");
            Usuario usuario2 = InicializarUsuario("Javier2", "García Rodríguez2", new DateTime(1981, 08, 12), "11729873B");


            controller.nuevoUsuario(usuario);
            controller.nuevoUsuario(usuario2);

            usuario.dni = "11729873B";

            String result = controller.editarUsuario(usuario);
            Assert.AreEqual("Usuario existente", result);

            controller.eliminarUsuario("12345667A");
            controller.eliminarUsuario(usuario2.dni);
        }

        [Test]
        public void indice_nombre_vista()
        {
            UsuariosController controller = new UsuariosController();
            var result = controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);

        }

        [Test]
        public void formato_fecha_nacimiento_usuario()
        {
            Usuario usuario = new Usuario();
            usuario.fechaNacimiento = new DateTime(1991, 5, 30);
            String fechaFormato = usuario.dateFormat;
            Assert.AreEqual("30/05/1991", fechaFormato);
        }

        [Test]
        public void create_nombre_vista()
        {
            UsuariosController controller = new UsuariosController();
            var result = controller.Create() as ViewResult;
            Assert.AreEqual("Create", result.ViewName);
        }

        [Test]
        public void delete_nombre_vista_usuario_no_existente()
        {
            UsuariosController controller = new UsuariosController();
            var result = controller.Delete("12345667A") as RedirectToRouteResult;
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [Test]
        public void delete_nombre_vista_usuario_existente()
        {
            UsuariosController controller = new UsuariosController();
            Usuario usuario = InicializarUsuario("Javier", "García Rodríguez", new DateTime(1981, 08, 12), "12345667A");

            controller.nuevoUsuario(usuario);
            var result = controller.Delete("12345667A") as RedirectToRouteResult;
            Assert.AreEqual("Index", result.RouteValues["action"]);
            controller.eliminarUsuario(usuario.dni);
        }
    }
}
