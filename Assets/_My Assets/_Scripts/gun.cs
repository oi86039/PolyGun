using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FIRE_MODE { PISTOL, BURST, AUTO };

public class gun : MonoBehaviour
{
    float timer; //Respawn timer
    public float timeLimit; //Time before respawn
    float ReloadTimer; //Reload timer
    public float ReloadTimeLimit; //Time before reload completion

    public Transform PistolOffset;
    public bool leftHandedMode;

    private Rigidbody hotBod;
    public GameObject lazur;
    public Transform spawnPos;
    public Text AmmoText;
    public Text LeftIndicator;
    public Text ModeIndicator; //Indicates fire mode
    private Vector3 originalPos; //Original position of gun
    private Quaternion originalRot; //Original rotation of gun
    private ParticleSystem shootSpark;

    bool TriggerInput;
    bool ReloadInput;
    bool fireModeInput;
    public OVRGrabbable grabbable;
    public OVRGrabber LeftGrabber; //Left Controller
    public OVRGrabber RightGrabber; //Right Controller

    public FIRE_MODE firemode;
    public float fireRate;
    public bool firing; //Is the gun firing right now?
    bool isReloading;
    public int PistolAmmo;
    public int reserve; //Reload into PistolAmmo when PistolAmmo = 0;

    private AudioSource sound;
    public AudioClip[] audios;
    int audioIndex;

