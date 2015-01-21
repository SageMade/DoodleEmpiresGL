using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace DoodleEmpires.Core.Graphics
{
    /// <summary>
    /// Represents a tesselation control shader stage
    /// </summary>
    /// <see cref="https://www.opengl.org/wiki/Tessellation_Control_Shader"/>
    public class TessControlShader : ShaderPart
    {
        /// <summary>
        /// Creates a new tesselation control shader, compied from the given source code
        /// </summary>
        /// <param name="source">The source code to create the shader from</param>
        public TessControlShader(string source)
            : base(source, ShaderType.TessControlShader)
        {

        }

        /// <summary>
        /// Creates a new tesselation control shader, compiled from the given source code file
        /// </summary>
        /// <param name="source">The source code file to create the shader from</param>
        public static TessControlShader FromFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    return new TessControlShader(reader.ReadToEnd());
                }
            }
            else
                throw new FileNotFoundException("Cannot find file at " + fileName);
        }
    }
}
