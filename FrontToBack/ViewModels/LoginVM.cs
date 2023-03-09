using System.ComponentModel.DataAnnotations;

namespace FrontToBack.ViewModels
{
    public class LoginVM
    {
        [System.ComponentModel.DataAnnotations.Required, StringLength(100)]
        public string UsernameOrEmail { get; set; }

        [System.ComponentModel.DataAnnotations.Required, DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
