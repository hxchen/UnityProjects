using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private int level;
    private int lives;
    private int score;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        NewGame();
    }

    private void LoadLevel(int index)
    {
        level = index;

        Camera camera = Camera.main;
        if (camera != null)
        {
            camera.cullingMask = 0;
        }
        Invoke(nameof(loadScene), 1f);
    }

    private void loadScene()
    {
        SceneManager.LoadScene(level);
    }
    private void NewGame()
    {
        lives = 3;
        score = 0;
        // Load level
        LoadLevel(1);

    }

    public void LevelComplete()
    {
        score += 1000;
        // load next level

        int nextLevel = level + 1;
        if (nextLevel < SceneManager.sceneCountInBuildSettings)
        {
            LoadLevel(nextLevel);
        }
        else {
            LoadLevel(1);
        }
        
    }

    public void LevelFailed()
    {
        lives--;

        if (lives <= 0)
        {
            NewGame();
        } else {
            LoadLevel(level);
        }
         
    }

}
