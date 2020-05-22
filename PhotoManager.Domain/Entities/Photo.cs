using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PhotoManager.Domain.Entities
{
    public class Photo
    {
        [HiddenInput(DisplayValue = false)]
        public int PhotoId { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }


        [Display(Name = "Description")]
        public string Description { get; set; }


        [Display(Name = "Album")]
        public string Category { get; set; }
    }
}
