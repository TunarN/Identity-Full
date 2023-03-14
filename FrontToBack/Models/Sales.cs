namespace FrontToBack.Models
{
    public class Sales
    {
        public int Id { get; set; }
        public int TotalPrice { get; set; }
        public string AppUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public List<SalesProducts> SalesProducts { get; set; }
    }
}
