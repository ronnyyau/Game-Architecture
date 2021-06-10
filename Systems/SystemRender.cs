using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.Components;
using OpenGL_Game.OBJLoader;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;

namespace OpenGL_Game.Systems
{
    class SystemRender : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_GEOMETRY);

        protected int pgmID;
        protected int vsID;
        protected int fsID;
        protected int uniform_stex;
        protected int uniform_mModelviewproj;
        protected int uniform_mModel;
        public SystemRender()
        {
            pgmID = GL.CreateProgram();
            LoadShader("Shaders/vs.glsl", ShaderType.VertexShader, pgmID, out vsID);
            LoadShader("Shaders/fs.glsl", ShaderType.FragmentShader, pgmID, out fsID);
            GL.LinkProgram(pgmID);
            Console.WriteLine(GL.GetProgramInfoLog(pgmID));

            uniform_stex = GL.GetUniformLocation(pgmID, "s_texture");
            uniform_mModelviewproj = GL.GetUniformLocation(pgmID, "ModelViewProjMat");
            uniform_mModel = GL.GetUniformLocation(pgmID, "ModelMat");
        }

        void LoadShader(String filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            using (StreamReader sr = new StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }

        public string Name
        {
            get { return "SystemRender"; }
        }

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent geometryComponent = components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_GEOMETRY;
                });
                Geometry geometry = ((ComponentGeometry)geometryComponent).Geometry();

                IComponent positionComponent = components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                Vector3 position = ((ComponentPosition)positionComponent).Position;
                if (entity.Name.Contains("Ghost"))
                {
                    if (entity.Model == Matrix4.Zero)
                    {
                        entity.Model = Matrix4.CreateTranslation(position);
                    }
                    else
                    {
                        entity.Model = entity.Model.ClearTranslation() * Matrix4.CreateTranslation(entity.getComponent<ComponentPosition>().Position);
                    }
                    if (GameScene.Load)
                    {
                        Minimap(entity.Model, geometry);
                    }
                    else
                    {
                        Draw(entity.Model, geometry);
                    }
                }
                else
                {
                    entity.Model = Matrix4.CreateTranslation(position);
                    if (GameScene.Load)
                    {
                        Minimap(entity.Model, geometry);
                    }
                    else
                    {
                        Draw(entity.Model, geometry);
                    }
                }
            }
        }

        public void Draw(Matrix4 Model, Geometry geometry)
        {
            GL.UseProgram(pgmID);

            GL.Uniform1(uniform_stex, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.UniformMatrix4(uniform_mModel, false, ref Model);
            Matrix4 ModelViewProjection = Model * GameScene.gameInstance.camera.view * GameScene.gameInstance.camera.projection;
            GL.UniformMatrix4(uniform_mModelviewproj, false, ref ModelViewProjection);

            geometry.Render();

            GL.UseProgram(0);
        }
        public void Minimap(Matrix4 Model, Geometry geometry)
        {
            GL.UseProgram(pgmID);

            GL.Uniform1(uniform_stex, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.UniformMatrix4(uniform_mModel, false, ref Model);
            Matrix4 ModelViewProjection = Model * GameScene.gameInstance.minimap.view * GameScene.gameInstance.minimap.projection;
            GL.UniformMatrix4(uniform_mModelviewproj, false, ref ModelViewProjection);

            geometry.Render();

            GL.UseProgram(0);
        }
    }
}
