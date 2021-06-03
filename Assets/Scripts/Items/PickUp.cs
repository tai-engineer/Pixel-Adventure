using UnityEngine;
using PA.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class PickUp : MonoBehaviour
{
    public VoidEvent onPickUp = new VoidEvent();

    /// <summary>
    /// Only be triggered by player layer corresponding to layer collision matrix
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        onPickUp.Invoke();

        // Destroy when audio ended
        GetComponent<SpriteRenderer>().enabled = false;
        
        if (gameObject.TryGetComponent<AudioSource>(out AudioSource audioSource))
        {
            float delay = audioSource.clip.length;
            audioSource.Play();
            Destroy(gameObject, delay);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
