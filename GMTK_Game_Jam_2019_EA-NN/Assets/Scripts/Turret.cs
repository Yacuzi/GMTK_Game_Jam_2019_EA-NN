using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform characterRef;
    public bool isIntroTurret;
    public AudioSource laserSound;
    public float rotSpeed;
    public Transform laserStart, laserEnd, laserEndPos;
    public float facCorrecScale;

    private bool playOnce;

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
        if (!isIntroTurret)
        {
            if (!characterRef.GetComponent<Character_Move>().dead)
            {
                animTurret.Play("Turret_Fire", -1, 0f);
                animLaser.Play("LaserStart_Fire", -1, 0f);
                animEndLaser.Play("LaserEnd_Fire", -1, 0f);
            }
        }
    }

    public void laserAnimIntro()
    {
            animTurret.Play("Turret_Fire", -1, 0f);
            animLaser.Play("LaserStart_Fire", -1, 0f);
            animEndLaser.Play("LaserEnd_Fire", -1, 0f);
    }

    private void Start()
    {
        animTurret = GetComponent<Animator>();
        animLaser = laserStart.GetComponent<Animator>();
        animEndLaser = laserEnd.GetComponent<Animator>();
    }

    private void Update()
    {
        if (isIntroTurret)
        {
            if ((int)Time.time % 5 == 2 && !playOnce)
            {
                playOnce = true;
                laserAnimIntro();
                laserSound.Play();
            }

            if ((int)Time.time % 5 == 4)
            {
                playOnce = false;
            }
        }

        laserLength();
        rotateLaser();
    }
}
