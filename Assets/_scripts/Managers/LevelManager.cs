using UnityEngine;

namespace BallObstacleGame
{
    [System.Serializable]
    public class Level
    {
        public GameObject Parent;
    }

    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private Level[] _levels;
        private int _level = 0;

        public void InitLevel()
        {
            _level = PlayerPrefs.GetInt("Lvl");
            _levels[_level].Parent.SetActive(true);
        }

        public void Complete()
        {
            _level++;
            if (_level >= _levels.Length) _level = 0;
            PlayerPrefs.SetInt("Lvl", _level);
            PlayerPrefs.Save();
        }
    }

}
