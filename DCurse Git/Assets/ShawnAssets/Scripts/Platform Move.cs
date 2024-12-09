using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlatformMove : MonoBehaviour
{
    public Vector3 locationA;
    public Vector3 locationB;
    
    public float moveSpeed = 3f;

    public Vector3 targetPosition;
    private bool isMovingToB = true;

    // Start is called before the first frame update
    void Start()
    {
        locationA = this.transform.position;

        targetPosition = locationB;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlatform();
    }

    void MovePlatform()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
       
        if (transform.position == targetPosition)
        {

            isMovingToB = !isMovingToB;

            if (isMovingToB)
            {
                targetPosition = locationB; 
            }
            else
            {
                targetPosition = locationA; 
            }

          
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Location A" || other.gameObject.name == "Location B")
        {
            isMovingToB = !isMovingToB;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if  (collision.gameObject.CompareTag("Player"))
            {
            collision.transform.SetParent(transform);

            }
        
            
        
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
