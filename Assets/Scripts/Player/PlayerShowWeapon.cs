using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShowWeapon : MonoBehaviour
{
    [SerializeField]
    public GameObject item1;
    [SerializeField]
    public GameObject item2;
    public bool showItem1;
    public bool showItem2;

    // Start is called before the first frame update
    void Start()
    {
        showItem1 = true;
        showItem2 = false;
    }

    // Update is called once per frame
    void Update()
    {
        //The shown item will be the active item
        if (showItem1 == false)
        {
            item1.SetActive(false);
        }
        if (showItem1 == true)
        {
            item1.SetActive(true);
        }
        if (showItem2 == false)
        {
            item2.SetActive(false);
        }
        if (showItem2 == true)
        {
            item2.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && showItem1 == false)
        {
            showItem1 = true;
            showItem2 = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && showItem2 == false)
        {
            showItem1 = false;
            showItem2 = true;
        }

    }
}
