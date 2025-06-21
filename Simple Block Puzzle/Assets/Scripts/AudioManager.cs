using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Clips")]
    public AudioClip blockPlacedSound;
    public AudioClip gameOverSound;
    public AudioClip rowClearedSound;

    private AudioSource audioSource;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayBlockPlaced()
    {
        PlaySound(blockPlacedSound);
    }

    public void PlayGameOver()
    {
        PlaySound(gameOverSound);
    }

    public void PlayRowCleared()
    {
        PlaySound(rowClearedSound);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }

    public void StopAllSounds()
    {
        foreach (var source in GetComponentsInChildren<AudioSource>())
        {
            source.Stop();
        }
    }
}
