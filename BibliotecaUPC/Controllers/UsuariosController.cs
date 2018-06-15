using BibliotecaUPC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using BibliotecaUPC.Utils;

namespace BibliotecaUPC.Controllers
{
    public enum ResultCodes : uint
    {
        EDS_ERROR = 999,
        [Description("Usuario registrado con éxito")]
        EDS_USUARIO_INSERT_CORRECT = 0,
        [Description("Usuario existente")]
        EDS_ERR_USUARIO_DUPLICADO = 1,
        [Description("El nombre no puede ser nulo")]
        EDS_ERR_NULL_NOMBRE = 2,
        [Description("El nombre no puede estar vacío")]
        EDS_ERR_EMPTY_NOMBRE = 3,
        [Description("El apellido no puede ser nulo")]
        EDS_ERR_NULL_APELLIDO = 4,
        [Description("El apellido no puede estar vacío")]
        EDS_ERR_EMPTY_APELLIDO = 5,
        [Description("La fecha de nacimiento no puede ser nula")]
        EDS_ERR_NULL_FECHANACIMIENTO = 6,
        [Description("La longitud del dni no es correcta")]
        EDS_ERR_LONGITUD_DNI = 7,
        [Description("El dni no puede ser nulo")]
        EDS_ERR_NULL_DNI = 8,
        [Description("El dni no puede estar vacío")]
        EDS_ERR_EMPTY_DNI = 9,
        [Description("La letra del dni no es correcta")]
        EDS_ERR_LETRA_DNI = 10,
        [Description("El formato del dni no es correcto")]
        EDS_ERR_FORMATO_DNI = 11,
        [Description("Usuario eliminado correctamente")]
        EDS_USUARIO_DELETED_CORRECT = 12,
        [Description("Usuario no se puede eliminar porque no existe en la base de datos")]
        EDS_ERR_USUARIO_NO_EXISTENTE = 13,
        [Description("Usuario modificado correctamente")]
        EDS_USUARIO_EDITED_CORRECT = 14
    }


    public class UsuariosController : Controller
    {
        private List<Usuario> usuarios;
        public List<Usuario> Usuarios { get => usuarios; set => usuarios = value; }

        public UsuariosController()
        {
            using (BibliotecaUPCEntities context = new BibliotecaUPCEntities())
            {
                Usuarios = new List<Usuario>();
                Usuarios = context.Usuarios.ToList();
            }
        }

        public ActionResult Index()
        {
            return View("Index",Usuarios);
        }

        public ActionResult Edit(int id)
        {
            using (BibliotecaUPCEntities context = new BibliotecaUPCEntities())
            {
                Usuario usuario = context.Usuarios.FirstOrDefault(u => u.id == id);
                if (usuario != null)
                {
                    return View("Edit",usuario);
                }
                else
                  return RedirectToAction("Index");
            }
        }

        public ActionResult Details(string dni)
        {
            using (BibliotecaUPCEntities context = new BibliotecaUPCEntities())
            {
                Usuario usuario = context.Usuarios.FirstOrDefault(u => u.dni == dni);
                if (usuario != null)
                {
                    return PartialView("_Details", usuario);
                }
                else
                    return RedirectToAction("Index");
            }
        }

        public ActionResult Create()
        {
            return View("Create");
        }

        public ActionResult Delete(String dni)
        {
            string message = eliminarUsuario(dni);
            ResultCodes result = Utils.Utils.GetValueFromDescription<ResultCodes>(message);

            if (result == ResultCodes.EDS_USUARIO_DELETED_CORRECT)
            {
                TempData["okMessage"] = message;
            }
            else
            {
                TempData["error"] = message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                string message = editarUsuario(usuario);
                ResultCodes result = Utils.Utils.GetValueFromDescription<ResultCodes>(message);

                if (result == ResultCodes.EDS_USUARIO_EDITED_CORRECT)
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
                return View(usuario);
            }
        }

        [HttpPost]
        public ActionResult Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                string message = nuevoUsuario(usuario);
                ResultCodes result = Utils.Utils.GetValueFromDescription<ResultCodes>(message);

