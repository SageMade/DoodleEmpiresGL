#region Using Statements
using DoodleEmpires.Core;
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace DoodleEmpires
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //try
            //{
                using (var game = new Game.MainGame())
                    game.Run();

                Logger.Close();
            //}
            //catch(Exception e)
            //{
            //    Logger.LogException(e);
            //    Logger.Close();
            //    throw e.InnerException;
            //}
        }
    }
#endif
}
