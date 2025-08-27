using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public static int CurrentIndex { get; private set; } = 1;

    private void Start() {
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
        LevelIndex.SetCurrent(SceneSwitcher.CurrentIndex);
        if (SceneSwitcher.CurrentIndex != 1)
        {
            SceneManager.UnloadSceneAsync(SceneSwitcher.CurrentIndex - 1);
        }

        SceneManager.LoadScene(SceneSwitcher.CurrentIndex, LoadSceneMode.Additive);
        SceneSwitcher.CurrentIndex++;
    }
}
