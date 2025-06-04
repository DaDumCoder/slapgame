using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager_ForUi : MonoBehaviour
{
    // Load scene based on given level number (like before)
    public void LoadLevel(int levelNumber)
    {
        string sceneName = "Level_" + (levelNumber + 1);
        Debug.Log("Scene to load: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }


    public void LoadScenee()
    {
        SceneManager.LoadScene("StartScreen");
    }
    // Load next scene based on build index
    public void LoadNextSceneInBuild()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("Loading next scene with build index: " + nextIndex);
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            Debug.LogWarning("No next scene found in build settings. You're at the last scene!");
        }
    }

    // 🔁 Reload current scene
    public void ReloadCurrentScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Reloading current scene: " + SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(currentIndex);
    }
}
