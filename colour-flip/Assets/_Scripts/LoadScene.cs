using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void GoToScene(string target)
    {
        SceneManager.LoadScene(target);
    }
}
