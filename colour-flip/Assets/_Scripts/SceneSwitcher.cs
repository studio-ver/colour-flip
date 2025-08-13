using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public static int CurrentIndex { get; private set; } = 1;

    private void Awake() {
        NextAdditiveScene();
    }

    public static void GoToNext()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;

        if (SceneManager.sceneCountInBuildSettings == currentIndex + 1)
            return;

        SceneManager.LoadScene(currentIndex + 1);
    }

    public static void NextAdditiveScene() {
        if (SceneManager.sceneCountInBuildSettings == SceneSwitcher.CurrentIndex + 2) {
            SceneManager.LoadScene(SceneSwitcher.CurrentIndex + 1);
            return;
        }
        
        LevelIndex.SetCurrent(SceneSwitcher.CurrentIndex);
        SceneManager.UnloadSceneAsync(SceneSwitcher.CurrentIndex);
        SceneSwitcher.CurrentIndex++;
        SceneManager.LoadScene(SceneSwitcher.CurrentIndex, LoadSceneMode.Additive);
    }
}
