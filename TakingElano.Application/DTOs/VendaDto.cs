namespace TakingElano.Application.DTOs;

public class VendaDto
{
    public int Id { get; set; }
    public DateTime Data { get; set; }
    public List<ItemDto> Itens { get; set; } = new List<ItemDto>();
    public decimal Total { get; set; }
}
