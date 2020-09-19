using System;
using System.Collections.Generic;
using DesafioPitang.Data;
using DesafioPitang.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesafioPitang.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserRepository repository;

        //Passando o repositório por injeção de dependência
        public UsersController(IUserRepository repo)
        {
            repository = repo;
        }

        [HttpGet]//Método get para recuperar todos os usuários (para testes da API)
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            var users = repository.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("signin")]//rota signin que verifica por usuário e senha, se o usuário existe e se os campos foram fornecidos
        public ActionResult<User> SignIn(string Email, string Password)
        {
            if (string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Password))
            {
                return BadRequest(new { message = "Missing Fields", errorCode = 400 });
            }
            var user = repository.GetUser(Email, Password);
            if (user == null)
            {
                return NotFound(new { message = "Invalid e-mail or password", errorCode = 404 });
            }
            // Gera o Token
            var token = TokenService.GenerateToken(user);
            user.Token = token;
            return Ok(user);
        }

        [HttpPost("signup")]//Rota signup para cadastro do usuário
        public ActionResult<User> SignUp(User user)
        {
            var u = repository.GetUserByEmail(user.Email);
            if (u != null)
            {
                return BadRequest(new { message = "E-mail already exists", errorCode = 400 });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Missing Fields", errorCode = 400 });
            }
            if (!IsValidEmail(user.Email))
            {
                return BadRequest(new { message = "Invalid Fields", errorCode = 400 });
            }
            repository.CreateUser(user);
            repository.SaveChanges();
            return Ok(new { message = "User Created", errorCode = 201 });
        }

        [HttpGet]
        [Route("me")]
        [Authorize]
        public string Authenticated() => String.Format("Autenticado - {0}", User.Identity.Name);

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

    }
}