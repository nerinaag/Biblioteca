using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BibliotecaUPC.Models
{
    /// <summary>
    /// partial class definition to associate the ORM generated class
    /// NO NEED to add the properties here.
    /// </summary>
    [MetadataType(typeof(LibroAnnotation))]
    public partial class Libro
    {
        
    }

    /// <summary>
    /// Buddy Class or Data Annotation Class
    /// Add the properties here with the associated annotations
    /// </summary>
    internal sealed class LibroAnnotation
    {
        [Required]
        [DisplayName("Autor")]
        public string autor { get; set; }

        [Required]
        [DisplayName("Título")]
        public string titulo { get; set; }

        [Required]
        [DisplayName("Editorial")]
        public string editorial { get; set; }

        [Required]
        [DisplayName("ISBN")]
        public string ISBN { get; set; }
    }
}