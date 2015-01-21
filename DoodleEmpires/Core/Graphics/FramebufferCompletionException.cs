using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoodleEmpires.Core.Graphics
{
    class FramebufferCompletionException : Exception
    {

        public FramebufferCompletionException(string message) : base(message)
        {

        }
    }
}
