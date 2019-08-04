using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Resetables
{
    public bool StartOpen;
    private bool opened;

    public Sprite Opened_Door, Closed_Door;

    private bool opened_ini;

    public Plate[] Linked_Plates;
    public Level_Manager The_Level_Manager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && opened)
        {
            Time_Lord.Acting = false;
            Time_Lord.Transitioning = true;
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
        if (opened)
        {
            GetComponent<Animator>().SetBool("IsOpen", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("IsOpen", false);
        }
    }

    public void Openini()
    {
        if (opened)
        {
            GetComponent<SpriteRenderer>().sprite = Opened_Door;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = Closed_Door;
        }
    }

    private void Start()
    {
        opened = StartOpen;
        opened_ini = opened;
        Openini();
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
