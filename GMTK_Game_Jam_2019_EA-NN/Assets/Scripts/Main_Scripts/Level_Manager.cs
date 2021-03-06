﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Level_Manager : MonoBehaviour
{
    public static int Current_Level;
    public static bool myTransition;
    public int finalLevel;
    public GameObject cameraLevel;
    public GameObject blackScreen;
    public GameObject drippingObject;

    public Transform[] Character_Pos, exitPos;
    public GameObject[] Level_Content;
    public bool[] Safe_Level;

    public GameObject The_Character;
    public GameObject MonMask;

    public float transitionTime;

    private bool transitionOut, changeOnce;
    private Vector3 mask_ini;
    private float transitionSize;

    public AudioSource soundEnter;

    public SpriteRenderer HideTurret;
    public Time_Lord theTimeLord;

    public AudioSource transitionSound;
    public AudioSource zapSound;
    public AudioSource openSound, closeSound, buttonSound;

    private bool loadMyScene;

    public TextMeshProUGUI gameSavedText;

    public void resetStatic()
    {
        Level_Manager.Current_Level = 0;
        Level_Manager.myTransition = false;

        Time_Lord.Acting = false;
        Time_Lord.Preparing = true;
        Time_Lord.Transitioning = false;
        Time_Lord.timebender = PlayerPrefs.GetFloat("TimeBender");
        Time_Lord.inTransition = false;
        Time_Lord.The_Timer = 0f;
    }

    public void Change_Level(bool initLevel)
    {
        Current_Level++;

        Level_Content[Current_Level - 1].SetActive(false);
        Level_Content[Current_Level].SetActive(true);

        The_Character.transform.position = Character_Pos[Current_Level].position + new Vector3(0.5f, 0.5f, 0);
        The_Character.GetComponent<Character_Move>().dead = false;

        if ((theTimeLord.IntroTime_Lord && Current_Level != 2) || !theTimeLord.IntroTime_Lord)
        {
            The_Character.GetComponent<Character_Move>().CharaAnim.Play("Blob_Enter_New", -1, 0f);
            if(!initLevel)
                soundEnter.PlayDelayed(1.2f);
            else
                soundEnter.PlayDelayed(0.6f);
        }

        The_Character.GetComponent<Character_Move>().ejectTrail();

        if (The_Character.GetComponent<Character_Move>().currentTrail != null)
            Destroy(The_Character.GetComponent<Character_Move>().currentTrail);

        The_Character.GetComponent<Character_Move>().washStains();

        if (!The_Character.GetComponent<Character_Move>().timeRuler && !initLevel)
        {
            Time_Lord.timebender = Mathf.Clamp(Time_Lord.timebender - 0.1f, 1f, 1.25f);         
            PlayerPrefs.SetFloat("TimeBender", Time_Lord.timebender);
        }

        if (Safe_Level[Current_Level])
        {
            HideTurret.enabled = false;
            if (!initLevel)
            {
                PlayerPrefs.SetInt("Level", Current_Level);
                PlayerPrefs.SetInt("Deaths", The_Character.GetComponent<Character_Move>().nbDeath);
                showGameSaved();
            }
        }
        else
        {
            HideTurret.enabled = true;
        }

        if (Current_Level == finalLevel -1)
        {
            cameraLevel.SetActive(true);
            The_Character.transform.rotation = Quaternion.identity;

            if (The_Character.GetComponent<Character_Move>().nbDeath > 10)
                The_Character.GetComponent<Character_Move>().speed = The_Character.GetComponent<Character_Move>().speed / 5;
            else
            {
                The_Character.GetComponent<Character_Move>().cannotMove = true;
                drippingObject.SetActive(true);
                The_Character.GetComponent<Character_Move>().speed = The_Character.GetComponent<Character_Move>().speed / 10;
            }
        }
    }

    private void showGameSaved()
    {
        Color myColor = gameSavedText.color;
        myColor.a = 1f;

        gameSavedText.color = myColor;
    }

    public void StartTransition()
    {
        myTransition = true;
        MonMask.transform.position = Character_Pos[Level_Manager.Current_Level + 1].position + new Vector3(0.5f, 0.5f, 0);
    }

    public void Transition()
    {
        if (!transitionOut)
        {
            if (Time_Lord.The_Timer <= 0.5f)
                transitionSize = 2 * Time_Lord.The_Timer;
            else
            {
                transitionSize = 1f;
                Time_Lord.Transitioning = false;
                Time_Lord.inTransition = false;
                transitionOut = true;
            }
        }
        else
        {
            if (!changeOnce)
            {
                zapSound.Stop();

                changeOnce = true;
                Change_Level(false);
            }

            if (Time_Lord.The_Timer <= 1f)
                transitionSize = 2 - (2 * Time_Lord.The_Timer);
            else
            {
                transitionSize = 0f;
                transitionOut = false;
                myTransition = false;
                changeOnce = false;
                theTimeLord.Reset_Level();
                Time_Lord.Preparing = true;
                Time_Lord.The_Timer = 0f;

                if (Current_Level == finalLevel -1)
                    blackScreen.SetActive(false);

                if (theTimeLord.IntroTime_Lord && Level_Manager.Current_Level == 2)
                {
                    loadMyScene = true;
                }
            }
        }

        MonMask.transform.localScale = Vector3.Lerp(mask_ini, Vector3.zero, transitionSize);
    }

    private void loadNewScene()
    {
        SceneManager.LoadScene(1);
        resetStatic();
        return;
    }

    public void DeleteSave ()
    {
            PlayerPrefs.SetInt("Level", 0);
            PlayerPrefs.SetInt("Deaths", 0);
            PlayerPrefs.SetFloat("TimeBender", 1f);
            PlayerPrefs.SetFloat("Sensitivity", 50);
            transitionSound.Play();
    }

    void ChangeFullScreen()
    {
        if (Input.GetKey(KeyCode.F) && Input.GetKey(KeyCode.LeftShift))
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
    }

    private void Awake()
    {
        Cursor.visible = false;

        Time_Lord.timebender = PlayerPrefs.GetFloat("TimeBender");

        if (Time_Lord.timebender < 1f)
        {
            PlayerPrefs.SetFloat("TimeBender", 1f);
            Time_Lord.timebender = 1f;
        }

        mask_ini = MonMask.transform.localScale;

        if (PlayerPrefs.GetInt("Level") > 0)
        {
            The_Character.GetComponent<Character_Move>().nbDeath = PlayerPrefs.GetInt("Deaths");

            The_Character.GetComponentInParent<Character_Birth>().enabled = false;
            The_Character.GetComponent<Character_Move>().enabled = true;
            The_Character.GetComponent<CircleCollider2D>().enabled = true;
            Level_Content[0].SetActive(false);
            Current_Level = PlayerPrefs.GetInt("Level") - 1;
            Change_Level(true);

            if (Current_Level == finalLevel - 1)
                blackScreen.SetActive(false);
        }
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.D) && Input.GetKeyDown(KeyCode.L))
        {
            DeleteSave();
        }

        ChangeFullScreen();

        if (loadMyScene && !transitionSound.isPlaying)
        {
            loadNewScene();
        }

        if (The_Character.GetComponent<Character_Move>().debug && Input.GetKeyDown(KeyCode.Space))
        {
            Change_Level(false);
            Time_Lord.Preparing = true;
            Time_Lord.The_Timer = 0f;
        }

            if (myTransition)
        {
            Transition();
        }

            if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState != CursorLockMode.Locked)
                Application.Quit();
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
