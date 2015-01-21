using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoodleEmpires.Core.Graphics.Shaders
{
    public class ShaderCompilationException : Exception
    {
        public ShaderCompilationException(string message) : base(message)
        {
        }
    }
}
