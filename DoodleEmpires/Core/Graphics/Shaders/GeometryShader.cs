using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace DoodleEmpires.Core.Graphics
{
    /// <summary>
    /// Represents a geometry shader stage
    /// </summary>
    /// <see cref="https://www.opengl.org/wiki/Geometry_Shader"/>
    public class GeometryShader : ShaderPart
    {
        /// <summary>
        /// Creates a new geometry shader, compied from the given source code
        /// </summary>
        /// <param name="source">The source code to create the shader from</param>
        public GeometryShader(string source)
            : base(source, ShaderType.GeometryShader)
        {

        }

        /// <summary>
        /// Creates a new geometry shader, compiled from the given source code file
        /// </summary>
        /// <param name="source">The source code file to create the shader from</param>
        public static GeometryShader FromFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    return new GeometryShader(reader.ReadToEnd());
                }
            }
            else
                throw new FileNotFoundException("Cannot find file at " + fileName);
        }
    }
}
