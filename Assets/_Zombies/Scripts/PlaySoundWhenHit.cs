using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySoundWhenHit : MonoBehaviour
{
    //require audio source component
    public AudioSource audioSource;
    //audio clip to play when hit  
    public AudioClip hitSound;

    public float pitchMin = 0.9f;
    public float pitchMax = 1.1f;

    private void Awake()
    {
        // Play the hit sound
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hitSound != null)
        {
            audioSource.pitch = Random.Range(pitchMin, pitchMax);
            audioSource.PlayOneShot(hitSound);
        }
    }
}
