using OpenGL_Game.Components;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;
using OpenTK;
using System;
using System.Collections.Generic;

namespace OpenGL_Game.Managers
{
    class AIManager
    {
        static float x = 0;
        static float z = 0;
        public static bool enable;
        static Vector2 StoreEnd = Vector2.Zero;

        public AIManager()
        {

        }
        public static void RunPath(List<Node> Path,Entity entity,Vector3 lastspeed)
        {
            if(entity.getComponent<ComponentVelocity>().velocity == Vector3.Zero)
            {
                entity.getComponent<ComponentVelocity>().velocity = lastspeed;
            }
            Path = SetLoop(entity, Path);
            Vector2 End = Vector2.Zero;
            Vector2 Start = Vector2.Zero;
            Vector3 GlobalStart = Vector3.Zero;
            float speed = 1.7f;
            //If the AI don't have a next node to walk to
            if (entity.endNode == Vector2.Zero)
            {
                for (int i = 0; i < Path.Count; i++)
                {
                    //Loop through each node and try to identify where is the AI
                    if (Path[i].X == entity.getComponent<ComponentPosition>().Position.X && 
                        Path[i].Y == entity.getComponent<ComponentPosition>().Position.Z)
                    {
                        //Setting the next node to AI
                        End = new Vector2(Path[i].NextNode.X, Path[i].NextNode.Y);
                        entity.endNode = End;
                        Start = new Vector2(Path[i].X, Path[i].Y);
                        //Get direction and set const speed
                        x = (Start.X - End.X);
                        z = (Start.Y - End.Y);
                        if (x == 0)
                        {
                            if (z > 0)
                            {
                                z = speed;
                            }
                            else
                            {
                                z = -speed;
                            }
                        }
                        else if (z == 0)
                        {
                            if (x > 0)
                            {
                                x = speed;
                            }
                            else
                            {
                                x = -speed;
                            }
                        }
                        //Set AI velcoity to speed and make the render object match its AI speed
                        entity.getComponent<ComponentVelocity>().velocity = new Vector3(-x, 0, -z);
                        Vector3 ghostSpeed = entity.getComponent<ComponentVelocity>().velocity;
                        double X = Math.Round(ghostSpeed.X, 1);
                        double Z = Math.Round(ghostSpeed.Z, 1);
                        Vector3 startPosition = new Vector3(Start.X, 0.14f, Start.Y);
                        //For speeding up Ghost Green and Pink for triggered condition
                        if(entity.Name == "GhostGreen" || entity.Name == "GhostPink")
                        {
                            GlobalStart = startPosition;
                        }
                        //Using AI speed to identify which direction should look at
                        if (X > 0)
                        {
                            entity.Model = Matrix4.CreateRotationY(0) * Matrix4.CreateTranslation(startPosition);
                            entity.Model = Matrix4.CreateRotationY(1.5708f) * Matrix4.CreateTranslation(startPosition);
                        }
                        else if (X < 0)
                        {
                            entity.Model = Matrix4.CreateRotationY(0) * Matrix4.CreateTranslation(startPosition);
                            entity.Model = Matrix4.CreateRotationY(-1.5708f) * Matrix4.CreateTranslation(startPosition);
                        }
                        else if (Z < 0)
                        {
                            entity.Model = Matrix4.CreateRotationY(0) * Matrix4.CreateTranslation(startPosition);
                            entity.Model = Matrix4.CreateRotationY(3.14159f) * Matrix4.CreateTranslation(startPosition);
                        }
                        else
                        {
                            entity.Model = Matrix4.CreateRotationY(0) * Matrix4.CreateTranslation(startPosition);
                        }
                    }
                }
            }
            else
            {
                Vector3 position = entity.getComponent<ComponentPosition>().Position;
                End = entity.endNode;
                //For Ghost Red return back to appropriate node after chasing
                if(entity.Name == "GhostRed")
                {
                    StoreEnd = End;
                }
                //if the AI reach a node
                if (Math.Round(position.X, 1) == Math.Round(End.X, 1) &&
                Math.Round(position.Z, 1) == Math.Round(End.Y, 1))
                {
                    //Set AI velocity and its endnode to be 0, so AIManager can calculate next node to walk to
                    entity.getComponent<ComponentVelocity>().velocity = new Vector3(0, 0, 0);
                    Vector3 setPosition = new Vector3(End.X, 0.14f, End.Y);
                    entity.getComponent<ComponentPosition>().Position = setPosition;
                    entity.endNode = Vector2.Zero;
                }
                else if((entity.Name == "GhostGreen" || entity.Name == "GhostPink") && GameScene.gameInstance.Player.getComponent<ComponentPlayer>().Timer <= 0)
                {
                    //If the powerUp is unactive, and player is in an area which Ghost Green petroling, Ghost green will try to trap player in by speeding up
                    //Ghost Pink will speed up if player in Green or Red petrol area
                    //Making sure Ghost Pink and Green is back to normal speed before it speed up again after direct chase player
                    if (GameScene.gameInstance.Player.getComponent<ComponentPosition>().Position.X >= 3 && GameScene.gameInstance.Player.getComponent<ComponentPosition>().Position.X <= 6.3f &&
                       GameScene.gameInstance.Player.getComponent<ComponentPosition>().Position.Z <= -1.93f && GameScene.gameInstance.Player.getComponent<ComponentPosition>().Position.Z >= -6.3f && entity.chaseFinish == false ||
                       (GameScene.gameInstance.Player.getComponent<ComponentPosition>().Position.X >= -6.3f && GameScene.gameInstance.Player.getComponent<ComponentPosition>().Position.X <= -0.95f &&
                       GameScene.gameInstance.Player.getComponent<ComponentPosition>().Position.Z <= 6.3f && GameScene.gameInstance.Player.getComponent<ComponentPosition>().Position.Z >= 0.66f && entity.Name == "GhostPink" && entity.chaseFinish == false))
                    {
                        Vector3 ghostSpeed = entity.getComponent<ComponentVelocity>().velocity;
                        double X = Math.Round(ghostSpeed.X, 1);
                        double Z = Math.Round(ghostSpeed.Z, 1);
                        if (X > 0)
                        {
                            entity.getComponent<ComponentVelocity>().velocity = new Vector3 (5, 0, 0);
                            entity.Model = Matrix4.CreateRotationY(0) * Matrix4.CreateTranslation(GlobalStart);
                            entity.Model = Matrix4.CreateRotationY(1.5708f) * Matrix4.CreateTranslation(GlobalStart);
                        }
                        else if (X < 0)
                        {
                            entity.getComponent<ComponentVelocity>().velocity = new Vector3(-5, 0, 0);
                            entity.Model = Matrix4.CreateRotationY(0) * Matrix4.CreateTranslation(GlobalStart);
                            entity.Model = Matrix4.CreateRotationY(-1.5708f) * Matrix4.CreateTranslation(GlobalStart);
                        }
                        else if (Z < 0)
                        {
                            entity.getComponent<ComponentVelocity>().velocity = new Vector3(0, 0, -5);
                            entity.Model = Matrix4.CreateRotationY(0) * Matrix4.CreateTranslation(GlobalStart);
                            entity.Model = Matrix4.CreateRotationY(3.14159f) * Matrix4.CreateTranslation(GlobalStart);
                        }
                        else
                        {
                            entity.getComponent<ComponentVelocity>().velocity = new Vector3(0, 0, 5);
                            entity.Model = Matrix4.CreateRotationY(0) * Matrix4.CreateTranslation(GlobalStart);
                        }
                    }
                }
                else if(entity.Name == "GhostRed")
                {
                    //Ghost Red will direct chase player if player is within Ghost Red petroling area
                    if (GameScene.gameInstance.Player.getComponent<ComponentPosition>().Position.X >= -6.3f && GameScene.gameInstance.Player.getComponent<ComponentPosition>().Position.X <= -0.95f &&
                       GameScene.gameInstance.Player.getComponent<ComponentPosition>().Position.Z <= 6.3f && GameScene.gameInstance.Player.getComponent<ComponentPosition>().Position.Z >= 0.66f && GameScene.gameInstance.Player.getComponent<ComponentPlayer>().Timer <= 0)
                    {
                        Chase(entity, new Vector2(GameScene.gameInstance.Player.getComponent<ComponentPosition>().Position.X, GameScene.gameInstance.Player.getComponent<ComponentPosition>().Position.Z));
                        entity.endNode = StoreEnd;
                    }
                    else
                    {
                        Chase(entity, entity.endNode);
                    }
                }
                //If player is within AI's Radius, AI will chase player directly
                if ((entity.getComponent<ComponentPosition>().Position - GameScene.gameInstance.Player.getComponent<ComponentPosition>().Position).Length < 1f && GameScene.gameInstance.Player.getComponent<ComponentPlayer>().Timer <= 0 && entity.Name == "GhostBlue")
                {
                    //This boolean is for indentify which AI is finish chasing play and need to go back to its place
                    entity.chaseFinish = true;
                    Chase(entity, new Vector2(GameScene.gameInstance.Player.getComponent<ComponentPosition>().Position.X, GameScene.gameInstance.Player.getComponent<ComponentPosition>().Position.Z));
                }
                else if (entity.chaseFinish == true)
                {
                    Chase(entity, entity.endNode);
                    entity.chaseFinish = false;
                }
            }
        }
        public static List<Node> SetLoop(Entity entity, List<Node> nodes)
        {
            //Setting each node have a next node for ghost to get and follow after it reach the node
            nodes[nodes.Count - 1].NextNode = nodes[entity.getComponent<ComponentAI>().loopIndex];
            return nodes;
        }
        public static void Chase(Entity entity, Vector2 EndNode)
        {
            //Setting start position of the path always in AI current position
            Vector2 Start = new Vector2(entity.getComponent<ComponentPosition>().Position.X, entity.getComponent<ComponentPosition>().Position.Z);
            Vector2 End = EndNode;
            x = (Start.X - End.X);
            z = (Start.Y - End.Y);
            if (x == 0)
            {
                if (z > 0)
                {
                    z = 1.7f;
                }
                else
                {
                    z = -1.7f;
                }
            }
            else if (z == 0)
            {
                if (x > 0)
                {
                    x = 1.7f;
                }
                else
                {
                    x = -1.7f;
                }
            }
            entity.getComponent<ComponentVelocity>().velocity = new Vector3(-x, 0, -z);
            Vector3 ghostSpeed = entity.getComponent<ComponentVelocity>().velocity;
            double X = Math.Round(ghostSpeed.X, 1);
            double Z = Math.Round(ghostSpeed.Z, 1);
            Vector3 startPosition = new Vector3(Start.X, 0.14f, Start.Y);
            if (X > 0)
            {
                entity.Model = Matrix4.CreateRotationY(0) * Matrix4.CreateTranslation(startPosition);
                entity.Model = Matrix4.CreateRotationY(1.5708f) * Matrix4.CreateTranslation(startPosition);
            }
            else if (X < 0)
            {
                entity.Model = Matrix4.CreateRotationY(0) * Matrix4.CreateTranslation(startPosition);
                entity.Model = Matrix4.CreateRotationY(-1.5708f) * Matrix4.CreateTranslation(startPosition);
            }
            else if (Z < 0)
            {
                entity.Model = Matrix4.CreateRotationY(0) * Matrix4.CreateTranslation(startPosition);
                entity.Model = Matrix4.CreateRotationY(3.14159f) * Matrix4.CreateTranslation(startPosition);
            }
            else
            {
                entity.Model = Matrix4.CreateRotationY(0) * Matrix4.CreateTranslation(startPosition);
            }
        }
    }
}
