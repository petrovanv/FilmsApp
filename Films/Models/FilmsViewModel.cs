using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Films.Models
{
    public class FilmsViewModel
    {
        public List<Film> Films { get; set; }
    }

    public class FilmMetaData
    {
        [Required]
        public string Name { get; set; }
    }


    [MetadataType(typeof(FilmMetaData))]
    public partial class Film
    {
    }
}