using Entities.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities;

public class User : IdentityUser
{
    public AccessLevel AccessLevel { get; set; }
}
