namespace TakingElano.Domain.Entities;

public class Item
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Desconto { get; private set; } = 0;

    public decimal TotalComDesconto => (Quantidade * PrecoUnitario) - Desconto;

    // Calcula desconto com base nas regras de negócio
    public void CalcularDesconto()
    {
        if (Quantidade > 20)
        {
            throw new InvalidOperationException("Não é permitido vender mais de 20 itens iguais.");
        }

        if (Quantidade >= 10)
        {
            Desconto = (Quantidade * PrecoUnitario) * 0.20m; // 20% de desconto
        }
        else if (Quantidade > 4)
        {
            Desconto = (Quantidade * PrecoUnitario) * 0.10m; // 10% de desconto
        }
        else
        {
            Desconto = 0; // Sem desconto
        }
    }
}
