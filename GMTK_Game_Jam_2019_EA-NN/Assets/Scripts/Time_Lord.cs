using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Time_Lord : MonoBehaviour
{

    public Character_Move Character;
    public Level_Manager The_Level_Manager;
    public Turret theTurret;
    public bool IntroTime_Lord;

    public static float The_Timer;

    public AudioSource spikeIn, spikeOut, tickSound, laserSound;

    public static bool Acting = false;
    public static bool Preparing = true;
    public static bool Transitioning = false, inTransition = false;

    public static float timebender = 1;

    public GameObject thebar;

    //J'augmente mon compteur de temps
    private void Update_Time()
    {
        The_Timer += Time.deltaTime / timebender;
    }

    //Je reset le timer
    public static void Reset_Time()
    {
        The_Timer = 0;
    }

    public void Reset_Level()
    {
        Resetables[] the_resets = The_Level_Manager.Level_Content[Level_Manager.Current_Level].GetComponentsInChildren<Resetables>();

        foreach (Resetables reset in the_resets)
        {
            reset.Reset();
        }
    }

    private void Tick ()
    {
        //Debug.Log(The_Timer + " Acting " + Acting + " Preparing " + Preparing + " Transitioning " + Transitioning);
        //Si je suis pas en train de reset la salle et que le temps est supérieur à 1 seconde
        if (The_Timer >= 1f)
        {
            if (Acting)
            {
                if (Level_Manager.Current_Level != 19)
                {
                    Reset_Time();
                    Acting = false;
                    Preparing = true;
                    Reset_Level();
                    theTurret.laserAnim();
                    if (!Character.dead)
                        laserSound.Play();
                    Character.Reset_Character();
                }
                thebar.SetActive(false);
                return;
            }
            if (Preparing)
            {
                Reset_Time();
                Preparing = false;
                Acting = true;
                Reset_Level();
                thebar.transform.localScale = Vector3.zero;
                thebar.SetActive(true);

                if (Level_Manager.Current_Level == 19)
                {
                    thebar.SetActive(false);
                }

                return;
            }
            if (Transitioning)
            {
                Reset_Time();
                thebar.SetActive(false);
                inTransition = true;
                The_Level_Manager.StartTransition();
                return;
            }
        }
    }

    public void theSpikeSound(bool activation)
    {
        if (activation)
        {
            if (!spikeIn.isPlaying)
                spikeIn.Play();
        }
        else
        {
            if (!spikeOut.isPlaying)
                spikeOut.Play();
        }
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IntroTime_Lord)
        {
            Acting = true;
        }

        if (!IntroTime_Lord)
        {
            Update_Time();
            Tick();
        }
    }
}