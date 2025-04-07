using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScript : MonoBehaviour
{
    //struct that contains necessary information
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

    private Queue<PlayerInfo> NextPos = new Queue<PlayerInfo>(); //List of player info structs that will be used as future positions
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
        //prevents warmup effect to start without being activated
        warmupEffect.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        //makes sure player is alive, when the shadow is activated
        if (active && player.HP > 0)
        {
            //plays warmupeffect and increments warmup timer
            if (warmupFrames <= totalWarmup)
            {
                warmupEffect.Play();
                warmupFrames += Time.deltaTime * 10;
            }

            //adds the current information of the player o every frame to the info list
            NextPos.Enqueue(new PlayerInfo(player.transform.position, playerSprite.transform.localScale, playerSprite.transform.localRotation, playerSprite.GetComponent<SpriteRenderer>().sprite));

            //stops effect when warmup frames run out
            if (warmupFrames > totalWarmup)
            {
                Shadow.SetActive(true);
                warmupEffect.Stop();
                warmupEffect.Clear();
                active = false;
            }
        }
        //after warmup ends and player still alive
        else if (!active && player.HP > 0 && warmupFrames > totalWarmup)
        {
            //adds the current information of the player o every frame to the info list
            NextPos.Enqueue(new PlayerInfo(player.transform.position, playerSprite.transform.localScale, playerSprite.transform.localRotation, playerSprite.GetComponent<SpriteRenderer>().sprite));


            //Sets shadow information to the first player information on the list
            PlayerInfo applyNext = NextPos.Dequeue();
            Shadow.transform.position = applyNext.position;
            Shadow.transform.localScale = applyNext.scale;
            Shadow.transform.localRotation = applyNext.rotation;
            Shadow.GetComponent<SpriteRenderer>().sprite = applyNext.sprite;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //activates when the player enters the trigger
        if (other.CompareTag("Player"))
        {
            active = true;
        }
    }
}
