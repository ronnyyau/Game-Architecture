namespace OpenGL_Game.Components
{
    class ComponentCollisionSphere : IComponent
    {
        public ComponentTypes ComponentType
        {
            get
            {
                return ComponentTypes.COMPONENT_COLLISIONSPHERE;
            }
        }

        public ComponentCollisionSphere(float r)
        {
            //Setting Radius for target
            Rad = r;
        }

        public float Rad { get; set; }
        public void Close()
        {
        }
    }
}
