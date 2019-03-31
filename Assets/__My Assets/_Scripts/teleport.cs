using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class teleport : MonoBehaviour
{

    public float speed;

    public Transform player;
    public Transform gun;
    public GameObject spawnPos; //Where player will spawn
    public Vector3 cubePos; //Where cube will spawn
    public Quaternion cubeRot; //Where cube will spawn
    public Vector3 gunPos; //Where Pistol will spawn
    public Quaternion gunRot; //Where Pistol will spawn
    public Vector3 offset;
    //public Teleport_Manager manager;
    public Rigidbody hotBod;
    public OVRGrabbable grabbable;

    public int current; //Current teleporter

    bool moveToTeleport;

    public float timer;
    public float timeLimit = 2;
    bool scaled; //True if velocity has been scaled

    float SceneTimer;
    public float SceneTimeLimit;
    bool special; //if hit special teleporter

    public OVRScreenFade fade;

    // Use this for initialization
    void Start()
    {
        hotBod.constraints = RigidbodyConstraints.FreezeAll;
        //current = manager.teleporters.Length + 1;
        cubePos = transform.position;
        cubeRot = transform.rotation;
        gunPos = gun.transform.position;
        gunRot = gun.transform.rotation;
        hotBod = GetComponent<Rigidbody>();
       // grabbable = GetComponent<OVRGrabbable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbable.isGrabbed)
        {
            hotBod.constraints = RigidbodyConstraints.None;
           // manager.TurnOn(current);
            timer = 0.0f;
            //Show all teleporters in world.
           // manager.TurnOn(9);
        }
        //else if not grabbed
        else
        {
            if (transform.position != cubePos) //If Cube isn't in original position, move it while keeping teleporters on
            {
                timer += Time.deltaTime;
                if (timer >= timeLimit)
                {
                    hotBod.velocity = Vector3.zero;
                    transform.position = Vector3.MoveTowards(transform.position, cubePos, speed); //Move to original position
                    //transform.rotation = cubeRot;
                }
            }
            else
            {
                hotBod.constraints = RigidbodyConstraints.FreezePosition;
                timer = 0;
               // manager.TurnOff(9);
            }//Turn off teleporters
        }

        if (moveToTeleport) { //Teleport player slowly
            if (player.transform.position != spawnPos.transform.position) { 
                player.transform.position = Vector3.MoveTowards(player.transform.position, spawnPos.transform.position, 0.13f);
                //Play sound
            }
            else
                moveToTeleport = false;
        }

        if (special) //If menu teleporter is activated
        {
            SceneTimer += Time.deltaTime;
            if (SceneTimer >= SceneTimeLimit)
            {
                Debug.Log("sce");
                if (SceneManager.GetActiveScene().buildIndex == 1) //Main Level
                    SceneManager.LoadScene(2); //Load Win Screen
                else
                    SceneManager.LoadScene(1); //Load Win Screen
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("teleporter") || other.gameObject.CompareTag("teleporter_special"))
        {
            spawnPos = other.gameObject.transform.parent.Find("spawnPos").gameObject; //Replace current spawnPos with touched one.
            gunPos = other.gameObject.transform.parent.Find("GunPos").gameObject.transform.position; //Replace current spawnPos with touched one.
            cubePos = other.gameObject.transform.parent.Find("CubePos").gameObject.transform.position; //Replace current spawnPos with touched one.;
                                                                                                       //Play teleport fx
            moveToTeleport = true;
            gun.position = gunPos;
            gun.rotation = gunRot;
            transform.position = cubePos;
            hotBod.constraints = RigidbodyConstraints.FreezeAll;
            gun.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            gun.gameObject.GetComponent<gun>().originalPos = gunPos;
            gun.gameObject.GetComponent<gun>().originalRot = gunRot;

            if (other.gameObject.CompareTag("teleporter_special")) {
                special = true;
                fade.FadeOut();
            }
            //other.gameObject.transform.parent.gameObject.SetActive(false); //Set teleporter = false
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("laser"))
        {
            hotBod.velocity = collision.gameObject.GetComponent<Rigidbody>().velocity;
        }
    }

}
