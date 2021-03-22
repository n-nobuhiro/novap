using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AttackMissile()
    {
        SpawnManager.Instance.SetMissile();
    }

    public void SetBoost()
    {
        print("SetBoost");
        UIManager.Instance._is_boost = true;
    }


    public void DisableBoost()
    {

        print("DisableBoost");
        UIManager.Instance._is_boost = false;
    }

}
