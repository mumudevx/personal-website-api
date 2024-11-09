using Core.DataAccess.EntityFramework;
using DataAccess.Context;
using Entities;

namespace DataAccess.Repositories;

public interface IPhotoItemRepository : IRepository<PhotoItem>;

public class PhotoItemRepository(PersonalWebsiteContext context) : Repository<PhotoItem>(context);