using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentra.Gr.BLL.Dtos
{
    public class EmailRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
