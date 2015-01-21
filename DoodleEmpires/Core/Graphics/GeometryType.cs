using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoodleEmpires.Core.Graphics
{    
    /// <summary>
    /// Defines the type of geometry to be rendered, the values are
    /// taken from OpenGL's PrimitiveType
    /// </summary>
    /// <see cref="https://www.opengl.org/registry/doc/glspec44.core.pdf#page=328"/>
    public enum GeometryType
    {
        /// <summary>
        /// Vertices will be drawn as individual points, <i>n</i> primitives will be drawn
        /// </summary>
        Point = 0x0000,
        /// <summary>
        /// Vertices will be drawn as individual lines, <i>n / 2</i> primitives will be drawn
        /// </summary>
        LineList = 0x0001,
        /// <summary>
        /// Vertices will be drawn as a closed strip of lines, <i>n</i> primitives will be drawn
        /// </summary>
        LineLoop = 0x0002,
        /// <summary>
        /// Vertices will be drawn as a strip of lines, <i>n - 1</i> primitives will be drawn
        /// </summary>
        LineStrip = 0x0003,
        /// <summary>
        /// Vertices will be drawn as individual triangles, <i>n / 3</i> primitives will be drawn
        /// </summary>
        TriangleList = 0x0004,
        /// <summary>
        /// Vertices will be drawn as a strip of triangles, <i>n - 2</i> primitives will be drawn
        /// </summary>
        TriangleStrip = 0x0005,
        /// <summary>
        /// Vertices will be drawn as a fan of triangles, originating at the first vertex, <i>n - 2</i> primitives will be drawn
        /// </summary>
        TriangleFan = 0x0006,
        /// <summary>
        /// Vertices will be drawn as individual quads, <i>n / 4</i> primitives will be drawn
        /// </summary>
        [Obsolete("Use triangles instead")]
        QuadList = 0x0007,
        /// <summary>
        /// Vertices will be drawn as strip of quads, <i>(n - 2) / 2</i> primitives will be drawn
        /// </summary>
        [Obsolete("Use triangles instead")]
        QuadStrip = 0x0008,
        /// <summary>
        /// Vertices will be drawn as one large polygon, <i>1</i> primitive will be drawn
        /// </summary>
        [Obsolete("Create polygon from triangles instead")]
        Polygon = 0x0009,
        /// <summary>
        /// Vertices will be passed to the geometry shader in a line list with access
        /// to adjecent vertices
        /// </summary>
        /// <see cref="http://msdn.microsoft.com/en-us/library/windows/desktop/bb205124%28v=vs.85%29.aspx"/>
        LineListAdjacency = 0x000A,
        /// <summary>
        /// Vertices will be passed to the geometry shader in a line strip with access
        /// to adjecent vertices
        /// </summary>
        /// <see cref="http://msdn.microsoft.com/en-us/library/windows/desktop/bb205124%28v=vs.85%29.aspx"/>
        LineStripAdjacency = 0x000B,
        /// <summary>
        /// Vertices will be passed to the geometry shader in a triangle list with access
        /// to adjecent vertices
        /// </summary>
        /// <see cref="http://msdn.microsoft.com/en-us/library/windows/desktop/bb205124%28v=vs.85%29.aspx"/>
        TriangleListAdjacency = 0x000C,
        /// <summary>
        /// Vertices will be passed to the geometry shader in a triangel strip with access
        /// to adjecent vertices
        /// </summary>
        /// <see cref="http://msdn.microsoft.com/en-us/library/windows/desktop/bb205124%28v=vs.85%29.aspx"/>
        TriangleStripAdjacency = 0x000D,
        /// <summary>
        /// Vertices will be passed to the tesselation shader for further processing
        /// </summary>
        Patches = 0x000E
    }
}
