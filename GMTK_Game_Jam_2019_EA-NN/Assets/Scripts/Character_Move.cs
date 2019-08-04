using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Move : MonoBehaviour
{
    public Level_Manager The_Level_Manager;
    public Animator VFX;

    public float speed;
    public bool debug;
    [HideInInspector]
    public Vector3 Pos_Ini;

    public static Vector3 posExit;

    private bool Ready, exiting;
    [HideInInspector]
    public bool Dead;
    private Animator CharaAnim;

    //Character Move
    void Move ()
    {
        if ((Time_Lord.Acting && !Dead))
        {
            if (Input.GetAxis("Vertical") != 0f)
            {
                transform.position += (speed * Time.fixedDeltaTime) * new Vector3(0, Input.GetAxis("Vertical"), 0);
            }

            if (Input.GetAxis("Horizontal") != 0f)
            {
                transform.position += (speed * Time.fixedDeltaTime) * new Vector3(Input.GetAxis("Horizontal"), 0, 0);
            }
        }
    }

    void AnimBlob()
    {
        if (Time_Lord.Acting && !Time_Lord.Transitioning && !Dead)
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
        Dead = true;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<AudioSource>().Play();

        VFX.Play("VFXDeath_Die", -1, 0f);
        CharaAnim.Play("Blob_Death",-1,0f);
    }

    public void Reset_Character()
    {
        if (!Dead)
        {
            Kill();
        }
    }

    public void resetPosChara()
    {
        transform.position = The_Level_Manager.Character_Pos[Level_Manager.Current_Level].position + new Vector3(0.5f, 0.5f, 0);
        GetComponent<BoxCollider2D>().enabled = true;
        Ready = true;
    }

    void Undead()
    {
        if (Ready && Time_Lord.Preparing && Time_Lord.Preparing && Time_Lord.The_Timer >= 0.5f)
        {
            CharaAnim.Play("Blob_Enter", -1, 0f);
            Ready = false;
            Dead = false;
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