using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShurikenUI : MonoBehaviour
{
    [SerializeField] private Image[] shurikens;
    [SerializeField] private Sprite shurikenSprite;
    [SerializeField] private Sprite shurikenBGSprite;
    private ShurikenThrow shurikenThrow;
    private int shurikenCount;
    private int currCount = 0;
    void Start()
    {
        shurikenThrow = GameObject.FindObjectOfType<ShurikenThrow>();
        shurikenCount = shurikenThrow.GetAmmoLeft();
        DisplayAvailableShurikens(shurikenCount);
        foreach (var shuriken in shurikens) {shuriken.sprite = shurikenSprite;}
    }

    // Update is called once per frame
    void Update()
    {
        // Get ShurikenCount
        shurikenCount = shurikenThrow.GetAmmoLeft();
        
        // If shurikenCount has changed from previous frame, update UI;
        if((currCount != shurikenCount) && (shurikenCount < shurikens.Length)) DisplayAvailableShurikens(shurikenCount);
    }

    void DisplayAvailableShurikens(int shurikenCount)
    {
        // If ShurikenCount has increased 
        if (currCount < shurikenCount)
        {
            while(currCount < shurikenCount)
            {
                shurikens[currCount++].sprite = shurikenSprite;
            }   
        }

        // If shuriken Count has decreased
        if (currCount > shurikenCount)
        {
            while(currCount >= shurikenCount)
            {
                shurikens[currCount--].sprite = shurikenBGSprite;
            }

            if (currCount < 0) currCount = 0;
        }
        
    }
}
