using Core.DataAccess.EntityFramework;
using DataAccess.Context;
using Entities;

namespace DataAccess.Repositories;

public interface IBookRepository : IRepository<Book>;

public class BookRepository(PersonalWebsiteContext context) : Repository<Book>(context);