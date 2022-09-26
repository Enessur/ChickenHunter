using UnityEngine;

namespace Script
{
    public class SceneManager : Singleton<SceneManager>

    {
        [SerializeField] private AudioSource themeAudioSource;
        public GameObject loseItem;
        public GameObject winItem;
        public GameObject creditsItem;
        public GameObject[] levelPrefabs;
        public Transform[] playerSpawnPoints;
        public Transform player;

        private void Start()
        {
            themeAudioSource.Play();
            loseItem.SetActive(false);
            winItem.SetActive(false);
            creditsItem.SetActive(false);
            LoadLevel();
        }

        public void LoadLevel()
        {
            Time.timeScale = 1f;
            if (levelPrefabs.Length > 0)
            {
                var currentLvl = ES3.Load("LevelIndex", 0);
                if (currentLvl == levelPrefabs.Length)
                {
                    currentLvl = 0;
                    ES3.Save("LevelIndex",0);
                }

                levelPrefabs[currentLvl].SetActive(true);
                player.position = playerSpawnPoints[currentLvl].position;
            }
        }

        public void NextLevel()
        {
            var a = ES3.Load("LevelIndex", 0);
            a++;
            if (levelPrefabs.Length == a)
            {
                a = 0;
                ES3.Save("LevelIndex", a);
                creditsItem.SetActive(true);
                winItem.SetActive(false);
            }
            else
            {
                LoadLevel();
                RestartButton();
            }

            ES3.Save("LevelIndex", a);
        }

        public void Replay()
        {
            ES3.Save("LevelIndex", 0);
            creditsItem.SetActive(false);
            winItem.SetActive(false);
            loseItem.SetActive(false);
            LoadLevel();
            levelPrefabs[levelPrefabs.Length-1].SetActive(false);
        }

        public void Setup(int score)
        {
            gameObject.SetActive(true);
        }

        public void RestartButton()
        {
            StopSound();
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        public void ExitButton()
        {
            //SceneManager.LoadScene("MainMenu");
        }

        private void StopSound()
        {
            themeAudioSource.Stop();
        }

        public void LoseGame()
        {
            loseItem.SetActive(true);
            Time.timeScale = 0f;
        }

        public void WinGame()
        {
            winItem.SetActive(true);
            Time.timeScale = 0f;
        }

        public void QuitGame()
        {
            Debug.Log("qqq");
            Application.Quit();
        }
    }
}