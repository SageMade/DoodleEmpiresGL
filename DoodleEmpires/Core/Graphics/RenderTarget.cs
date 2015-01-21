using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoodleEmpires.Core.Graphics
{
    public class RenderTarget : OpenGLObject
    {
        Texture2D m_colorTarget;
        RenderTargetFlags m_flags;

        int m_depthStencil;

        public Texture2D Color
        {
            get { return m_colorTarget; }
        }

        public RenderTarget(int width, int height, RenderTargetFlags flags)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("Width and height must be greater than 0!");
            if (flags == 0)
                throw new ArgumentException("Must have at least one frame buffer attacment specified!");

            m_flags = flags;

            int boundTexture = GL.GetInteger(GetPName.Texture2D);
            int boundFbo = GL.GetInteger(GetPName.FramebufferBinding);

            m_glId = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, m_glId);

            if ((flags & RenderTargetFlags.Color) != 0)
            {
                m_colorTarget = new Texture2D(width, height, PixelFormat.Bgra, PixelInternalFormat.Rgba, PixelType.UnsignedByte, true);

                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, m_colorTarget.GlID, 0);

                OpenTKUtils.CheckError();
            }
            if ((flags & (RenderTargetFlags.Depth | RenderTargetFlags.Stencil)) != 0)
            {
                m_depthStencil = GL.GenRenderbuffer();
                GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, m_depthStencil);
                GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, width, height);

                GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, m_depthStencil);

                if ((flags | RenderTargetFlags.Stencil) != 0)
                    GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.StencilAttachment, RenderbufferTarget.Renderbuffer, m_depthStencil);

                OpenTKUtils.CheckError();
            }

            FramebufferErrorCode errorCode = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);

            if (errorCode != FramebufferErrorCode.FramebufferComplete)
            {
                throw new FramebufferCompletionException("Frame buffer initialization not complete: " + errorCode);
            }

            GL.BindTexture(TextureTarget.Texture2D, boundTexture);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, boundFbo);

            OpenTKUtils.CheckError();
        }

        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, m_glId);
        }

        public static void Unbind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public override void Dispose()
        {

        }
    }

    [Flags]
    public enum RenderTargetFlags 
    {
        Color =     1,
        Depth =     2,
        Stencil =   4,
    }
}
