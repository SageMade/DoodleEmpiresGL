using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoodleEmpires.Core.Graphics
{
    /// <summary>
    /// Represents the interface for a type that applies a vertex
    /// type to the GL context
    /// </summary>
    public interface IVertexApplicator
    {
        /// <summary>
        /// The size of each vertex
        /// </summary>
        int SizeInBytes { get; }

        /// <summary>
        /// Applies the vertex type to the GL context
        /// </summary>
        void Apply();
    }
}
