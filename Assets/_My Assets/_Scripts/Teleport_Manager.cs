using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport_Manager : MonoBehaviour {

    public GameObject[] teleporters;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void TurnOn() {
        for (int i = 0; i < teleporters.Length; i++)
        {
            if (teleporters[i].activeSelf == false)
                teleporters[i].SetActive(true);
            Debug.Log("Ran");
        }
    }
}
