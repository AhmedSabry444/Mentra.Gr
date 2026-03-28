using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentra.Gr.BLL.Exceptions
{
    public class RegistrationBadRequsetException(List<string> errors) : BadRequestException(string.Join(",", errors))
    {
    }
}
