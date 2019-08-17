using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rename_Objects : MonoBehaviour
{
    public bool renameObjects;

    private Transform[] allGameObjects;

    void RenameMyObjects()
    {
        foreach (Transform anObject in allGameObjects)
        {
            int nbLevels = 0, nbExits = 0, nbEntries = 0;

            if (anObject.CompareTag("Level"))
            {
                anObject.name = "Level (" + nbLevels + ")";
                nbLevels++;
            }

            if(anObject.CompareTag("Exit"))
            {
                anObject.name = "Exit (" + nbExits + ")";
                nbLevels++;
            }

            if(anObject.CompareTag("Entry"))
            {
                anObject.name = "Entry (" + nbEntries + ")";
                nbLevels++;
            }
        }

        renameObjects = false;
    }

    private void Start()
    {
        allGameObjects = GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (renameObjects)
        {
            RenameMyObjects();
        }
    }
}
