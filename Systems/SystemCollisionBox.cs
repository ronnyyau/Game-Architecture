using OpenGL_Game.Components;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;
using System.Collections.Generic;

namespace OpenGL_Game.Systems
{
    class SystemCollisionBox : ISystem
    {
        public static bool enable = true;
        CollisionManager collisionManager;
        public SystemCollisionBox(CollisionManager collisionManager)
        {
            this.collisionManager = collisionManager;
        }
        public string Name
        {
            get
            {
                return "SystemCollisionBox";
            }
        }
        const ComponentTypes PLAYERMASK = (ComponentTypes.COMPONENT_COLLISIONSPHERE | ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_PLAYER);
        const ComponentTypes MASK2 = (ComponentTypes.COMPONENT_COLLISIONLINE);
        public void OnAction(Entity entity)
        {
            if ((entity.Mask & PLAYERMASK) == PLAYERMASK && enable == true)
            {
                List<IComponent> components = entity.Components;

                IComponent positionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                CheckWallCollision((ComponentPosition)positionComponent);
            }
        }
        public void CheckWallCollision(ComponentPosition pos)
        {
            foreach (Entity e in GameScene.gameInstance.EntityManager.Entities())
            {
                //Identify if the player hit the wall or the boundary of the map
                if (e.Name == "Map")
                {
                    if ((e.Mask & MASK2) == MASK2)
                    {
                        ComponentCollisionLine Wall = e.getComponent<ComponentCollisionLine>();
                        if(pos.Position.Z <= Wall.GetMax.Y || pos.Position.Z >= Wall.GetMin.Y)
                        {
                            collisionManager.CollisionBetweenCamera(e,COLLISIONTYPE.LINE_LINE);
                        }
                        if(pos.Position.X >= Wall.GetMax.X || pos.Position.X <= Wall.GetMin.X)
                        {
                            collisionManager.CollisionBetweenCamera(e, COLLISIONTYPE.LINE_LINE);
                        }
                        
                    }
                }
                else if(e.Name == "Wall")
                {
                    if ((e.Mask & MASK2) == MASK2)
                    {
                        ComponentCollisionLine Wall = e.getComponent<ComponentCollisionLine>();
                        if (pos.Position.Z >= Wall.GetMax.Y && pos.Position.X <= Wall.GetMax.X &&
                           pos.Position.Z <= Wall.GetMin.Y && pos.Position.X >= Wall.GetMin.X)
                        {
                            collisionManager.CollisionBetweenCamera(e, COLLISIONTYPE.LINE_LINE);
                            //Debug.WriteLine("HIT1");
                        }
                    }
                }
            }
        }
    }
}
