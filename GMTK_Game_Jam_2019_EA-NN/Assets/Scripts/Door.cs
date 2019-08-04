using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Resetables
{
    public bool StartOpen;
    private bool opened;

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
