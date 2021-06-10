using OpenGL_Game.Components;
using OpenTK;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Objects
{
    class Node
    {
        public Node(float x,float y)
        {
            this.X = x;
            this.Y = y;
        }
        public float X { get; set; }
        public float Y { get; set; }
        //public float GCost { get; set; }
        //public float HCost { get; set; }
        //public float FCost { get; set; } = 0;
        public Node NextNode { get; set; }
        //public List<Node> NodeList { get; set; }

    }
}