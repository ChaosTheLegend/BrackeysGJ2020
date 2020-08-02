using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenThrow : MonoBehaviour
{
    [SerializeField] private KeyCode shootButton = KeyCode.Mouse0;

    [SerializeField] private GameObject shurikenPrefab;
    [SerializeField] private GameObject shurikenSpawnPoint;
    [SerializeField] private GameObject shurikenSpawnParent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(shootButton))
        {
            Instantiate(shurikenPrefab, shurikenSpawnPoint.transform.position, Quaternion.identity);
        }
    }
}
