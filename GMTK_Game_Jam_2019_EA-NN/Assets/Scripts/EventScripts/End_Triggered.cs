using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class End_Triggered : MonoBehaviour
{
    public SpriteRenderer shineBright;
    public CinemachineVirtualCamera myCamera;
    public Character_Move myCharacter;
    public GameObject mouseImage;
    public GameObject endingScoreboard;

    public Level_Manager theLevelManager;
    public Character_Move theCharacterMove;

    public TextMeshProUGUI deathTotal, endType;

    private bool ending, onlyOnce, canInputEnd;

    private void OnTriggerExit2D(Collider2D collision)
    {
        ending = true;
    }

    void EndGame()
    {
        if (!onlyOnce)
        {
            onlyOnce = true;
            myCamera.Follow = null;
            myCharacter.GetComponent<Rigidbody2D>().drag = 0;
            myCharacter.dead = true;
            mouseImage.SetActive(false);
        }

        if (shineBright.color.a < 1f)
        {
            Color myColor = shineBright.color;
            myColor.a += Time.deltaTime / 2;

            shineBright.color = myColor;
        }
        else
        {
            deathTotal.GetComponent<TextMeshProUGUI>().text = theCharacterMove.nbDeath.ToString();

            if (theCharacterMove.nbDeath > 10)
            {
                endType.GetComponent<TextMeshProUGUI>().text = "1/2 NORMAL";
            }
            else
            {
                endType.GetComponent<TextMeshProUGUI>().text = "2/2 SPECIAL";
            }

            endingScoreboard.SetActive(true);
            canInputEnd = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ending)
        {
            EndGame();
        }

        if (canInputEnd)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                theLevelManager.DeleteSave();
                theLevelManager.resetStatic();
                SceneManager.LoadScene(0);
            }
        }
    }
}