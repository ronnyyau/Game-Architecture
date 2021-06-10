using OpenTK;

namespace OpenGL_Game.Scenes
{
    interface IScene
    {
        void Render(FrameEventArgs e);
        void Update(FrameEventArgs e);
        void Close();
    }
}
