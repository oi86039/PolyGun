using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pixel : MonoBehaviour
{
    float timer;
    public float timeLimit;
    public AudioSource hurtSound;

    public int health; //Health before cube flies off
    private Rigidbody rb;

    private ParticleSystem fx;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        fx = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        { //If not attached to enemy anymore
            transform.parent = null;
            rb.isKinematic = false;
            rb.useGravity = true;
            //Vector3 force = new Vector3(Random.Range(0, 2), Random.Range(0, 2), Random.Range(0, 2)); //Add random force
            //rb.AddForce(force, ForceMode.Impulse);
            timer += Time.deltaTime;
            if (timer >= timeLimit) Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("pixel"))
        {
            fx.Play();
            hurtSound.pitch = Random.Range(0.5f, 1f);
            if (collision.gameObject.CompareTag("laser"))
                hurtSound.volume = 0.55f;
            else
                hurtSound.volume = 0.10f;
            hurtSound.Play();
        }
        //if (collision.gameObject.CompareTag("laser")) {
        //}
    }
}
