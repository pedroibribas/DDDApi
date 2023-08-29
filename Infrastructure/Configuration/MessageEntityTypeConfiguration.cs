using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

/// <summary>
/// Classe <c>IEntityTypeConfiguration</c> que define as configurações do tipo da entidade
/// para a Fluent API, retirando a responsabilidade de definir as configurações do tipo
/// no modelo usando data annotations.
/// <para>
/// O tipo da entidade é a classe da entidade (e.g. User).
/// </para>
/// <para>
/// A propriedade da entidade são as propriedades da classe (e.g. Id).
/// Caso a propriedade não esteja declarada na classe, ela é criada pela configuração.
/// </para>
/// <para>Refs:</para>
/// <para>
/// <see href="https://learn.microsoft.com/en-us/ef/core/modeling/#use-fluent-api-to-configure-a-model">
/// Use Fluent API to Configure a Model
/// </see>
/// </para>
/// <para>
/// <see href="https://learn.microsoft.com/en-us/ef/core/modeling/entity-types">
/// Tipos de entidade
/// </see>
/// </para>
/// </para>
/// <para>
/// <see href="https://learn.microsoft.com/en-us/ef/core/modeling/entity-properties">
/// Propriedades da entidade
/// </see>
/// </para>
/// <para>
/// <see href=https://www.postgresqltutorial.com/postgresql-tutorial/postgresql-identity-column/">
/// PostgreSQL Identity Column
/// </see>
/// </para>
/// <para>
/// <see href=https://learn.microsoft.com/en-us/ef/ef6/modeling/code-first">
/// EF6
/// </see>
/// </para>
/// </summary>
public class MessageEntityTypeConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        // Entity Type

        // Relação one-to-many sem navegação do principal para os dependentes
        // https://learn.microsoft.com/en-us/ef/core/modeling/relationships/one-to-many#one-to-many-without-navigation-to-dependents
        builder
            .ToTable("Messages")
            .HasOne(m => m.User)
            .WithMany()
            .HasForeignKey(m => m.UserId)
            .IsRequired()
            .HasPrincipalKey(m => m.Id);

        // Entity Properties

        builder
            .Property(m => m.Id)
            .HasColumnName("Id")
            .UseIdentityAlwaysColumn()
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(m => m.Title)
            .HasColumnName("Title");

        builder
            .Property(m => m.Active)
            .HasColumnName("Active");

        builder
            .Property(m => m.CreatedAt)
            .HasColumnName("CreatedAt");

        builder
            .Property(m => m.UserId)
            .HasColumnName("UserId")
            .HasColumnOrder(1);
    }
}
