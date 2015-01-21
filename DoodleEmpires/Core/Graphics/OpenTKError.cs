using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoodleEmpires.Core.Graphics
{
    public class OpenTKError : Exception
    {
        public OpenTKError(string message) : base(message)
        {
        }
    }
}
