using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public GameObject _target_object = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_target_object == null)
        {
            Destroy(gameObject);
            return;
        }

        SetRotationEnemy();
        SetPositionEnemy();
    }


    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("当たった!");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("BulletController 当たった! OnTriggerEnter");
        if (_target_object == other.gameObject)
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


    void SetRotationEnemy()
    {
        if(_target_object == null)
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
        gameObject.transform.rotation = Quaternion.Euler(0, player_angle, 0);

    }


    void SetPositionEnemy()
    {
        if (_target_object == null)
        {
            return;
        }

        Vector2 enemy_pos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
        Vector2 player_pos = new Vector2(_target_object.transform.position.x, _target_object.transform.position.z);

        if (Vector3.Distance(player_pos, enemy_pos) <= 50)
        {
            //return;
        }



        float player_angle = GetAngle(enemy_pos, player_pos);
        gameObject.transform.position += gameObject.transform.forward * 0.1f;

    }
}
