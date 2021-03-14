using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public GameObject _player_object = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        SetRotationEnemy();
        SetPositionEnemy();
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
        Vector2 enemy_pos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
        Vector2 player_pos = new Vector2(_player_object.transform.position.x, _player_object.transform.position.z);

        if(Vector3.Distance(player_pos, enemy_pos) <= 60)
        {
            return;
        }

        float player_angle = GetAngle(enemy_pos, player_pos);
        gameObject.transform.rotation = Quaternion.Euler(0, player_angle, 0);

    }


    void SetPositionEnemy()
    {
        Vector2 enemy_pos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
        Vector2 player_pos = new Vector2(_player_object.transform.position.x, _player_object.transform.position.z);

        if (Vector3.Distance(player_pos, enemy_pos) <= 50)
        {
            return;
        }



        float player_angle = GetAngle(enemy_pos, player_pos);
        gameObject.transform.position += gameObject.transform.forward * 0.1f;

    }

}
