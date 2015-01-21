using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoodleEmpires.Core.Graphics
{
    public interface IVertex
    {
        IVertexApplicator Applicator { get; }
    }
}
