﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using TMPro;

public class Character_Move : MonoBehaviour
{
    public Level_Manager The_Level_Manager;
    public Time_Lord theTimeLord;
    public Animator VFX;

    public float speed;
    public bool debug, timeRuler;
    [HideInInspector]
    public Vector3 Pos_Ini;

    public static Vector3 posExit;

    private bool Ready, exiting;
    [HideInInspector]
    public bool dead, cannotMove;
    public Animator CharaAnim;

    public AudioSource soundEnter;

    public float camShakeCor, shakeTime;
    private bool shake;
    private float shakeTimer;
    private Vector3 cameraIni;

    public GameObject trailRef;
    [HideInInspector]
    public GameObject currentTrail;

    public GameObject deathStain;
    public GameObject deathStains;

    public GameObject mouseCursor;
    public GameObject mouseImage;
    private bool mouseMoving;

    public float blobRotSpeed;

    public PostProcessVolume myPostProcess;

    public AudioSource[] sourcesToMute;
    private DepthOfField myBlur;

    [HideInInspector]
    public int nbDeath;

    public Transform cameraShake;

    private int mouseSensValue = 50;
    private float mouseSensitivity;
    public TextMeshProUGUI sensitivityUI;

    void MouseCursor ()
    {
        //mouseImage.transform.position = Input.mousePosition;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            mouseSensValue ++;
            mouseSensValue = Mathf.Clamp(mouseSensValue, 0, 100);
            PlayerPrefs.SetFloat("Sensitivity", mouseSensitivity);

            updateAndShowSensitivity();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            mouseSensValue--;
            mouseSensValue = Mathf.Clamp(mouseSensValue, 0, 100);
            PlayerPrefs.SetFloat("Sensitivity", mouseSensitivity);

            updateAndShowSensitivity();
        }

        mouseSensitivity = ((mouseSensValue * 3.6f) / 100f) - 0.8f;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        Vector3 mouseMovement = new Vector3(mouseX, mouseY, 0);

        Vector3 mousePosCor = mouseImage.transform.position + (mouseMovement * 20f * (1f + mouseSensitivity));
        float mousePosCorx = Mathf.Clamp(mousePosCor.x, 0, Screen.width);
        float mousePosCory = Mathf.Clamp(mousePosCor.y, 0, Screen.height);
        mousePosCor = new Vector3(mousePosCorx, mousePosCory, 0);

        mouseImage.transform.position = mousePosCor;

