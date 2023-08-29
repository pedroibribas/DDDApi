using Microsoft.AspNetCore.Identity;

namespace WebApi.Mappers;

/// <summary>
/// Esta classe está em construção.
/// </summary>
public class IdentityResultMapper
{
    private readonly Dictionary<string, string> ErrorsMapping = new()
    {
        { IdentityResultErrorCode.PasswordTooShort.ToString(), "Senhas devem ter ao menos 6 caracteres." }
    };

    private IEnumerable<string> MapIdentityResultErrors(IEnumerable<IdentityError> identityResultErrors)
    {
        List<string> errors = new() { };

        foreach (var error in identityResultErrors)
        {
            if (!ErrorsMapping.ContainsKey(error.Code))
                throw new ArgumentOutOfRangeException("IdentityError não mapeado.");

            errors.Add(ErrorsMapping[error.Code]);
        }

        return errors;
    }
}

public enum IdentityResultErrorCode
{
    PasswordTooShort
}

