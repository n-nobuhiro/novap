using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public GameObject _target_object = null;

    public SpawnManager _spawn_manager = null;

    public GameObject _start_pos_object = null;

    public bool _is_missile = false;

    public bool _is_player_bullet = true;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    float _destroy_countdown = 0;

    // Update is called once per frame
    void Update()
    {
        if(_target_object == null)
        {
            Destroy(gameObject);
            return;
        }

        SetBulletRotation();
        SetBulletPosition();

        bool is_destroy = false;

        if(Vector3.Distance(gameObject.transform.position, _target_object.transform.position) <= 10f)
        {
            _destroy_countdown += Time.deltaTime;
        }


        if(_destroy_countdown > 2)
        {
            is_destroy = true;
        }

        Vector3 camera_pos = _spawn_manager._main_camera.gameObject.transform.position;

        if (Vector3.Distance(gameObject.transform.position, camera_pos) > 1000
            || is_destroy)
        {
            // 自分自身（弾丸）を削除
            Destroy(gameObject);
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("当たった!");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("BulletController 当たった! OnTriggerEnter");

        //if (_target_object == other.gameObject)
        if (other.gameObject.GetComponent<EnemyController>() != null && _is_player_bullet == true)
        {
            // 自分自身（弾丸）を削除
            Destroy(gameObject);
        }
    }




    public float GetAngle(Vector2 p1, Vector2 p2)
    {
        float dx = p2.x - p1.x;
        float dy = p2.y - p1.y;
        float rad = Mathf.Atan2(dx, dy);
        return rad * Mathf.Rad2Deg;
    }


    void SetBulletRotation()
    {
        if (_target_object == null || _start_pos_object == null)
        {
            return;
        }

        Vector2 enemy_pos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
        Vector2 player_pos = new Vector2(_target_object.transform.position.x, _target_object.transform.position.z);

        if (Vector3.Distance(player_pos, enemy_pos) <= 60)
        {
            //return;
        }

        float player_angle = GetAngle(enemy_pos, player_pos);
        gameObject.transform.rotation = _start_pos_object.transform.rotation; //Quaternion.Euler(0, player_angle, 0);

    }


    float rate = 0;


    void SetBulletPosition()
    {
        if (_target_object == null)
        {
            //return;
        }

        if (_is_missile)
        {

            rate += 0.01f;
            Vector3 StartPos = _start_pos_object.transform.position;
            Vector3 EndPos = _target_object.transform.position;
            float easingValueX = Mathf.Lerp(StartPos.x, EndPos.x, rate);
            float easingValueY = Mathf.Lerp(StartPos.y, EndPos.y, rate);

            if (Vector3.Distance(gameObject.transform.position, _target_object.transform.position) >= 0.1f)
            {
                easingValueY += 10 * Mathf.Sin(rate * 3.14f);
            }

            float easingValueZ = Mathf.Lerp(StartPos.z, EndPos.z, rate);

            gameObject.transform.position = new Vector3(easingValueX, easingValueY, easingValueZ);
            
        }
        else
        {
            gameObject.transform.position += gameObject.transform.forward * 1.1f;
        }
    }
}