        //Cursor the player actually follows movement
        mouseCursor.transform.position = Camera.main.ScreenToWorldPoint(mousePosCor);
        mouseCursor.transform.position = new Vector3(mouseCursor.transform.position.x, mouseCursor.transform.position.y, 0f);
    }

    private void updateAndShowSensitivity()
    {
        sensitivityUI.GetComponent<TextMeshProUGUI>().text = "Sensitivity : " + mouseSensValue.ToString();

        Color myColor = sensitivityUI.color;
        myColor.a = 1f;

        sensitivityUI.color = myColor;
    }

    void showCursor ()
    {
        if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
        {
            mouseImage.GetComponent<Image>().enabled = false;
            mouseMoving = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        else if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            mouseImage.GetComponent<Image>().enabled = true;
            mouseMoving = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void mouseMove ()
    {
        if (mouseMoving)
        {
            Vector3 mouseDirection = Vector3.Normalize(mouseCursor.transform.position - transform.position);

            if ((Time_Lord.Acting && !dead && !cannotMove))
            {
                if (Vector3.Distance(mouseCursor.transform.position, transform.position) >= 0.5f)

                    if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
                    {
                        GetComponent<Rigidbody2D>().AddForce(new Vector3(speed * mouseDirection.x, speed * mouseDirection.y, 0));
                    }
            }
        }
    }

    //Character Move
    void keyboardMove ()
    {
        if (!mouseMoving)
        {
            if ((Time_Lord.Acting && !dead && !cannotMove))
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
    }

    void RotateBlobMouse()
    {
        if (mouseMoving)
        {
            if (Time_Lord.Acting && !Time_Lord.Transitioning && !dead && !cannotMove)
            {
                Vector3 mouseDirection = Vector3.Normalize(mouseCursor.transform.position - transform.position);
                float angle = Vector3.SignedAngle(mouseDirection, -transform.up, -Vector3.forward);
                transform.Rotate(Vector3.forward, Time.deltaTime * blobRotSpeed * angle);

                if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
                {
                    if (Vector3.Distance(mouseCursor.transform.position, transform.position) >= 0.5f)
                    {
                        if (Level_Manager.Current_Level != The_Level_Manager.finalLevel - 1)
                            CharaAnim.Play("Blob_Down");

                        return;
                    }
                }

                if (!CharaAnim.GetCurrentAnimatorStateInfo(0).IsName("Blob_Enter") && !CharaAnim.GetCurrentAnimatorStateInfo(0).IsName("Blob_Enter_New"))
                    CharaAnim.Play("Blob_Idle");
            }
        }
    }

    void RotateBlobKeyboard()
    {
        if (!mouseMoving)
        {
            if (Time_Lord.Acting && !Time_Lord.Transitioning && !dead && !cannotMove)
            {
                Vector3 keyboardDirection = Vector3.Normalize(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),0));
                float angle = Vector3.SignedAngle(keyboardDirection, -transform.up, -Vector3.forward);
                transform.Rotate(Vector3.forward, Time.deltaTime * blobRotSpeed * angle);

                if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0 || Mathf.Abs(Input.GetAxis("Vertical")) > 0)
                {
                    if (Level_Manager.Current_Level != The_Level_Manager.finalLevel - 1)
                        CharaAnim.Play("Blob_Down");

                        return;
                }

                if (!CharaAnim.GetCurrentAnimatorStateInfo(0).IsName("Blob_Enter") && !CharaAnim.GetCurrentAnimatorStateInfo(0).IsName("Blob_Enter_New"))
                    CharaAnim.Play("Blob_Idle");
            }
        }
    }

    void AnimBlobMouse()
    {
        if (mouseMoving)
        {
            Vector3 mouseDirection = Vector3.Normalize(mouseCursor.transform.position - transform.position);

            if (Time_Lord.Acting && !Time_Lord.Transitioning && !dead && !cannotMove)
            {
                bool up = false, left = false, down = false, right = false;

                if (Vector3.Distance(mouseCursor.transform.position, transform.position) >= 0.5f)
                {
                    if (Mathf.Abs(mouseDirection.y) >= Mathf.Abs(mouseDirection.x))
                    {
                        if (mouseDirection.y > 0)
                            up = true;
                        else if (mouseDirection.y < 0)
                            down = true;
                    }
                    else if (mouseDirection.x > 0)
                        right = true;
                    else if (mouseDirection.x < 0)
                        left = true;
                }

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
    }

    void AnimBlobKeyboard()
    {
        if (!mouseMoving)
        {
            if (Time_Lord.Acting && !Time_Lord.Transitioning && !dead && !cannotMove)
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
    }

    public void Kill ()
    {
        if (Time_Lord.Acting || Time_Lord.Preparing)
        {
            nbDeath++;

            shake = true;
            shakeTimer = 0;

            ejectTrail();
            generateDeathStain();

            dead = true;
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<AudioSource>().Play();

            if (!timeRuler)
                Time_Lord.timebender = Mathf.Clamp(Time_Lord.timebender + 0.025f, 1f, 1.5f);

            PlayerPrefs.SetFloat("TimeBender", Time_Lord.timebender);

            VFX.Play("VFXDeath_Die", -1, 0f);
            CharaAnim.Play("Blob_Death", -1, 0f);
        }
    }

    private void Mute()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            foreach (AudioSource toMute in sourcesToMute)
            {
                toMute.mute = !toMute.mute;
            }
        }
    }

    private void shakyCam ()
    {
        if (shake)
        {
            shakeTimer += Time.deltaTime;
            cameraShake.position = cameraIni + new Vector3(Random.value * camShakeCor, Random.value * camShakeCor, Random.value * camShakeCor);
            if (shakeTimer >= shakeTime)
                shake = false;

            if (myPostProcess != null)
            {
                myPostProcess.profile.TryGetSettings(out myBlur);
                myBlur.focalLength.value = Mathf.Lerp(1f, 50f, -Mathf.Abs(2 * ((shakeTimer / shakeTime) - 0.5f)) + 1);
            }
        }
    }

    public void Reset_Character()
    {
        if (!dead && !cannotMove)
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
        if (Ready)
        {
            if ((Time_Lord.Preparing && Time_Lord.The_Timer >= 0.5f) || The_Level_Manager.Safe_Level[Level_Manager.Current_Level])
            {
                soundEnter.PlayDelayed(0.2f);

                transform.rotation = Quaternion.identity;

                CharaAnim.Play("Blob_Enter", -1, 0f);
                Ready = false;
                //dead = false;
            }
        }
    }

    public void setUndead()
    {
        dead = false;
        if (currentTrail == null)
            generateTrail();
    }

    //Called by animation
    public void generateTrail ()
    {
        currentTrail = Instantiate(trailRef, this.transform);
    }

    public void generateDeathStain()
    {
        GameObject newStain = Instantiate(deathStain, deathStains.transform);
        newStain.transform.position = this.transform.position;
        newStain.transform.Rotate(0, 0, Random.value * 360);
    }

    public void washStains()
    {
        GrowStain[] theStains = deathStains.GetComponentsInChildren<GrowStain>();

        foreach (GrowStain stain in theStains)
        {
            Destroy(stain.gameObject);
        }
    }

    public void ejectTrail ()
    {
        if (currentTrail != null)
        {
            currentTrail.transform.SetParent(null, true);
            currentTrail.GetComponent<TrailRenderer>().autodestruct = true;
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
        cameraIni = Camera.main.transform.position;
        if (currentTrail == null)
            generateTrail();

        if (!theTimeLord.IntroTime_Lord)
        {
            CharaAnim.Play("Blob_Enter");
            //soundEnter.PlayDelayed(0.2f);
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        mouseSensitivity = PlayerPrefs.GetFloat("Sensitivity");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MouseCursor();
        showCursor();

        keyboardMove();
        mouseMove();

        RotateBlobMouse();
        RotateBlobKeyboard();
        //AnimBlobMouse();
        //AnimBlobKeyboard();

        Undead();
        moveToGutter();
        shakyCam();

        Mute();
    }
}