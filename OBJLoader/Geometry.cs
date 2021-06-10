using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.Managers;

namespace OpenGL_Game.OBJLoader
{
    public class Group
    {
        public List<float> vertices = new List<float>();
        public List<float> textureCoords = new List<float>();
        public List<float> normals = new List<float>();
        public PrimitiveType primitiveType;
        public int primitiveScale = 0;
        public int numberOfFaces = 0;
        public int texture = 0;
        // handles to vao and vbos
        public int vao_Handle;
        public int vbo_verts;
        public int vbo_texs;
        public int vbo_normals;
    }
    /// <summary>
    /// This is the object that we use to store our geometry that we will use to render in the game
    /// </summary>
    public class Geometry
    {
        List<Group> groups = new List<Group>();

        public Geometry()
        {
        }

        public void LoadObject(string filename)
        {
            try
            {
                // This OBJ parser library is developed by chrisjansson and available at https://github.com/chrisjansson/ObjLoader
                ObjLoader.Loader.Loaders.LoadResult obj = LoadOBJObject(filename);

                // This code assumes that all faces in all groups are defined as triangles or quads
                foreach (var group in obj.Groups)
                {
                    Group newGroup = new Group();
                    if (group.Material.DiffuseTextureMap != null)
                    {
                        newGroup.texture = ResourceManager.LoadTexture(group.Material.DiffuseTextureMap);
                    }
                    else
                    {
                        newGroup.texture = 0;
                    }
                    bool error = false;
                    string errorMessage = "";
                    bool primitiveSet = false;
                    foreach (var face in group.Faces)
                    {
                        ++newGroup.numberOfFaces;

                        if (face.Count == 3)
                        {
                            if(primitiveSet && newGroup.primitiveScale != 3)
                            {
                                error = true;
                                errorMessage = "The " + filename + " file has both triangular and quad faces and so will not be rendered correctly";
                            }
                            newGroup.primitiveType = PrimitiveType.Triangles;
                            newGroup.primitiveScale = 3;
                            primitiveSet = true;
                        }
                        else if (face.Count == 4)
                        {
                            if (primitiveSet && newGroup.primitiveScale != 4)
                            {
                                error = true;
                                errorMessage = "The " + filename + " file has both triangular and quad faces and so will not be rendered correctly";
                            }
                            newGroup.primitiveType = PrimitiveType.Quads;
                            newGroup.primitiveScale = 4;
                            primitiveSet = true;
                        }
                        else
                        {
                            error = true;
                            errorMessage = "The " + filename + " file does not have triangular or quad faces and so will not be rendered correctly";
                        }

                        for (int i = 0; i < face.Count; ++i)
                        {
                            // obj indexing starts at 1, so we need to subtract 1
                            int v = face[i].VertexIndex - 1;
                            newGroup.vertices.Add(obj.Vertices[v].X);
                            newGroup.vertices.Add(obj.Vertices[v].Y);
                            newGroup.vertices.Add(obj.Vertices[v].Z);

                            if (obj.Textures.Count > 0)
                            {
                                // obj indexing starts at 1, so we need to subtract 1
                                int t = face[i].TextureIndex - 1;
                                newGroup.textureCoords.Add(obj.Textures[t].X);
                                // OpenGL tex coords start at top-left
                                newGroup.textureCoords.Add(1.0f-obj.Textures[t].Y);
                            }

                            if (obj.Normals.Count > 0)
                            {
                                // obj indexing starts at 1, so we need to subtract 1
                                int n = face[i].NormalIndex - 1;
                                newGroup.normals.Add(obj.Normals[n].X);
                                newGroup.normals.Add(obj.Normals[n].Y);
                                newGroup.normals.Add(obj.Normals[n].Z);
                            }
                        }
                    }

                    if (error)
                    {
                        MessageBox.Show(errorMessage, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Create the single VAO that will hold all information for this group
                    GL.GenVertexArrays(1, out newGroup.vao_Handle);
                    GL.BindVertexArray(newGroup.vao_Handle);

                    // Create the buffer for the vertices
                    GL.GenBuffers(1, out newGroup.vbo_verts);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, newGroup.vbo_verts);
                    GL.BufferData<float>(BufferTarget.ArrayBuffer, (IntPtr)(newGroup.vertices.Count * 4), newGroup.vertices.ToArray<float>(), BufferUsageHint.StaticDraw);
                    GL.EnableVertexAttribArray(0);
                    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * 4, 0);

                    // Tex Coords
                    if (obj.Textures.Count > 0)
                    {
                        // Create the buffer for the texture coords
                        GL.GenBuffers(1, out newGroup.vbo_texs);
                        GL.BindBuffer(BufferTarget.ArrayBuffer, newGroup.vbo_texs);
                        GL.BufferData<float>(BufferTarget.ArrayBuffer, (IntPtr)(newGroup.textureCoords.Count * 4), newGroup.textureCoords.ToArray<float>(), BufferUsageHint.StaticDraw);
                        GL.EnableVertexAttribArray(1);
                        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 2 * 4, 0);
                    }

                    // Normals
                    if (obj.Normals.Count > 0)
                    {
                        // Create the buffer for the normals
                        GL.GenBuffers(1, out newGroup.vbo_normals);
                        GL.BindBuffer(BufferTarget.ArrayBuffer, newGroup.vbo_normals);
                        GL.BufferData<float>(BufferTarget.ArrayBuffer, (IntPtr)(newGroup.normals.Count * 4), newGroup.normals.ToArray<float>(), BufferUsageHint.StaticDraw);
                        GL.EnableVertexAttribArray(2);
                        GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 3 * 4, 0);
                    }

                    groups.Add(newGroup);
                    GL.BindVertexArray(0);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public ObjLoader.Loader.Loaders.LoadResult LoadOBJObject(string filename)
        {
            var objLoaderFactory = new ObjLoader.Loader.Loaders.ObjLoaderFactory();
            var objLoader = objLoaderFactory.Create();
            var fileStream = new FileStream(filename, FileMode.Open);
            var obj = objLoader.Load(fileStream);
            fileStream.Close();
            return obj;
        }

        // Render this object
        public void Render()
        {
            foreach (var group in groups)
            {
                if (group.texture > 0)
                {
                    GL.BindVertexArray(group.vao_Handle);
                    GL.BindTexture(TextureTarget.Texture2D, group.texture);
                    GL.DrawArrays(group.primitiveType, 0, group.numberOfFaces * group.primitiveScale);
                }
            }
            GL.BindVertexArray(0);
        }

        public void RemoveGeometry()
        {
            foreach (var group in groups)
            {
                GL.DeleteBuffer(group.vbo_normals);
                GL.DeleteBuffer(group.vbo_texs);
                GL.DeleteBuffer(group.vbo_verts);
                GL.DeleteVertexArray(group.vao_Handle);
            }
        }
    }
}
