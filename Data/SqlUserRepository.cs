using System;
using System.Collections.Generic;
using System.Linq;
using DesafioPitang.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioPitang.Data
{
    public class SqlUserRepository : IUserRepository
    {
        private readonly ApiContext _context;

        public SqlUserRepository(ApiContext context)
        {
            _context = context;
        }

        //Método para criar o usuário, salvando a data de criação
        public void CreateUser(User user)
        {
            user.CreatedAt = DateTime.Now;
            _context.Add(user);
        }

        //Método auxiliar para recuperar todos os usuários (para testes da API)
        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.Include(u => u.Phones).ToList();
        }

        //Método para recuperar o usuário pelo email e senha
        public User GetUser(string Email, string Password)
        {
            return _context.Users.Include(u => u.Phones).FirstOrDefault(p => p.Email == Email && p.Password == Password);
        }

        //Método auxiliar para recuperar o usuário apenas pelo email (para validação da API)
        public User GetUserByEmail(string Email)
        {
            return _context.Users.FirstOrDefault(p => p.Email.ToUpper() == Email.ToUpper());
        }

        //Método auxiliar para salvar as mudanças do contexto
        public bool SaveChanges()
        {
            //Quando se muda os dados do contexto, até chamarmos o método savechanges de context, ele não salva
            return (_context.SaveChanges() >= 0);
        }
    }
}