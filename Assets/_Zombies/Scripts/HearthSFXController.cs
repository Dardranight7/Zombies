using UnityEngine;
using UnityEngine.Audio;

public class HearthSFXController : MonoBehaviour
{
    //controll audio mixer group for hearth sound effects with exposed propertie LifeSignal
    [SerializeField] private AudioMixerGroup hearthAudioMixerGroup = null;

    private void Start()
    {
        ChangeGroupVolume(0.05f); // Set initial volume to 0
    }

    public void ChangeGroupVolume(float value)
    {
        hearthAudioMixerGroup.audioMixer.SetFloat("LifeSignal", value);
    } 
}
