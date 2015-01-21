using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace DoodleEmpires.Core.Graphics
{
    /// <summary>
    /// Represents a tesselation evaluation shader stage
    /// </summary>
    /// <see cref="https://www.opengl.org/wiki/Tessellation_Evaluation_Shader"/>
    public class TessEvaluationShader : ShaderPart
    {
        /// <summary>
        /// Creates a new tesselation evaluation shader, compied from the given source code
        /// </summary>
        /// <param name="source">The source code to create the shader from</param>
        public TessEvaluationShader(string source)
            : base(source, ShaderType.TessEvaluationShader)
        {

        }

        /// <summary>
        /// Creates a new tesselation evaluation shader, compiled from the given source code file
        /// </summary>
        /// <param name="source">The source code file to create the shader from</param>
        public static TessEvaluationShader FromFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    return new TessEvaluationShader(reader.ReadToEnd());
                }
            }
            else
                throw new FileNotFoundException("Cannot find file at " + fileName);
        }
    }
}
