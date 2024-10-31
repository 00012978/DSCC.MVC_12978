namespace DSCC.MVC.Models;

public class Adoption
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int PetId { get; set; }
    public Pet Pet { get; set; }
}
