using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonFocus : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if( target !=  null)
        {
            transform.LookAt(target);
        }

    }
}
