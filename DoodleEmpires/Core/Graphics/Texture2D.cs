using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoodleEmpires.Core.Graphics
{
    public class Texture2D : OpenGLObject
    {
        public Texture2D(int width, int height, PixelFormat format = PixelFormat.Bgra, PixelInternalFormat internalFormat = PixelInternalFormat.Rgba, PixelType pixelType = PixelType.UnsignedByte, bool noBind = false)
        {
            m_glId = GL.GenTexture();

            int boundTexture = -1;

            if (!noBind)
            {
                boundTexture = GL.GetInteger(GetPName.Texture2D);
            }

            GL.BindTexture(TextureTarget.Texture2D, m_glId);

            // Use linear min and mag filters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            // Define the texture's attributes
            GL.TexImage2D(TextureTarget.Texture2D, 0, internalFormat, width, height, 0, format, pixelType, IntPtr.Zero);
            
            if (!noBind)
            {
                GL.BindTexture(TextureTarget.Texture2D, boundTexture);
            }

            OpenTKUtils.CheckError();
        }

        public Texture2D(int width, int height, PixelFormat format = PixelFormat.Bgra, PixelInternalFormat internalFormat = PixelInternalFormat.Rgba, PixelType pixelType = PixelType.UnsignedByte)
            : this(width, height, format, internalFormat, pixelType, false)
        {
        }

        public override void Dispose()
        {
            GL.DeleteTexture(m_glId);
        }

        internal void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, m_glId);
        }
    }
}
