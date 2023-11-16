using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityNative.Models
{
    public class Login
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public bool Ativo { get; set; }
        public List<string> Error { get; set; } =  new List<string>();
    }
}
