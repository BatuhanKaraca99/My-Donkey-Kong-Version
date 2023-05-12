using UnityEngine;
using UnityEngine.SceneManagement;

//Start from Preload Scene
public class GameManager : MonoBehaviour
{
    private int level; //current level
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

        if(camera != null)
        {
            camera.cullingMask = 0; //saying camera to not render anything
        }

        Invoke(nameof(LoadScene), 1f); //wait 1 second before loading scene

        SceneManager.LoadScene(level);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(level);
    }

    private void NewGame()
    {
        lives = 3;
        score = 0;

        LoadLevel(1);
    }

    public void LevelComplete()
    {
        score += 1000;

        int nextLevel = level + 1;
        if(nextLevel < SceneManager.sceneCountInBuildSettings) //if there is next level
        {
            LoadLevel(nextLevel);
        }
        else
        {
            LoadLevel(1);
        }
    }
    public void LevelFailed()
    {
        lives--;

        if(lives <= 0)
        {
            NewGame();
        }
        else
        {
            LoadLevel(level); //load current level
        }
    }

}
