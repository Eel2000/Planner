using Microsoft.EntityFrameworkCore;
using Planner.core.Models;

namespace Planner.core.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {

    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<WorkSpace> WorkSpaces { get; set; }
    public virtual DbSet<Project> Projects { get; set; }
    public virtual DbSet<Collaborator> Collaborators { get; set; }
    public virtual DbSet<ToDo> ToDos { get; set; }
    public virtual DbSet<Tag> Tags { get; set; }

}
