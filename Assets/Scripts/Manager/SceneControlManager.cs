using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControlManager : MonoBehaviour
{
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
    public void LoadNextLevel()
    {
        int levelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(levelIndex);
    }
    public void LoadBeforeLevel()
    {
        int levelIndex = SceneManager.GetActiveScene().buildIndex - 1;
        SceneManager.LoadScene(levelIndex);
    }
    public void RestartLevel()
    {
        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(levelIndex);
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
