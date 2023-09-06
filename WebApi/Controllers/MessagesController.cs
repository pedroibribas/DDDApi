using AutoMapper;
using Domain.DTOs;
using Domain.Interfaces.Services;
using Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

/// <summary>
/// Uma ControllerBase ASP.NET Core que roteia o acesso à entidade Message.
/// <para>
/// O controlador é responsável por rotear o acesso às entidades e chamar
/// os serviços, que são os responsáveis pela efetivação do CRUD na base de dados.
/// </para>
/// </summary>  
[Route("api/[controller]")]
[ApiController]
public class MessagesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMessageService _messageService;

    public MessagesController(IMapper mapper, IMessageService messageService)
    {
        _mapper = mapper;
        _messageService = messageService;
    }

    /// <summary>Recupera uma mensagem da base de dados pelo <c>id</c>.
    /// <para>Uso: <c>GET api/Messages?id={id}</c></para>
    /// </summary>
    /// <returns>Um mensagem.</returns>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(string), 401)]
    [ProducesResponseType(typeof(string), 404)]
    [ProducesResponseType(typeof(string), 500)]
    public async Task<IActionResult> GetMessageById([FromQuery] string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id)) 
                throw new ArgumentNullException(nameof(id));

            var message = await _messageService.GetMessageById(Int32.Parse(id));
            
            if (message == null)
            {
                return NotFound($"A mensagem não foi encontrada; Id enviado: {id}");
            }

            return Ok(message);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex);
        }
        catch (Exception ex)
        {
            return Problem(ex.ToString());
        }
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("List")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 401)]
    [ProducesResponseType(typeof(string), 404)]
    [ProducesResponseType(typeof(string), 500)]
    public IActionResult GetUserMessages()
    {
        try
        {
            var userId = User.Claims.First(claim => claim.Type == "id").Value
                ?? throw new NullReferenceException($"Nenhum 'userId' foi obtido.");

            var messages = _messageService.ListMessagesByUserId(userId)
                ?? throw new KeyNotFoundException($"Nenhuma lista de mensagens foi encontrada para o usuário; UserId = {userId}");

            return Ok(messages);
        }
        catch (NullReferenceException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem(ex.ToString());
        }
    }

    /// <summary>Cria uma nova mensagem na base de dados.
    /// <para>Uso: <c>POST api/Messages/CreateMessage</c></para>
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("CreateMessage")]
    [ProducesResponseType(typeof(string), 201)]
    [ProducesResponseType(typeof(string), 401)]
    [ProducesResponseType(typeof(string), 404)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 500)]
    public async Task<IActionResult> CreateMessage(MessageDTO messageDTO)
    {
        try
        {
            if (string.IsNullOrEmpty(messageDTO.Title))
                throw new ArgumentNullException(
                    nameof(messageDTO.Title),
                    $"O campo de título está vazio ou nulo");

            var message = new Message
            {
                Title = messageDTO.Title,
                CreatedAt = DateTime.UtcNow,
                Active = true,
                UserId = GetLoggedUserId()
            };

            await _messageService.AddMessage(message);

            return CreatedAtAction(
                nameof(GetMessageById),
                new { id = "[UnderConstruction]" },
                message);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex);
        }
        catch (Exception ex)
        {
            return Problem(ex.ToString());
        }
    }

    /// <summary>Deleta uma mensagem da base de dados pelo <c>id</c>.
    /// <para>Uso: <c>DEL api/Messages?id={id}</c></para>
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpDelete("")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(string), 401)]
    [ProducesResponseType(typeof(string), 404)]
    [ProducesResponseType(typeof(string), 500)]
    public async Task<IActionResult> DeleteMessage([FromQuery] string id)
    {
        try
        {
            await _messageService.DeleteMessage(id);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex);
        }
        catch (Exception ex)
        {
            return Problem(ex.ToString());
        }
    }

    /// <summary>
    /// Método utilitário para obter o <c>id</c> do usuário da requisição.
    /// <para>O acesso aos dados do usuário é feito via <c>ControllerBase.User.</c></para>
    /// </summary>
    /// <returns><c>UserId</c></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    private string GetLoggedUserId()
    {
        if (User == null)
            throw new ArgumentNullException("Nenhuma declaração User detectada na MessagesController.");

        var userId = User.FindFirst("id") ??
            throw new KeyNotFoundException(
                $"Não foi possível recuperar o id do usuário na declaração User.");

        return userId.Value;
    }
}
