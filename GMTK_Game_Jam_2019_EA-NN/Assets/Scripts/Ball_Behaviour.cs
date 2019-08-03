using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Behaviour : Danger
{
    public float speed;
    private float speed_ini;

    public Vector3 direction;
    private Vector3 pos_ini;

    public override void Reset()
    {
        base.Reset();

        speed = speed_ini;
        transform.position = pos_ini;
    }

    void Move_Ball()
    {
        transform.position += speed * Time.deltaTime * Vector3.Normalize(direction);
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        speed = -speed;
    }

    void Start()
    {
        speed_ini = speed;
        pos_ini = transform.position;
    }

    private void Update()
    {
        Move_Ball();
    }
}
