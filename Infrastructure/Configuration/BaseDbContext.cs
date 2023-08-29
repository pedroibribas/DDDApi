using Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configuration;

/// <summary>
/// Classe DbContext que expõe DbSets para cada entidade e lida 
/// com a configuração do provedor de base de dados e
/// a configuração do mapeamento das entidades para a base de dados.
/// <para>
/// A instância DbContext deve ser configurada para usar um único provedor de base de dados.
/// </para>
/// <para>
/// O mapeamento da entidade em DbSet<T> permite que o EF Core
/// faça operações da entidade na base de dados via migration.
/// </para>
/// <para>
/// Ver mais sobre a configuração do provedor de base de dados em
/// <see href="https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/">
/// DbContext configuration and initialization.
/// </see>
/// </para>
/// <para>
/// Ver mais sobre a configuração do mapeamento das entidades em
/// <see href="https://learn.microsoft.com/en-us/ef/core/modeling/">
/// Creating and Configuring a Model.
/// </para>
/// </summary>
public class BaseDbContext : IdentityDbContext<User>
{
    /// <summary>
    /// Expõe o DbSet de User para o EF Core.
    /// </summary>
    public DbSet<User>? User { get; set; }
    /// <summary>
    /// Expõe o DbSet de Message para o EF Core.
    /// </summary>
    public DbSet<Message>? Message { get; set; }

    /// <summary>
    /// Expõe construtor público para que a configuração de contexto do
    /// seja passado ao <see cref="DbContext"/> via DI.
    /// </summary>
    /// <param name="options">DbContextOptionsBuilder</param>
    public BaseDbContext(DbContextOptions<BaseDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Método que sobrescreve a configuração DbContextOptionsBuilder
    /// injetada via DI, no AddDbContext.
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(
                "Server=localhost; Port=5432; User Id=postgres; Password=pedro; Database=AspNetApi");
        }
    }
    /// <summary>
    /// Fluent API para expôr e configurar uma entidade, sobrescrevendo
    /// qualquer configuração prévia, como data annotations ou convenções,
    /// sem modificar a entidade.
    /// <param name="builder"></param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        new MessageEntityTypeConfiguration().Configure(builder.Entity<Message>());
        new UserEntityTypeConfiguration().Configure(builder.Entity<User>());
        base.OnModelCreating(builder);
    }
}
