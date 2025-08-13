using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelIndex : MonoBehaviour {
    public static TMP_Text Label { get; private set; }

    private void Awake()
    {
        if (LevelIndex.Label == null) {
            LevelIndex.Label = GetComponent<TMP_Text>();
        }

        LevelIndex.SetCurrent(SceneSwitcher.CurrentIndex);
    }

    public static void SetCurrent(int index) {
        LevelIndex.Label.text = index.ToString();
    }


}
