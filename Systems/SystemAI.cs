using OpenGL_Game.Components;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using System.Collections.Generic;
using OpenTK;

namespace OpenGL_Game.Systems
{
    class SystemAI : ISystem
    {
        public static bool enable = false;
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_AI);
        public string Name
        {
            get { return "SystemAI"; }
        }
        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent aiComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_AI;
                });
                IComponent VelComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_VELOCITY;
                });
                Path((ComponentAI)aiComponent, entity,(ComponentVelocity)VelComponent);
            }
        }
        private void Path(ComponentAI aI,Entity entity,ComponentVelocity Vel)
        {
            //Make all ghost run its own path
            List<Node> nodes = aI.nodeList;
            //Make AI stop for debug
            if(AIManager.enable)
            {
                if(Vel.velocity != Vector3.Zero)
                {
                    Vel.lastvelocity = Vel.velocity;
                }
                Vel.velocity = Vector3.Zero;
            }
            else
            {
                AIManager.RunPath(nodes, entity, Vel.lastvelocity);
            }

        }
        
    }
}
