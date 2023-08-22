namespace Nspiration.Model
{
    public class ShadeCard
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Color> Color { get; set; }
    }
}
