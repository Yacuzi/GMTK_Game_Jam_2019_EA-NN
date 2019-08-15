using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Resetables
{
    public bool StartOpen;
    public AudioClip openClip, closeClip;

    private bool opened, lastOpened;

    private bool opened_ini;

    public Plate[] Linked_Plates;
    public Level_Manager The_Level_Manager;

    public AudioSource soundExit, soundTransition;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && opened)
        {
            Time_Lord.Acting = false;
            Time_Lord.Transitioning = true;
            soundExit.Play();
            soundTransition.PlayDelayed(0.7f);
        }
    }

    public override void Reset()
    {
        base.Reset();

        opened = opened_ini;
        if (!StartOpen)
            Open();
    }

    public void Open ()
    {
        GetComponent<Animator>().SetBool("IsOpen", opened);

        if (lastOpened != opened)
        {
            if (opened)
                GetComponent<AudioSource>().clip = openClip;
            else
                GetComponent<AudioSource>().clip = closeClip;

            GetComponent<AudioSource>().Play();
        }

        lastOpened = opened;
    }

    private void Start()
    {
        opened = StartOpen;
        opened_ini = opened;
        GetComponent<Animator>().SetBool("StartOpen", StartOpen);
    }

    private void Check_Opening ()
    {
        foreach (Plate linked_plate in Linked_Plates)
        {
            if (linked_plate.activated == false)
                return;
        }

        opened = true;
        Open();
    }

    private void Update()
    {
        if (!StartOpen)
            Check_Opening();
    }
}
