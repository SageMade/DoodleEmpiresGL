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
    public abstract class ShaderPart
    {
        int m_glID;
        ShaderType m_shaderType;

        /// <summary>
        /// Gets the GL index for this shader part
        /// </summary>
        public int GlID
        {
            get { return m_glID; }
        }
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
            m_glID = GL.CreateShader(shaderType);
            m_shaderType = shaderType;

            GL.ShaderSource(m_glID, source);
            GL.CompileShader(m_glID);

            int wasSucess;
            GL.GetShader(m_glID, ShaderParameter.CompileStatus, out wasSucess);

            string message;
            GL.GetShaderInfoLog(m_glID, out message);
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
    }
}
