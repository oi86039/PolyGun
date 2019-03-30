using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{

    public Transform button;
    public Vector3 buttonLocation;
    public Transform teleporter;
    public GameObject teleText;
    public bool play = false;
    public bool quit = false;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (teleporter.position != button.position && play)
            teleporter.position = Vector3.MoveTowards(teleporter.position, buttonLocation, 0.1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (button.gameObject.tag.Equals("Play"))
        {
            play = true;
            button.GetChild(1).GetComponent<Text>().color = Color.green;
        }
        else if ((collision.gameObject.CompareTag("laser") || collision.gameObject.CompareTag("tCube") || collision.gameObject.CompareTag("gun")) && button.gameObject.tag.Equals("Quit"))
        {
            quit = true;
            //Quit Applicaiton
            Application.Quit();
        }
    }


}
