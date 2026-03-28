using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentra.Gr.BLL.Dtos
{
    public class SignUpDto
    {
        [Required(ErrorMessage = "UserName Is Required")]
        public string UserName { get; set; }



        [Required(ErrorMessage = "DisplayName Is Required")]
        public string DisplayName { get; set; }



        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress]
        public string Email { get; set; }



        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        public int Age { get; set; }
    }
}
