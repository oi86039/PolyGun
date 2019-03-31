﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enemyID { SPUNK, SQUASH, STRETCH };

public class pixel_enemy : MonoBehaviour
{
    public bool dead;

    public int cubes_Count; //Number of cubes in list 
    public int health;
    public int damage;
    public float timeLimit;
    public float horizontalSpeed;
    public float verticalSpeed;

    public float distThreshold;
    public GameObject Player;
    //public GameObject evil_projectile;
    float timer;
    public float TimeLimit;

    public float distance;

    public enemyID id; //Different behaviors; 

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
        distance = Vector3.Distance(transform.position, Player.transform.position) ;
        //Activate behavior if in proximity
        if (distance <= distThreshold*2 && !dead) //If in proximity with player
            //Behavior function run
            Chase(id);
        else
            Behave(id);


        //Check if dead
        if (transform.childCount < cubes_Count / 2)
        { //if dead
          //Stop behaving
            dead = true;
            //Explode
            foreach (Transform child in transform)
                child.gameObject.GetComponent<pixel>().health = 0;
        }
        else if (transform.childCount <= 0)
            Destroy(gameObject);
    }

    void Chase(enemyID id) //Actual enemy Update method
    {
        switch (id)
        {
            case enemyID.SPUNK:
                transform.RotateAround(Player.transform.position,Vector3.up, horizontalSpeed); //Rotates Around Player
                
                timer += Time.deltaTime;
                if (timer >= TimeLimit) {
                    //TimeLimit += Random.Range(-0.001, 0.1);
                    horizontalSpeed *= -1;
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
                transform.RotateAround(Player.transform.position, Vector3.up, -horizontalSpeed); //Rotates Around Player
                timer += Time.deltaTime;
                if (timer >= TimeLimit)
                {
                   // TimeLimit += Random.Range(-1, 1);
                    horizontalSpeed *= -1;
                    //Shoot
                    //GameObject instance = Instantiate(evil_projectile, transform.position, new Quaternion(0, 0, 0, 0));
                    //instance.GetComponent<enemyLazer>().damage = damage;
                    //instance.GetComponent<enemyLazer>().player = Player;
                    timer = 0;
                }
                transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, -verticalSpeed);
                if (distance < distThreshold / 2 || distance > distThreshold)
                    verticalSpeed*=-1;

                break;
        }
    }    void Behave(enemyID id) //Actual enemy Update method
    {
        switch (id)
        {
            case enemyID.SPUNK:
                transform.position += Vector3.right * (horizontalSpeed/15); //Rotates Around Player
                timer += Time.deltaTime;
                if (timer >= TimeLimit) {
                    horizontalSpeed *= -1;
                    timer = 0;
                }
                break;
            case enemyID.SQUASH:

                break;
            case enemyID.STRETCH:
                transform.position += Vector3.right * (horizontalSpeed/15); //Rotates Around Player
                timer += Time.deltaTime;
                if (timer >= TimeLimit)
                {
                    horizontalSpeed *= -1;
                    timer = 0;
                }
                break;
        }
    }
}


