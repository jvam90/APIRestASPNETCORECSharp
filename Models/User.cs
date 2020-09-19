using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesafioPitang.Models
{
    public class User
    {
        /*
        Classe User, possui entre seus atributos, referências com a classe Phone para denotar o relacionamento um para muitos
        (um usuário possui vários telefones), além de Data de Criação (CreatedAt), último login (LastLogin) e o token para a autenticação
        */
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public List<Phone> Phones { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        [NotMapped]
        public string Token { get; set; }
    }
}