﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public GameObject _target_object = null;

    public SpawnManager _spawn_manager = null;

    public GameObject _start_pos_object = null;

    public bool _is_missile = false;

    public bool _is_player_bullet = true;


    int curve_direction = 0;

    // Start is called before the first frame update
    void Start()
    {
        curve_direction = Random.Range(0, 5);
    }

    float _destroy_countdown = 0;

    // Update is called once per frame
    void Update()
    {
        if(_target_object == null 
            && _is_missile == false 
            && _is_player_bullet == false)
        {
            Destroy(gameObject);
            return;
        }

        SetBulletRotation();
        SetBulletPosition();

        bool is_destroy = false;

        //if(Vector3.Distance(gameObject.transform.position, _target_object.transform.position) <= 10f)
        {
            _destroy_countdown += Time.deltaTime;
        }


        if(_destroy_countdown > 10)
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

        //if (_target_object == other.gameObject)
        if (other.gameObject.GetComponent<EnemyController>() != null && _is_player_bullet == true)
        {

            Debug.Log("BulletController 当たった! OnTriggerEnter");

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
        if (_target_object == null
            || _start_pos_object == null 
            || _is_reach_target_pos == true)
        {
            return;
        }

        Vector2 enemy_pos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
        Vector2 player_pos = new Vector2(_target_object.transform.position.x, _target_object.transform.position.z);

        if (Vector3.Distance(player_pos, enemy_pos) <= 3)
        {
            return;
        }

        float player_angle = GetAngle(enemy_pos, player_pos);

        Quaternion target_rotation = _start_pos_object.transform.rotation;

        if (_is_missile)
        {
            target_rotation = Quaternion.Euler(90, player_angle, 0);

        }

        gameObject.transform.rotation = target_rotation;


    }


    float rate = 0;

    bool _is_reach_target_pos = false;

    void SetBulletPosition()
    {
        if (_target_object == null)
        {
            //return;
        }

        if (_is_missile)
        {

            if (Vector3.Distance(gameObject.transform.position, _target_object.transform.position) <= 0.1f)
            {
                _is_reach_target_pos = true;
            }

             
            if(_is_reach_target_pos) {
                // 目標を通り過ぎあとは追尾せずに直進だけさせる.
                gameObject.transform.position += gameObject.transform.up;
                return;
            }

            rate += 0.01f;
            Vector3 StartPos = _start_pos_object.transform.position;
            Vector3 EndPos = _target_object.transform.position;
            float easingValueX = Mathf.Lerp(StartPos.x, EndPos.x, rate);
            float easingValueY = Mathf.Lerp(StartPos.y, EndPos.y, rate);
            float easingValueZ = Mathf.Lerp(StartPos.z, EndPos.z, rate);

            gameObject.transform.position = new Vector3(easingValueX, easingValueY, easingValueZ) + AddCureve(rate);
            
        }
        else
        {
            gameObject.transform.position += gameObject.transform.forward * 1.1f;
        }
    }


    Vector3 AddCureve(float rate)
    {

        Vector3 add_rueve = new Vector3(0, 0, 0);

        if (Vector3.Distance(gameObject.transform.position, _target_object.transform.position) <= 0.1f)
        {
            return add_rueve;
        }

        float curve_value = 10 * Mathf.Sin(rate * 3.14f);
        switch (curve_direction)
        {
            case 0:
                add_rueve = new Vector3(0, curve_value, 0);
                break;
            case 1:
                add_rueve = new Vector3(curve_value, curve_value, 0);
                break;
            case 2:
                add_rueve = new Vector3(-curve_value, curve_value, 0);
                break;
            case 3:
                add_rueve = new Vector3(curve_value, 0, 0);
                break;
            case 4:
                add_rueve = new Vector3(-curve_value, 0, 0);
                break;
            default:
                break;

        }

        return add_rueve;
        
    }
}
