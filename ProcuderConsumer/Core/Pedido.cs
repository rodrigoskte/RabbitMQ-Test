namespace Core;

public class Pedido
{
    public int Id { get; set; }
    public Usuario Usuario { get; set; }
    public DateTime DataCriacao { get; set; }

    public Pedido(int id, Usuario usuario, DateTime dataCriacao)
    {
        Id = id;
        Usuario = usuario;
        DataCriacao = dataCriacao;
    }
    
    public override string ToString()
        => $"Id: {Id}, " +
           $"Usuario: {Usuario}, " +
           $"Data de Criação: {DataCriacao:dd/MM/yyyy}";
}