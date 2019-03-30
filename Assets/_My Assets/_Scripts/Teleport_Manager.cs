using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport_Manager : MonoBehaviour
{

    public GameObject[] teleporters;
    int currentTeleport; //Teleporter that player is on

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TurnOn(int current)
    {
        //currentTeleport = current;

        for (int i = 0; i < teleporters.Length; i++)
        {
            //if (current < teleporters.Length + 1)
            //{
              //  if (teleporters[i] == teleporters[currentTeleport])
               //     continue;
            //}
            teleporters[i].transform.parent.gameObject.SetActive(true);
            Debug.Log("Ran");
        }
    }
    public void TurnOff(int current)
    {
        currentTeleport = current;

        for (int i = 0; i < teleporters.Length; i++)
        {
            if (current > teleporters.Length + 1)
            {
                if (teleporters[i] == teleporters[currentTeleport])
                    continue;
            }
            teleporters[i].transform.parent.gameObject.SetActive(false);
            Debug.Log("Ran");
        }
    }
}
