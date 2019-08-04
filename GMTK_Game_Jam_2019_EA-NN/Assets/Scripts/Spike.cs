﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : Danger
{
    public bool StartActivated;
    public Time_Lord theTimeLord;

    private bool activated, anim_activation;
    private bool activated_ini, doOnce, doOnceAnim, doTwiceAnim;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (activated)
        {
            Debug.Log("Kill" + Time_Lord.The_Timer);
            collision.gameObject.GetComponent<Character_Move>().Kill();
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
       
    }

    public override void Reset()
    {
        base.Reset();
        
        activated = activated_ini;
        anim_activation = activated_ini;
        Change_Sprite();
        doOnce = false;
        doOnceAnim = false;
        doTwiceAnim = false;
    }

    void Activation ()
    {
        if (Time_Lord.The_Timer >= 0.35f && !doOnceAnim)
        {
            doOnceAnim = true;
            anim_activation = !anim_activation;
            Change_Sprite();
        }

        if (Time_Lord.The_Timer >= 0.85f && !doTwiceAnim)
        {
            doOnceAnim = false;
            doTwiceAnim = true;
        }

        if (Time_Lord.The_Timer >= 0.5f && Time_Lord.Acting && !doOnce)
        {
            doOnce = true;
            activated = !activated;
        }
    }

    void Change_Sprite()
    {
        theTimeLord.theSpikeSound(anim_activation);

        GetComponent<Animator>().SetBool("activated", anim_activation);
    }

    private void Start()
    {
        activated = StartActivated;
        anim_activation = activated;
        activated_ini = activated;

        GetComponent<Animator>().SetBool("StartActivated", StartActivated);
        Change_Sprite();
    }

    private void Update()
    {
        Activation();
    }

}
