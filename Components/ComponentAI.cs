using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using System.Collections.Generic;

namespace OpenGL_Game.Components
{
    class ComponentAI : IComponent
    {
        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_AI; }
        }
        public ComponentAI(string fileName, int index)
        {
            //Get Path from TXT file and store into the ComponentAI
            //loopIndex is for setting last node to s specific node for looping path
            this.nodeList = ResourceManager.LoadPath(fileName);
            this.loopIndex = index;
        }

        public List<Node> nodeList { get; set; } = new List<Node>();
        public int loopIndex { get; set; } = 0;
        public void Close()
        {
            nodeList.Clear();
            loopIndex = 0;
        }
    }
}
