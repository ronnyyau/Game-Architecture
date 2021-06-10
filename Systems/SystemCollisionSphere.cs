using System;
using System.Collections.Generic;
using OpenGL_Game.Components;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;

namespace OpenGL_Game.Systems
{
    class SystemCollisionSphere : ISystem
    {
        CollisionManager collisionManager;
        public SystemCollisionSphere(CollisionManager collisionManager)
        {
            this.collisionManager = collisionManager;
        }
        public string Name
        {
            get 
            { 
                return "SystemCollision"; 
            }
        }
        const ComponentTypes PLAYERMASK = (ComponentTypes.COMPONENT_COLLISIONSPHERE | ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_PLAYER);
        const ComponentTypes MASK2 = (ComponentTypes.COMPONENT_COLLISIONSPHERE | ComponentTypes.COMPONENT_POSITION);
        public void OnAction(Entity entity)
        {
            if ((entity.Mask & PLAYERMASK) == PLAYERMASK)
            {
                List<IComponent> components = entity.Components;

                IComponent SphereComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_COLLISIONSPHERE;
                });
                IComponent positionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                CheckSphereCollision((ComponentPosition)positionComponent,(ComponentCollisionSphere)SphereComponent);
            }
       }
        public void CheckSphereCollision(ComponentPosition position,ComponentCollisionSphere SphereCol)
        {
            foreach (Entity e in GameScene.gameInstance.EntityManager.Entities())
            {
                if (e.getComponent<ComponentPlayer>() == null)
                {
                    if ((e.Mask & MASK2) == MASK2)
                    {
                        //get another entity information and work out sphere collision
                        ComponentCollisionSphere otherColliderRad = e.getComponent<ComponentCollisionSphere>();
                        ComponentPosition otherCollider = e.getComponent<ComponentPosition>();
                        var dx = position.Position.X - otherCollider.Position.X;
                        var dy = position.Position.Z - otherCollider.Position.Z; 
                        if (Math.Sqrt(dx*dx+dy*dy) < SphereCol.Rad + otherColliderRad.Rad)
                        {
                            collisionManager.CollisionBetweenCamera(e,COLLISIONTYPE.SPHERE_SPHERE);
                            break;
                        }
                    }
                }
            }
        }
    }
}
