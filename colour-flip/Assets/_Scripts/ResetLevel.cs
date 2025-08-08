using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour
{
    public void ResetThis()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
