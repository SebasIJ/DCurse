using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    //public PlayerScript getDimension;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
        //if (getDimension.dimension)
        //{
        //    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        //}
        //else if (!getDimension.dimension)
        //{
        //    transform.rotation = Quaternion.Euler(new Vector3(0, 45, 0));
        //}
    }
}
