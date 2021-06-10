using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.OBJLoader;
using System.IO;
using OpenTK.Audio.OpenAL;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Managers
{
    static class ResourceManager
    {
        static Dictionary<string, Geometry> geometryDictionary = new Dictionary<string, Geometry>();
        static Dictionary<string, int> textureDictionary = new Dictionary<string, int>();
        public static Dictionary<string, int> audioDictionary = new Dictionary<string, int>();

        public static void RemoveAllAssets()
        {
            foreach(var geometry in geometryDictionary)
            {
                geometry.Value.RemoveGeometry();
            }
            geometryDictionary.Clear();
            foreach(var texture in textureDictionary)
            {
                GL.DeleteTexture(texture.Value);
            }
            textureDictionary.Clear();
            foreach (var audio in textureDictionary)
            {
                AL.SourceStop(audio.Value);
                AL.DeleteSource(audio.Value);
                AL.DeleteBuffer(audio.Value);
            }
            audioDictionary.Clear();
        }

        public static Geometry LoadGeometry(string filename)
        {
            Geometry geometry;
            geometryDictionary.TryGetValue(filename, out geometry);
            if (geometry == null)
            {
                geometry = new Geometry();
                geometry.LoadObject(filename);
                geometryDictionary.Add(filename, geometry);
            }

            return geometry;
        }
      

        public static int LoadTexture(string filename)
        {
            if (String.IsNullOrEmpty(filename))
                throw new ArgumentException(filename);

            int texture;
            textureDictionary.TryGetValue(filename, out texture);
            if (texture == 0)
            {
                texture = GL.GenTexture();
                textureDictionary.Add(filename, texture);
                GL.BindTexture(TextureTarget.Texture2D, texture);

                // We will not upload mipmaps, so disable mipmapping (otherwise the texture will not appear).
                // We can use GL.GenerateMipmaps() or GL.Ext.GenerateMipmaps() to create
                // mipmaps automatically. In that case, use TextureMinFilter.LinearMipmapLinear to enable them.
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                Bitmap bmp = new Bitmap(filename);
                BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

                bmp.UnlockBits(bmp_data);
            }
 
            return texture;
        }
        public static int LoadAudio(string filename)
        {
            int audioBuffer;
            audioDictionary.TryGetValue(filename, out audioBuffer);
            // reserve a Handle for the audio file
            if(audioBuffer == 0)
            {
                audioBuffer = AL.GenBuffer();
                audioDictionary.Add(filename, audioBuffer);
                // Load a .wav file from disk.
                int channels, bits_per_sample, sample_rate;
                byte[] sound_data = LoadWave(
                    File.Open(filename, FileMode.Open),
                    out channels,
                    out bits_per_sample,
                    out sample_rate);
                ALFormat sound_format =
                    channels == 1 && bits_per_sample == 8 ? ALFormat.Mono8 :
                    channels == 1 && bits_per_sample == 16 ? ALFormat.Mono16 :
                    channels == 2 && bits_per_sample == 8 ? ALFormat.Stereo8 :
                    channels == 2 && bits_per_sample == 16 ? ALFormat.Stereo16 :
                    (ALFormat)0; // unknown
                AL.BufferData(audioBuffer, sound_format, sound_data, sound_data.Length, sample_rate);
                if (AL.GetError() != ALError.NoError)
                {
                    // respond to load error etc.
                }
            }
            return audioBuffer;
        }
        private static byte[] LoadWave(Stream stream, out int channels, out int bits, out int rate)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            using (BinaryReader reader = new BinaryReader(stream))
            {
                // RIFF header
                string signature = new string(reader.ReadChars(4));
                if (signature != "RIFF")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                int riff_chunck_size = reader.ReadInt32();

                string format = new string(reader.ReadChars(4));
                if (format != "WAVE")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                // WAVE header
                string format_signature = new string(reader.ReadChars(4));
                if (format_signature != "fmt ")
                    throw new NotSupportedException("Specified wave file is not supported.");

                int format_chunk_size = reader.ReadInt32();
                int audio_format = reader.ReadInt16();
                int num_channels = reader.ReadInt16();
                int sample_rate = reader.ReadInt32();
                int byte_rate = reader.ReadInt32();
                int block_align = reader.ReadInt16();
                int bits_per_sample = reader.ReadInt16();

                string data_signature = new string(reader.ReadChars(4));
                if (data_signature != "data")
                    throw new NotSupportedException("Specified wave file is not supported.");

                int data_chunk_size = reader.ReadInt32();

                channels = num_channels;
                bits = bits_per_sample;
                rate = sample_rate;

                return reader.ReadBytes((int)reader.BaseStream.Length);
            }
        }
        public static List<Node> LoadPath(string filename)
        {
            List<Node> Nodes = new List<Node>();
            using (StreamReader sr = new StreamReader(filename))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] item = line.Split(',');
                    Node node = new Node(float.Parse(item[0]),float.Parse(item[2]));
                    Nodes.Add(node);
                }
            }
            for(int i = 0;i<Nodes.Count;i++)
            {
                try
                {
                    Nodes[i].NextNode = Nodes[i + 1];
                }
                catch
                {
                }
            }
            return Nodes;
        }
    }
}
