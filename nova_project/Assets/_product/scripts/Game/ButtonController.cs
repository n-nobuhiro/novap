using System;
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


    public void AttackGun()
    {
        SpawnManager.Instance.SetGunBullet();
    }


    public void AttackMissile()
    {

        int num = Convert.ToInt32(UIManager.Instance._missile_num_ui.text);

        if (num <= 0)
        {
            return;
        }

        SpawnManager.Instance.SetMissile();

        num = num - 1;

        UIManager.Instance._missile_num_ui.text = Convert.ToString(num);
    }


    public void SetLockOn()
    {

        UIManager.Instance.SetLockOn();
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
