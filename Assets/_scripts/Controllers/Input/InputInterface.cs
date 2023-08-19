namespace BallObstacleGame
{
    public interface IInput
    {
        public bool OnTouch();
        public bool OnHold();
        public bool OnRelease();
    }
}