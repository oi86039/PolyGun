using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleporter_script : MonoBehaviour
{
    public Transform Player;
    public float distance;
    public float MaxDistance;
    public float cubeDistance;
    public float cubeMaxDistance;
    public ParticleSystem fx;
   // public Transform cube;
   // public float homingSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //cube = GameObject.Find("TeleportCube 2.0").transform;
        Player = GameObject.Find("LocalAvatarWithGrab").transform;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, Player.transform.position);
        //cubeDistance = Vector3.Distance(transform.position, cube.position);

        if (distance <= MaxDistance / 2)
            fx.Play();
        else
        {
            fx.Stop();
        }

       // if (cubeDistance <= cubeMaxDistance)
          //  cube.position += Vector3.MoveTowards(cube.position, transform.position, homingSpeed);
    }
}