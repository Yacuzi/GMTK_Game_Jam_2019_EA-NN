using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Rename_Objects : MonoBehaviour
{
    public bool renameObjects;

    private Transform[] allGameObjects;

    void RenameMyObjects()
    {
        allGameObjects = GetComponentsInChildren<Transform>();
        int nbLevels = 1, nbExits = 1, nbEntries = 1;

        foreach (Transform anObject in allGameObjects)
        {
            if (anObject.CompareTag("Level"))
            {
                anObject.name = "Level (" + nbLevels + ")";
                nbLevels++;
            }

            if(anObject.CompareTag("Exit"))
            {
                anObject.name = "Exit (" + nbExits + ")";
                nbExits++;
            }

            if(anObject.CompareTag("Entry"))
            {
                anObject.name = "Entry (" + nbEntries + ")";
                nbEntries++;
            }
        }

        renameObjects = false;
    }

    private void Start()
    {
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
