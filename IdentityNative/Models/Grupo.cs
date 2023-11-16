namespace IdentityNative.Models
{
    public class Grupo
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public virtual List<Permissao> Permissoes { get; set;} = new List<Permissao>();
    }
}
