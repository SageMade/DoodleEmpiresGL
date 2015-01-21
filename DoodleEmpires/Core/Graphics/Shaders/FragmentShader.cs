using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace DoodleEmpires.Core.Graphics
{
    /// <summary>
    /// Represents a fragment shader stage
    /// </summary>
    /// <see cref="https://www.opengl.org/wiki/Fragment_Shader"/>
    public class FragmentShader : ShaderPart
    {
        /// <summary>
        /// Creates a new fragment shader, compied from the given source code
        /// </summary>
        /// <param name="source">The source code to create the shader from</param>
        public FragmentShader(string source) 
            : base(source, ShaderType.FragmentShader)
        {

        }

        /// <summary>
        /// Creates a new fragment shader, compiled from the given source code file
        /// </summary>
        /// <param name="source">The source code file to create the shader from</param>
        public static FragmentShader FromFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    return new FragmentShader(reader.ReadToEnd());
                }
            }
            else
                throw new FileNotFoundException("Cannot find file at " + fileName);
        }
    }
}
