using UnityEngine;
using System;
using Random = UnityEngine.Random;
public class RandomAudio : MonoBehaviour
{
    [SerializeField] AudioClip[] _clips = default;

    [SerializeField] RangedFloat _volume = default;

    [MinMaxRange(0, 1)]
    [SerializeField] RangedFloat _pitch = default;


    /// <summary>
    /// Play random clip from array with random pitch and volume
    /// </summary>
    /// <param name="audioSource"></param>
    public void PlayRandom(AudioSource audioSource)
    {
        audioSource.clip = _clips[Random.Range(0, _clips.Length)];
        audioSource.pitch = Random.Range(_pitch.minValue, _pitch.maxValue);
        audioSource.volume = Random.Range(_volume.minValue, _volume.maxValue);
        audioSource.Play();
    }
}

[Serializable]
public struct RangedFloat
{
    public float minValue;
    public float maxValue;
}