    // Use this for initialization
    void Start()
    {
        firemode = FIRE_MODE.PISTOL;
        ModeIndicator.text = "Pistol";
        audioIndex = 0;
        UpdateAmmo();
        hotBod = GetComponent<Rigidbody>();
        sound = GetComponent<AudioSource>();
        shootSpark = GetComponent<ParticleSystem>();
        originalPos = transform.position;
        originalRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAmmo(); //Update Ammo count and UI display

        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick)) //Toggle Left Handed Mode
        {
            LeftHandedModeToggle();
        }

        //If we grab 
        if (grabbable.isGrabbed)
        {
            Vibration(0.1f, 0.1f, 0.1f);
            hotBod.constraints = RigidbodyConstraints.None;
            timer = 0.0f;

            //Check which hand is grabbing
            if (grabbable.grabbedBy == LeftGrabber)
            {
                switch (firemode)
                {
                    case (FIRE_MODE.PISTOL):
                        TriggerInput = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger); //Left hand
                        break;
                    case (FIRE_MODE.AUTO):
                        TriggerInput = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger); //Left hand
                        break;
                    case (FIRE_MODE.BURST):
                        TriggerInput = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger); //Left hand
                        break;
                }
                ReloadInput = OVRInput.Get(OVRInput.Button.Three); //Left hand
                fireModeInput = OVRInput.Get(OVRInput.Button.Four);
            }
            else if (grabbable.grabbedBy == RightGrabber)
            {
                switch (firemode)
                {
                    case (FIRE_MODE.PISTOL):
                        TriggerInput = OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger); //Right hand
                        break;
                    case (FIRE_MODE.AUTO):
                        TriggerInput = OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger); //Right hand
                        break;
                    case (FIRE_MODE.BURST):
                        TriggerInput = OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger); //Right hand
                        break;
                }
                ReloadInput = OVRInput.Get(OVRInput.Button.One); //Right hand
                fireModeInput = OVRInput.GetDown(OVRInput.Button.Two);
            }
            //if we press trigger
            if (TriggerInput)
            {
                if (PistolAmmo >= 1)
                    Fire();
                else if (!sound.isPlaying)
                {
                    sound.clip = audios[10];
                    sound.Play();
                }
            }
            if (fireModeInput)
            {//Change Fireing mode
                ChangeFireMode();
                Debug.Log("Firemodechange");
            }
        }
        else
        {
            //If not grabbing
            timer += Time.deltaTime;
            //Respawn Gun
            if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick) || timer >= timeLimit)
            {
                RespawnGun();
            }


        }
    }

    void Fire()
    {
        switch (firemode)
        {
            case (FIRE_MODE.PISTOL):
                firing = true;
                Instantiate(lazur, spawnPos.position, spawnPos.rotation);
                //Play particle effect
                shootSpark.Play();
                //Play lazr sound
                sound.clip = audios[audioIndex];
                sound.Play();
                audioIndex = Random.Range(0, 8); //Randomly select next sound
                StartCoroutine(Vibration(1, 30, 0.1f));
                PistolAmmo--;
                firing = false;
                break;
            case (FIRE_MODE.AUTO):
                if (!firing)
                    StartCoroutine(BurstFire());
                break;
            case (FIRE_MODE.BURST):
                if (!firing)
                    StartCoroutine(BurstFire());
                break;
        }

    }

    void ChangeFireMode()
    {
        if (firemode == FIRE_MODE.PISTOL)
        {
            firemode = FIRE_MODE.BURST;
            ModeIndicator.text = "BURST";
            ModeIndicator.color = Color.yellow;
        }
        else if (firemode == FIRE_MODE.BURST)
        {
            firemode = FIRE_MODE.AUTO;
            ModeIndicator.text = "AUTO";
            ModeIndicator.color = new Color(244 / 255f, 119 / 255f, 17 / 255f);
        }

        else if (firemode == FIRE_MODE.AUTO)
        {
            firemode = FIRE_MODE.PISTOL;
            ModeIndicator.text = "PISTOL";
            ModeIndicator.color = new Color(.29f, 2.24f, 2.26f);
        }
    }

    void UpdateAmmo()
    {
        //Check state
        if (grabbable.isGrabbed)
        {
            //Reloading
            if (ReloadInput && PistolAmmo <= 0 && reserve > 0)
            { //Can reload
                isReloading = true;
                ReloadTimer += Time.deltaTime;
                AmmoText.color = Color.yellow;
                AmmoText.text = "---\n" + (ReloadTimeLimit - ReloadTimer).ToString("F2");
                if (ReloadTimer >= ReloadTimeLimit)
                {
                    if (reserve >= 30) { PistolAmmo = 30; reserve -= 30; }
                    else { PistolAmmo = reserve; reserve = 0; }
                    ReloadTimer = 0.0f;
                    StartCoroutine(Vibration(0.2f, 0.5f, 0.1f));
                }
            }
            else //Not Reloading
            {
                isReloading = false;
                ReloadTimer = 0.0f;
                CheckAmmoColor(1);
            }
        }
        else if (transform.position != originalPos) //Preping to respawn
            CheckAmmoColor(2);
        else //In respawn
            CheckAmmoColor(3);

    }

    void RespawnGun() //Reset gun position to teleported position
    {
        transform.position = originalPos;
        transform.rotation = originalRot;
        hotBod.constraints = RigidbodyConstraints.FreezeAll;
        timer = 0.0f;
        sound.clip = audios[9];
        sound.Play();
    }

    IEnumerator BurstFire()
    {
        float bulletDelay = 60 / fireRate; //10 = fire rate
        firing = true;
        for (int i = 0; i < 3; i++)
        {
            Instantiate(lazur, spawnPos.position, spawnPos.rotation);
            //Play particle effect
            shootSpark.Play();
            //Play lazr sound
            sound.clip = audios[audioIndex];
            sound.Play();
            audioIndex = Random.Range(0, 8); //Randomly select next sound
            StartCoroutine(Vibration(1, 30, 0.1f));
            PistolAmmo--;
            yield return new WaitForSeconds(bulletDelay); // wait till the next round
        }
        firing = false;
    }

    void CheckAmmoColor(int flag)
    { //Flag - 1 = Held, 2 = Floating, 3 = Reset to original position
        //Check PistolAmmo
        if (PistolAmmo > 0)
        {
            if (PistolAmmo == 30) //Full 
                AmmoText.color = Color.green;
            else if (PistolAmmo > 6) //Normal
                AmmoText.color = new Color(.29f, 2.24f, 2.26f); //Standary Cyan Blue
            else
                AmmoText.color = Color.yellow;
            if (flag == 1)
                AmmoText.text = PistolAmmo + "\n" + reserve;
            else if (flag == 2)
                AmmoText.text = "---" + "\n" + (timeLimit - timer).ToString("F2"); //Display countdown
            else if (flag == 3)
                AmmoText.text = "---\n";
        }

        else if (reserve > 0) //Reloadable
        {
            AmmoText.color = new Color(244 / 255f, 119 / 255f, 17 / 255f); //Orange
            if (flag == 1)
            {
                AmmoText.text = "Rel\n" + reserve;
                if (!isReloading) StartCoroutine(Vibration(0.2f, 0.3f, 0.1f));
            }
            else if (flag == 2)
                AmmoText.text = "---" + "\n" + (timeLimit - timer).ToString("F2"); //Display countdown
            else if (flag == 3)
                AmmoText.text = "---\n";
        }

        else //Empty
        {
            AmmoText.color = Color.red;
            if (flag == 1)
            {
                AmmoText.text = "Empty";
                StartCoroutine(Vibration(0.2f, 0.15f, 0.1f));
            }
            else if (flag == 2)
                AmmoText.text = "---" + "\n" + (timeLimit - timer).ToString("F2"); //Display countdown
            else if (flag == 3)
                AmmoText.text = "---\n";

        }
    }

    public void LeftHandedModeToggle()
    {
        leftHandedMode = !leftHandedMode;
        if (leftHandedMode)
        { //Left hand
            LeftIndicator.text = "L";
            PistolOffset.position = new Vector3(0, 0.02f, -0.11f);

        }
        else if (!leftHandedMode)
        { //Right hand
            PistolOffset.position = new Vector3(0.05f, 0.02f, -0.11f);
            LeftIndicator.text = "R";
        }
    }

    IEnumerator Vibration(float frequency, float amplitude, float timeLimit)
    {
        if (grabbable.grabbedBy == LeftGrabber)
        {
            OVRInput.SetControllerVibration(frequency, amplitude, OVRInput.Controller.LTouch);
        }
        else if (grabbable.grabbedBy == RightGrabber)
        {
            OVRInput.SetControllerVibration(frequency, amplitude, OVRInput.Controller.RTouch);
        }

        yield return new WaitForSeconds(timeLimit); //Set vibration on until timer runs out

        if (grabbable.grabbedBy == LeftGrabber)
        {
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
        }
        else if (grabbable.grabbedBy == RightGrabber)
        {
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
        }

    }

}
