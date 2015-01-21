using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using GraphicsD = System.Drawing.Graphics;

namespace DoodleEmpires.Core.Graphics
{
    /// <summary>
    /// A class that uses the System.Drawing interfaces to render text to a OpenGL texture
    /// </summary>
    /// <see cref="http://www.opentk.com/doc/graphics/how-to-render-text-using-opengl"/>
    public class TextRenderer : IDisposable
    {
        int m_glID;
        Bitmap m_bitmap;
        bool m_autoSize;
        int m_width;
        int m_height;

        string m_text;
        Font m_font;
        Color m_color;
        Color m_backgroundColor = Color.Transparent;

        /// <summary>
        /// Gets or sets the text color for this text renderer
        /// </summary>
        public OpenTK.Graphics.Color4 TextColor
        {
            get { return m_color; }
            set
            {
                if (m_color != value)
                {
                    m_color = (Color)value;
                    Invalidate();
                }
            }
        }
        /// <summary>
        /// Gets or sets the background color for this text renderer
        /// </summary>
        public OpenTK.Graphics.Color4 BackgroundColor
        {
            get { return m_backgroundColor; }
            set
            {
                if (m_backgroundColor != value)
                {
                    m_backgroundColor = (Color)value;
                    Invalidate();
                }
            }
        }
        /// <summary>
        /// Gets or sets the font for this text renderer
        /// </summary>
        public Font Font
        {
            get { return m_font; }
            set
            {
                if (m_font != value)
                {
                    m_font = value;
                    Invalidate();
                }
            }
        }
        /// <summary>
        /// Gets or sets the text for this text renderer
        /// </summary>
        public string Text
        {
            get { return m_text; }
            set
            {
                if (m_text != value)
                {
                    m_text = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether this text will automagically resize itself to fit it's text
        /// </summary>
        public bool AutoSize
        {
            get { return m_autoSize; }
            set { m_autoSize = value; }
        }
        /// <summary>
        /// Gets or sets the width for this text renderer
        /// </summary>
        public int Width
        {
            get { return m_width; }
            set
            {
                if (AutoSize)
                    throw new InvalidOperationException("Cannot set width when autosize is set to true");
                else
                {
                    m_width = value;
                    Invalidate();
                }
            }
        }
        /// <summary>
        /// Gets or sets the height for this text renderer
        /// </summary>
        public int Height
        {
            get { return m_height; }
            set
            {
                if (AutoSize)
                    throw new InvalidOperationException("Cannot set height when autosize is set to true");
                else
                {
                    m_height = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets the texture ID for this text renderer
        /// </summary>
        public int TextureID
        {
            get { return m_glID; }
        }

        /// <summary>
        /// Creates a new text rendering instance
        /// </summary>
        /// <param name="text">The text to render in this text renderer</param>
        /// <param name="font">The font to render the text with</param>
        /// <param name="textColor">The color to render the text with</param>
        /// <param name="autoSize">True if this text renderer resizes to fit it's text</param>
        public TextRenderer(string text, Font font, OpenTK.Graphics.Color4 textColor, bool autoSize = true)
        {
            // Copy parameters
            m_text = text;
            m_font = font;
            m_color = (Color)textColor;
            m_autoSize = autoSize;

            // Create a tiny bitmap so we can perform later operations on it
            m_bitmap = new Bitmap(1, 1);

            // Generate the OpenGL texture
            m_glID = GL.GenTexture();

            // Get the currently bound texture to avoid causing issue
            int boundTexture = GL.GetInteger(GetPName.Texture2D);

            // Binds the OpenGL texture for use
            GL.BindTexture(TextureTarget.Texture2D, m_glID);

            // Use linear min and mag filters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);

            // Define the texture's attributes
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, 1, 1, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);

            // Binds the previously bound texture
            GL.BindTexture(TextureTarget.Texture2D, boundTexture);

            // Check for OpenGL errors
            OpenTKUtils.CheckError();

            // Invalidate so we can resize and render the text
            Invalidate();
        }

        /// <summary>
        /// Handles autosizing (if enabled) and calling the UpdateRender method
        /// </summary>
        private void Invalidate()
        {
            // Only autosize if we are required to
            if (m_autoSize)
            {
                // Measure the size of the text using using a GDI graphics device
                using (GraphicsD device = GraphicsD.FromImage(m_bitmap))
                {
                    // Measure the size of the text
                    SizeF size = device.MeasureString(m_text, m_font);

                    // We round up to make sure that the texture will be large enough
                    m_width = (int)(size.Width + 1);
                    m_height = (int)(size.Height + 1);
                }
            }

            // Get the currently bound texture so we can advoid interfering with other code
            int boundTexture = GL.GetInteger(GetPName.Texture2D);

            // Binds the openGL texture so we can use it
            GL.BindTexture(TextureTarget.Texture2D, m_glID);

            // Resize if needed
            Resize();
            // Update the image
            UpdateRender();

            // Restore the bound texture
            GL.BindTexture(TextureTarget.Texture2D, boundTexture);
        }

        /// <summary>
        /// Handles resizing this text renderer's textures
        /// </summary>
        private void Resize()
        {
            // Only resize if the new size is different than the bitmap's size
            if ((m_width != m_bitmap.Width || m_height != m_bitmap.Height) && Width != 0 && Height != 0)
            {
                // We need to re-create the GDI bitmap
                m_bitmap.Dispose();
                m_bitmap = new Bitmap(m_width, m_height);
                
                // We also need to resize the GL texture
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, m_width, m_height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
                // Ensure that no errors occured
                OpenTKUtils.CheckError();
            }
        }

        /// <summary>
        /// Update's this text renderer's texture
        /// </summary>
        private void UpdateRender()
        {
            // We use a GDI drawing device to render the text
            using (GraphicsD device = GraphicsD.FromImage(m_bitmap))
            {
                // Start by clearing the background
                device.Clear(m_backgroundColor);
                // Then render the text
                device.DrawString(m_text, m_font, new SolidBrush(m_color), PointF.Empty);
            }

            // We need to lock the bitmap data so we can access it
            BitmapData data = m_bitmap.LockBits(new System.Drawing.Rectangle(Point.Empty, m_bitmap.Size), 
                ImageLockMode.ReadOnly, m_bitmap.PixelFormat);

            // The image is already bound by the call to invalidate, so we simply update the texture with the GDI texture's
            // data
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            // Unlock the bits so we can work with the bitmap later
            m_bitmap.UnlockBits(data);

            // Make sure that no errors occured
            OpenTKUtils.CheckError();
        }

        /// <summary>
        /// Disposes this text renderer
        /// </summary>
        public void Dispose()
        {
            GL.DeleteTexture(m_glID);
            m_bitmap.Dispose();
        }
    }
}
