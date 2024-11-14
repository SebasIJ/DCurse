using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GoalScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator goalAnim;
    private bool goalGet = false;
    public PlayerScript getDimension;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (getDimension.dimension)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else if (!getDimension.dimension)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 45, 0));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !goalGet)
        {
            goalAnim.SetBool("GoalGet", true);
            Rigidbody goalFall = this.AddComponent<Rigidbody>();
            goalFall.useGravity = true;
            goalGet = true;
        }        
    }
}
