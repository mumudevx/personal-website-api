using Core.DataAccess.EntityFramework;
using DataAccess.Context;
using Entities;

namespace DataAccess.Repositories;

public interface IBlogPostRepository : IRepository<BlogPost>;

public class BlogPostRepository(PersonalWebsiteContext context) : Repository<BlogPost>(context);