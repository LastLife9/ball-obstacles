using UnityEngine;

namespace BallObstacleGame
{
    public class MobileDesktopInput : IInput
    {
        public bool OnHold()
        {
            return Input.GetMouseButton(0);
        }

        public bool OnRelease()
        {
            return Input.GetMouseButtonUp(0);
        }

        public bool OnTouch()
        {
            return Input.GetMouseButtonDown(0);
        }
    }
}