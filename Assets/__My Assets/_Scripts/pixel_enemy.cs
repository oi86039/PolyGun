using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enemyID { SPUNK, SQUASH, STRETCH };

public class pixel_enemy : MonoBehaviour
{
    public bool dead;
    public bool deSpawned;

    public int cubes_Count; //Number of cubes in list 
    public int health;
    public int damage;
    public float timeLimit;
    public float horizontalSpeed;
    public float direction;
    public float verticalSpeed;

    [Range(0, 1)]
    public float deathPercent;

    public AudioSource deathSound;

    public float distThreshold;
    public GameObject Player;
    public GameObject gun;
    //public GameObject evil_projectile;
    float timer;
    public float TimeLimit;

    float deathTimer;
    float deathTimeLimit = 0.2f;

    public float distance;

    public enemyID id; //Different behaviors; 

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("LocalAvatarWithGrab");
        gun = GameObject.Find("Functional Pistol");
        timeLimit = Random.Range(1, 3);
        horizontalSpeed = Random.Range(0.3f, 0.7f);
        //Select random starting direciton
        float dirVal = Random.value * 100;
        if (dirVal <= 50) direction = 1;
        else direction = -1;
        cubes_Count = transform.childCount;
        foreach (Transform child in transform)
        {//Set health for all child objects
            child.gameObject.GetComponent<pixel>().health = health;
            child.gameObject.GetComponent<pixel>().timeLimit = TimeLimit;
        }
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, Player.transform.position);
        //Spawn if in proximity
        if (distance <= distThreshold * 3 && !dead) //If in proximity with player
        {
            for (int j = 0; j < transform.childCount; j++)
            {
                transform.GetChild(j).gameObject.SetActive(true); //Spawn all enemy things
                deSpawned = false;
            }
        }
        else {
            for (int j = 0; j < transform.childCount; j++)
            {
                transform.GetChild(j).gameObject.SetActive(false); //Despawn all things
                deSpawned = true;
            }
        }

        if (!dead || !deSpawned) //Only act if !dead or !offScreen
        {
            if (distance <= distThreshold * 2 && !dead) //If in proximity with player
                                                        //Behavior function run
                Chase(id);
            else
                Behave(id);

            //Check if dead
            if (transform.childCount < (int)(cubes_Count * deathPercent)) //If cubes count is less than given percentage
            { //if dead
              //Stop behaving
                dead = true;
                foreach (Transform child in transform)
                    child.gameObject.GetComponent<pixel>().health = 0;
            }
            if (transform.childCount <= 0)
            {
                if (!deathSound.isPlaying)
                {
                    deathSound.Play();
                }
                deathTimer += Time.deltaTime;
                if (deathTimer >= deathTimeLimit)
                {
                    gun.GetComponent<gun>().score += 90;            //Explode
                    Destroy(gameObject);
                }
            }
        }
    }

    void Chase(enemyID id) //Actual enemy Update method
    {
        switch (id)
        {
            case enemyID.SPUNK:
                transform.RotateAround(Player.transform.position, Vector3.up, horizontalSpeed*direction); //Rotates Around Player

                timer += Time.deltaTime;
                if (timer >= TimeLimit)
                {
                    timeLimit = Random.Range(0.08f, 1.5f);
                    //TimeLimit += Random.Range(-0.001, 0.1);
                    horizontalSpeed = Random.Range(0.3f, 0.7f);
                    direction *= -1;
                    //Shoot
                    //GameObject instance = Instantiate(evil_projectile, transform.position, new Quaternion(0,0,0,0) );
                    //instance.GetComponent<enemyLazer>().damage = damage;
                    //instance.GetComponent<enemyLazer>().player = Player;
                    timer = 0;
                }
                break;
            case enemyID.SQUASH:

                break;
            case enemyID.STRETCH:
                transform.RotateAround(Player.transform.position, Vector3.up, -horizontalSpeed * direction); //Rotates Around Player
                timer += Time.deltaTime;
                if (timer >= TimeLimit)
                {
                    timeLimit = Random.Range(0.08f, 1.5f);
                    // TimeLimit += Random.Range(-1, 1);
                    horizontalSpeed = Random.Range(0.3f, 0.7f);
                    direction *= -1;
                    //Shoot
                    //GameObject instance = Instantiate(evil_projectile, transform.position, new Quaternion(0, 0, 0, 0));
                    //instance.GetComponent<enemyLazer>().damage = damage;
                    //instance.GetComponent<enemyLazer>().player = Player;
                    timer = 0;
                }
                transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, -verticalSpeed);
                if (distance < distThreshold / 2 || distance > distThreshold)
                    verticalSpeed *= -1;

                break;
        }
    }
    void Behave(enemyID id) //Actual enemy Update method
    {
        switch (id)
        {
            case enemyID.SPUNK:
                transform.position += Vector3.right * (horizontalSpeed / 15 * direction); //Rotates Around Player
                timer += Time.deltaTime;
                if (timer >= TimeLimit)
                {
                    timeLimit = Random.Range(1, 3);
                    horizontalSpeed = Random.Range(0.2f, 0.5f);
                    direction *= -1;
                    timer = 0;
                }
                break;
            case enemyID.SQUASH:

                break;
            case enemyID.STRETCH:
                transform.position += Vector3.right * (horizontalSpeed / 15 * direction); //Rotates Around Player
                timer += Time.deltaTime;
                if (timer >= TimeLimit)
                {
                    timeLimit = Random.Range(1, 3);
                    horizontalSpeed = Random.Range(0.3f, 0.7f);
                    direction *= -1;
                    timer = 0;
                }
                break;
        }
    }
}


