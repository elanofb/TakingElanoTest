namespace TakingElano.Application.DTOs;

public class ItemDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Desconto { get; set; }
    public decimal TotalComDesconto { get; set; }
}
