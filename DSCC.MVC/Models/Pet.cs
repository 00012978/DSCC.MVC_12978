namespace DSCC.MVC.Models;

public class Pet
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Species { get; set; }
    public int? AdoptionId { get; set; }
    public Adoption? Adoption { get; set; }
}
