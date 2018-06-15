using System;
using System.Web.Mvc;
using BibliotecaUPC.Controllers;
using BibliotecaUPC.Models;
using NUnit.Framework;

namespace BibliotecaUPC.Tests.Controllers
{
    [TestFixture]
    public class LibrosControllerTest
    {
        [Test]
        public void get_index_nombre_vista_libros()
        {
            LibrosController controller = new LibrosController();
            var result = controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }

        [Test]
        public void registrar_nuevo_libro_mensaje_correcto()
        {
            LibrosController controller = new LibrosController();
            Libro libro = InicializarLibro("CATHERINE NIXEY", "TAURUS", "9788430619542", "La edad de la penumbra");

            String result = controller.nuevoLibro(libro);

            Assert.AreEqual("Libro registrado con éxito", result);
            controller.eliminarLibro(libro.ISBN);
        }

        [Test]
        public void registrar_nuevo_libro_en_bbdd()
        {
            LibrosController controller = new LibrosController();
            Libro libro = InicializarLibro("CATHERINE NIXEY", "TAURUS", "9788430619542", "La edad de la penumbra");

            var originalStrength = controller.Libros.Count;
            Console.WriteLine(originalStrength);

            controller.nuevoLibro(libro);
            Console.WriteLine(controller.Libros.Count);

            Assert.IsTrue(controller.Libros.Count > originalStrength, "No se ha añadido el elemento a la lista");

            controller.eliminarLibro(libro.ISBN);
        }

        [Test]
        public void registrar_nuevo_libro_ISBN_existente()
        {
            LibrosController controller = new LibrosController();
            Libro libro = InicializarLibro("CATHERINE NIXEY", "TAURUS", "9788430619542", "La edad de la penumbra");
            Libro libro2 = InicializarLibro("CATHERINE NIXEY", "TAURUS", "9788430619542", "La edad de la penumbra");

            String result = controller.nuevoLibro(libro);
            String result2 = controller.nuevoLibro(libro);
            Assert.AreEqual("Libro registrado con éxito", result);
            Assert.AreEqual("El libro ya ha sido registrado, diríjare al menú Registrar Ejemplar", result2);
            controller.eliminarLibro(libro.ISBN);
        }

        [Test]
        public void registrar_nuevo_libro_sin_titulo_informado()
        {
            LibrosController controller = new LibrosController();
            Libro libro = InicializarLibro("CATHERINE NIXEY", "TAURUS", "9788430619542", null);

            var originalStrength = controller.Libros.Count;

            String result = controller.nuevoLibro(libro);

            Assert.AreEqual("El título no puede ser nulo", result);
            Assert.IsTrue(controller.Libros.Count == originalStrength);
            controller.eliminarLibro(libro.ISBN);
        }

        [Test]
        public void registrar_nuevo_libro_titulo_vacio()
        {
            LibrosController controller = new LibrosController();
            Libro libro = InicializarLibro("CATHERINE NIXEY", "TAURUS", "9788430619542", String.Empty);
            var originalStrength = controller.Libros.Count;

            String result = controller.nuevoLibro(libro);

            Assert.AreEqual("El título no puede estar vacío", result);
            Assert.IsTrue(controller.Libros.Count == originalStrength);
            controller.eliminarLibro(libro.ISBN);
        }

        [Test]
        public void registrar_nuevo_libro_sin_autor_informado()
        {
            LibrosController controller = new LibrosController();
            Libro libro = InicializarLibro(null, "TAURUS", "9788430619542", "La edad de la penumbra");

            var originalStrength = controller.Libros.Count;

            String result = controller.nuevoLibro(libro);

            Assert.AreEqual("El autor no puede ser nulo", result);
            Assert.IsTrue(controller.Libros.Count == originalStrength);
            controller.eliminarLibro(libro.ISBN);
        }

        [Test]
        public void registrar_nuevo_libro_autor_vacio()
        {
            LibrosController controller = new LibrosController();
            Libro libro = InicializarLibro(String.Empty, "TAURUS", "9788430619542", "La edad de la penumbra");
            var originalStrength = controller.Libros.Count;

            String result = controller.nuevoLibro(libro);

            Assert.AreEqual("El autor no puede estar vacío", result);
            Assert.IsTrue(controller.Libros.Count == originalStrength);
            controller.eliminarLibro(libro.ISBN);
        }

