using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoodleEmpires.Core.Graphics
{
    public class DisplayList : IDisposable
    {
        private int m_glID;

        public DisplayList(int glID)
        {
            // TODO: Complete member initialization
            m_glID = glID;
        }

        /// <summary>
        /// Begin passing render calls to this list
        /// </summary>
        internal void Begin()
        {
            GL.NewList(m_glID, ListMode.Compile);
        }

        /// <summary>
        /// Stop sending render calls to this list
        /// </summary>
        internal void End()
        {
            GL.EndList();
        }

        /// <summary>
        /// Excecutes this display list
        /// </summary>
        public void Excecute()
        {
            GL.CallList(m_glID);
        }

        public void Dispose()
        {
            GL.DeleteLists(m_glID, 1);
        }
    }
}
