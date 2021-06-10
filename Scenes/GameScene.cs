using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenGL_Game.Components;
using OpenGL_Game.Systems;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using System.Drawing;
using System;
using System.IO;

namespace OpenGL_Game.Scenes
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>#

    class GameScene : Scene
    {
        public int ItemCollect = 0;
        public static float dt = 0;
        public static bool Load = false;
        EntityManager entityManager;
        SystemManager systemManager;
        CollisionManager collisionManager;
        AudioManager audioManager;
        public Camera camera;
        public Minimap minimap;
        bool[] keyPressed = new bool[255];
        public static GameScene gameInstance;
        public Entity Player;
        public EntityManager EntityManager 
        {
            get 
            { 
                return entityManager; 
            } 
        }
        public GameScene(SceneManager sceneManager) : base(sceneManager)
        {
            gameInstance = this;
            entityManager = new EntityManager();
            systemManager = new SystemManager();
            collisionManager = new GameCollisionManager(this);
            audioManager = new AudioManager();
            // Set the title of the window
            sceneManager.Title = "Game";
            // Set the Render and Update delegates to the Update and Render methods of this class
            sceneManager.renderer = Render;
            sceneManager.updater = Update;
            // Set Keyboard events to go to a method in this class
            sceneManager.keyboardDownDelegate += Keyboard_KeyDown;
            sceneManager.keyboardUpDelegate += Keyboard_KeyUp;

            // Enable Depth Testing
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
            GL.Disable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            // Set Camera
            camera = new Camera(new Vector3(0, 0.2f, 5), new Vector3(0, 0.2f, 0), (float)(sceneManager.Width) / (float)(sceneManager.Height), 0.1f, 100f);
            //Set Minimap Camera
            minimap = new Minimap(new Vector3(0, 15, 0.1f), new Vector3(0, 0, 0), (float)(sceneManager.Width) / (float)((sceneManager.Height)*1.70), 0.1f, 30f);

            CreateEntities();
            CreateSystems();
        }

        private void CreateEntities()
        {
            //Create all Entities here
            Entity newEntity;
            Vector3 sourcePosition = new Vector3(10.0f, 0.0f, 0.0f);

            newEntity = new Entity("SkyBox");
            sourcePosition = new Vector3(0.0f, 0.0f, 0.0f);
            newEntity.AddComponent(new ComponentPosition(sourcePosition));
            newEntity.AddComponent(new ComponentGeometry("Geometry/SkyBox/SkyBox.obj"));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Map");
            sourcePosition = new Vector3(0.0f, 0.0f, 0.0f);
            newEntity.AddComponent(new ComponentPosition(sourcePosition));
            newEntity.AddComponent(new ComponentGeometry("Geometry/Maze/Maze.obj"));
            newEntity.AddComponent(new ComponentCollisionLine(new Vector2(6.3f,-6.3f),new Vector2(-6.3f,6.3f)));
            entityManager.AddEntity(newEntity);

            CreateWall(newEntity);

            newEntity = new Entity("Player");
            newEntity.AddComponent(new ComponentPosition(camera.cameraPosition));
            newEntity.AddComponent(new ComponentCollisionSphere(0.1f));
            newEntity.AddComponent(new ComponentGeometry("Geometry/Player/Player.obj"));
            newEntity.AddComponent(new ComponentAudio("Audio/invincible.wav", sourcePosition, false));
            newEntity.AddComponent(new ComponentPlayer());
            Player = newEntity;
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("GhostPink");
            sourcePosition = new Vector3(0.5f, 0.14f, -1f);
            newEntity.AddComponent(new ComponentPosition(sourcePosition));
            newEntity.AddComponent(new ComponentVelocity(0, 0, 0));
            newEntity.AddComponent(new ComponentCollisionSphere(0.11f));
            newEntity.AddComponent(new ComponentGeometry("Geometry/Ghost/ghostPink.obj"));
            newEntity.AddComponent(new ComponentAudio("Audio/Hit_Ghost.wav", sourcePosition, false));
            newEntity.AddComponent(new ComponentAI("Path/Pink.txt", 4));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("GhostRed");
            sourcePosition = new Vector3(0.5f, 0.14f, -0.5f);
            newEntity.AddComponent(new ComponentPosition(sourcePosition));
            newEntity.AddComponent(new ComponentVelocity(0, 0, 0));
            newEntity.AddComponent(new ComponentCollisionSphere(0.11f));
            newEntity.AddComponent(new ComponentGeometry("Geometry/Ghost/GhostRed.obj"));
            newEntity.AddComponent(new ComponentAudio("Audio/Hit_Ghost.wav", sourcePosition, false));
            newEntity.AddComponent(new ComponentAI("Path/Red.txt", 2));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("GhostGreen");
            sourcePosition = new Vector3(0.5f, 0.14f, 0f);
            newEntity.AddComponent(new ComponentPosition(sourcePosition));
            newEntity.AddComponent(new ComponentVelocity(0, 0, 0));
            newEntity.AddComponent(new ComponentCollisionSphere(0.11f));
            newEntity.AddComponent(new ComponentGeometry("Geometry/Ghost/GhostGreen.obj"));
            newEntity.AddComponent(new ComponentAudio("Audio/Hit_Ghost.wav", sourcePosition, false));
            newEntity.AddComponent(new ComponentAI("Path/Green.txt", 7));
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("GhostBlue");
            sourcePosition = new Vector3(0.5f, 0.14f, 0.4f);
            newEntity.AddComponent(new ComponentPosition(sourcePosition));
            newEntity.AddComponent(new ComponentVelocity(0.0f, 0, 0));
            newEntity.AddComponent(new ComponentCollisionSphere(0.11f));
            newEntity.AddComponent(new ComponentGeometry("Geometry/Ghost/GhostBlue.obj"));
            newEntity.AddComponent(new ComponentAudio("Audio/Hit_Ghost.wav", sourcePosition, false));
            newEntity.AddComponent(new ComponentAI("Path/Blue.txt", 4));
            entityManager.AddEntity(newEntity);

            CreatePowerUp(newEntity);
            CreatePellets(newEntity);
        }
        private void CreatePowerUp(Entity newEntity)
        {
            float rad = 0.05f;
            newEntity = new Entity("PowerUp");
            Vector3 sourcePosition = new Vector3(-6f, 0.15f, 6f);
            newEntity.AddComponent(new ComponentPosition(sourcePosition));
            newEntity.AddComponent(new ComponentCollisionSphere(rad));
            newEntity.AddComponent(new ComponentGeometry("Geometry/PowerUp/PowerUp.obj"));
            newEntity.AddComponent(new ComponentAudio("Audio/buzz.wav", sourcePosition, true));
            entityManager.AddEntity(newEntity);
            ItemCollect++;

            newEntity = new Entity("PowerUp");
            sourcePosition = new Vector3(-6f, 0.15f, -6f);
            newEntity.AddComponent(new ComponentPosition(sourcePosition));
            newEntity.AddComponent(new ComponentCollisionSphere(rad));
            newEntity.AddComponent(new ComponentGeometry("Geometry/PowerUp/PowerUp.obj"));
            newEntity.AddComponent(new ComponentAudio("Audio/buzz.wav", sourcePosition, true));
            entityManager.AddEntity(newEntity);
            ItemCollect++;

            newEntity = new Entity("PowerUp");
            sourcePosition = new Vector3(6.2f, 0.15f, -6f);
            newEntity.AddComponent(new ComponentPosition(sourcePosition));
            newEntity.AddComponent(new ComponentCollisionSphere(rad));
            newEntity.AddComponent(new ComponentGeometry("Geometry/PowerUp/PowerUp.obj"));
            newEntity.AddComponent(new ComponentAudio("Audio/buzz.wav", sourcePosition, true));
            entityManager.AddEntity(newEntity);
            ItemCollect++;

            newEntity = new Entity("PowerUp");
            sourcePosition = new Vector3(6.2f, 0.15f, 6f);
            newEntity.AddComponent(new ComponentPosition(sourcePosition));
            newEntity.AddComponent(new ComponentCollisionSphere(rad));
            newEntity.AddComponent(new ComponentGeometry("Geometry/PowerUp/PowerUp.obj"));
            newEntity.AddComponent(new ComponentAudio("Audio/buzz.wav", sourcePosition, true));
            entityManager.AddEntity(newEntity);
            ItemCollect++;

            newEntity = new Entity("PowerUp");
            sourcePosition = new Vector3(-2.14f, 0.15f, 1f);
            newEntity.AddComponent(new ComponentPosition(sourcePosition));
            newEntity.AddComponent(new ComponentCollisionSphere(rad));
            newEntity.AddComponent(new ComponentGeometry("Geometry/PowerUp/PowerUp.obj"));
            newEntity.AddComponent(new ComponentAudio("Audio/buzz.wav", sourcePosition, true));
            entityManager.AddEntity(newEntity);
            ItemCollect++;
        }

        private void CreatePellets(Entity newEntity)
        {
            Vector3 sourcePosition;
            float rad = 0.05f;
            //use StreamReader to write all Item location and add into entity
            using (StreamReader sr = new StreamReader("Path/Item.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] item = line.Split(',');
                    newEntity = new Entity("Pellet");
                    sourcePosition = new Vector3(float.Parse(item[0]), float.Parse(item[1]), float.Parse(item[2]));
                    newEntity.AddComponent(new ComponentPosition(sourcePosition));
                    newEntity.AddComponent(new ComponentCollisionSphere(rad));
                    newEntity.AddComponent(new ComponentGeometry("Geometry/Pellet/Pellet.obj"));
                    newEntity.AddComponent(new ComponentAudio("Audio/pacman_chomp.wav", sourcePosition, false));
                    entityManager.AddEntity(newEntity);
                    ItemCollect++;
                }
            }
        }
        private void CreateWall(Entity newEntity)
        {
            //use StreamReader to write all wall location for box collision and add into entity
            using (StreamReader sr = new StreamReader("Path/Wall.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] item = line.Split(',');
                    newEntity = new Entity("Wall");
                    newEntity.AddComponent(new ComponentCollisionLine(new Vector2(float.Parse(item[0]), float.Parse(item[1])), new Vector2(float.Parse(item[2]), float.Parse(item[3]))));
                    entityManager.AddEntity(newEntity);
                }
            }
        }
        private void CreateSystems()
        {
            ISystem newSystem;

            newSystem = new SystemRender();
            systemManager.AddSystem(newSystem);

            newSystem = new SystemPhysics();
            systemManager.AddSystem(newSystem);

            newSystem = new SystemAudio(audioManager);
            systemManager.AddSystem(newSystem);

            newSystem = new SystemCollisionSphere(collisionManager);
            systemManager.AddSystem(newSystem);

            newSystem = new SystemCollisionBox(collisionManager);
            systemManager.AddSystem(newSystem);

            newSystem = new SystemPlayer();
            systemManager.AddSystem(newSystem);

            newSystem = new SystemAI();
            systemManager.AddSystem(newSystem);
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>
        public override void Update(FrameEventArgs e)
        {
            dt = (float)e.Time;

            // all the available key function here
            if (keyPressed[(char)Key.W])
            {
                camera.MoveForward(0.018f);
            }
            if (keyPressed[(char)Key.S])
            {
                camera.MoveForward(-0.018f);
            }
            if (keyPressed[(char)Key.A])
            {
                camera.RotateY(-0.02f);
            }
            if (keyPressed[(char)Key.D])
            {
                camera.RotateY(0.02f);
            }
            if (keyPressed[(char)Key.M])
            {
                sceneManager.StartMenu();
            }
            //update player position to camera position
            Player.getComponent<ComponentPosition>().Position = camera.cameraPosition;
            //Set win and lose condition, remove all asset and transfer to appropriate scene
            if (Player.getComponent<ComponentPlayer>().Pellet == ItemCollect)
            {
                Close();
                sceneManager.StartGameWin();

            }
            if(Player.getComponent<ComponentPlayer>().Health == 0)
            {
                Close();
                sceneManager.StartGameOver();
            }
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>
        public override void Render(FrameEventArgs e)
        {
            
            GL.Viewport(0, 0, sceneManager.Width, sceneManager.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Action ALL systems
            systemManager.ActionSystems(entityManager);

            // Render score and health
            float width = sceneManager.Width, height = sceneManager.Height, fontSize = Math.Min(width, height) / 10f;
            GUI.clearColour = Color.Transparent;
            GUI.Label(new Rectangle(0, 0, (int)width, (int)(fontSize * 2f)), "Health: " + Player.getComponent<ComponentPlayer>().Health, 18, StringAlignment.Near, Color.White);
            GUI.Label(new Rectangle(0, 0, (int)width, (int)(fontSize * 0.4f)), "Pellet:" + Player.getComponent<ComponentPlayer>().Pellet+"/"+ItemCollect, 18, StringAlignment.Far, Color.White);
            if(AIManager.enable)
            {
                GUI.Label(new Rectangle(0, 25, (int)width, (int)(fontSize * 0.4f)), "Debug Ghost Stop", 18, StringAlignment.Center, Color.White);
            }
            if (GameCollisionManager.enable)
            {
                GUI.Label(new Rectangle(0, 0, (int)width, (int)(fontSize * 0.4f)), "Wall Hack Started", 18, StringAlignment.Center, Color.White);
            }
            GUI.Render();
            //render minimap to Bottom left of the HUD
            GL.Enable(EnableCap.ScissorTest);
            GL.Viewport(0, 0, 350, 350);
            GL.Scissor(0, 0, 350, 350);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Load = true;
            systemManager.RenderMiniMap(entityManager);
            Load = false;
            GL.Disable(EnableCap.ScissorTest);
        }

        /// <summary>
        /// This is called when the game exits.
        /// </summary>
        public override void Close()
        {
            sceneManager.keyboardDownDelegate -= Keyboard_KeyDown;
            sceneManager.keyboardUpDelegate -= Keyboard_KeyUp;
            EntityManager.RemoveAllEntity();
            ResourceManager.RemoveAllAssets();
        }

        public void Keyboard_KeyDown(KeyboardKeyEventArgs e)
        {
            keyPressed[(char)e.Key] = true;
            switch (e.Key)
            {
                case Key.Number1:
                    if(AIManager.enable)
                    {
                        AIManager.enable = false;
                    }
                    else
                    {
                        AIManager.enable = true;
                    }
                    break;
                case Key.Number2:
                    if(GameCollisionManager.enable)
                    {
                        GameCollisionManager.enable = false;
                    }
                    else
                    {
                        GameCollisionManager.enable = true;
                    }
                    break;
            }
        }
        public void Keyboard_KeyUp(KeyboardKeyEventArgs e)
        {
            keyPressed[(char)e.Key] = false;
        }
    }
}
