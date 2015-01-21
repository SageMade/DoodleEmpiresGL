using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace DoodleEmpires.Core.Graphics
{
    /// <summary>
    /// Represents a compute shader stage
    /// </summary>
    /// <see cref="https://www.opengl.org/wiki/Compute_Shader"/>
    public class ComputeShader : ShaderPart
    {
        /// <summary>
        /// Creates a new compute shader, compied from the given source code
        /// </summary>
        /// <param name="source">The source code to create the shader from</param>
        public ComputeShader(string source) 
            : base(source, ShaderType.ComputeShader)
        {

        }

        /// <summary>
        /// Creates a new compute shader, compiled from the given source code file
        /// </summary>
        /// <param name="source">The source code file to create the shader from</param>
        public static ComputeShader FromFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    return new ComputeShader(reader.ReadToEnd());
                }
            }
            else
                throw new FileNotFoundException("Cannot find file at " + fileName);
        }
    }
}
