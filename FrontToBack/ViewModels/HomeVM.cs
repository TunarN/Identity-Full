using FrontToBack.Models;

namespace FrontToBack.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public SliderDetail SliderDetails { get; set; }
        public List<Category> Categories{ get; set; }
        public List<Product> Products { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        
    }
}
