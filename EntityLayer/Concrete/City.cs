namespace EntityLayer.Concrete;

public class City
{
    public int CityId { get; set; }
    public string CityName { get; set; }

    // Navigation property
    public virtual ICollection<Event> Events { get; set; }
}