using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelIndex : MonoBehaviour
{
    [SerializeField] private TMP_Text label;

    private void Awake()
    {
        label.text = (SceneManager.GetActiveScene().buildIndex).ToString();
    }
}
