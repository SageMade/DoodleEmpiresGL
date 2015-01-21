using DoodleEmpires.Core;
using DoodleEmpires.Core.Graphics;
using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;

namespace DoodleEmpires.Game
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Core.Game
    {
        VertexBuffer vBuffer;
        IndexBuffer iBuffer;
        ShaderProgram sampleShader;
        ShaderProgram basicShader;
        ShaderProgram currentShader;
        ShaderProgram textureShader;
        TextRenderer textRenderer;

        RenderTarget renderTarget;

        int samples = 15;
        Vector2 start;
        Vector2 end;
        Vector2 controlA;
        Vector2 controlB;

        Random rand = new Random();

        Vector2[] curve;

        const float moveSpeed = 0.01f;

        public MainGame()
            : base()
        {

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Window.KeyDown += Window_KeyDown;
            Window.KeyPress += Window_KeyPress;

            start = -Vector2.One;
            end = Vector2.One;
            controlA = new Vector2(-0.3f, -1f);
            controlB = new Vector2(0.3f, 1f);

            GenRandomCurve();

                base.Initialize();
        }

        void Window_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'r')
            {
                GenRandomCurve();
            }
        }

        void Window_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.F1)
                currentShader = currentShader == basicShader ? sampleShader : basicShader;

            if (e.Key == OpenTK.Input.Key.Space)
                samples += 1;
            if (e.Key == OpenTK.Input.Key.ControlLeft)
                samples -= 1;

            if (e.Key == OpenTK.Input.Key.A)
                controlA.X -= moveSpeed;
            if (e.Key == OpenTK.Input.Key.D)
                controlA.X += moveSpeed;
            if (e.Key == OpenTK.Input.Key.W)
                controlA.Y += moveSpeed;
            if (e.Key == OpenTK.Input.Key.S)
                controlA.Y -= moveSpeed;

            if (e.Key == OpenTK.Input.Key.Left)
                controlB.X -= moveSpeed;
            if (e.Key == OpenTK.Input.Key.Right)
                controlB.X += moveSpeed;
            if (e.Key == OpenTK.Input.Key.Up)
                controlB.Y += moveSpeed;
            if (e.Key == OpenTK.Input.Key.Down)
                controlB.Y -= moveSpeed;
            
            if (e.Key == Key.Z)
                OpenTK.Graphics.OpenGL.GL.PolygonMode(OpenTK.Graphics.OpenGL.MaterialFace.FrontAndBack, OpenTK.Graphics.OpenGL.PolygonMode.Line);
            if (e.Key == Key.X)
                OpenTK.Graphics.OpenGL.GL.PolygonMode(OpenTK.Graphics.OpenGL.MaterialFace.FrontAndBack, OpenTK.Graphics.OpenGL.PolygonMode.Fill);

            samples = samples < 2 ? 2 : samples;
        }

        private void GenRandomCurve()
        {
            int points = 2 + rand.Next(10);

            curve = new Vector2[points];

            curve[0] = new Vector2(-2, 0);

            for (int i = 1; i < points - 1; i++)
            {
                curve[i] = new Vector2(((float)rand.NextDouble() - 0.5f) * 4.0f, ((float)rand.NextDouble() - 0.5f) * 2.0f);
            }

            curve[points - 1] = new Vector2(2, 0);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            renderTarget = new RenderTarget(200, 200, RenderTargetFlags.Color);

            vBuffer = Graphics.CreateVertexBuffer();
            iBuffer = Graphics.CreateIndexBuffer();

            VertexColor[] verts = new VertexColor[]
            {
                new VertexColor(new Vector2(0, 0), Color4.Red),
                new VertexColor(new Vector2(0, 0.5f), Color4.Blue),
                new VertexColor(new Vector2(0.5f, 0), Color4.Green),
                new VertexColor(new Vector2(0.5f, 0.5f), Color4.White),
            };

            vBuffer.SetData(verts);

            ushort[] indices = new ushort[]
            {
                0, 1, 2, 3
            };

            iBuffer.SetData(indices);

            System.Drawing.Text.InstalledFontCollection fonts = new System.Drawing.Text.InstalledFontCollection();

            textRenderer = new TextRenderer("Sample text", new System.Drawing.Font("Batang", 12.0f), Color4.White, true);
            textRenderer.BackgroundColor = new Color4(0,0,0,0.4f);

            sampleShader = ShaderProgram.LoadFromPath(@"Content", "Sample");
            basicShader = ShaderProgram.LoadFromPath(@"Content", "Basic");
            textureShader = ShaderProgram.LoadFromPath(@"Content", "Textured");

            currentShader = basicShader;

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(double elapsedTime)
        {
           
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(double elapsedTime)
        {
            currentShader.Apply();

            renderTarget.Bind();
            Graphics.Clear(Color4.Black);

            Graphics.SetMatrix(Matrix4.CreateScale(0.5f, 0.5f * (Window.Width / (float)Window.Height), 1.0f));
            Graphics.SetLineWidth(3.0f);

            Graphics.DrawBezierCurve(start, end, controlA, controlB, Color4.Red, samples);
            //Graphics.DrawBezierCurve(new Vector2(-1, -1), new Vector2(1, -1), new Vector2(-1, 1), new Vector2(1, 1), Color4.Red, samples);
            // Graphics.DrawArc(new Vector2(-1, -1), new Vector2(1, -1), new Vector2(0, 1), Color4.Red, samples);
            Graphics.DrawCurve(curve, Color4.Blue, samples);
            Graphics.DrawCircleFilled(Vector2.Zero, 0.75f, Color4.White, Color4.Red, samples);

            Graphics.SetPointSize(5.0f);
            Graphics.Begin(GeometryType.Point);

            Graphics.DrawPoints(curve, Color4.Blue);

            Graphics.DrawPoint(start, Color4.Green);
            Graphics.DrawPoint(end, Color4.Green);
            Graphics.DrawPoint(controlA, Color4.Green);
            Graphics.DrawPoint(controlB, Color4.Green);

            Graphics.End();

            Graphics.SetLineWidth(1);
            Graphics.Begin(GeometryType.LineList);

            Graphics.DrawLine(new Vector2(-1, 0.75f), new Vector2(1, 0.75f), Color4.White);

            //Graphics.DrawLine(-Vector2.One, Vector2.One, Color4.Black);
            //Graphics.DrawLine(new Vector2(-1, 1), new Vector2(1, -1), Color4.Black);

            Graphics.End();

            RenderTarget.Unbind();

            Graphics.Clear(Color4.CornflowerBlue);

            textureShader.Apply();

            OpenTK.Graphics.OpenGL.GL.LoadIdentity();
            OpenTK.Graphics.OpenGL.GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Projection);
            OpenTK.Graphics.OpenGL.GL.LoadIdentity();
            OpenTK.Graphics.OpenGL.GL.Ortho(0, 800, 600, 0, -1, 1);

            OpenTK.Graphics.OpenGL.GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, textRenderer.TextureID);

            renderTarget.Color.Bind();

            OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Texture2D);
            OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Blend);
            OpenTK.Graphics.OpenGL.GL.BlendFunc(OpenTK.Graphics.OpenGL.BlendingFactorSrc.One, OpenTK.Graphics.OpenGL.BlendingFactorDest.OneMinusSrcAlpha);

            OpenTK.Graphics.OpenGL.GL.Begin(OpenTK.Graphics.OpenGL.PrimitiveType.Quads);
            OpenTK.Graphics.OpenGL.GL.TexCoord2(0, 1);
            OpenTK.Graphics.OpenGL.GL.Vertex2(0, 200);
            OpenTK.Graphics.OpenGL.GL.TexCoord2(1, 1);
            OpenTK.Graphics.OpenGL.GL.Vertex2(200, 200);
            OpenTK.Graphics.OpenGL.GL.TexCoord2(1, 0);
            OpenTK.Graphics.OpenGL.GL.Vertex2(200, 0);
            OpenTK.Graphics.OpenGL.GL.TexCoord2(0, 0);
            OpenTK.Graphics.OpenGL.GL.Vertex2(0, 0);
            OpenTK.Graphics.OpenGL.GL.End();

            OpenTK.Graphics.OpenGL.GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, 0);
            OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Blend);
            OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Texture2D);

            OpenTK.Graphics.OpenGL.GL.LoadIdentity();
            OpenTK.Graphics.OpenGL.GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Modelview);
            OpenTK.Graphics.OpenGL.GL.LoadIdentity();
            
            // Graphics.DrawFilledRectangle(new Rectangle(-0.25f, -0.25f, 0.5f, 0.5f), Color4.Red, Color4.Black, 4.0f);

            // TODO: implement display lists

            // Graphics.DrawPrimitives(vBuffer, GeometryType.TriangleStrip);
            //Graphics.DrawIndexedPrimitives(vBuffer, iBuffer, GeometryType.TriangleList);
        }
    }
}
