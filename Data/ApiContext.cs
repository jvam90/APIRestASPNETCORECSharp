using DesafioPitang.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioPitang.Data
{
    public class ApiContext : DbContext
    {
        //Classe API Context - Usada para criar uma 'ponte' entre repositório e banco de dados
        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {

        }

        //DBsets de usuários e telefones de cada usuário
        public DbSet<User> Users { get; set; }
        public DbSet<Phone> Phones { get; set; }
    }
}