        [Test]
        public void registrar_nuevo_libro_sin_editorial_informada()
        {
            LibrosController controller = new LibrosController();
            Libro libro = InicializarLibro("CATHERINE NIXEY", null, "9788430619542", "La edad de la penumbra");

            var originalStrength = controller.Libros.Count;

            String result = controller.nuevoLibro(libro);

            Assert.AreEqual("La editorial no puede ser nula", result);
            Assert.IsTrue(controller.Libros.Count == originalStrength);
            controller.eliminarLibro(libro.ISBN);
        }

        [Test]
        public void registrar_nuevo_libro_editorial_vacia()
        {
            LibrosController controller = new LibrosController();
            Libro libro = InicializarLibro("CATHERINE NIXEY", String.Empty, "9788430619542", "La edad de la penumbra");
            var originalStrength = controller.Libros.Count;

            String result = controller.nuevoLibro(libro);

            Assert.AreEqual("La editorial no puede estar vacía", result);
            Assert.IsTrue(controller.Libros.Count == originalStrength);
            controller.eliminarLibro(libro.ISBN);
        }

        [Test]
        public void registrar_nuevo_libro_sin_ISBN_informado()
        {
            LibrosController controller = new LibrosController();
            Libro libro = InicializarLibro("CATHERINE NIXEY", "TAURUS", null, "La edad de la penumbra");

            var originalStrength = controller.Libros.Count;

            String result = controller.nuevoLibro(libro);

            Assert.AreEqual("El ISBN no puede ser nulo", result);
            Assert.IsTrue(controller.Libros.Count == originalStrength);
            controller.eliminarLibro(libro.ISBN);
        }

        [Test]
        public void registrar_nuevo_libro_ISBN_vacio()
        {
            LibrosController controller = new LibrosController();
            Libro libro = InicializarLibro("CATHERINE NIXEY", "TAURUS", String.Empty, "La edad de la penumbra");
            var originalStrength = controller.Libros.Count;

            String result = controller.nuevoLibro(libro);

            Assert.AreEqual("El ISBN no puede estar vacío", result);
            Assert.IsTrue(controller.Libros.Count == originalStrength);
            controller.eliminarLibro(libro.ISBN);
        }

        [Test]
        public void registrar_nuevo_libro_ISBN_formato_no_valido()
        {
            LibrosController controller = new LibrosController();
            Libro libro = InicializarLibro("CATHERINE NIXEY", "TAURUS", "ISBN 11978-0-596-52068-7", "La edad de la penumbra");
            var originalStrength = controller.Libros.Count;

            String result = controller.nuevoLibro(libro);

            Assert.AreEqual("El ISBN no tiene un formato correcto", result);
            Assert.IsTrue(controller.Libros.Count == originalStrength);
            controller.eliminarLibro(libro.ISBN);
        }

        [Test]
        public void registrar_nuevo_libro_ISBN_formato_no_valido_2()
        {
            LibrosController controller = new LibrosController();
            Libro libro = InicializarLibro("CATHERINE NIXEY", "TAURUS", "ISBN - 12: 978 - 0 - 596 - 52068 - 7", "La edad de la penumbra");
            var originalStrength = controller.Libros.Count;

            String result = controller.nuevoLibro(libro);

            Assert.AreEqual("El ISBN no tiene un formato correcto", result);
            Assert.IsTrue(controller.Libros.Count == originalStrength);
            controller.eliminarLibro(libro.ISBN);
        }

        [Test]
        public void registrar_nuevo_libro_ISBN_formato_no_valido_3()
        {
            LibrosController controller = new LibrosController();
            Libro libro = InicializarLibro("CATHERINE NIXEY", "TAURUS", "978 10 596 52068 7", "La edad de la penumbra");
            var originalStrength = controller.Libros.Count;

            String result = controller.nuevoLibro(libro);

            Assert.AreEqual("El ISBN no tiene un formato correcto", result);
            Assert.IsTrue(controller.Libros.Count == originalStrength);
            controller.eliminarLibro(libro.ISBN);
        }

