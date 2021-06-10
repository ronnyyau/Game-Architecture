using OpenGL_Game.Components;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;
using OpenTK;
using System;
using System.Diagnostics;

namespace OpenGL_Game.Managers
{
    class GameCollisionManager : CollisionManager
    {
        GameScene gameScene;
        //This "enable" is for debugging
        public static bool enable = false;
        public GameCollisionManager(GameScene gameScene)
        {
            this.gameScene = gameScene;
        }

        public override void ProcessCollisions()
        {
            if (collisionManifold.Count > 0)
            {
                foreach (Collision collision in collisionManifold)
                {
                    string entityName = collision.entity.Name;
                    if(entityName == "Pellet")
                    {
                        PelletCollision(collision.entity);
                    }
                    if(entityName == "Map")
                    {
                        MapCollision();
                    }
                    if(entityName == "Wall" && enable == false)
                    {
                        WallCollision(collision.entity);
                    }
                    if(entityName.Contains("Ghost"))
                    {
                        GhostCollision(collision.entity);
                    }
                    if(entityName == "PowerUp")
                    {
                        PowerUpCollision(collision.entity);
                    }
                }
            }
            ClearManifold();
        }

        private void PelletCollision(Entity entity)
        {
            //Once the item is ate by player, update the item count, play sound once and delete the item
            GameScene.gameInstance.Player.getComponent<ComponentPlayer>().Pellet++;
            AudioManager.playOnce(entity);
            GameScene.gameInstance.EntityManager.Entities().Remove(entity);
        }
        private void PowerUpCollision(Entity entity)
        {
            //Once the PowerUp is ate by player, update the item count, play sound once,set time to 10s and delete the PowerUp
            GameScene.gameInstance.Player.getComponent<ComponentPlayer>().Pellet++;
            GameScene.gameInstance.Player.getComponent<ComponentPlayer>().Timer = 10f;
            AudioManager.pause(entity);
            GameScene.gameInstance.EntityManager.Entities().Remove(entity);
        }
        private void MapCollision()
        {
            //Making sure the player won't able to move outside the map
            if(gameScene.camera.cameraPosition.Z <= -6.3 || gameScene.camera.cameraPosition.Z >=6.3)
            {
                if(gameScene.camera.cameraPosition.Z <= -6.3)
                {
                    gameScene.camera.cameraPosition.Z = -6.3f;
                }
                else
                {
                    gameScene.camera.cameraPosition.Z = 6.3f;
                }
            }
            if(gameScene.camera.cameraPosition.X <= -6.3 || gameScene.camera.cameraPosition.X >= 6.3)
            {
                if (gameScene.camera.cameraPosition.X <= -6.3)
                {
                    gameScene.camera.cameraPosition.X = -6.3f;
                }
                else
                {
                    gameScene.camera.cameraPosition.X = 6.3f;
                }
            }
        }
        private void WallCollision(Entity entity)
        {
            //Calculate Each box area for collision
            ComponentCollisionLine Box = entity.getComponent<ComponentCollisionLine>();
            Vector3 cameraPosition = gameScene.camera.cameraPosition;
            double RoundX = Math.Round(cameraPosition.X, 1);
            double RoundY = Math.Round(cameraPosition.Z, 1);
            double RoundMaxX = Math.Round(Box.GetMax.X, 1);
            double RoundMaxY = Math.Round(Box.GetMax.Y, 1);
            double RoundMinX = Math.Round(Box.GetMin.X, 1);
            double RoundMinY = Math.Round(Box.GetMin.Y, 1);
            //Top Line
            if (RoundY == RoundMaxY && RoundX < RoundMaxX && RoundX > RoundMinX)
            {
                gameScene.camera.cameraPosition.Z = Box.GetMax.Y;
                Debug.WriteLine("TOP");
            }

            //Right Line
            else if (RoundX == RoundMaxX && RoundY > RoundMaxY && RoundY < RoundMinY)
            {
                gameScene.camera.cameraPosition.X = Box.GetMax.X;
                Debug.WriteLine("Right");
            }

            //Bottom Line
            else if (RoundY == RoundMinY && RoundX < RoundMaxX && RoundX > RoundMinX)
            {
                gameScene.camera.cameraPosition.Z = Box.GetMin.Y;
                Debug.WriteLine("Bottom");
            }

            //Left Line
            else if (RoundX == RoundMinX && RoundY > RoundMaxY && RoundY < RoundMinY)
            {
                gameScene.camera.cameraPosition.X = Box.GetMin.X;
                Debug.WriteLine("Left");
            }
        }
        private void GhostCollision(Entity ghost)
        {
            //Check if PowerUp is active
            if(GameScene.gameInstance.Player.getComponent<ComponentPlayer>().Timer <= 0)
            {
                GameScene.gameInstance.Player.getComponent<ComponentPlayer>().Health--;
                AudioManager.playOnce(ghost);
                //Respawn Player and ghosts back to origin place using ComponentPosition Origin Value
                GameScene.gameInstance.Player.getComponent<ComponentPosition>().Position = GameScene.gameInstance.Player.getComponent<ComponentPosition>().origin;
                GameScene.gameInstance.camera.cameraPosition = GameScene.gameInstance.Player.getComponent<ComponentPosition>().origin;
                foreach(Entity entity in GameScene.gameInstance.EntityManager.Entities())
                {
                    if(entity.Name.Contains("Ghost"))
                    {
                        entity.getComponent<ComponentPosition>().Position = entity.getComponent<ComponentPosition>().origin;
                        entity.endNode = Vector2.Zero;
                    }
                }
            }
            else
            {
                //Only sending eaten ghost back to original
                ghost.getComponent<ComponentPosition>().Position = ghost.getComponent<ComponentPosition>().origin;
                //Here set ghost velcoity back to 0, which will trigger path calculation
                ghost.endNode = Vector2.Zero;
            }
        }
    }
}
