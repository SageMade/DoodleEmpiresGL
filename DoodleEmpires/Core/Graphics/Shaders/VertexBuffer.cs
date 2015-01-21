using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoodleEmpires.Core.Graphics
{
    public class VertexBuffer : IDisposable
    {
        int m_glId;
        BufferUsageHint m_usage;
        IVertexApplicator m_vertexApplicator;

        public int Size { get; private set; }
        public IVertexApplicator VertexApplicator
        {
            get { return m_vertexApplicator; }
        }

        internal VertexBuffer(int glId, bool dynamic)
        {
            m_glId = glId;
            m_usage = dynamic ? BufferUsageHint.DynamicDraw : BufferUsageHint.StaticDraw;
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, m_glId);
        }

        public void SetData<T>(T[] vertices) where T : struct, IVertex
        {
            if (vertices.Length > 0)
            {
                m_vertexApplicator = vertices[0].Applicator;

                GL.BindBuffer(BufferTarget.ArrayBuffer, m_glId);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * m_vertexApplicator.SizeInBytes), vertices, m_usage);
                Size = vertices.Length;
            }
            else
                throw new ArgumentException("Cannot create an empty veretx buffer!");
        }

        public static implicit operator int(VertexBuffer buffer)
        {
            return buffer.m_glId;
        }

        public void Dispose()
        {
            GL.DeleteBuffer(m_glId);
        }
    }
}
