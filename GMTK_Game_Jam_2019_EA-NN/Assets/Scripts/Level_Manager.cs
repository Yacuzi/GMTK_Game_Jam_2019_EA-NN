using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Manager : MonoBehaviour
{
    public static int Current_Level;

    public Transform[] Camera_Pos, Character_Pos;
    public GameObject[] Level_Content;

    public GameObject The_Character;

    public void Change_Level()
    {
        Level_Content[Current_Level - 1].SetActive(false);
        Level_Content[Current_Level].SetActive(true);
        The_Character.transform.position = Character_Pos[Current_Level].position;
        Camera.main.transform.position = Camera_Pos[Current_Level].position;
    }

}
