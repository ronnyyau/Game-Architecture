using System;
using OpenGL_Game.Managers;

namespace OpenGL_Game
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class MainEntry
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new SceneManager())
                game.Run();
        }
    }
#endif
}
