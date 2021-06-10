using OpenTK;

namespace OpenGL_Game.Components
{
    class ComponentCollisionLine : IComponent
    {
        Vector2 max;
        Vector2 min;
        public ComponentTypes ComponentType
        {
            get
            {
                return ComponentTypes.COMPONENT_COLLISIONLINE;
            }
        }
        public ComponentCollisionLine(Vector2 Max,Vector2 Min)
        {
            //For Calculate Box collision
            this.max = Max;
            this.min = Min;
            Height = min.Y - max.Y;
            Width = max.X - min.X;
        }
        public Vector2 GetMin
        {
            get
            {
                return min;
            }
            set
            {
                min = value;
            }
        }
        public Vector2 GetMax
        {
            get
            {
                return max;
            }
            set
            {
                max = value;
            }
        }
        public float Height { get; set; }
        public float Width { get; set; }
        public void Close()
        {
        }
    }
}
