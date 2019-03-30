using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{

    public GameObject player; //For sake of demonstration, it will be the gun.
    public GameObject spawnPos; //Where player will spawn
    public Vector3 cubePos; //Where cube will spawn
    public Vector3 cubeRot; //Where cube will spawn
    public Vector3 gunPos; //Where Pistol will spawn
    public Vector3 gunRot; //Where Pistol will spawn
    public Vector3 offset;
    private Teleport_Manager manager;
    private Rigidbody hotBod;
    private OVRGrabbable grabbable;

    float timer;
    public float timeLimit = 2;

    // Use this for initialization
    void Start()
    {
        manager = player.GetComponent<Teleport_Manager>();
        cubePos = transform.position;
        hotBod = GetComponent<Rigidbody>();
        grabbable = GetComponent<OVRGrabbable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbable.isGrabbed)
        {
            timer = 0.0f;
            //Show all teleporters in world.
        }
        //else



    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Teleporter"))
        {
            manager.TurnOn();
            spawnPos = other.gameObject.transform.parent.Find("spawnPos").gameObject; //Replace current spawnPos with touched one.
            cubePos = spawnPos.transform.position + offset;
            player.transform.position = spawnPos.transform.position;
            transform.position = cubePos;
            hotBod.constraints = RigidbodyConstraints.FreezeAll;
            hotBod.constraints = RigidbodyConstraints.None;
            other.gameObject.transform.parent.gameObject.SetActive(false); //Set teleporter = false
        }
    }
}
