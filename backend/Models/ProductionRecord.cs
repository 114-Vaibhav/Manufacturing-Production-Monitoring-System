namespace backend.Models;
public class ProductionRecord
{
    public int Id { get; set; }

    public int ProductionPlanId { get; set; }

    public int ProducedQuantity { get; set; }

    public DateTime ProductionDate { get; set; }

    public ProductionPlan? ProductionPlan { get; set; }
}