using OpenTK;
using OpenGL_Game.Managers;

namespace OpenGL_Game.Scenes
{
    abstract class Scene : IScene
    {
        protected SceneManager sceneManager;

        public Scene(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
        }

        public abstract void Render(FrameEventArgs e);

        public abstract void Update(FrameEventArgs e);

        public abstract void Close();
    }
}
