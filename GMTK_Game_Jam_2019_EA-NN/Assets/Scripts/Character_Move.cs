using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Move : MonoBehaviour
{
    public Level_Manager The_Level_Manager;
    public Time_Lord theTimeLord;
    public Animator VFX;

    public float speed;
    public bool debug;
    [HideInInspector]
    public Vector3 Pos_Ini;

    public static Vector3 posExit;

    private bool Ready, exiting;
    [HideInInspector]
    public bool dead;
    private Animator CharaAnim;

    public AudioSource soundEnter;

    //Character Move
    void Move ()
    {
        if ((Time_Lord.Acting && !dead))
        {
            if (Mathf.Abs(Input.GetAxis("Vertical")) >= 0.01f)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, speed * Input.GetAxis("Vertical")));
                //transform.position += (speed * Time.fixedDeltaTime) * new Vector3(0, Input.GetAxis("Vertical"), 0);
            }

            if (Mathf.Abs(Input.GetAxis("Horizontal")) >= 0.01f)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(speed * Input.GetAxis("Horizontal"), 0));
                //transform.position += (speed * Time.fixedDeltaTime) * new Vector3(Input.GetAxis("Horizontal"), 0, 0);
            }
        }
    }

    void AnimBlob()
    {
        if (Time_Lord.Acting && !Time_Lord.Transitioning && !dead)
        {
            bool up = false, left = false, down = false, right = false;

            if (Mathf.Abs(Input.GetAxis("Vertical")) >= Mathf.Abs(Input.GetAxis("Horizontal")))
            {
                if (Input.GetAxis("Vertical") > 0)
                    up = true;
                else if (Input.GetAxis("Vertical") < 0)
                    down = true;
            }
            else if (Input.GetAxis("Horizontal") > 0)
                right = true;
            else if (Input.GetAxis("Horizontal") < 0)
                left = true;

            if (up)
                CharaAnim.Play("Blob_Up");
            else if (down)
                CharaAnim.Play("Blob_Down");
            else if (left)
                CharaAnim.Play("Blob_Left");
            else if (right)
                CharaAnim.Play("Blob_Right");
            else
                CharaAnim.Play("Blob_Idle");
        }            
    }

    public void Kill ()
    {
        dead = true;
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<AudioSource>().Play();

        if (Time_Lord.timebender < 1.5f)
            Time_Lord.timebender += 0.025f;

        VFX.Play("VFXDeath_Die", -1, 0f);
        CharaAnim.Play("Blob_Death",-1,0f);
    }

    public void Reset_Character()
    {
        if (!dead)
        {
            Kill();
        }
    }

    //Called by animation
    public void resetPosChara()
    {
        transform.position = The_Level_Manager.Character_Pos[Level_Manager.Current_Level].position + new Vector3(0.5f, 0.5f, 0);
        GetComponent<CircleCollider2D>().enabled = true;
        Ready = true;
    }
    
    void Undead()
    {
        if (Ready && Time_Lord.Preparing && Time_Lord.The_Timer >= 0.5f)
        {
            soundEnter.PlayDelayed(0.2f);
            CharaAnim.Play("Blob_Enter", -1, 0f);
            Ready = false;
            dead = false;
        }
    }

    void moveToGutter()
    {
            if (Time_Lord.Transitioning)
            {
                transform.position = Vector3.Lerp(transform.position, The_Level_Manager.exitPos[Level_Manager.Current_Level].position + new Vector3(0.5f, 0.5f, 0), Time_Lord.The_Timer);
                
                if (!exiting)
                {
                    exiting = true;
                    CharaAnim.Play("Blob_Exit", -1, 0f);
                }
            }
            else
                exiting = false;
    }

    private void Start()
    {
        CharaAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        AnimBlob();
        Undead();
        moveToGutter();
    }
}