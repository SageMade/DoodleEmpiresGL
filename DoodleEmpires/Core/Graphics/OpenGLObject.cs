using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoodleEmpires.Core.Graphics
{
    /// <summary>
    /// This is the base class for all OpenGL objects, or objects that wrap around an OpenGL object
    /// </summary>
    public abstract class OpenGLObject : IDisposable
    {
        protected int m_glId;

        /// <summary>
        /// Gets the GL index for this object
        /// </summary>
        public int GlID
        {
            get { return m_glId; }
            protected set { m_glId = value; }
        }

        public abstract void Dispose();
    }
}
