using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace DoodleEmpires.Core.Graphics
{
    /// <summary>
    /// Represents a vertex shader stage
    /// </summary>
    /// <see cref="https://www.opengl.org/wiki/Vertex_Shader"/>
    public class VertexShader : ShaderPart
    {
        /// <summary>
        /// Creates a new vertex shader, compied from the given source code
        /// </summary>
        /// <param name="source">The source code to create the shader from</param>
        public VertexShader(string source) 
            : base(source, ShaderType.VertexShader)
        {
        }

        /// <summary>
        /// Creates a new vertex shader, compiled from the given source code file
        /// </summary>
        /// <param name="source">The source code file to create the shader from</param>
        public static VertexShader FromFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    return new VertexShader(reader.ReadToEnd());
                }
            }
            else
                throw new FileNotFoundException("Cannot find file at " + fileName);
        }
    }
}
