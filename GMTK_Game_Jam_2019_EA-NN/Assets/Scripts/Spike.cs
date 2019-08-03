using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : Danger
{
    public bool activated;
    private bool activated_ini, doOnce;

    public Sprite S_activated, S_deactivated;

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (activated)
        {
            base.OnCollisionEnter2D(collision);
        }
    }

    public override void Reset()
    {
        base.Reset();

        activated = activated_ini;
        Change_Sprite();
        doOnce = false;
    }

    void Activation ()
    {
        if (Time_Lord.The_Timer >= 0.5f && !doOnce)
        {
            doOnce = true;
            activated = !activated;
            Change_Sprite();
        }
    }

    void Change_Sprite()
    {
        if (activated)
        {
            GetComponent<SpriteRenderer>().sprite = S_activated;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = S_deactivated;
        }
    }

    private void Start()
    {
        activated_ini = activated;
        Change_Sprite();
    }

    private void Update()
    {
        Activation(); 
    }

}
