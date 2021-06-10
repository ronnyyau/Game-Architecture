using OpenGL_Game.Managers;
using OpenTK;
using OpenTK.Audio.OpenAL;

namespace OpenGL_Game.Components
{
    class ComponentAudio : IComponent
    {
        
        Vector3 sourcePosition;
        int audioBuffer;
        int audioSource;
        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_AUDIO; }
        }
        public ComponentAudio(string filename, Vector3 Position,bool loop)
        {
            audioBuffer = ResourceManager.LoadAudio(filename);
            audioSource = AL.GenSource();
            AL.Source(audioSource, ALSourcei.Buffer, audioBuffer); // attach the buffer to a source
            AL.Source(audioSource, ALSourceb.Looping, loop); // source loops infinitely
            sourcePosition = Position; // give the source a position
            AL.Source(audioSource, ALSource3f.Position, ref sourcePosition);
            if(loop == true)
            {
                AL.SourcePlay(audioSource);
            }
        }
        public Vector3 Audio
        {
            get 
            {
                return sourcePosition; 
            }
            set 
            { 
                sourcePosition = value;
                AL.Source(audioSource, ALSource3f.Position, ref sourcePosition);
            }
        }
        public int AudioSource
        {
            get
            {
                return audioSource;
            }
            set
            {
                audioSource = value;
            }
        }
        public void Close()
        {
            AL.DeleteSource(audioSource);
        }
    }
}