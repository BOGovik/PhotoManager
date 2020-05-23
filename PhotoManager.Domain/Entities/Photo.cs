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
        [Required(ErrorMessage = "Please enter a photo name")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        [Required(ErrorMessage = "Please enter a photo description")]
        public string Description { get; set; }


        [Display(Name = "Album")]
        [Required(ErrorMessage = "Please enter a album")]
        public string Category { get; set; }

        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
        public string UserId { get; set; }
        public string Place { get; set; }
        public string Camera { get; set; }
        public string FocalLength { get; set; }
        public string Diaphragm { get; set; }
        public string CameraLockSpeed { get; set; }
        public string ISO { get; set; }

    }
}