        [Test]
        public void registrar_nuevo_libro_ISBN_formato_no_valido_4()
        {
            LibrosController controller = new LibrosController();
            Libro libro = InicializarLibro("CATHERINE NIXEY", "TAURUS", "119780596520687", "La edad de la penumbra");
            var originalStrength = controller.Libros.Count;

            String result = controller.nuevoLibro(libro);

            Assert.AreEqual("El ISBN no tiene un formato correcto", result);
            Assert.IsTrue(controller.Libros.Count == originalStrength);
            controller.eliminarLibro(libro.ISBN);
        }

        public Libro InicializarLibro(String autor, String editorial, string ISBN, String titulo)
        {
            Libro libro = new Libro();
            libro.autor = autor;
            libro.editorial = editorial;
            libro.ISBN = ISBN;
            libro.titulo = titulo;
            return libro;
        }

        [Test]
        public void create_nombre_vista()
        {
           LibrosController controller = new LibrosController();
            var result = controller.Create() as ViewResult;
            Assert.AreEqual("Create", result.ViewName);
        }

        [Test]
        public void get_libros_detalle_vista_editar()
        {
            LibrosController controller = new LibrosController();

            Libro libro = InicializarLibro("CATHERINE NIXEY", "TAURUS", "9788430619542", "La edad de la penumbra");
            controller.nuevoLibro(libro);

            var result = controller.Edit(libro.id) as ViewResult;
            var libroVista = (Libro)result.ViewData.Model;

            controller.eliminarLibro(libro.ISBN);

            Assert.AreEqual("CATHERINE NIXEY", libroVista.autor);
            Assert.AreEqual("TAURUS", libroVista.editorial);
            Assert.AreEqual("9788430619542", libroVista.ISBN);
            Assert.AreEqual("La edad de la penumbra", libroVista.titulo);
        }

        [Test]
        public void editar_libro_ISBN_formato_no_valido_4()
        {
            LibrosController controller = new LibrosController();
            Libro libro = InicializarLibro("CATHERINE NIXEY", "TAURUS", "9788430619542", "La edad de la penumbra");

            controller.nuevoLibro(libro);
            libro.ISBN = "119780596520687";
            String result = controller.editarLibro(libro);

            Assert.AreEqual("El ISBN no tiene un formato correcto", result);
            controller.eliminarLibro("9788430619542");
        }

        [Test]
        public void editar_libro_ISBN_formato_no_valido_3()
        {
            LibrosController controller = new LibrosController();
            Libro libro = InicializarLibro("CATHERINE NIXEY", "TAURUS", "9788430619542", "La edad de la penumbra");
            
            controller.nuevoLibro(libro);
            libro.ISBN = "978 10 596 52068 7";
            String result = controller.editarLibro(libro);

            Assert.AreEqual("El ISBN no tiene un formato correcto", result);
            controller.eliminarLibro("9788430619542");
        }

        [Test]
        public void editar_libro_ISBN_formato_no_valido_2()
        {
            LibrosController controller = new LibrosController();
            Libro libro = InicializarLibro("CATHERINE NIXEY", "TAURUS", "9788430619542", "La edad de la penumbra");

            controller.nuevoLibro(libro);
            libro.ISBN = "ISBN - 12: 978 - 0 - 596 - 52068 - 7";

            String result = controller.editarLibro(libro);

            Assert.AreEqual("El ISBN no tiene un formato correcto", result);
            controller.eliminarLibro("9788430619542");
        }

        [Test]
        public void editar_libro_ISBN_formato_no_valido()
        {
            LibrosController controller = new LibrosController();
            Libro libro = InicializarLibro("CATHERINE NIXEY", "TAURUS", "9788430619542", "La edad de la penumbra");

            controller.nuevoLibro(libro);
            libro.ISBN = "ISBN 11978-0-596-52068-7";

            String result = controller.editarLibro(libro);

            Assert.AreEqual("El ISBN no tiene un formato correcto", result);
            controller.eliminarLibro("9788430619542");
        }

        [Test]
        public void editar_libro_ISBN_vacio()
        {
            LibrosController controller = new LibrosController();
            Libro libro = InicializarLibro("CATHERINE NIXEY", "TAURUS", "9788430619542", "La edad de la penumbra");

            controller.nuevoLibro(libro);
            libro.ISBN = String.Empty;

            String result = controller.editarLibro(libro);

            Assert.AreEqual("El ISBN no puede estar vacío", result);
            controller.eliminarLibro("9788430619542");
        }

