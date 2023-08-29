using System.ComponentModel.DataAnnotations.Schema;

namespace Entities;

/// <summary>
/// Configura o modelo Notifier com data annotations.
/// <para>
/// A data annotation [NotMapped] evita que o tipo ou propriedade seja incluído no modelo EF Core.
/// </para>
/// </summary>
public class Notifier
{
    [NotMapped]
    public string? PropertyName { get; set; }

    [NotMapped]
    public string? Message { get; set; }

    [NotMapped]
    public List<Notifier>? Notifiers { get; set; }

    public Notifier()
    {
        Notifiers = new List<Notifier>();
    }

    public bool ValidateStringProperty(string propertyName, string value)
    {
        if (string.IsNullOrEmpty(propertyName) || string.IsNullOrEmpty(value))
        {
            Notifiers?.Add(new Notifier()
            {
                PropertyName = propertyName,
                Message = "PropertyName and Value required."
            });
            return false;
        }
        return true;
    }

    public bool ValidateIntProperty(string propertyName, int value)
    {
        if (string.IsNullOrEmpty(propertyName) || value < 1)
        {
            Notifiers?.Add(new Notifier()
            {
                PropertyName = propertyName,
                Message = "PropertyName and Value required."
            });
            return false;
        }
        return true;
    }
}
