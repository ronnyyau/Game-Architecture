using OpenGL_Game.Components;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;
using OpenTK;
using OpenTK.Audio.OpenAL;

namespace OpenGL_Game.Managers
{
    class AudioManager
    {
        public AudioManager()
        {

        }
        public static void playOnce(Entity entity)
        {
            //Play Once
            int source = entity.getComponent<ComponentAudio>().AudioSource;
            AL.SourcePlay(source);
        }
        public static void pause(Entity entity)
        {
            //Pause selected audio
            int source = entity.getComponent<ComponentAudio>().AudioSource;
            AL.SourcePause(source);
        }

        public void AudioUpdate()
        {
            //Update audio position
            AL.Listener(ALListener3f.Position, ref GameScene.gameInstance.camera.cameraPosition);
            AL.Listener(ALListenerfv.Orientation, ref GameScene.gameInstance.camera.cameraDirection, ref GameScene.gameInstance.camera.cameraUp);
        }
    }
}
