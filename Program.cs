#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace TileRendering
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
            Game1 game = new Game1();
            game.Run();
            /*try
            {
                game.Run();
            }
            catch (Exception e)
            {
                game.Quit();
                throw e;
            }*/
        }
    }
#endif
}
