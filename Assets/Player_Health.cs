using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player_Health : MonoBehaviour
{

    public float health = 30;
    public float maxHealth;
    public Text healthText;


    // Start is called before the first frame update
    void Start()
    {
        //maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + health + "/" + maxHealth;
        if (health > maxHealth)
            health = maxHealth;

        if (health == maxHealth) //Full
            healthText.color = Color.green;
        else if (health > maxHealth / 2) //Normal
            healthText.color = new Color(.29f, 2.24f, 2.26f); //Standary Cyan Blue
        else if (health > maxHealth / 4) //Warning
            healthText.color = Color.yellow;
        else if (health > 0) //Near dead
            healthText.color = new Color(244 / 255f, 119 / 255f, 17 / 255f); //Orange

        if (health <= 0)
            //Fade
            //Load Game over
            SceneManager.LoadScene(3);
    }


    //private void OnCollisionEnter(Collision other)
    //{
    //    //If enemy projectile (each tag does different damage)
    //    if (other.gameObject.CompareTag("evilLaser")) {
            
    //    }
    //}
}
