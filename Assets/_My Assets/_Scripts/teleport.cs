using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Teleport_Manager manager;
    private Rigidbody hotBod;
    private OVRGrabbable grabbable;
    public int current; //Current teleporter

    public float timer;
    public float timeLimit = 2;

    // Use this for initialization
    void Start()
    {
        current = manager.teleporters.Length + 1;
        cubePos = transform.position;
        cubeRot = transform.rotation;
        gunPos = gun.transform.position;
        gunRot = gun.transform.rotation;
        hotBod = GetComponent<Rigidbody>();
        grabbable = GetComponent<OVRGrabbable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbable.isGrabbed)
        {
            hotBod.constraints = RigidbodyConstraints.None;
            manager.TurnOn(current);
            timer = 0.0f;
            //Show all teleporters in world.
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
            else timer = 0;//Turn off teleporters

        }



    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("teleporter"))
        {
            spawnPos = other.gameObject.transform.parent.Find("spawnPos").gameObject; //Replace current spawnPos with touched one.
            gunPos = other.gameObject.transform.parent.Find("GunPos").gameObject.transform.position; //Replace current spawnPos with touched one.
            cubePos = other.gameObject.transform.parent.Find("CubePos").gameObject.transform.position; //Replace current spawnPos with touched one.;
            //Play teleport fx
            player.transform.position = spawnPos.transform.position;
            gun.position = gunPos;
            gun.rotation = gunRot;
            transform.position = cubePos;
            hotBod.constraints = RigidbodyConstraints.FreezeAll;
            gun.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            gun.gameObject.GetComponent<gun>().originalPos = gunPos;
            gun.gameObject.GetComponent<gun>().originalRot = gunRot;
            //other.gameObject.transform.parent.gameObject.SetActive(false); //Set teleporter = false
        }
    }
}
