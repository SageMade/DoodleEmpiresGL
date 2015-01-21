using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace DoodleEmpires.Core.Graphics
{
    /// <summary>
    /// Represents a graphics manager, which should handle all drawing wit OpenGL
    /// </summary>
    /// <remarks>
    ///     Although it is possible to create multiple GraphicsDevice objects at once, it is highly recommended
    ///     against unless you know what you are doing. GraphicsDevice makes calls to the static GL type, and as
    ///     such running two GraphicsDevice's at once will cause unexpected results.
    /// </remarks>
    public class GraphicsDevice
    {
        GameWindow m_gameWindow;
        Color4 m_clearColor;

        /// <summary>
        /// Gets or sets the color to clear the screen to when Clear() is called
        /// </summary>
        public Color4 ClearColor
        {
            get { return m_clearColor; }
            set
            {
                m_clearColor = value;
                GL.ClearColor(m_clearColor);
            }
        }

        /// <summary>
        /// Creates the graphics device for the given game window
        /// </summary>
        /// <param name="window">The window to create the graphics device for</param>
        public GraphicsDevice(GameWindow window)
        {
            m_gameWindow = window;
            m_clearColor = Color4.CornflowerBlue;
        }

        /// <summary>
        /// Creates a new vertex buffer object
        /// </summary>
        /// <param name="dynamic">True if this vertex buffer will not be static</param>
        /// <returns>A new instance of a vertex buffer object</returns>
        public VertexBuffer CreateVertexBuffer(bool dynamic = false)
        {
            return new VertexBuffer(GL.GenBuffer(), dynamic);             
        }

        /// <summary>
        /// Creates a new index buffer object
        /// </summary>
        /// <param name="dynamic">True if this index buffer will not be static</param>
        /// <returns>A new instance of an index buffer object</returns>
        public IndexBuffer CreateIndexBuffer(bool dynamic = false)
        {
            return new IndexBuffer(dynamic);
        }

        /// <summary>
        /// Greates a new display list
        /// </summary>
        /// <returns>A new display list instance</returns>
        public DisplayList CreateDisplayList()
        {
            return new DisplayList(GL.GenLists(1));
        }

        /// <summary>
        /// Draws a vertex buffer
        /// </summary>
        /// <param name="vBuffer">The vertex buffer to draw</param>
        /// <param name="mode">The type of geometry to draw</param>
        public void DrawPrimitives(VertexBuffer vBuffer, GeometryType mode)
        {
            // We need to bind the vertex buffer so that it is used
            vBuffer.Bind();
            // We then apply the vertex applicator to set the correct GL states
            vBuffer.VertexApplicator.Apply();
            // Finally we draw our list
            GL.DrawArrays((PrimitiveType)mode, 0, vBuffer.Size);
        }

        /// <summary>
        /// Draws an indexed set of vertices
        /// </summary>
        /// <param name="vBuffer">The vertex buffer to draw</param>
        /// <param name="iBuffer">The indices of the vertices to draw</param>
        /// <param name="mode">The type of geometry to draw</param>
        public void DrawIndexedPrimitives(VertexBuffer vBuffer, IndexBuffer iBuffer, GeometryType mode)
        {
            vBuffer.Bind();
            vBuffer.VertexApplicator.Apply();
            //iBuffer.Bind();
            GL.BindBuffer(BufferTarget.ArrayBuffer, iBuffer);
            GL.DrawElements((PrimitiveType)mode, iBuffer.Size, DrawElementsType.UnsignedShort, 0);
        }

        /// <summary>
        /// Begins immediate context rendering
        /// </summary>
        /// <param name="mode">The type of geometry to render</param>
        public void Begin(GeometryType mode)
        {
            GL.Begin((PrimitiveType)mode);
        }
        
        /// <summary>
        /// Ends immediate context rendering
        /// </summary>
        public void End()
        {
            GL.End();
        }

        /// <summary>
        /// Starts routing calls to this graphics device to a display list
        /// </summary>
        /// <param name="list">The list to route render calls to</param>
        public void StartDisplayList(DisplayList list)
        {
            list.Begin();
        }

        /// <summary>
        /// Stops routing calls to this graphics device to a display list
        /// </summary>
        /// <param name="list">The list to route render calls to</param>
        public void StopDisplayList(DisplayList list)
        {
            list.End();
        }

        #region Immediate context passes

        public void SetMatrix(Matrix4 matrix)
        {
            GL.LoadMatrix(ref matrix);
        }

        /// <summary>
        /// Passes a position to the immediate context
        /// </summary>
        /// <param name="position">The postion to pass</param>
        public void Vertex(Vector2 position)
        {
            GL.Vertex2(position);
        }

        /// <summary>
        /// Passes a position to the immediate context
        /// </summary>
        /// <param name="position">The postion to pass</param>
        public void Vertex(Vector3 position)
        {
            GL.Vertex3(position);
        }

        /// <summary>
        /// Passes a color to the immediate context
        /// </summary>
        /// <param name="color">The color to pass</param>
        public void Color(Color4 color)
        {
            GL.Color4(color);
        }

        #endregion

        #region Immediate context drawing tools

        /// <summary>
        /// Sets the width to render lines with
        /// </summary>
        /// <remarks>
        /// This must be called before GraphicsDevice.Begin to apply coorectly, and will only be active
        /// until a call to GraphicsDevice.end
        /// </remarks>
        /// <param name="size">The size of the lines</param>
        public void SetLineWidth(float width)
        {
            GL.LineWidth(width);
        }

        /// <summary>
        /// Sets the size to render points with
        /// </summary>
        /// <remarks>
        /// This must be called before GraphicsDevice.Begin to apply coorectly, and will only be active
        /// until a call to GraphicsDevice.end
        /// </remarks>
        /// <param name="size">The size of the points</param>
        public void SetPointSize(float size)
        {
            GL.PointSize(size);
        }

        /// <summary>
        /// Draws a point at the given position with a given color
        /// </summary>
        /// <remarks>
        /// To use this command, it must be placed between a GraphicsDevice.Begin and a GraphicsDevice.End
        /// </remarks>
        /// <param name="position">The position to draw the point at</param>
        /// <param name="color">The color to draw with</param>
        public void DrawPoint(Vector3 position, Color4 color)
        {
            Color(color);
            Vertex(position);
        }
        
        /// <summary>
        /// Draws a point at the given position with a given color
        /// </summary>
        /// <remarks>
        /// To use this command, it must be placed between a GraphicsDevice.Begin and a GraphicsDevice.End
        /// </remarks>
        /// <param name="position">The position to draw the point at</param>
        /// <param name="color">The color to draw with</param>
        public void DrawPoint(Vector2 position, Color4 color)
        {
            Color(color);
            Vertex(position);
        }

        /// <summary>
        /// Draws an array of points with a given color
        /// </summary>
        /// <remarks>
        /// To use this command, it must be placed between a GraphicsDevice.Begin and a GraphicsDevice.End
        /// </remarks>
        /// <param name="position">The position to draw the point at</param>
        /// <param name="color">The color to draw with</param>
        public void DrawPoints(Vector3[] positions, Color4 color)
        {
            Color(color);

            for (int index = 0; index < positions.Length; index++)
                Vertex(positions[index]);
        }

        /// <summary>
        /// Draws an array of points with the given color
        /// </summary>
        /// <remarks>
        /// To use this command, it must be placed between a GraphicsDevice.Begin and a GraphicsDevice.End
        /// </remarks>
        /// <param name="positions">The position to draw the point at</param>
        /// <param name="color">The color to draw with</param>
        public void DrawPoints(Vector2[] positions, Color4 color)
        {
            Color(color);
            for (int index = 0; index < positions.Length; index++)
                Vertex(positions[index]);
        }

        /// <summary>
        /// Draws a line between two given points with a given color
        /// </summary>
        /// <remarks>
        /// To use this command, it must be placed between a GraphicsDevice.Begin and a GraphicsDevice.End
        /// </remarks>
        /// <param name="start">The point to start at</param>
        /// <param name="end">The point to end at</param>
        /// <param name="color">The color to draw with</param>
        public void DrawLine(Vector3 start, Vector3 end, Color4 color)
        {
            DrawPoint(start, color);
            DrawPoint(end, color);
        }

        /// <summary>
        /// Draws a line between two given points with a start and end color
        /// </summary>
        /// <remarks>
        /// To use this command, it must be placed between a GraphicsDevice.Begin and a GraphicsDevice.End
        /// </remarks>
        /// <param name="start">The point to start at</param>
        /// <param name="end">The point to end at</param>
        /// <param name="startColor">The color to draw start with</param>
        /// <param name="endColor">The color to end with</param>
        public void DrawLine(Vector3 start, Vector3 end, Color4 startColor, Color4 endColor)
        {
            DrawPoint(start, startColor);
            DrawPoint(end, endColor);
        }

        /// <summary>
        /// Draws a line between two given points with a given color
        /// </summary>
        /// <remarks>
        /// To use this command, it must be placed between a GraphicsDevice.Begin and a GraphicsDevice.End
        /// </remarks>
        /// <param name="start">The point to start at</param>
        /// <param name="end">The point to end at</param>
        /// <param name="color">The color to draw with</param>
        public void DrawLine(Vector2 start, Vector2 end, Color4 color)
        {
            DrawPoint(start, color);
            DrawPoint(end, color);
        }

        /// <summary>
        /// Draws a line between two given points with a start and end color
        /// </summary>
        /// <remarks>
        /// To use this command, it must be placed between a GraphicsDevice.Begin and a GraphicsDevice.End
        /// </remarks>
        /// <param name="start">The point to start at</param>
        /// <param name="end">The point to end at</param>
        /// <param name="startColor">The color to draw start with</param>
        /// <param name="endColor">The color to end with</param>
        public void DrawLine(Vector2 start, Vector2 end, Color4 startColor, Color4 endColor)
        {
            DrawPoint(start, startColor);
            DrawPoint(end, endColor);
        }

        #endregion

        #region Advanced draw functions

        /// <summary>
        /// Draws a rectangle with the given color this will begin and end it's own immediate context
        /// </summary>
        /// <remarks>
        /// This must NOT be called between a GraphicsDevice.Begin and a GraphicsDevice.End, as this function
        /// calls it's own begining and end
        /// </remarks>
        /// <param name="rectangle">The rectangle to draw</param>
        /// <param name="color">The color of the rectangle</param>
        /// <param name="filled">True if the rectangle should be a solid fill</param>
        /// <param name="lineWidth">The width of the lines if the rectangle is not filled</param>
        public void DrawRectangle(Rectangle rectangle, Color4 color, bool filled = false, float lineWidth = 1.0f)
        {
            if (filled)
            {
                Begin(GeometryType.TriangleStrip);

                DrawPoint(rectangle.TopLeft, color);
                DrawPoint(rectangle.TopRight, color);
                DrawPoint(rectangle.BottomLeft, color);
                DrawPoint(rectangle.BottomRight, color);

                End();
            }
            else
            {
                SetLineWidth(lineWidth);

                Begin(GeometryType.LineLoop);

                DrawPoint(rectangle.TopLeft, color);
                DrawPoint(rectangle.TopRight, color);
                DrawPoint(rectangle.BottomRight, color);
                DrawPoint(rectangle.BottomLeft, color);

                End();
            }
        }

        /// <summary>
        /// Draws a rectangle with the given fill color and outline color. this will begin and end it's own immediate context
        /// </summary>
        /// <remarks>
        /// This must NOT be called between a GraphicsDevice.Begin and a GraphicsDevice.End, as this function
        /// calls it's own begining and end
        /// </remarks>
        /// <param name="rectangle">The rectangle to draw</param>
        /// <param name="fillColor">The fill color of the rectangle</param>
        /// <param name="outlineColor">The outline color of the rectangle</param>
        /// <param name="lineWidth">The width of the outline</param>
        public void DrawFilledRectangle(Rectangle rectangle, Color4 fillColor, Color4 outlineColor, float lineWidth = 1.0f)
        {
            Begin(GeometryType.TriangleStrip);

            DrawPoint(rectangle.TopLeft, fillColor);
            DrawPoint(rectangle.TopRight, fillColor);
            DrawPoint(rectangle.BottomLeft, fillColor);
            DrawPoint(rectangle.BottomRight, fillColor);

            End();

            SetLineWidth(lineWidth);

            Begin(GeometryType.LineLoop);

            DrawPoint(rectangle.TopLeft, outlineColor);
            DrawPoint(rectangle.TopRight, outlineColor);
            DrawPoint(rectangle.BottomRight, outlineColor);
            DrawPoint(rectangle.BottomLeft, outlineColor);

            End();
        }

        /// <summary>
        /// Renders a bezier curve with this graphics device
        /// </summary>
        /// <param name="start">The start point of the curve</param>
        /// <param name="end">The end point of the curve</param>
        /// <param name="controlPoint1">The first control point</param>
        /// <param name="controlPoint2">The second control point</param>
        /// <param name="color">The color to draw the curve</param>
        /// <param name="samples">The number of samples to use</param>
        public void DrawBezierCurve(Vector2 start, Vector2 end, Vector2 controlPoint1, Vector2 controlPoint2, Color4 color, int samples = 10)
        {
            Vector2[] points = new Vector2[] // TODO. cache a vector array for use later
            {
                start, controlPoint1, controlPoint2, end
            };
            DrawCurveInternal(points, color, samples, GeometryType.LineStrip);
        }

        /// <summary>
        /// Renders an arc with this graphics device
        /// </summary>
        /// <param name="start">The start point of the curve</param>
        /// <param name="end">The end point of the curve</param>
        /// <param name="controlPoint">The first control point</param>
        /// <param name="color">The color to draw the curve</param>
        /// <param name="samples">The number of samples to use</param>
        public void DrawArc(Vector2 start, Vector2 end, Vector2 controlPoint, Color4 color, int samples = 10)
        {
            Vector2[] points = new Vector2[] // TODO. cache a vector array for use later
            {
                start, controlPoint, end
            };
            DrawCurveInternal(points, color, samples, GeometryType.LineStrip);
        }

        /// <summary>
        /// Renders a curve using an arbitrary number of control points with this graphics device
        /// </summary>
        /// <param name="points">The control point of the curve</param>
        /// <param name="color">The color to draw the curve</param>
        /// <param name="samples">The number of samples to use</param>
        public void DrawCurve(Vector2[] points, Color4 color, int samples = 10)
        {
            DrawCurveInternal(points, color, samples, GeometryType.LineStrip);
        }

        private void DrawCurveInternal(Vector2[] points, Color4 color, int samples, GeometryType geometryType)
        {
            float[] controlPoints = new float[points.Length * 3];

            for (int index = 0; index < points.Length; index++)
            {
                controlPoints[index * 3] = points[index].X;
                controlPoints[index * 3 + 1] = points[index].Y;
                controlPoints[index * 3 + 2] = 0;
            }

            GL.Map1(MapTarget.Map1Vertex3, 0, 1, 3, points.Length, controlPoints);
            GL.Enable(EnableCap.Map1Vertex3);

            GL.Color4(color);
            GL.Begin((PrimitiveType)geometryType);

            for (int index = 0; index < samples; index++)
                GL.EvalCoord1(index / (samples - 1.0));

            GL.End();
            GL.Disable(EnableCap.Map1Vertex3);
        }

        public void DrawCircle(Vector2 centre, float radius, Color4 color, int samples = 20)
        {
            Vector2[] corners = new Vector2[]
            {
                centre + new Vector2(-radius, 0),
                centre + new Vector2(-radius, radius),
                centre + new Vector2(radius, radius),
                centre + new Vector2(radius, 0),
            };

            DrawCurveInternal(corners, color, samples, GeometryType.LineStrip);

            corners = new Vector2[]
            {
                centre + new Vector2(-radius, 0),
                centre + new Vector2(-radius, -radius),
                centre + new Vector2(radius, -radius),
                centre + new Vector2(radius, 0),
            };

            DrawCurveInternal(corners, color, samples, GeometryType.LineStrip);
        }

        public void DrawCircleFilled(Vector2 centre, float radius, Color4 color, Color4 outlineColor, int samples = 20)
        {
            Vector2[] corners = new Vector2[]
            {
                centre + new Vector2(-radius, 0),
                centre + new Vector2(-radius, radius),
                centre + new Vector2(0, radius * 1.25f),
                centre + new Vector2(radius, radius),
                centre + new Vector2(radius, 0),
            };

            float[] controlPoints = new float[corners.Length * 3];

            for (int index = 0; index < corners.Length; index++)
            {
                controlPoints[index * 3] = corners[index].X;
                controlPoints[index * 3 + 1] = corners[index].Y;
                controlPoints[index * 3 + 2] = 0;
            }

            GL.Map1(MapTarget.Map1Vertex3, 0, 1, 3, corners.Length, controlPoints);
            GL.Enable(EnableCap.Map1Vertex3);

            GL.Color4(color);
            GL.Begin((PrimitiveType)GeometryType.TriangleFan);

            GL.Vertex2(centre);

            for (int index = 0; index < samples; index++)
                GL.EvalCoord1(index / (samples - 1.0));

            GL.End();
            GL.Disable(EnableCap.Map1Vertex3);

            GL.Begin((PrimitiveType)GeometryType.Point);
            DrawPoints(corners, Color4.Green);
            GL.End();

            DrawCurveInternal(corners, color, samples, GeometryType.LineStrip);
        }

        #endregion

        /// <summary>
        /// Clears the backbuffer for this device using the default color
        /// </summary>
        public void Clear()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
        }

        /// <summary>
        /// Clears the backbuffer for this device using the specified color and clear mask
        /// </summary>
        /// <param name="color">The color to clear the color buffer to</param>
        /// <param name="mask">The mask for the buffers to clear</param>
        /// <param name="clearDepth">The value to clear the depth buffer to</param>
        /// <param name="clearStencil">The value to clear the stencil buffer to</param>
        public void Clear(Color4 color, ClearBufferMask mask = ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit, float clearDepth = 0.0f, int clearStencil = 0)
        {
            GL.ClearColor(color);
            GL.ClearDepth(clearDepth);
            GL.ClearStencil(clearStencil);
            GL.Clear(mask);

            // Reset the clear color
            GL.ClearColor(m_clearColor);
        }
        
        /// <summary>
        /// Called when this Graphics device should present it's buffer
        /// </summary>
        internal void SwapBuffers()
        {
            m_gameWindow.SwapBuffers();
        }
    }
}
