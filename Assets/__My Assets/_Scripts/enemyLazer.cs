using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyLazer : MonoBehaviour
{

    public float speed;
    // public Vector3 offset;
    public GameObject player;
    public int damage;
    //private Rigidbody rb;

    float timer;
    public float TimeLimit;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        // offset = new Vector3(Random.Range(0, 0.5f), Random.Range(0, 0.5f), Random.Range(0, 0.5f));
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed);

        timer += Time.deltaTime;
        if (timer >= TimeLimit)
            Destroy(gameObject);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

}
