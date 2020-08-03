using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textField;

    private ShurikenThrow ammoScript;
    // Start is called before the first frame update
    void Start()
    {
        textField = GetComponent<TextMeshProUGUI>();
        ammoScript = GameObject.FindObjectOfType<ShurikenThrow>();
    }

    // Update is called once per frame
    void Update()
    {
        textField.text = "Ammo: " + ammoScript.GetAmmoLeft().ToString();
    }
}
