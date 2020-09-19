using System.ComponentModel.DataAnnotations;

namespace DesafioPitang.Models
{
    public class Phone
    {

        /*
        Classe Phone, possui entre seus atributos, referências com a classe User para denotar o relacionamento um para muitos
        (um usuário possui vários telefones)
        */
        public int Id { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public int AreaCode { get; set; }
        [Required]
        public string CountryCode { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}