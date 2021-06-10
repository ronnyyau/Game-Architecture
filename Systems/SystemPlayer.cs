using OpenGL_Game.Components;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;
using System.Collections.Generic;

namespace OpenGL_Game.Systems
{
    class SystemPlayer : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_PLAYER);
        public string Name
        {
            get
            {
                return "SystemPlayer";
            }
        }
        public void OnAction(Entity entity)
        {
            if((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent playerComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_PLAYER;
                });
                Timer((ComponentPlayer)playerComponent, entity);
            }
        }
        private void Timer(ComponentPlayer player,Entity entity)
        {
            //If the powerUp is active, GameCollisionManager will add 10 second in, which here will countdown
            if(player.Timer > 0)
            {
                player.Timer -= GameScene.dt;
                AudioManager.playOnce(entity);
            }
        }
    }
}
