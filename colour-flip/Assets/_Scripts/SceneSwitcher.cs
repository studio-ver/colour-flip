using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public static void GoToNext()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;

        if (SceneManager.sceneCountInBuildSettings == currentIndex + 1)
            return;

        SceneManager.LoadScene(currentIndex + 1);
    }
}
