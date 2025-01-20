namespace TakingElano.Domain.Entities{

    public class Venda
    {
        public int Id { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;
        public List<Item> Itens { get; set; } = new List<Item>();

        //public decimal Total => Itens.Sum(item => item.TotalComDesconto);
        public decimal Total { get; set; }

        // Aplica regras de negócio para todos os itens
        public void AplicarRegras()
        {
            foreach (var item in Itens)
            {
                item.CalcularDesconto();
            }
        }
    }
}