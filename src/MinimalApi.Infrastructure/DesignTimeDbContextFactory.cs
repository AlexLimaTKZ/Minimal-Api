using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MinimalApi.Infrastructure.DB;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DbContexto>
{
    public DbContexto CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DbContexto>();
        optionsBuilder.UseMySql("Server=localhost;Database=minimal_api;Uid=root;Pwd=Zeni@5Nezu.;", ServerVersion.AutoDetect("Server=localhost;Database=minimal_api;Uid=root;Pwd=Zeni@5Nezu.;"));

        return new DbContexto(optionsBuilder.Options);
    }
}
