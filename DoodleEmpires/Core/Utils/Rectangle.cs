using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoodleEmpires.Core
{
    public class Rectangle
    {
        float m_x;
        float m_y;
        float m_width;
        float m_height;

        public float X
        {
            get { return m_x; }
            set {  m_x = value; }
        }
        public float Y
        {
            get { return m_y; }
            set { m_y = value; }
        }
        public float Width
        {
            get { return m_width; }
            set { m_width = value; }
        }
        public float Height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        public float Left
        {
            get { return m_x; }
            set { m_x = value; }
        }
        public float Right
        {
            get { return m_x + m_width; }
            set { m_width = value - m_x; }
        }
        public float Top
        {
            get { return m_y + m_height; }
            set { m_height = value - m_y; }
        }
        public float Bottom
        {
            get { return m_y; }
            set { m_y = value; }
        }

        public Vector2 TopLeft
        {
            get { return new Vector2(Left, Top); }
        }
        public Vector2 TopRight
        {
            get { return new Vector2(Right, Top); }
        }
        public Vector2 BottomLeft
        {
            get { return new Vector2(Left, Bottom); }
        }
        public Vector2 BottomRight
        {
            get { return new Vector2(Right, Bottom); }
        }

        public Rectangle(float x, float y, float width, float height)
        {
            m_x = x;
            m_y = y;
            m_width = width;
            m_height = height;
        }

        public bool Intersects(Rectangle other)
        {
            return (other.Left <= Right || other.Right >= Left) && (other.Bottom <= Top && other.Top >= Bottom);
        }

        public static implicit operator System.Drawing.Rectangle(Rectangle item)
        {
            return new System.Drawing.Rectangle((int)item.X, (int)item.Y, (int)(item.Width + 1), (int)(item.Height + 1));
        }
    }
}
