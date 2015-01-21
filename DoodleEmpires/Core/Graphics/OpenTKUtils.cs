using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace DoodleEmpires.Core.Graphics
{
    public static class OpenTKUtils
    {
        public static bool ThrowError = true;
        public static bool CheckError()
        {
            ErrorCode eCode = GL.GetError();

            if (eCode != ErrorCode.NoError)
            {
                if (ThrowError)
                    throw new OpenTKError("OpenGL threw error: " + eCode);
                else
                {
                    Logger.LogMessage("OpenGL threw error: " + eCode);
                    return true;
                }
            }
            else
                return false;
        }
    }
}
