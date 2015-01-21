using DoodleEmpires.Core.Graphics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DoodleEmpires.Game
{
    public struct VertexColor : IVertex
    {
        Vector2 Position;
         Color4 Color;

        public VertexColor(Vector2 position, Color4 color)
        {
            Position = position;
            Color = color;
        }
        
        public IVertexApplicator Applicator
        {
            get { return APPLICATOR; }
        }

        static VertexColorApplicator APPLICATOR = new VertexColorApplicator();
    }

    public class VertexColorApplicator : IVertexApplicator
    {
        public int SizeInBytes
        {
            get { return 2 * sizeof(float) + 4 * sizeof(float); }
        }

        public void Apply()
        {
            GL.EnableClientState(ArrayCap.ColorArray);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, SizeInBytes, Marshal.OffsetOf(typeof(VertexColor), "Color"));
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, true, SizeInBytes, Marshal.OffsetOf(typeof(VertexColor), "Position"));
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.VertexPointer(2, VertexPointerType.Float, SizeInBytes, Marshal.OffsetOf(typeof(VertexColor), "Position"));
            GL.ColorPointer(4, ColorPointerType.Float, SizeInBytes, Marshal.OffsetOf(typeof(VertexColor), "Color"));
        }
    }
}
