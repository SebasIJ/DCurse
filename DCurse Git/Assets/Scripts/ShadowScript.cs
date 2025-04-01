using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScript : MonoBehaviour
{
    private struct PlayerInfo
    {
        public Vector3 position;
        public Vector3 scale;
        public Quaternion rotation;
        public Sprite sprite;

        public PlayerInfo(Vector3 position, Vector3 scale, Quaternion rotation, Sprite sprite)
        {
            this.position = position;
            this.scale = scale;
            this.rotation = rotation;
            this.sprite = sprite;
        }
    }

    private List<PlayerInfo> NextPos = new List<PlayerInfo>(); //List of future positions
    private float warmupFrames = 0; //Frames from activation to spawn
    private bool active = false; //Is the shadow active

    public GameObject Shadow; //Gameobject of the shadow
    public PlayerScript player; //The player and its script values
    public Transform playerSprite; //The sprite of the player
    public ParticleSystem warmupEffect; //Particle effect that plays during warmup
    public int totalWarmup; //total time for warmup to finish


    // Start is called before the first frame update
    void Start()
    {
        warmupEffect.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        if(active && player.HP > 0)
        {
            if(warmupFrames <= totalWarmup)
            {
                warmupEffect.Play();
                warmupFrames += Time.deltaTime * 10;
            }

            NextPos.Add(new PlayerInfo(player.transform.position, playerSprite.transform.localScale, playerSprite.transform.localRotation, playerSprite.GetComponent<SpriteRenderer>().sprite));

            if(warmupFrames > totalWarmup)
            {
                Shadow.SetActive(true);
                warmupEffect.Stop();
                warmupEffect.Clear();
                active = false;
            }
        }
        else if(!active && player.HP > 0 && warmupFrames > totalWarmup)
        {
            NextPos.Add(new PlayerInfo(player.transform.position, playerSprite.transform.localScale, playerSprite.transform.localRotation, playerSprite.GetComponent<SpriteRenderer>().sprite));

            //Sets shadow information to the first player information on the list
            Shadow.transform.position = NextPos[0].position;
            Shadow.transform.localScale = NextPos[0].scale;
            Shadow.transform.localRotation = NextPos[0].rotation;
            Shadow.GetComponent<SpriteRenderer>().sprite = NextPos[0].sprite;
            //Removes the already used value from the list
            NextPos.RemoveAt(0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            active = true;
        }
    }
}
