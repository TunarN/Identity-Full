namespace FrontToBack.ViewModels
{
    public class ProductCreateVM
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int CategoryId { get; set; }
        public IFormFile[] Photos { get; set; }
    }
}
