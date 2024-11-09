using Core.DataAccess.EntityFramework;
using DataAccess.Context;
using Entities;

namespace DataAccess.Repositories;

public interface IPhotoGalleryRepository : IRepository<PhotoGallery>;

public class PhotoGalleryRepository(PersonalWebsiteContext context) : Repository<PhotoGallery>(context);