        [Test]
        public void editar_libro_sin_ISBN_informado()
        {
            LibrosController controller = new LibrosController();
            Libro libro = InicializarLibro("CATHERINE NIXEY", "TAURUS", "9788430619542", "La edad de la penumbra");

            controller.nuevoLibro(libro);
            libro.ISBN = null;

            String result = controller.editarLibro(libro);

            Assert.AreEqual("El ISBN no puede ser nulo", result);
            controller.eliminarLibro("9788430619542");
        }

        [Test]
        public void editar_libro_editorial_vacia()
        {
            LibrosController controller = new LibrosController();
            Libro libro = InicializarLibro("CATHERINE NIXEY", "TAURUS", "9788430619542", "La edad de la penumbra");

            controller.nuevoLibro(libro);
            libro.editorial = String.Empty;

            String result = controller.editarLibro(libro);

            Assert.AreEqual("La editorial no puede estar vacía", result);
            controller.eliminarLibro(libro.ISBN);
        }

        [Test]
        public void editar_libro_sin_editorial_informada()
        {
            LibrosController controller = new LibrosController();
            Libro libro = InicializarLibro("CATHERINE NIXEY", "TAURUS", "9788430619542", "La edad de la penumbra");

            controller.nuevoLibro(libro);
            libro.editorial = null;

            String result = controller.editarLibro(libro);

            Assert.AreEqual("La editorial no puede ser nula", result);
            controller.eliminarLibro(libro.ISBN);
        }

        [Test]
        public void editar_libro_autor_vacio()
        {
            LibrosController controller = new LibrosController();
            Libro libro = InicializarLibro("CATHERINE NIXEY", "TAURUS", "9788430619542", "La edad de la penumbra");

            controller.nuevoLibro(libro);
            libro.autor = String.Empty;

            String result = controller.editarLibro(libro);

            Assert.AreEqual("El autor no puede estar vacío", result);
            controller.eliminarLibro(libro.ISBN);
        }

        [Test]
        public void editar_libro_sin_autor_informado()
        {
            LibrosController controller = new LibrosController();
            Libro libro = InicializarLibro("CATHERINE NIXEY", "TAURUS", "9788430619542", "La edad de la penumbra");

            controller.nuevoLibro(libro);
            libro.autor = null;

            String result = controller.editarLibro(libro);

            Assert.AreEqual("El autor no puede ser nulo", result);
            controller.eliminarLibro(libro.ISBN);
        }

        [Test]
        public void editar_libro_titulo_vacio()
        {
            LibrosController controller = new LibrosController();
            Libro libro = InicializarLibro("CATHERINE NIXEY", "TAURUS", "9788430619542", "La edad de la penumbra");

            controller.nuevoLibro(libro);
            libro.titulo = String.Empty;

            String result = controller.editarLibro(libro);

            Assert.AreEqual("El título no puede estar vacío", result);
            controller.eliminarLibro(libro.ISBN);
        }

        [Test]
        public void editar_libro_sin_titulo_informado()
        {
            LibrosController controller = new LibrosController();
            Libro libro = InicializarLibro("CATHERINE NIXEY", "TAURUS", "9788430619542", "La edad de la penumbra");

            controller.nuevoLibro(libro);
            libro.titulo = null;

            String result = controller.editarLibro(libro);

            Assert.AreEqual("El título no puede ser nulo", result);
            controller.eliminarLibro(libro.ISBN);
        }

        [Test]
        public void editar_libro_bbdd()
        {
            LibrosController controller = new LibrosController();
            Libro libro = InicializarLibro("CATHERINE NIXEY", "TAURUS", "9788430619542", "La edad de la penumbra");

            controller.nuevoLibro(libro);

            libro.titulo = "Nuevo titulo";
            libro.autor = "Nuevos autor";
            libro.editorial = "Nueva editorial";
            libro.ISBN = "9788430619544";

            String result = controller.editarLibro(libro);
            Assert.AreEqual("Libro modificado correctamente", result);

            controller.eliminarLibro(libro.ISBN);
        }
    }
}
