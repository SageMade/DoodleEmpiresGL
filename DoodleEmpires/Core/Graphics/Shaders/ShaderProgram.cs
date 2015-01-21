using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Text.RegularExpressions;

namespace DoodleEmpires.Core.Graphics
{
    /// <summary>
    /// Represents a shader program
    /// </summary>
    /// <see cref="https://www.opengl.org/wiki/GLSL_Object"/>
    public class ShaderProgram : OpenGLObject
    {
        VertexShader m_vShader;
        FragmentShader m_fShader;

        ComputeShader m_cShader;
        GeometryShader m_gShader;
        TessControlShader m_tCShader;
        TessEvaluationShader m_tEShader;

        ShaderResource[] m_resources;

        /// <summary>
        /// Gets whether this shader program has been linked or not
        /// </summary>
        public bool IsLinked
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a new shader program
        /// </summary>
        public ShaderProgram()
        {
            m_glId = GL.CreateProgram();

            OpenTKUtils.CheckError();
        }

        /// <summary>
        /// The internal method to attach a shader part to this shader program
        /// </summary>
        /// <param name="part">The shader part to attach</param>
        public void AttachShader(ShaderPart part)
        {
            switch(part.ShaderType)
            {
                case ShaderType.ComputeShader:
                    m_cShader = (ComputeShader)part;
                    break;
                case ShaderType.FragmentShader:
                    m_fShader = (FragmentShader)part;
                    break;
                case ShaderType.GeometryShader:
                    m_gShader = (GeometryShader)part;
                    break;
                case ShaderType.TessControlShader:
                    m_tCShader = (TessControlShader)part;
                    break;
                case ShaderType.TessEvaluationShader:
                    m_tEShader = (TessEvaluationShader)part;
                    break;
                case ShaderType.VertexShader:
                    m_vShader = (VertexShader)part;
                    break;
                default:
                    throw new ArgumentException("Cannot bind shader type " + part.ShaderType);
            }

            GL.AttachShader(GlID, part.GlID);

            OpenTKUtils.CheckError();
        }

        public void BuildResources()
        {
            if (IsLinked)
            {
                int parameterCount = 0;
                GL.GetProgramInterface(m_glId, ProgramInterface.Uniform, ProgramInterfaceParameter.ActiveResources, out parameterCount);

                m_resources = new ShaderResource[parameterCount];

                for (int rIndex = 0; rIndex < parameterCount; rIndex++)
                {
                    m_resources[rIndex] = new ShaderResource(this, rIndex);
                }
            }
        }
        
        /// <summary>
        /// Links this program's shaders together. This must be called before the shader can be applied
        /// </summary>
        public void Link()
        {
            if (!IsLinked)
            {
                if (m_vShader == null || m_fShader == null)
                    throw new InvalidOperationException("Cannot link shader without both a vertex and fragment shader defined");
                else
                {
                    GL.LinkProgram(m_glId);
                    GL.ValidateProgram(m_glId);


                    if (!GL.IsProgram(m_glId))
                    {
                        throw new OpenTKException("Program is no longer an OpenGL program!");
                    }
                    else
                        Logger.LogMessage(GL.GetProgramInfoLog(m_glId));

                    if (!OpenTKUtils.CheckError())
                        IsLinked = true;
                }
            }
            else
                throw new InvalidOperationException("Cannot link an already linked shader");
        }

        /// <summary>
        /// Unlinks this shader's shader parts, this can be called once the shader is linked
        /// and the shader will still operate normally
        /// </summary>
        public void Unattach()
        {
            if (IsLinked)
            {
                GL.DetachShader(m_glId, m_vShader.GlID);
                GL.DetachShader(m_glId, m_fShader.GlID);

                if (m_cShader != null)
                    GL.DetachShader(m_glId, m_cShader.GlID);

                if (m_gShader != null)
                    GL.DetachShader(m_glId, m_gShader.GlID);

                if (m_tCShader != null)
                    GL.DetachShader(m_glId, m_tCShader.GlID);

                if (m_tEShader != null)
                    GL.DetachShader(m_glId, m_tEShader.GlID);

                IsLinked = false;
            }
            else
            {
                throw new InvalidOperationException("Cannot unlink an unlinked shader");
            }
        }

        /// <summary>
        /// Unlinks this shader's shader parts and disposes of the shader parts. Note that this will delete the shader parts stored for this program,
        /// and the program will need to be rebuilt before linking can be performed
        /// </summary>
        public void UnattachAndDestroy()
        {
            DeleteShaderPart(m_vShader);
            DeleteShaderPart(m_fShader);
            DeleteShaderPart(m_cShader);
            DeleteShaderPart(m_gShader);
            DeleteShaderPart(m_tEShader);
            DeleteShaderPart(m_tCShader);
        }

        /// <summary>
        /// Applies this shader program
        /// </summary>
        public void Apply()
        {
            if (IsLinked)
            {
                if (!GL.IsProgram(m_glId))
                    throw new OpenTKException("Program is no longer an OpenGL program!");

                GL.UseProgram(m_glId);

                OpenTKUtils.CheckError();
            }
            else
                throw new InvalidOperationException("Shader must be linked before it can be applied");
        }

        /// <summary>
        /// Handles deleting a shader part
        /// </summary>
        /// <param name="part">The shader part to delete</param>
        private void DeleteShaderPart(ShaderPart part)
        {
            if (part != null)
            {
                GL.DetachShader(GlID, part.GlID);
                GL.DeleteShader(part.GlID);
                part = null;
            }
        }

        /// <summary>
        /// Disposes this shader and releases all of it's unmanaged resources 
        /// </summary>
        public override void Dispose()
        {
            if (IsLinked)
            {
                UnattachAndDestroy();

                GL.DeleteProgram(GlID);
            }
            else
            {
                DeleteShaderPart(m_vShader);
                DeleteShaderPart(m_fShader);
                DeleteShaderPart(m_cShader);
                DeleteShaderPart(m_gShader);
                DeleteShaderPart(m_tEShader);
                DeleteShaderPart(m_tCShader);

                GL.DeleteProgram(m_glId);
            }
        }

        public static ShaderProgram LoadFromPath(string path, string shaderName = null)
        {
            shaderName = shaderName == null ? "*" : shaderName;

            string[] vertexShaderPaths = Directory.GetFiles(path, shaderName + ".vert");
            string[] fragmentShaderPaths = Directory.GetFiles(path, shaderName + "*.frag");

            if (vertexShaderPaths.Length > 0 && fragmentShaderPaths.Length > 0)
            {
                ShaderProgram program = new ShaderProgram();

                VertexShader vertexShader = VertexShader.FromFile(vertexShaderPaths[0]);
                FragmentShader fragmentShader = FragmentShader.FromFile(fragmentShaderPaths[0]);

                program.AttachShader(vertexShader);
                program.AttachShader(fragmentShader);

                program.Link();
                program.BuildResources();

                return program;
            }
            else
            {
                throw new FileNotFoundException("Cannot find a vertex and fragment shader!");
            }
        }
    }
}
