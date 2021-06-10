using OpenTK;

namespace OpenGL_Game.Components
{
    class ComponentVelocity : IComponent
    {
        Vector3 Velocity;
        Vector3 LastVelocity;

        public ComponentVelocity(float x, float y, float z)
        {
            Velocity = new Vector3(x, y, z);
        }
        public Vector3 velocity
        {
            get { return Velocity; }
            set { Velocity = value; }
        }
        public Vector3 lastvelocity
        {
            get { return LastVelocity; }
            set { LastVelocity = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_VELOCITY; }
        }
        public void Close()
        {
        }
    }
}
