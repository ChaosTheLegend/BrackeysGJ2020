using System.Collections;
using System.Collections.Generic;
using BrackeysGJ.ClassFiles;
using UnityEngine;
using UnityEngine.UI;

public class LoadChecker : MonoBehaviour
{
    private Button bt;
    // Start is called before the first frame update
    void Start()
    {
        bt = GetComponent<Button>();
        if (SaveSystem.Load() == null) bt.interactable = false;
    }
}
