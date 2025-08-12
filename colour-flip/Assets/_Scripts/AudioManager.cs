using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static SoundFXManager Sounds;

    private void Awake() {
        Sounds = GetComponentInChildren<SoundFXManager>();
    }
}
