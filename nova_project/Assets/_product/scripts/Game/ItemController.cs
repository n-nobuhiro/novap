using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SerializeField]
    public int missile_add = 3;

    [SerializeField]
    public float hp_add = 60;


    // effect name = "ConfettiBlastBlue"
    [SerializeField]
    public GameObject destroy_effect = null;


    bool _is_dead = false;
            

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
        if(_is_dead)
        {
            return;
        }

        //if (_target_object == other.gameObject)
        if (other.gameObject.GetComponent<BulletController>() != null && other.gameObject.GetComponent<BulletController>()._is_player_bullet == true)
        {
            _is_dead = true;
            
            Debug.Log("ItemController 当たった! OnTriggerEnter");

            if (missile_add > 0)
            {
                int num = Convert.ToInt32(UIManager.Instance._missile_num_ui.text);

                UIManager.Instance._missile_num_ui.text = Convert.ToString(num + missile_add);
            }


            if(hp_add > 0)
            {
                UIManager.Instance.SetHPbarValue(hp_add);
            }

            if (destroy_effect)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                Instantiate(destroy_effect, gameObject.transform);
            }
            
            StartCoroutine(delayDestroy(1.5f));

        }
    }






    IEnumerator delayDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 自分自身を削除
        Destroy(gameObject);
    }


}
