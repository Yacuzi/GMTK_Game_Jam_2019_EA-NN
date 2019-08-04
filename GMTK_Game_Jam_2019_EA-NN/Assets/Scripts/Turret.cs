using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform characterRef;
    public float rotSpeed;
    public Transform laserStart, laserEnd, laserEndPos;
    public float facCorrecScale;

    private Animator animTurret, animLaser, animEndLaser;

    void rotateLaser()
    {
        Vector3 direction = Vector3.Normalize(characterRef.position - transform.position);
        float angle = Vector3.SignedAngle(direction, -transform.up,-Vector3.forward);
        transform.Rotate(Vector3.forward, Time.deltaTime * rotSpeed * angle);
    }

    void laserLength()
    {
        float distanceChara = Vector3.Distance(laserStart.position, characterRef.position);
        laserStart.localScale = new Vector3(1, facCorrecScale * distanceChara, 1);

        laserEnd.position = laserEndPos.position;
    }

    public void laserAnim()
    {
        if (!characterRef.GetComponent<Character_Move>().Dead)
        {
            animTurret.Play("Turret_Fire", -1, 0f);
            animLaser.Play("LaserStart_Fire", -1, 0f);
            animEndLaser.Play("LaserEnd_Fire", -1, 0f);
        }
    }

    private void Start()
    {
        animTurret = GetComponent<Animator>();
        animLaser = laserStart.GetComponent<Animator>();
        animEndLaser = laserEnd.GetComponent<Animator>();
    }

    private void Update()
    {
        laserLength();
        rotateLaser();
    }
}
