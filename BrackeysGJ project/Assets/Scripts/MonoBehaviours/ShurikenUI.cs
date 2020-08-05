using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShurikenUI : MonoBehaviour
{
    /// <summary>
    /// All shuriken UI Images. [0] is the first shuriken in the Ammo bar.
    /// </summary>
    [SerializeField] private Image[] shurikenImages;
    [SerializeField] private Sprite shurikenFullSprite;
    [SerializeField] private Sprite shurikenEmptySprite;
    private ShurikenThrow shurikenThrow;

    /// <summary>
    /// How many Shuriken the player currently has.
    /// </summary>
    private int shurikenLeft;
    private int previousShurikenLeft;

    /// <summary>
    /// The maximum number of Shuriken that the player can hold.
    /// </summary>
    private int maxShurikenAllowed;
    //private int currCount = 0;
    void Start()
    {
        shurikenThrow = GameObject.FindObjectOfType<ShurikenThrow>();

        previousShurikenLeft = 0;
        shurikenLeft = shurikenThrow.GetAmmoLeft();
        maxShurikenAllowed = shurikenThrow.MaxShuriken;

        DisableExtraShurikenUI();
        UpdateShurikenDisplay();

    }

    // Update is called once per frame
    void Update()
    {
        // Store how many shuriken the player had during the last frame
        previousShurikenLeft = shurikenLeft;

        // Get the current number of shuriken the player has
        shurikenLeft = shurikenThrow.GetAmmoLeft();

        UpdateShurikenDisplay();
    }

    /// <summary>
    /// Turns off shuriken UI that exceeds the maximum the player is allowed to carry.
    /// </summary>
    private void DisableExtraShurikenUI()
    {
        // If the UI has more shuriken images than allowed
        if (shurikenImages.Length > maxShurikenAllowed)
        {
            // Then disable the extra images
            for (int i = 0; i < shurikenImages.Length; i++)
            {
                if (i + 1 > maxShurikenAllowed)
                {
                    shurikenImages[i].gameObject.SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// Updates the Shuriken UI to show the correct number of shuriken left.
    /// </summary>
    /// <param name="shurikenCount"></param>
    private void UpdateShurikenDisplay()
    {
        // If the player gained or lost a shuriken
        if(previousShurikenLeft != shurikenLeft)
        {
            for(int i = 0; i <= maxShurikenAllowed - 1; i++)
            {
                if(i + 1 > shurikenLeft)
                {
                    EmptyShurikenSlot(i);
                }
                else
                {
                    FillShurikenSlot(i);
                }
            }
        }
    }

    /// <summary>
    /// Switches the UI to the shuriken background image
    /// </summary>
    /// <param name="i">Index for the image to change</param>
    private void EmptyShurikenSlot(int i)
    {
        shurikenImages[i].sprite = shurikenEmptySprite;
    }

    /// <summary>
    /// Switches the UI to the shuriken full image
    /// </summary>
    /// <param name="i">Index for the image to change</param>
    private void FillShurikenSlot(int i)
    {
        shurikenImages[i].sprite = shurikenFullSprite;
    }

}
