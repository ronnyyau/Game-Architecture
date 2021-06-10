using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK.Input;
using OpenGL_Game.Managers;

namespace OpenGL_Game.Scenes
{
    class GameWinScene : Scene
    {
        public GameWinScene(SceneManager sceneManager) : base(sceneManager)
        {
            // Set the title of the window
            sceneManager.Title = "Game Win!!!";
            // Set the Render and Update delegates to the Update and Render methods of this class
            sceneManager.renderer = Render;
            sceneManager.updater = Update;

            sceneManager.keyboardDownDelegate += Keyboard_KeyDown;
        }

        public override void Update(FrameEventArgs e)
        {
        }

        public override void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, sceneManager.Width, sceneManager.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, sceneManager.Width, 0, sceneManager.Height, -1, 1);

            GUI.clearColour = Color.Cyan;

            //Display the Title
            float width = sceneManager.Width, height = sceneManager.Height, fontSize = Math.Min(width, height) / 10f;
            GUI.Label(new Rectangle(0, 0, (int)width, (int)(fontSize * 2f)), "You win", (int)fontSize, StringAlignment.Center, Color.Yellow);
            GUI.Label(new Rectangle(10, 0, (int)width, (int)(fontSize * 2f)), "You win", (int)fontSize, StringAlignment.Center, Color.Black);
            GUI.Label(new Rectangle(0, 700, (int)width, (int)(fontSize * 0.4f)), "Press M to Main Menu", 30, StringAlignment.Center, Color.White);
            GUI.Label(new Rectangle(0, 750, (int)width, (int)(fontSize * 0.4f)), "Press Enter to play Again", 30, StringAlignment.Center, Color.Green);
            GUI.Label(new Rectangle(0, 800, (int)width, (int)(fontSize * 0.4f)), "Press ESC to Exit", 30, StringAlignment.Center, Color.Red);

            GUI.Render();
        }
        public void Keyboard_KeyDown(KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    sceneManager.StartNewGame();
                    break;
                case Key.M:
                    sceneManager.StartMenu();
                    break;
                case Key.Escape:
                    Close();
                    break;
            }
        }
        public override void Close()
        {
            sceneManager.keyboardDownDelegate -= Keyboard_KeyDown;
        }
    }
}