                if (result == ResultCodes.EDS_USUARIO_INSERT_CORRECT)
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
                return View(usuario);
            }
        }

        public String editarUsuario(Usuario usuario)
        {
            using (BibliotecaUPCEntities context = new BibliotecaUPCEntities())
            {
                ResultCodes result = verificarCamposUsuario(usuario, true);
                if (result == ResultCodes.EDS_USUARIO_EDITED_CORRECT)
                {
                    //Falta validar los campos del usuaario
                    var usuarioEditar = context.Usuarios.Single(u => u.id == usuario.id);


                    context.Entry(usuarioEditar).CurrentValues.SetValues(usuario);
                    context.SaveChanges();
                }
                return ResultCodeDescription(result);
            }
        }

        public String nuevoUsuario(Usuario usuario)
        {
            using (BibliotecaUPCEntities context = new BibliotecaUPCEntities())
            {
              
                Console.WriteLine(Usuarios.Count);
              
                ResultCodes result  = verificarCamposUsuario(usuario, false);

                if(result == ResultCodes.EDS_USUARIO_INSERT_CORRECT)
                {
                    context.Usuarios.Add(usuario);
                    context.SaveChanges();
                    Usuarios = context.Usuarios.ToList();
                }
              
                Console.WriteLine(ResultCodeDescription(result));
                return ResultCodeDescription(result);
            }
        }

        private ResultCodes verificarCamposUsuario(Usuario usuario, bool isEditUsuario)
        {
            Usuario duplicado;
            //Comprobar que no existe el usuario con el DNI indicado
            if (isEditUsuario)
            {
                 duplicado = Usuarios.FirstOrDefault(u => u.dni == usuario.dni && u.id != usuario.id);
            }
            else
                 duplicado = Usuarios.FirstOrDefault(u => u.dni == usuario.dni);

            if (duplicado != null)
            {
                return ResultCodes.EDS_ERR_USUARIO_DUPLICADO;
            }
            else if (usuario.nombre == null)
            {
                return ResultCodes.EDS_ERR_NULL_NOMBRE;
            }
            else if (usuario.nombre == String.Empty)
            {
                return ResultCodes.EDS_ERR_EMPTY_NOMBRE;
            }
            else if (usuario.apellidos == null)
            {
                return ResultCodes.EDS_ERR_NULL_APELLIDO;
            }
            else if (usuario.apellidos == String.Empty)
            {
                return ResultCodes.EDS_ERR_EMPTY_APELLIDO;
            }
            else if (usuario.fechaNacimiento == null)
            {
                return ResultCodes.EDS_ERR_NULL_FECHANACIMIENTO;
            }
            else if(usuario.dni == null)
            {
                return ResultCodes.EDS_ERR_NULL_DNI;
            }
            else if(usuario.dni == String.Empty)
            {
                return ResultCodes.EDS_ERR_EMPTY_DNI;
            }
            else if (usuario.dni.Length != 9)
            {
                return ResultCodes.EDS_ERR_LONGITUD_DNI;
            }
            else if (!validarFormatoDni(usuario.dni))
            {
                return ResultCodes.EDS_ERR_FORMATO_DNI;
            }
            else if(!validarLetraDni(usuario.dni))
            {
                return ResultCodes.EDS_ERR_LETRA_DNI;
            }
            else if(isEditUsuario)
            {
                return ResultCodes.EDS_USUARIO_EDITED_CORRECT;
            }
            else{
                   return ResultCodes.EDS_USUARIO_INSERT_CORRECT;
                
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

        private const string correspondencia = "TRWAGMYFPDXBNJZSQVHLCKET";

        private bool validarLetraDni(String dni)
        {
            string initialLetter = string.Empty;
            string controlDigit = string.Empty;
            int dniNumber;
            bool result = false;
            
            Int32.TryParse(dni.Substring(0, 8), out dniNumber);
            controlDigit = dni.LastOrDefault().ToString();

            // Check letter.
            if (controlDigit == GetNIFLetter(dniNumber)) result = true;

            return result;
        }

        private string GetNIFLetter(int numeroDNI)
        {
            int indice = numeroDNI % 23;
            return correspondencia[indice].ToString();
        }

        private bool validarFormatoDni(String dni)
        {
            bool result = false;
            result = System.Text.RegularExpressions.Regex.IsMatch(dni, @"[0-9]{8,10}[" + correspondencia + "]$");
            return result;
        }

        public string eliminarUsuario(String dni)
        {
            using (BibliotecaUPCEntities context = new BibliotecaUPCEntities())
            {
                Usuario eliminar = context.Usuarios.Where(u => u.dni == dni).FirstOrDefault();
                ResultCodes result = new ResultCodes();

                if (eliminar != null)
                {
                    context.Usuarios.Remove(eliminar);
                    int i = context.SaveChanges();
                    Usuarios = context.Usuarios.ToList();
                    if (i > 0) result = ResultCodes.EDS_USUARIO_DELETED_CORRECT;
                }   
                else
                {
                    // El usuario no existe
                    result = ResultCodes.EDS_ERR_USUARIO_NO_EXISTENTE;
                }

                return ResultCodeDescription(result);
            }
        }
    }
}