using Microsoft.EntityFrameworkCore;
using Repository.Entities;

namespace Repository.Persistence;

public interface IApplicationDbContext
{
    public DbSet<AppUser> AppUsers { get;  }
    public DbSet<PostCategory> PostCategories { get;  }
    public DbSet<Post> Posts { get;  }
}