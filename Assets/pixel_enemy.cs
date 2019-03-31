using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pixel_enemy : MonoBehaviour
{
    public int cubes_Count; //Number of cubes in list 
    public int health;
    public float timeLimit;

    // Start is called before the first frame update
    void Start()
    {
        cubes_Count = transform.childCount;
        foreach (Transform child in transform)
        {//Set health for all child objects
            child.gameObject.GetComponent<pixel>().health = health;
            child.gameObject.GetComponent<pixel>().timeLimit = timeLimit;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Behavior set
        //Behavior function run
        //Check if dead
        if (transform.childCount < cubes_Count / 2)
        { //if dead
          //Stop behaving
          //Explode
            foreach (Transform child in transform)
                child.gameObject.GetComponent<pixel>().health = 0;
        }
    }
}

// private void OnCollisionEnter(Collision collision)
//{
//   Debug.Log("Yes is");
//}
//}
