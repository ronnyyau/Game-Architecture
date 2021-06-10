using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL_Game
{
    class Minimap
    {
        public Matrix4 view, projection;
        public Vector3 MinimapPosition, MinimapDirection, MinimapUp;
        private Vector3 targetPosition;
        public Minimap(Vector3 MinimapPos, Vector3 targetPos, float ratio, float near, float far)
        {
            MinimapUp = new Vector3(0.0f, 1.0f, 0.0f);
            MinimapPosition = MinimapPos;
            MinimapDirection = targetPos-MinimapPos;
            MinimapDirection.Normalize();
            UpdateView();
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45), ratio, near, far);
        }
        public void UpdateView()
        {
            targetPosition = MinimapPosition + MinimapDirection;
            view = Matrix4.LookAt(MinimapPosition, targetPosition, MinimapUp);
        }
    }
}
