using System.Collections.Generic;
using DesafioPitang.Models;

namespace DesafioPitang.Data
{
    public interface IUserRepository
    {
        //Interface IUserRepository, usada para especificar os métodos a serem implementados pelo repositório (SqlUserRepository)
        bool SaveChanges();
        IEnumerable<User> GetAllUsers();
        User GetUser(string Email, string Password);
        void CreateUser(User user);
        User GetUserByEmail(string Email);

    }
}