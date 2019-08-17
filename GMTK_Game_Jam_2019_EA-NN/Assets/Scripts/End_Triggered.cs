using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class End_Triggered : MonoBehaviour
{
    public SpriteRenderer shineBright;
    public CinemachineVirtualCamera myCamera;
    public Character_Move myCharacter;
    public GameObject mouseImage;

    private bool ending, onlyOnce;

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
            Debug.Log("That's it! I quit!");
            Application.Quit();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ending)
            EndGame();
    }
}
