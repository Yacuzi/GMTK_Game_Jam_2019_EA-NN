using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Resetables
{
    public bool opened;

    public Sprite Opened_Door, Closed_Door;

    private bool opened_ini;

    public Plate[] Linked_Plates;
    public Level_Manager The_Level_Manager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && opened)
        {
            Level_Manager.Current_Level++;
            The_Level_Manager.Change_Level();
        }
    }

    public override void Reset()
    {
        base.Reset();

        opened = opened_ini;
        Open();
    }

    public void Open ()
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
        opened_ini = opened;
        Open();
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
        Check_Opening();
    }
}
