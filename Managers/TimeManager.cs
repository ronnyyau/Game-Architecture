using System.Diagnostics;

namespace OpenGL_Game.Managers
{
    class TimeManager
    {
        private float time;
        private float dt;

        private static TimeManager instance;
        public static float Time
        {
            get
            {
                return instance.time;
            }
        }
        public static float deltaTime
        {
            get
            {
                return instance.dt;
            }
        }
        public TimeManager()
        {
            time = 0.0f;
        }
        public void update(float delta)
        {
            time += delta;
            dt += delta;
            Debug.WriteLine(time);
        }
    }
}
