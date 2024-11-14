using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScroll : MonoBehaviour
{
    public float ySpeed;
    public float xSpeed;
    public MeshRenderer scrollTexture;


    // Start is called before the first frame update
    void Start()
    {
        scrollTexture = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        scrollTexture.material.mainTextureOffset = new Vector2(Time.timeSinceLevelLoad * xSpeed, Time.timeSinceLevelLoad * ySpeed);
    }
}
