using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Time_Lord : MonoBehaviour
{

    public Character_Move Character;
    public Level_Manager The_Level_Manager;
    public Turret theTurret;

    public static float The_Timer;

    public AudioSource spikeIn, spikeOut, tickSound, laserSound;

    public static bool Acting = false;
    public static bool Preparing = true;
    public static bool Transitioning = false, inTransition = false;

    private static float timebender = 1;

    //J'augmente mon compteur de temps
    private void Update_Time ()
    {
        if (!inTransition)
            The_Timer += Time.deltaTime * timebender;
    }

    //Je reset le timer
    public static void Reset_Time()
    {
        The_Timer = 0;
    }

    void Reset_Level()
    {
        Resetables[] the_resets = The_Level_Manager.Level_Content[Level_Manager.Current_Level].GetComponentsInChildren<Resetables>();

        foreach (Resetables reset in the_resets)
        {
            reset.Reset();
        }
    }

    private void Tick ()
    {
        //Si je suis pas en train de reset la salle et que le temps est supérieur à 1 seconde
        if (The_Timer >= 1f)
        {
            Reset_Time();

            if (Acting)
            {                
                Acting = false;
                Preparing = true;
                Reset_Level();
                theTurret.laserAnim();
                Character.Reset_Character();
                return;
            }
            if (Preparing)
            {
                Preparing = false;
                Acting = true;
                Reset_Level();
                return;
            }
            if (Transitioning)
            {
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
        Update_Time();
        Tick();
    }
}