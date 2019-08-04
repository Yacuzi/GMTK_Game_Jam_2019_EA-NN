using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : Resetables
{
    public bool activated;

    public Sprite activated_Plate, up_Plate;

    private bool activated_ini;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !activated)
        {
            activated = true;
            GetComponent<AudioSource>().Play();
            Activate();
        }
    }

    public override void Reset()
    {
        base.Reset();

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
