namespace OpenGL_Game.Components
{
    class ComponentPlayer : IComponent
    {
        public ComponentTypes ComponentType
        {
            get
            {
                return ComponentTypes.COMPONENT_PLAYER;
            }
        }
        public ComponentPlayer()
        {
            //Setting Health, Item Count for checking if player lose or win the game
            //Timer is for power up Countdown
            Health = 3;
            Pellet = 0;
            Timer = 0;
        }

        public int Health { get; set; }
        public int Pellet { get; set; }
        public float Timer { get; set; }
        public void Close()
        {
        }
    }
}
