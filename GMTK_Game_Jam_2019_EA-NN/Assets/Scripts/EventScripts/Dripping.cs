using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dripping : MonoBehaviour
{
    public GameObject theBlob;

    private float timer;
    private bool undead;

    public AudioSource dripSound;

    void addDrip()
    {
        if (theBlob.transform.localScale.x <= 7.5f)
        {
            theBlob.transform.localScale += Vector3.one * 0.2f;
            GetComponent<Animator>().speed += 0.05f;
            dripSound.Play();
        }
        else
        {
            theBlob.GetComponent<Character_Move>().currentTrail.GetComponent<TrailRenderer>().time = 20f;
            theBlob.GetComponent<Character_Move>().currentTrail.GetComponent<TrailRenderer>().startWidth = 4f;

            undead = true;
            timer = 0;
            GetComponent<Animator>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;

        if (timer >= 2f)
        {
            GetComponent<Animator>().enabled = true;
            theBlob.GetComponent<Character_Move>().CharaAnim.Play("Blob_Idle");

            if (undead)
            {
                theBlob.GetComponent<Character_Move>().cannotMove = false;
                this.gameObject.SetActive(false);
            }
        }
    }
}
