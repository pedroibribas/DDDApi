using System.ComponentModel.DataAnnotations.Schema;

namespace Entities;
/// <summary>
/// Configura o modelo Message com data annotations.
/// <para>
/// As data annotations são atributos que servem para o mapeamento da entidade para a base de dados.
/// Elas sobrescrevem as conventions, e são sobrescritas pela configuração da Fluent API.
/// </para>
/// <para>
/// Ver mais sobre a configuração do mapeamento das entidades em
/// <see href="https://learn.microsoft.com/en-us/ef/core/modeling/">
/// Creating and Configuring a Model.
/// </see>
/// </para>
/// </summary>
public class Message : Notifier
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }

    public string? UserId { get; set; }
    public virtual User? User { get; set; }
}
