using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoodleEmpires.Core.Graphics
{
    public class IndexBuffer : IDisposable
    {
        int m_glId;
        BufferUsageHint m_usage;

        public int Size { get; private set; }

        internal IndexBuffer(bool dynamic)
        {
            GL.GenBuffers(1, out m_glId);
            m_usage = dynamic ? BufferUsageHint.DynamicDraw : BufferUsageHint.StaticDraw;
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, m_glId);
        }

        public void SetData(ushort[] indices)
        {
            if (indices.Length > 0)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, m_glId);
                GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Length * sizeof(ushort)), indices, m_usage);
                Size = indices.Length;
            }
            else
                throw new ArgumentException("Cannot create an empty index buffer!");
        }

        public static implicit operator int(IndexBuffer buffer)
        {
            return buffer.m_glId;
        }

        public void Dispose()
        {
            GL.DeleteBuffer(m_glId);
        }
    }
}
