using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleporter_script : MonoBehaviour
{
    public Transform Player;
    public float distance;
    public float MaxDistance;
    public ParticleSystem fx;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("LocalAvatarWithGrab").transform;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, Player.transform.position);

        if (distance <= MaxDistance / 2)
            fx.Play();
        else
        {
            fx.Stop();
        }
    }
}