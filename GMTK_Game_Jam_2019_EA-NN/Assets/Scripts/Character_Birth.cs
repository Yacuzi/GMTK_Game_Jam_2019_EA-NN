using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Character_Birth : MonoBehaviour
{
    public Animator characterAnim;
    public Character_Move characMove;
    private bool playOnce;

    public AudioSource soundBirth, soundEnter;

    public SpriteRenderer titleSprite;
    public TextMeshProUGUI tutoSprite;

    private bool tutoIn;

    // Start is called before the first frame update
    void Start()
    {
        characMove.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
    }

    //Called by animation
    void canMove ()
    {
        characMove.gameObject.GetComponent<CircleCollider2D>().enabled = true;
        characMove.enabled = true;
        tutoIn = true;
    }

    void fadeTuto()
    {
        if (tutoSprite.color.a < 1f)
        {
            Color myColor = tutoSprite.color;
            myColor.a += Time.deltaTime / 2;

            tutoSprite.color = myColor;
        }
    }

    void fadeTitle ()
    {
        if (titleSprite.color.a > 0f)
        {
            Color myColor = titleSprite.color;
            myColor.a -= Time.deltaTime;

            titleSprite.color = myColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= 5f)
        {
            fadeTitle();
        }

        if (Time.time >= 10f && !playOnce)
        {
            playOnce = true;

            GetComponent<Animator>().Play("Birth_Trajectory");
            characterAnim.Play("Blob_Birth");
            characMove.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;

            soundBirth.Play();
            soundEnter.PlayDelayed(0.75f);
        }

        if (tutoIn)
        {
            fadeTuto();
        }
    }
}
