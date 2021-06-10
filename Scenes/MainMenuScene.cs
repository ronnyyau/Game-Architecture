using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK.Input;
using OpenGL_Game.Managers;

namespace OpenGL_Game.Scenes
{
    class MainMenuScene : Scene
    {
        public MainMenuScene(SceneManager sceneManager) : base(sceneManager)
        {
            // Set the title of the window
            sceneManager.Title = "Main Menu";
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
            GUI.clearColour = Color.CornflowerBlue;
            //Display the Title
            float width = sceneManager.Width, height = sceneManager.Height, fontSize = Math.Min(width, height) / 10f;
            GUI.Label(new Rectangle(0, (int)(fontSize / 2f), (int)width, (int)(fontSize * 15f)), "Press Enter to play", (int)fontSize-60, StringAlignment.Center);
            GUI.Label(new Rectangle(0, (int)(fontSize / 2f), (int)width, (int)(fontSize * 14f)), "Use WASD to control Player", (int)fontSize - 60, StringAlignment.Center);
            GUI.Label(new Rectangle(0, (int)(fontSize / 2f), (int)width, (int)(fontSize * 2f)), "3D PAC-MAN", (int)fontSize, StringAlignment.Center, Color.Black);
            GUI.Label(new Rectangle(0, (int)(fontSize / 2f), (int)width+17, (int)(fontSize * 2f)), "3D PAC-MAN", (int)fontSize,StringAlignment.Center, Color.Yellow);
            GUI.Render();
        }
        public void Keyboard_KeyDown(KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    sceneManager.StartNewGame();
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