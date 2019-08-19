using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : Danger
{
    public AudioSource zapSound;

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision); 
    }

    private void Start()
    {
        if (!zapSound.isPlaying)
        {
            zapSound.Play();
        }
    }
}
