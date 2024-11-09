using Core.DataAccess.EntityFramework;
using DataAccess.Context;
using Entities;

namespace DataAccess.Repositories;

public interface IImageFileRepository : IRepository<ImageFile>;

public class ImageFileRepository(PersonalWebsiteContext context) : Repository<ImageFile>(context);