using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public PlayerScript player;
    //public Transform locationB;
    public float moveSpeed = 3f;
    public Vector3 targetPosition;
    private bool Moving = false;

    public AudioSource doorSound;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = this.transform.position - new Vector3(0, 20, 0);
    }


    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject == player.gameObject )
        {
            if (player.keyCollected > 0 && !Moving)
            {
                Moving = true;
                player.keyCollected--;
                doorSound.Play();
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (transform.position == targetPosition)
            {
                Moving = false;
                Destroy(this.gameObject);
            }
        }
    }
}
