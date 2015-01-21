using DoodleEmpires.Core.Graphics;
using OpenTK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DoodleEmpires.Core
{
	/// <summary>
	/// The base class for an OpenTK enabled game
	/// </summary>
    public abstract class Game : IDisposable
    {
        GameWindow m_window;
        protected GraphicsDevice Graphics;

		/// <summary>
		/// Gets the game window associated with this game
		/// </summary>
		protected GameWindow Window
        {
            get { return m_window; }
        }
		/// <summary>
		/// Gets or sets the border style for this game's window 
        /// default is WindowBorder.Fixed
		/// </summary>
        protected WindowBorder WindowBorderStyle
        {
            get
            {
                return m_window.WindowBorder;
            }
            set
            {
                m_window.WindowBorder = value;
            }
        }

        /// <summary>
        /// Creates a new game instance
        /// </summary>
		/// <param name="title">The title for the game's window</param>
        public Game(string title = "OpenTK Game")
        {
            // Create a new window for this game to diplay content in
            m_window = new GameWindow(800, 600, OpenTK.Graphics.GraphicsMode.Default, title);

            // Wire the window's events to the event handlers
            m_window.UpdateFrame += m_window_UpdateFrame;
            m_window.RenderFrame += m_window_RenderFrame;
            m_window.Load += m_window_Load;
            m_window.Unload += m_window_Unload;
            m_window.Closing += m_window_Closing;
            m_window.Closed += m_window_Closed;
            m_window.Resize += m_window_Resize;

            // Set the default window border style
            m_window.WindowBorder = WindowBorder.Fixed;
        }

		/// <summary>
		/// Runs this game
		/// </summary>
		/// <param name="updatesPerSecond">The target number of updates per second</param>
        /// <param name="framesPerSecond">The target number of frames per second</param>
        public void Run(double updatesPerSecond = 60.0, double framesPerSecond = 60.0)
        {
			// Creates the graphics device to use for rendering
            Graphics = new GraphicsDevice(m_window);

			// Begin pre-graphics initialization
            Initialize();

			// Begin the window's update cycle
            m_window.Run(updatesPerSecond, framesPerSecond);
        }

        #region OpenTK Wrapper

		/// <summary>
		/// Routes the window's load event to the LoadContent method
		/// </summary>
		/// <param name="sender">The object to invoke the event (m_window)</param>
		/// <param name="e">The event arguments for this event</param>
        void m_window_Load(object sender, EventArgs e)
        {
            LoadContent();
        }
        /// <summary>
        /// Routes the window's unload event to the UnloadContent method
        /// </summary>
        /// <param name="sender">The object to invoke the event (m_window)</param>
        /// <param name="e">The event arguments for this event</param>
        void m_window_Unload(object sender, EventArgs e)
        {
            UnloadContent();
        }
        /// <summary>
        /// Routes the windows render frame event to the Draw method
        /// </summary>
        /// <param name="sender">The object to invoke the event (m_window)</param>
        /// <param name="e">The frame event arguments for this frame</param>
        void m_window_RenderFrame(object sender, FrameEventArgs e)
        {
            Draw(e.Time);
            Graphics.SwapBuffers();
        }
        /// <summary>
        /// Routes the windows update frame event to the Update method
        /// </summary>
        /// <param name="sender">The object to invoke the event (m_window)</param>
        /// <param name="e">The frame event arguments for this frame</param>
        void m_window_UpdateFrame(object sender, FrameEventArgs e)
        {
            Update(e.Time);
        }
        /// <summary>
        /// Routes the windows closing event to the Closing method
        /// </summary>
        /// <param name="sender">The object to invoke the event (m_window)</param>
        /// <param name="e">The cancel event used to cancel form closing</param>
        void m_window_Closing(object sender, CancelEventArgs e)
        {

        }
        /// <summary>
        /// Routes the windows closed event to the Closed method
        /// </summary>
        /// <param name="sender">The object to invoke the event (m_window)</param>
        /// <param name="e">The event arguments for this event</param>
        void m_window_Closed(object sender, EventArgs e)
        {
            Closed();
        }
        /// <summary>
        /// Routes the windows resized event to the Resized method
        /// </summary>
        /// <param name="sender">The object to invoke the event (m_window)</param>
        /// <param name="e">The event arguments for this event</param>
        void m_window_Resize(object sender, EventArgs e)
        {
            Resize();
        }

        #endregion

        #region Methods

        /// <summary>
		/// This is called when the form has been resized
		/// </summary>
        protected virtual void Resize()
        {

        }

		/// <summary>
		/// This is called when the form is closed
		/// </summary>
		protected virtual void Closed()
        {

        }

		/// <summary>
		/// This is called when the form is about to close. This can
        /// be used to cancel the form's closing
		/// </summary>
		/// <param name="e">The cancel event args that can be used to cancel the close operation</param>
		protected virtual void Closing(CancelEventArgs e)
        {

        }

		/// <summary>
		/// Called when the game needs to initilize itself. This is called before
        /// the game's window has been initalized and run (ie no graphics)
		/// </summary>
        protected virtual void Initialize()
        {

        }

		/// <summary>
		/// Called when the game needs to load it's content
		/// </summary>
        protected virtual void LoadContent()
        {

        }

        /// <summary>
        /// Called when the game needs to unload it's content
        /// </summary>
        protected virtual void UnloadContent()
        {

        }

		/// <summary>
		/// Called when the game needs to render a frame
		/// </summary>
		/// <param name="elapsedTime">The time since the last frame in seconds</param>
        protected abstract void Draw(double elapsedTime);

        /// <summary>
        /// Called when the game needs to update
        /// </summary>
        /// <param name="elapsedTime">The time since the last update in seconds</param>
        protected abstract void Update(double elapsedTime);

        #endregion

        public void Dispose()
        {
            m_window.Dispose();
        }
    }
}
