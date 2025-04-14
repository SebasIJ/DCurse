using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class Witch : MonoBehaviour
{
    private enum WitchState
    {
        flying,
        casting,
        falling
    }





    //horFire headFire;

    public Transform shadowSprite;
    public Animator witchAnim;
    private WitchState currentState;
    public float flySpeed;
    public int castTime;
    private int currentCast;
    public int fallPause = 60;
    private int currentPause;
    private int warning = 90;
    public float fallSpeed;
    private float flyTarget;
    private float lastTarget;
    private int lastAttack;
    public GameObject fireBall;
    public GameObject elecField;
    public GameObject warningSprite;
    public LevelClear defeatClear;
    public ParticleSystem castEffect;
    private int HP = 3;
    private bool canHit = true;
    private bool targetSet = false;
    private bool sinkAtk = false;
    private bool lightAtk = false;
    private System.Random RNG = new System.Random();
    public Transform mainPlatform;
    public Transform[] spawns;

    public AudioSource castSound;
    public AudioSource damageSound;

    public class FireNode
    {
        public int fireY;
        public FireNode next;
    }

    public class LinkedFire
    {
        public FireNode headFire;

        public void Add(int data)
        {
            FireNode newNode = new FireNode();
            newNode.fireY = data;

            if (headFire == null)
            {
                headFire = newNode;
            }
            else
            {
                FireNode currNode = headFire;

                while (currNode.next != null)
                {
                    currNode = currNode.next;
                }
                currNode.next = newNode;
            }
        }
    }

    LinkedFire currentFire;

    // Start is called before the first frame update
    void Start()
    {
        currentFire = new LinkedFire();
        currentFire.Add(RNG.Next(5, 20));
        currentFire.Add(RNG.Next(5, 20));

        currentState = WitchState.flying;
        castEffect.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        PlaceShadow();
        AnimateWitch();
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case WitchState.flying:
                {
                    FlyState();
                    break;
                }
            case WitchState.casting:
                {
                    CastState();
                    break;
                }
            case WitchState.falling:
                {
                    FallState();
                    break;
                }
        }

        if (lightAtk)
        {
            LightAttack();
        }
        SinkAttack();

        if (transform.position.y < -2)
        {
            StartCoroutine(defeatClear.waitLand());
        }
    }

    private void PlaceShadow()
    {
        //A ray is sent downwards placing the shadow where it collides with an object
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 100, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            shadowSprite.gameObject.SetActive(true);
            shadowSprite.position = hitInfo.point + new Vector3(0, 0.1f, 0);
        }
        else
        {
            shadowSprite.gameObject.SetActive(false);
        }
    }

    private void AnimateWitch()
    {
        switch (currentState)
        {
            case WitchState.flying:
                witchAnim.SetBool("Flying", true);
                witchAnim.SetBool("Casting", false);
                witchAnim.SetBool("Damage", false);
                break;

            case WitchState.casting:
                witchAnim.SetBool("Flying", false);
                witchAnim.SetBool("Casting", true);
                witchAnim.SetBool("Damage", false);
                break;

            case WitchState.falling:
                witchAnim.SetBool("Flying", false);
                witchAnim.SetBool("Casting", false);
                witchAnim.SetBool("Damage", true);
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetKey(KeyCode.Z) && canHit)
        {
            flySpeed = flySpeed * 1.5f;
            castTime = castTime -= 60;
            canHit = false;
            currentState = WitchState.falling;
        }
    }

    private void FlyState()
    {
        currentPause = fallPause;
        currentCast = castTime;

        if (transform.position.y >= 32)
        {
            transform.Translate(Vector3.down * Time.deltaTime * flySpeed);
        }

        if (!targetSet)
        {
            switch (RNG.Next(1, 4))
            {
                case 1:
                    {
                        flyTarget = 20;
                        if (lastTarget == flyTarget)
                        {
                            switch (RNG.Next(1, 3))
                            {
                                case 1:
                                    {
                                        flyTarget = -20;

                                        break;
                                    }
                                case 2:
                                    {
                                        flyTarget = 0;
                                        break;
                                    }
                            }
                        }
                        break;
                    }
                case 2:
                    {
                        flyTarget = 0;
                        if (lastTarget == flyTarget)
                        {
                            switch (RNG.Next(1, 3))
                            {
                                case 1:
                                    {
                                        flyTarget = -20;

                                        break;
                                    }
                                case 2:
                                    {
                                        flyTarget = 20;
                                        break;
                                    }
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        flyTarget = -20;
                        if (lastTarget == flyTarget)
                        {
                            switch (RNG.Next(1, 3))
                            {
                                case 1:
                                    {
                                        flyTarget = 20;

                                        break;
                                    }
                                case 2:
                                    {
                                        flyTarget = 0;
                                        break;
                                    }
                            }
                        }
                        break;
                    }
            }
            targetSet = true;
            lastTarget = flyTarget;
        }


        if (transform.position.x > flyTarget + 1 || transform.position.x < flyTarget - 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(flyTarget, transform.position.y, transform.position.z), flySpeed * Time.deltaTime);
        }
        else
        {
            currentState = WitchState.casting;
            targetSet = false;
        }
    }

    private void FallState()
    {
        if (currentPause > 0)
        {
            currentPause--;
        }
        else
        {
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
        }

        if (transform.position.y >= 60)
        {
            HP--;
            currentState = WitchState.flying;
            fallSpeed = Mathf.Abs(fallSpeed) / 2;
            canHit = true;
            currentFire.Add(RNG.Next(5, 20));
        }
        else if (transform.position.y <= 0 && HP > 0)
        {
            damageSound.Play();
            fallSpeed = -fallSpeed * 2;
        }
        else if (transform.position.y <= 5 && HP == 1)
        {
            damageSound.Play();
            HP--;
            fallSpeed = fallSpeed * 0.1f;
            transform.position = new Vector3(transform.position.x, transform.position.y, -20);
        }
    }

    private void CastState()
    {
        if (transform.position.y >= 32)
        {
            transform.Translate(Vector3.down * Time.deltaTime * flySpeed);
        }

        if (currentCast == castTime)
        {
            castSound.Play();
        }


        if (currentCast > 0)
        {
            castEffect.gameObject.SetActive(true);
            castEffect.Emit(1);
            castEffect.Play();
            currentCast--;
        }
        else
        {
            switch (RNG.Next(4))
            {
                case 0:
                    {
                        if (lastAttack != 0)
                        {
                            for (int i = 0; i < spawns.Length; i++)
                            {
                                GameObject newFire = Instantiate(fireBall, new Vector3(spawns[i].position.x, 32, spawns[i].position.z), spawns[i].rotation);
                                newFire.GetComponent<Rigidbody>().AddForce(Vector3.down * RNG.Next(1, 3));
                            }
                            lastAttack = 0;
                        }
                        else
                        {
                            switch (RNG.Next(2))
                            {
                                case 0:
                                    {
                                        sinkAtk = true;
                                        lastAttack = 1;
                                        break;
                                    }
                                case 1:
                                    {
                                        warning = 90;
                                        lightAtk = true;
                                        lastAttack = 2;
                                        break;
                                    }
                            }

                        }
                        break;
                    }
                case 1:
                    {
                        if (lastAttack != 1)
                        {
                            sinkAtk = true;
                            lastAttack = 1;
                        }
                        else
                        {
                            switch (RNG.Next(2))
                            {
                                case 0:
                                    {
                                        for (int i = 0; i < spawns.Length; i++)
                                        {
                                            GameObject newFire = Instantiate(fireBall, new Vector3(spawns[i].position.x, 32, spawns[i].position.z), spawns[i].rotation);
                                            newFire.GetComponent<Rigidbody>().AddForce(Vector3.down * RNG.Next(1, 3));
                                        }
                                        lastAttack = 0;
                                        break;
                                    }
                                case 1:
                                    {
                                        warning = 90;
                                        lightAtk = true;
                                        lastAttack = 2;
                                        break;
                                    }
                            }

                        }
                        break;
                    }
                case 2:
                    {
                        if (lastAttack != 2)
                        {
                            warning = 90;
                            lightAtk = true;
                            lastAttack = 2;
                        }
                        else
                        {
                            switch (RNG.Next(2))
                            {
                                case 0:
                                    {
                                        for (int i = 0; i < spawns.Length; i++)
                                        {
                                            GameObject newFire = Instantiate(fireBall, new Vector3(spawns[i].position.x, 32, spawns[i].position.z), spawns[i].rotation);
                                            newFire.GetComponent<Rigidbody>().AddForce(Vector3.down * RNG.Next(1, 3));
                                        }
                                        lastAttack = 0;
                                        break;
                                    }
                                case 1:
                                    {
                                        sinkAtk = true;
                                        lastAttack = 1;
                                        break;
                                    }
                            }

                        }
                        break;
                    }
                case 3:
                    {
                        if (lastAttack != 3)
                        {

                            FireNode newFire = currentFire.headFire;

                            while (newFire.next != null)
                            {
                                GameObject Fire = Instantiate(fireBall, new Vector3(-30, newFire.fireY, 0), Quaternion.Euler(new Vector3(0, 0, 90)));
                                Fire.GetComponent<Rigidbody>().useGravity = false;
                                Fire.GetComponent<Rigidbody>().AddForce(Vector3.right * 30, ForceMode.Impulse);
                                newFire = newFire.next;
                            }

                        }
                        else
                        {
                            switch (RNG.Next(2))
                            {
                                case 0:
                                    {
                                        for (int i = 0; i < spawns.Length; i++)
                                        {
                                            GameObject newFire = Instantiate(fireBall, new Vector3(spawns[i].position.x, 32, spawns[i].position.z), spawns[i].rotation);
                                            newFire.GetComponent<Rigidbody>().AddForce(Vector3.down * RNG.Next(1, 3));
                                        }
                                        lastAttack = 0;
                                        break;
                                    }
                                case 1:
                                    {
                                        sinkAtk = true;
                                        lastAttack = 1;
                                        break;
                                    }
                            }

                        }
                        break;
                    }
            }

            castEffect.Stop();
            castEffect.gameObject.SetActive(false);
            currentState = WitchState.flying;
        }
    }

    private void LightAttack()
    {
        if (warning > 0)
        {
            warning--;
            warningSprite.SetActive(true);
            warningSprite.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 0.01f);
        }
        else
        {
            warningSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            warningSprite.SetActive(false);
            GameObject newElec = Instantiate(elecField);
            Destroy(newElec, 2);
            lightAtk = false;
        }
    }

    private void SinkAttack()
    {
        if (mainPlatform.position.y < -4 && sinkAtk && HP == 1)
        {
            warning = 90;
            lightAtk = true;
            sinkAtk = false;
        }
        else if (sinkAtk || (HP == 1 && canHit))
        {
            if (mainPlatform.position.y < -4)
            {
                sinkAtk = false;
            }
            else
            {
                mainPlatform.Translate(Vector3.down * 3 * Time.deltaTime);
            }
        }
        else if ((mainPlatform.position.y < -0.1 && HP != 1) || (HP == 1 && canHit == false && mainPlatform.position.y < -0.1))
        {
            mainPlatform.Translate(Vector3.up * 4 * Time.deltaTime);
        }
    }

    //private void HeadFireSet()
    //{
    //    headFire = new horFire();
    //    headFire.fireY = RNG.Next(0, 100);
    //}

    //private void AddFire()
    //{
    //    horFire newFire = new horFire();
    //    newFire.fireY = RNG.Next(0, 100);

    //    horFire temp = headFire;
    //    while (temp.next != null)
    //    {
    //        temp = temp.next;
    //    }
    //    temp.next = newFire;
    //}

    //IEnumerator spawnFire(int y)
    //{
    //    GameObject newFire = Instantiate(fireBall, new Vector3(-100, y, 0), Quaternion.Euler(new Vector3(0,0,90)));
    //    newFire.GetComponent<Rigidbody>().useGravity = false;
    //    newFire.GetComponent<Rigidbody>().AddForce(Vector3.right * 10);

    //    yield return new WaitForSeconds(1);
    //}

    //private void generatePattern()
    //{
    //    int totalAtk = 0;

    //    headAttack = new attackPattern();
    //    headAttack.atckId = RNG.Next(1, 4);

    //    totalAtk += headAttack.atckId;

    //    attackPattern currentAttack = headAttack;

    //    while (totalAtk < 6)
    //    {
    //        currentAttack.next = new attackPattern();
    //        currentAttack = currentAttack.next;

    //        if (totalAtk <= 3)
    //        {
    //            currentAttack.atckId = 4 - totalAtk;
    //        }
    //        else
    //        {
    //            currentAttack.atckId = 6 - totalAtk;
    //        }

    //    }
    //}
}
