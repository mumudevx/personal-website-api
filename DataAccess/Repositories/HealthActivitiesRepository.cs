using Core.DataAccess.EntityFramework;
using DataAccess.Context;
using Entities;

namespace DataAccess.Repositories;

public interface IHealthActivitiesRepository : IRepository<HealthActivity>;

public class HealthActivityRepository(PersonalWebsiteContext context) : Repository<HealthActivity>(context);