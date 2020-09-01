using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinManager : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (DoggoController.win)
        {
            _anim.SetTrigger($"Win");
        }
    }
}
