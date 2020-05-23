using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoManager.Domain.Entities;
using PhotoManager.Domain.Abstract;

namespace PhotoManager.Domain.Concrete
{
    public class EFPhotoRepository : IPhotoRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Photo> Photos
        {
            get { return context.Photos; }
        }

        public void SavePhoto(Photo photo)
        {
            if (photo.PhotoId == 0)
                context.Photos.Add(photo);
            else
            {
                Photo dbEntry = context.Photos.Find(photo.PhotoId);
                if (dbEntry != null)
                {
                    dbEntry.Name = photo.Name;
                    dbEntry.Description = photo.Description;
                    dbEntry.Category = photo.Category;
                    dbEntry.ImageData = photo.ImageData;
                    dbEntry.ImageMimeType = photo.ImageMimeType;
                    dbEntry.Place = photo.Place;
                    dbEntry.Camera = photo.Camera;
                    dbEntry.FocalLength = photo.FocalLength;
                    dbEntry.Diaphragm = photo.Diaphragm;
                    dbEntry.CameraLockSpeed = photo.CameraLockSpeed;
                    dbEntry.ISO = photo.ISO;
                    dbEntry.UserId = photo.UserId;
                }
            }
            context.SaveChanges();
        }

        public Photo DeletePhoto(int photoId)
        {
            Photo dbEntry = context.Photos.Find(photoId);
            if (dbEntry != null)
            {
                context.Photos.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
