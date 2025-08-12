using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SoundFXManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] slideSounds;
    [SerializeField] private AudioClip[] rotateSounds;
    [SerializeField] private AudioClip winSound;
    private AudioSource source;

    private void Awake() {
        source = GetComponent<AudioSource>();
    }

    public void PlaySlide() {
        source.clip = slideSounds[UnityEngine.Random.Range(0, slideSounds.Length)];
        source.Play();
    }

    public void PlayRotate() {
        source.clip = rotateSounds[UnityEngine.Random.Range(0, rotateSounds.Length)];
        source.Play();
    }

    public void PlayWin()
    {
        source.clip = winSound;
        source.Play();
    }
}
