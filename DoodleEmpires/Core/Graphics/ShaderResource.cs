using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoodleEmpires.Core.Graphics
{
    public class ShaderResource
    {
        static readonly ProgramProperty[] PROPERTIES = new ProgramProperty[] { ProgramProperty.BlockIndex, ProgramProperty.Type, ProgramProperty.NameLength, ProgramProperty.Location };

        public string Name
        {
            get;
            private set;
        }

        internal ShaderResource(ShaderProgram program, int index)
        {
            int[] resourceValues = new int[PROPERTIES.Length];

            int crap = 0;
            GL.GetProgramResource(program.GlID, ProgramInterface.Uniform, index, PROPERTIES.Length, PROPERTIES, PROPERTIES.Length, out crap, resourceValues);
            StringBuilder name = new StringBuilder();
            GL.GetProgramResourceName(program.GlID, ProgramInterface.Uniform, resourceValues[3], resourceValues[2], out crap, name);

            Name = name.ToString();
        }
    }
}
