using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BibliotecaUPC.Models
{

    /// <summary>
    /// partial class definition to associate the ORM generated class
    /// NO NEED to add the properties here.
    /// </summary>
    [MetadataType(typeof(UsuarioAnnotation))]
    public partial class Usuario
    {
        [DisplayName("Fecha de nacimiento")]
        [Required]
        public string dateFormat
        {
            get
            {
                return fechaNacimiento.Value.ToString("dd/MM/yyyy");
            }
        }
    }

    /// <summary>
    /// Buddy Class or Data Annotation Class
    /// Add the properties here with the associated annotations
    /// </summary>
    internal sealed class UsuarioAnnotation
    {
        [Required]
        [DisplayName("Nombre")]
        public string nombre { get; set; }

        [Required]
        [DisplayName("Apellidos")]
        public string apellidos { get; set; }

        [Required]
        [DisplayName("Fecha de nacimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> fechaNacimiento { get; set; }

        [Required]
        [DisplayName("Dni")]
        public string dni { get; set; }
    }
}