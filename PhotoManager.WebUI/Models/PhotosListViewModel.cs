using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PhotoManager.Domain.Entities;

namespace PhotoManager.WebUI.Models
{
    public class PhotosListViewModel
    {
        public IEnumerable<Photo> Photos { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}