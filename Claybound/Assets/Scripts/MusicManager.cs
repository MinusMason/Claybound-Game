using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip musicClip;
    [Range(0f, 1f)] public float volume = 0.5f;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip  = musicClip;
        audioSource.loop  = true;
        audioSource.volume  = volume;
        audioSource.spatialBlend = 0f;
        audioSource.playOnAwake = false;

        if (musicClip != null)
            audioSource.Play();
    }
}
