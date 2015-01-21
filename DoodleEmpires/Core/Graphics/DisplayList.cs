using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoodleEmpires.Core.Graphics
{
    public class DisplayList : OpenGLObject
    {
        public DisplayList(int glID)
        {
            GlID = glID;
        }

        /// <summary>
        /// Begin passing render calls to this list
        /// </summary>
        internal void Begin()
        {
            GL.NewList(GlID, ListMode.Compile);
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
            GL.CallList(GlID);
        }

        public override void Dispose()
        {
            GL.DeleteLists(GlID, 1);
        }
    }
}
