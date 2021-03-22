using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {

        //if (_target_object == other.gameObject)
        if (other.gameObject.GetComponent<BulletController>() != null 
            && other.gameObject.GetComponent<BulletController>()._is_player_bullet == false)
        {

            Debug.Log("Friend Controller 当たった! OnTriggerEnter");

            UIManager.Instance.SetHPbarValue(30);
        }
    }


}
