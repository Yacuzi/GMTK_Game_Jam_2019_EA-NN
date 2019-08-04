using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Manager : MonoBehaviour
{
    public static int Current_Level;

    public Transform[] Character_Pos, exitPos;
    public GameObject[] Level_Content;

    public GameObject The_Character;
    public GameObject MonMask;

    public float transitionTime;

    private bool myTransition, transitionOut, changeOnce;
    private Vector3 mask_ini;
    private float transitionSize;
    
    public void Change_Level()
    {
        Current_Level++;
        Level_Content[Current_Level - 1].SetActive(false);
        Level_Content[Current_Level].SetActive(true);
        The_Character.transform.position = Character_Pos[Current_Level].position + new Vector3(0.5f, 0.5f, 0);
        The_Character.GetComponent<Animator>().Play("Blob_Enter_New", -1, 0f);
    }

    public void StartTransition()
    {
        myTransition = true;
        MonMask.transform.position = Character_Pos[Level_Manager.Current_Level+1].position + new Vector3(0.5f, 0.5f, 0);
    }

    private void Transition()
    {
        if (!transitionOut)
        {
            if (transitionSize <= 1f)
                transitionSize += Time.deltaTime / transitionTime;
            else
            {
                transitionSize = 1f;
                transitionOut = true;
            }
        }
        else
        {
            if (!changeOnce)
            {
                changeOnce = true;
                Change_Level();
            }

            if (transitionSize >= 0f)
                transitionSize -= Time.deltaTime / transitionTime;
            else
            {
                transitionSize = 0f;
                transitionOut = false;
                myTransition = false;
                changeOnce = false;
                Time_Lord.Transitioning = false;
                Time_Lord.inTransition = false;
                Time_Lord.Preparing = true;
            }
        }

        MonMask.transform.localScale = Vector3.Lerp(mask_ini, Vector3.zero, transitionSize);
    }

    private void Update()
    {
        if (The_Character.GetComponent<Character_Move>().debug && Input.GetKeyDown(KeyCode.Space))
            Change_Level();

            if (myTransition)
        {
            Transition();
        }
    }

    private void Start()
    {
        mask_ini = MonMask.transform.localScale;
    }

}
