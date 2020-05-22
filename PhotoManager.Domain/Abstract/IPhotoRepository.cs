using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoManager.Domain.Entities;

namespace PhotoManager.Domain.Abstract
{
    public interface IPhotoRepository
    {
        IEnumerable<Photo> Photos { get; }
        void SavePhoto(Photo photo);
    }
}
