using System.Collections.Generic;
using OpenGL_Game.Components;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Systems
{
    class SystemAudio : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_AUDIO | ComponentTypes.COMPONENT_POSITION);
        AudioManager audioManager;
        public string Name
        {
            get { return "SystemAudio"; }
        }
        public SystemAudio(AudioManager audio)
        {
            this.audioManager = audio;
        }
        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent audioComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_AUDIO;
                });
                IComponent positionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                GetSound((ComponentAudio)audioComponent,(ComponentPosition)positionComponent);
                audioManager.AudioUpdate();
            }
        }
        public void GetSound(ComponentAudio audio, ComponentPosition position)
        {
            //update audio position to match moving object
            audio.Audio = position.Position;
        }
    }
}
