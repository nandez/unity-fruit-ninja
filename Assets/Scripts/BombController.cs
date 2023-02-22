using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField] protected AudioSource explosionSound;
    [SerializeField] protected AudioSource wickSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            explosionSound.Play();
            FindObjectOfType<GameManager>().OnBombExplosion();
        }
    }

    public void StopWickSound()
    {
        wickSound.Stop();
    }
}
