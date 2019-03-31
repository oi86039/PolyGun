using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lazur : MonoBehaviour {

    private Rigidbody hotBod;
    public float speed;
    float timer;
    public float timeLimit;
    public GameObject player;
    public GameObject gun;
	// Use this for initialization
	void Start () {
        hotBod = GetComponent<Rigidbody>();
        hotBod.AddForce(transform.up * speed, ForceMode.Impulse);

	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer >= timeLimit) Destroy(gameObject);
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("gun"))
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        //if contacted with enemies, spawn explosion fx and destroy projectile
        else if (collision.gameObject.CompareTag("pixel"))
        {
            collision.gameObject.GetComponent<pixel>().health--;
        }
        else if (collision.gameObject.CompareTag("pickUpHealth"))
        {
            player.GetComponent<Player_Health>().health += 5;
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("pickUpAmmo"))
        {
            player.GetComponent<gun>().reserve += 5;
            if (player.GetComponent<gun>().reserve > 90)
            {
                player.GetComponent<gun>().reserve = 90;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
