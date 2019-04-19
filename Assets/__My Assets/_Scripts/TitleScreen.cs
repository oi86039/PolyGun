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
    public float timer;

    public OVRScreenFade fade;
    bool isQuitExecuting;

    // Use this for initialization
    void Start()
    {

        fade = GameObject.Find("CenterEyeAnchor").GetComponent<OVRScreenFade>();

    }

    // Update is called once per frame
    void Update()
    {
        if (quit)
        {
            StartCoroutine(FadeToQuit());
        }

        if (teleporter.position != button.position && play)
            teleporter.position = Vector3.MoveTowards(teleporter.position, buttonLocation, 0.05f);
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
            FadeToQuit();
        }
    }

    IEnumerator FadeToQuit()
    {
        if (isQuitExecuting)
            yield break;

        isQuitExecuting = true;

        fade.FadeOut();
        yield return new WaitForSeconds(1);
        Application.Quit();
    }


}
