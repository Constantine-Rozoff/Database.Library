using ContextAndModels.Models;
using Database.Library.Entity;
using LibraryAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryAPI.Models;

namespace LibraryAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly LibraryContext _libraryContext;
    private readonly ITokenService _tokenService;

    public AccountController(LibraryContext libraryContext, ITokenService tokenService)
    {
        _libraryContext = libraryContext;
        _tokenService = tokenService;
    }

    [HttpPost("register/{userType}")]
    [AllowAnonymous]
    public async Task<ActionResult> Register(string userType, AccountRegisterDto.AccounRegisterDto registerDto)
    {
        if (await _libraryContext.Employees.AnyAsync(u => u.Login == registerDto.login))
        {
            return BadRequest();
        }

        Employee employee = new Employee();

        if (userType == "Reader")
        {
            Reader reader = new Reader()
            {
                Login = registerDto.login,
                Password = registerDto.password,
                Email = registerDto.email,
                FirstName = registerDto.Name,
                LastName = registerDto.LastName,
                DocumentTypeId = registerDto.DocumentId,
                DocumentNumber = registerDto.DocumentNumber
            };
            await _libraryContext.Readers.AddAsync(reader);
            employee = reader;
        }
        else if (userType == "Employee")
        {
            employee = new Employee()
            {
                Login = registerDto.login,
                Password = registerDto.password,
                Email = registerDto.email,
            };
            await _libraryContext.Employees.AddAsync(employee);
        }
        else
        {
            return BadRequest("Invalid user type.");
        }


        var token = _tokenService.CreateToken(employee);
        await _libraryContext.SaveChangesAsync();

        return Ok(new AccountLoginResultDto.ACcountLoginResultDto(employee.Login, token));
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(AccountLoginDto.AccounLoginDto loginDto)
    {
        var user = await _libraryContext.Employees.SingleOrDefaultAsync(u => u.Login == loginDto.login);
        if (user == null)
        {
            return NotFound();
        }

        if (user.Password != loginDto.password)
        {
            return Unauthorized();
        }

        var token = _tokenService.CreateToken(user);
        return Ok((new AccountLoginResultDto.ACcountLoginResultDto(user.Login, token)));
    }
}