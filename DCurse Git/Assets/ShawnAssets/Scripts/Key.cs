using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{

    public PlayerScript player;
    public AudioSource keySound;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject == player.gameObject)
        {
            player.keyCollected = player.keyCollected +1;
            keySound.Play();
            Destroy(this.gameObject, 0.3f);
           
        }
    }
}
