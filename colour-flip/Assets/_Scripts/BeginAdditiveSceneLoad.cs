using UnityEngine;
using UnityEngine.SceneManagement;

public class BeginAdditiveSceneLoad : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Additive);
    }
}
