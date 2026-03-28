using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentra.Gr.BLL.Exceptions
{
    public class UserNotFoundException(string email) : NotFoundException($"User with email '{email}' was not found.")
    {
    }
}
