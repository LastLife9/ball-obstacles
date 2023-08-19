using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BallObstacleGame
{
    public class GameManager : Singleton<GameManager>
    {
        private void Start()
        {
            Init();
        }

        public void Init()
        {
            Menu();
            LevelManager.Instance.InitLevel();
        }

        public void StartGame()
        {
            UIManager.Instance.EnablePanel(PanelType.Game);
            PlayerController.Instance.CanInput = true;
        }

        public void Menu()
        {
            UIManager.Instance.EnablePanel(PanelType.Menu);
        }

        public void Win()
        {
            UIManager.Instance.EnablePanel(PanelType.Win);
            LevelManager.Instance.Complete();
        }

        public void Lose()
        {
            UIManager.Instance.EnablePanel(PanelType.Lose);
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}