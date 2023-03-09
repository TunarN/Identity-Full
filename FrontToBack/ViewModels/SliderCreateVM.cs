using Microsoft.Build.Framework;

namespace FrontToBack.ViewModels
{
    public class SliderCreateVM
    {
        [Required]
        public IFormFile Photo { get; set; }
    }
}
