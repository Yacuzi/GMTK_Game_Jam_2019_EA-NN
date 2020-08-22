using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Behaviour : Danger
{
    public Transform from, to;

    private Vector3 pos_ini;
    private int resetInt;

    public AudioSource zapSound;

    public override void Reset()
    {
        base.Reset();

        if (resetInt % 2 == 0)
        {
            transform.position = pos_ini;
        }

        resetInt++;
    }

    void Move_Ball()
    {
        float progression = 2 * (Time_Lord.The_Timer + (Vector3.Distance(from.position, pos_ini) / (2 * Vector3.Distance(from.position, to.position))));

        if (progression % 2 < 1)
        {
            transform.position = Vector3.Lerp(from.position,to.position,progression % 1);
        }
        else
        {
            transform.position = Vector3.Lerp(from.position, to.position, 1 - (progression % 1));
        }

    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    void Start()
    {
        pos_ini = transform.position;

        if (!zapSound.isPlaying)
        {
            zapSound.Play();
        }
    }

    private void Update()
    {
        Move_Ball();
    }
}
