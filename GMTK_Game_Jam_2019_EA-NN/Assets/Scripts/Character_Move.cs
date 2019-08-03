using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Move : MonoBehaviour
{
    public Level_Manager The_Level_Manager;
    public float speed;
    public bool debug;
    [HideInInspector]
    public Vector3 Pos_Ini;

    private bool Dead;

    // Start is called before the first frame update
    void Start()
    {

    }

    //Character Move
    void Move ()
    {
        if ((Time_Lord.Acting && !Dead) || debug)
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

    public void Kill ()
    {
        Dead = true;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void Reset_Character()
    {
        transform.position = The_Level_Manager.Character_Pos[Level_Manager.Current_Level].position;
        GetComponent<BoxCollider2D>().enabled = true;
        Dead = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }
}