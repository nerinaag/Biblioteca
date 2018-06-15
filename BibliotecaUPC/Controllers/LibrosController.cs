using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using BibliotecaUPC.Models;
using BibliotecaUPC.Utils;

namespace BibliotecaUPC.Controllers
{
    public class LibrosController : Controller
    {
        public enum ResultCodes : uint
        {
            EDS_ERROR = 999,
            [Description("Libro registrado con éxito")]
            EDS_LIBRO_INSERT_CORRECT = 0,
            [Description("El libro ya ha sido registrado, diríjare al menú Registrar Ejemplar")]
            EDS_ERR_LIBRO_DUPLICADO = 1,
            [Description("El título no puede ser nulo")]
            EDS_ERR_NULL_TITULO = 2,
            [Description("El título no puede estar vacío")]
            EDS_ERR_EMPTY_TITULO = 3,
            [Description("El autor no puede ser nulo")]
            EDS_ERR_NULL_AUTOR = 4,
            [Description("El autor no puede estar vacío")]
            EDS_ERR_EMPTY_AUTOR = 5,
            [Description("La editorial no puede ser nula")]
            EDS_ERR_NULL_EDITORIAL = 6,
            [Description("La editorial no puede estar vacía")]
            EDS_ERR_EMPTY_EDITORIAL = 7,
            [Description("El ISBN no puede ser nulo")]
            EDS_ERR_NULL_ISBN = 8,
            [Description("El ISBN no puede estar vacío")]
            EDS_ERR_EMPTY_ISBN = 9,
            [Description("El ISBN no tiene un formato correcto")]
            EDS_ERR_ISBN_INCORRECT = 10,
            [Description("Libro modificado correctamente")]
            EDS_LIBRO_EDITED_CORRECT = 11
        }

        private List<Libro> libros;
        public List<Libro> Libros { get => libros; set => libros = value; }

        public LibrosController()
        {
            using (BibliotecaUPCEntities context = new BibliotecaUPCEntities())
            {
                Libros = new List<Libro>();
                Libros = context.Libros.ToList();
            }
        }

        // GET: Libros
        public ActionResult Index()
        {
            return View("Index", Libros);
        }

        [HttpPost]
        public ActionResult Create(Libro libro)
        {
            if (ModelState.IsValid)
            {
                string message = nuevoLibro(libro);
                ResultCodes result = Utils.Utils.GetValueFromDescription<ResultCodes>(message);

                if (result == ResultCodes.EDS_LIBRO_INSERT_CORRECT)
                {
                    TempData["okMessage"] = message;
                }
                else
                {
                    TempData["error"] = message;
                }

                return RedirectToAction("Index");
            }
            else
            {
                return View(libro);
            }
        }

        public string nuevoLibro(Libro libro)
        {
            using (BibliotecaUPCEntities context = new BibliotecaUPCEntities())
            {
                ResultCodes result = verificarCamposLibro(libro, false);

                if (result == ResultCodes.EDS_LIBRO_INSERT_CORRECT)
                {
                    context.Libros.Add(libro);
                    context.SaveChanges();
                    Libros = context.Libros.ToList();
                }

                return ResultCodeDescription(result);
            }
        }

        private ResultCodes verificarCamposLibro(Libro libro, bool isEditLibro)
        {
            Libro duplicado;
            //Comprobar que no existe el libro con el ISBN indicado
            if (isEditLibro)
            {
                duplicado = Libros.FirstOrDefault(l => l.ISBN == libro.ISBN && l.id != libro.id);
            }
            else
                duplicado = Libros.FirstOrDefault(l => l.ISBN == libro.ISBN);
            
            if (duplicado != null)
            {
                return ResultCodes.EDS_ERR_LIBRO_DUPLICADO;
            }
            else if(libro.titulo == null)
            {
                return ResultCodes.EDS_ERR_NULL_TITULO;
            }
            else if(libro.titulo == String.Empty)
            {
                return ResultCodes.EDS_ERR_EMPTY_TITULO;
            }
            else if(libro.autor == null)
            {
                return ResultCodes.EDS_ERR_NULL_AUTOR;
            }
            else if(libro.autor == String.Empty)
            {
                return ResultCodes.EDS_ERR_EMPTY_AUTOR;
            }
            else if(libro.editorial == null)
            {
                return ResultCodes.EDS_ERR_NULL_EDITORIAL;
            }
            else if(libro.editorial == String.Empty)
            {
                return ResultCodes.EDS_ERR_EMPTY_EDITORIAL;
            }
            else if(libro.ISBN == null)
            {
                return ResultCodes.EDS_ERR_NULL_ISBN;
            }
            else if(libro.ISBN == String.Empty)
            {
                return ResultCodes.EDS_ERR_EMPTY_ISBN;
            }
            else if(!validarISBN(libro.ISBN))
            {
                return ResultCodes.EDS_ERR_ISBN_INCORRECT;
            }
            else if (isEditLibro)
            {
                return ResultCodes.EDS_LIBRO_EDITED_CORRECT;
            }
            else
            {
                return ResultCodes.EDS_LIBRO_INSERT_CORRECT;
            }
        }

        public bool validarISBN(String isbn)
        {
            String regex = "^(?:ISBN(?:-13)?:? )?(?=[0-9]{13}$|(?=(?:[0-9]+[- ]){4})[- 0-9]{17}$)97[89][- ]?[0-9]{1,5}[- ]?[0-9]+[- ]?[0-9]+[- ]?[0-9]$";

            return Regex.IsMatch(isbn, regex, RegexOptions.IgnoreCase);
        }

        public void eliminarLibro(string iSBN)
        {
            using (BibliotecaUPCEntities context = new BibliotecaUPCEntities())
            {
                Libro eliminar = context.Libros.Where(l => l.ISBN == iSBN).FirstOrDefault();
              
                if (eliminar != null)
                {
                    context.Libros.Remove(eliminar);
                    int i = context.SaveChanges();
                }
            }
        }

        //Obtiene la descripción de un valor de la enumeración ResultCodes
        public string ResultCodeDescription(Enum ResultCode)
        {
            FieldInfo rc = ResultCode.GetType().GetField(ResultCode.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])rc.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return ResultCode.ToString();
            }
        }

        public ActionResult Create()
        {
            return View("Create");
        }

        public ActionResult Edit(int id)
        {
            using (BibliotecaUPCEntities context = new BibliotecaUPCEntities())
            {
                Libro libro = context.Libros.FirstOrDefault(u => u.id == id);
                if (libro != null)
                {
                    return View("Edit", libro);
                }
                else
                    return RedirectToAction("Index");
            }
        }
        
        [HttpPost]
        public ActionResult Edit(Libro libro)
        {
            if (ModelState.IsValid)
            {
                string message = editarLibro(libro);
                ResultCodes result = Utils.Utils.GetValueFromDescription<ResultCodes>(message);

                if (result == ResultCodes.EDS_LIBRO_EDITED_CORRECT)
                {
                    TempData["okMessage"] = message;
                }
                else
                {
                    TempData["error"] = message;
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View(libro);
            }
        }

        public String editarLibro(Libro libro)
        {
            using (BibliotecaUPCEntities context = new BibliotecaUPCEntities())
            {
                ResultCodes result = verificarCamposLibro(libro, true);
                if (result == ResultCodes.EDS_LIBRO_EDITED_CORRECT)
                {
                    var libroEditar = context.Libros.Single(l => l.id == libro.id);


                    context.Entry(libroEditar).CurrentValues.SetValues(libro);
                    context.SaveChanges();
                }
                return ResultCodeDescription(result);
            }
        }
    }
}