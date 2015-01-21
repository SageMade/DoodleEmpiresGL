using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using DoodleEmpires.Core.Graphics.Shaders;

namespace DoodleEmpires.Core.Graphics
{
    /// <summary>
    /// The base class for all shader parts
    /// </summary>
    public abstract class ShaderPart : OpenGLObject
    {
        ShaderType m_shaderType;

        /// <summary>
        /// Gets this shader part's shader type
        /// </summary>
        public ShaderType ShaderType
        {
            get { return m_shaderType; }
        }

        /// <summary>
        /// Represents a single part of a shader
        /// </summary>
        /// <param name="source">The source code for the shader part</param>
        /// <param name="shaderType">The type of shader to create</param>
        public ShaderPart(string source, ShaderType shaderType)
        {
            m_glId = GL.CreateShader(shaderType);
            m_shaderType = shaderType;

            GL.ShaderSource(m_glId, source);
            GL.CompileShader(m_glId);

            int wasSucess;
            GL.GetShader(m_glId, ShaderParameter.CompileStatus, out wasSucess);

            string message;
            GL.GetShaderInfoLog(m_glId, out message);
            Logger.LogMessage(message);

            if (wasSucess == 0)
            {
                throw new ShaderCompilationException(message);
            }
            

            OpenTKUtils.CheckError();
        }

        /// <summary>
        /// Adds this shader to the given shader program
        /// </summary>
        /// <param name="program">The shader to add this shader part to</param>
        public void AddToProgram(ShaderProgram program)
        {
            program.AttachShader(this);
        }

        public override void Dispose()
        {
            GL.DeleteShader(m_glId);
        }
    }
}
