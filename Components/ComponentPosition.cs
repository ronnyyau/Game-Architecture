using OpenTK;

namespace OpenGL_Game.Components
{
    class ComponentPosition : IComponent
    {
        Vector3 position;
        Vector3 Origin;
        public ComponentPosition(Vector3 pos)
        {
            Origin = pos;
            position = pos;
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }
        public Vector3 origin
        {
            get 
            { 
                return Origin;
            }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_POSITION; }
        }
        public void Close()
        {
        }
    }
}
