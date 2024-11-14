using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCamera : MonoBehaviour
{
    public PlayerScript playerInfo;
    public Camera myCamera;
    private bool twoD;
    private bool switched = false;
    private Transform playerPos;
    private float Ztwo;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = playerInfo.gameObject.transform;
        myCamera = GetComponent<Camera>();
        myCamera.orthographic = true;
    }

    // Update is called once per frame
    void Update()
    {
        twoD = playerInfo.dimension;
        Ztwo = playerInfo.ZTWO;

        if (twoD)
        {
            //sets proper camera position
            //myCamera.transform.position = new Vector3(playerPos.position.x, playerPos.position.y + 2, Ztwo - 25);
            myCamera.transform.position = new Vector3(0, 18, -60);
            myCamera.orthographic = true; //sets camera type to orthographic
            myCamera.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            switched = true;
        }
        else if (!twoD) //not twoD means current dimension is 3D
        {
            //sets proper camera position
            if (switched)
            {
                myCamera.transform.position = new Vector3(playerPos.position.x - 10, playerPos.position.y + 10, playerPos.position.z - 10);
                switched = false;
            }
            myCamera.transform.position = new Vector3(playerPos.position.x - 10, myCamera.transform.position.y, playerPos.position.z - 10);
            //adjusts camera height dinamically to help with platforming
            if (myCamera.transform.position.y < playerPos.position.y + 8)
            {
                myCamera.transform.Translate(new Vector3(0, 0.04f, 0));
            }
            else if (myCamera.transform.position.y > playerPos.position.y + 10)
            {
                myCamera.transform.Translate(new Vector3(0, -0.04f, 0));
            }

            myCamera.fieldOfView = 70;
            myCamera.orthographic = false; //sets camera type to perspective
            myCamera.transform.rotation = Quaternion.Euler(new Vector3(20, 45, 0));
            myCamera.farClipPlane = 100;
        }
    }
}
