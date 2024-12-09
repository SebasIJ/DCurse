using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCamera : MonoBehaviour
{
    public PlayerScript playerInfo;
    public Camera myCamera;
    private bool twoD;
    private bool switched;
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
    void FixedUpdate()
    {
        twoD = playerInfo.dimension;
        Ztwo = playerInfo.ZTWO;

        if (twoD)
        {
            //sets proper camera position
            if (!switched)
            {
                myCamera.transform.position = new Vector3(playerPos.position.x, playerPos.position.y + 2, Ztwo - 50);
                switched = true;
            }
            myCamera.transform.position = new Vector3(playerPos.position.x, myCamera.transform.position.y, Ztwo - 50);
            //adjusts camera height dinamically to help with platforming
            if (myCamera.transform.position.y < playerPos.position.y - 2)
            {
                myCamera.transform.Translate(new Vector3(0, 0.2f, 0));
            }
            else if (myCamera.transform.position.y > playerPos.position.y + 2)
            {
                myCamera.transform.Translate(new Vector3(0, -0.2f, 0));
            }

            myCamera.orthographic = true; //sets camera type to orthographic
            myCamera.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            myCamera.farClipPlane = 150;
            switched = true;
        }
        else if (!twoD) //not twoD means current dimension is 3D
        {
            //sets proper camera position
            if (switched)
            {
                myCamera.transform.position = new Vector3(0, playerPos.position.y + 8, 0);
                switched = false;
            }
            myCamera.transform.position = new Vector3(0, myCamera.transform.position.y, 0);
            //adjusts camera height dinamically to help with platforming
            if (myCamera.transform.position.y < playerPos.position.y + 6)
            {
                myCamera.transform.Translate(new Vector3(0, 0.2f, 0));
            }
            else if (myCamera.transform.position.y > playerPos.position.y + 8)
            {
                myCamera.transform.Translate(new Vector3(0, -0.2f, 0));
            }

            myCamera.fieldOfView = 70;
            myCamera.orthographic = false; //sets camera type to perspective
            myCamera.transform.LookAt(playerPos);
            myCamera.farClipPlane = 500;
        }
    }
}
