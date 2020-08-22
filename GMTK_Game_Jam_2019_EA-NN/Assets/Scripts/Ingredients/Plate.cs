using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : Resetables
{
    public bool activated;

    public Sprite activated_Plate, up_Plate;

    public Level_Manager theLevelManager;

    private bool activated_ini;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !activated)
        {
            theLevelManager.buttonSound.Play();

            activated = true;
            Activate();
        }
    }

    public override void Reset()
    {
        base.Reset();

        if (!theLevelManager.Safe_Level[Level_Manager.Current_Level])
            activated = activated_ini;

        Activate();
    }

    public void Activate()
    {
        if (activated)
        {
            GetComponent<SpriteRenderer>().sprite = activated_Plate;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = up_Plate;
        }
    }

    private void Start()
    {
        activated_ini = activated;
        Activate();
    }
}
