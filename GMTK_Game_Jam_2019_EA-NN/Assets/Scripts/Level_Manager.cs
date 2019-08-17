using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Manager : MonoBehaviour
{
    public static int Current_Level;
    public static bool myTransition;
    public int finalLevel;
    public GameObject cameraLevel;
    public GameObject blackScreen;

    public Transform[] Character_Pos, exitPos;
    public GameObject[] Level_Content;

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

    private bool loadMyScene;

    public void resetStatic()
    {
        Level_Manager.Current_Level = 0;
        Level_Manager.myTransition = false;

        Time_Lord.Acting = false;
        Time_Lord.Preparing = true;
        Time_Lord.Transitioning = false;
        Time_Lord.timebender = 1f;
        Time_Lord.inTransition = false;
        Time_Lord.The_Timer = 0f;
    }

    public void Change_Level()
    {
        Current_Level++;
        Level_Content[Current_Level - 1].SetActive(false);
        Level_Content[Current_Level].SetActive(true);
        The_Character.transform.position = Character_Pos[Current_Level].position + new Vector3(0.5f, 0.5f, 0);
        The_Character.GetComponent<Character_Move>().dead = false;

        if ((theTimeLord.IntroTime_Lord && Current_Level != 2) || !theTimeLord.IntroTime_Lord)
        {
            The_Character.GetComponent<Animator>().Play("Blob_Enter_New", -1, 0f);
            soundEnter.PlayDelayed(1.2f);
        }

        The_Character.GetComponent<Character_Move>().ejectTrail();
        Destroy(The_Character.GetComponent<Character_Move>().currentTrail);

        The_Character.GetComponent<Character_Move>().washStains();

        if (Time_Lord.timebender > 1f)
        {
            Time_Lord.timebender -= 0.1f;
        }

        if (Current_Level == finalLevel -1)
        {
            HideTurret.enabled = false;
            cameraLevel.SetActive(true);
            blackScreen.SetActive(false);
            The_Character.GetComponent<Character_Move>().speed = The_Character.GetComponent<Character_Move>().speed / 5;
        }
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
                changeOnce = true;
                Change_Level();
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

    private void Start()
    {
        mask_ini = MonMask.transform.localScale;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (loadMyScene && !transitionSound.isPlaying)
        {
            loadNewScene();
        }

        if (The_Character.GetComponent<Character_Move>().debug && Input.GetKeyDown(KeyCode.Space))
        {
            Change_Level();
            Time_Lord.Preparing = true;
            Time_Lord.The_Timer = 0f;
        }

            if (myTransition)
        {
            Transition();
        }

            if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
