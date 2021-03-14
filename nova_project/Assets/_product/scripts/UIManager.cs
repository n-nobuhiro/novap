using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// TapGestureRecognizer用の宣言
using DigitalRubyShared;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    SpawnManager _spawn_manager = null;

    private TapGestureRecognizer tapGesture;
    private TapGestureRecognizer doubleTapGesture;
    private TapGestureRecognizer tripleTapGesture;
    private SwipeGestureRecognizer swipeGesture;
    private PanGestureRecognizer panGesture;
    private ScaleGestureRecognizer scaleGesture;
    private RotateGestureRecognizer rotateGesture;
    private LongPressGestureRecognizer longPressGesture;

    // Start is called before the first frame update
    void Start()
    {
        panGesture = new PanGestureRecognizer();
        panGesture.StateUpdated += PanGestureCallback;

        panGesture.MinimumNumberOfTouchesToTrack = 1;
        FingersScript.Instance.AddGesture(panGesture);

        CreateTapGesture();
        // pan, scale and rotate can all happen simultaneously
        //panGesture.AllowSimultaneousExecution(rotateGesture);


        // pan, scale and rotate can all happen simultaneously
        // panGesture.AllowSimultaneousExecution(scaleGesture);
        //panGesture.AllowSimultaneousExecution(rotateGesture);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateTapGesture()
    {
        tapGesture = new TapGestureRecognizer();
        tapGesture.StateUpdated += TapGestureCallback;
        tapGesture.RequireGestureRecognizerToFail = doubleTapGesture;


        tapGesture.AllowSimultaneousExecution(panGesture);

        FingersScript.Instance.AddGesture(tapGesture);
    }


    Vector2 begin_pos;

    Vector2 current_pos;

    //スワイプ操作
    void PanGestureCallback(DigitalRubyShared.GestureRecognizer gesture)
    {

        if (gesture.State == GestureRecognizerState.Began)
        {

            begin_pos = new Vector2(gesture.FocusX, gesture.FocusY);
        } else if (gesture.State == GestureRecognizerState.Executing)
        {
            //DebugText("Panned, Location: {0}, {1}, Delta: {2}, {3}", gesture.FocusX, gesture.FocusY, gesture.DeltaX, gesture.DeltaY);

            float deltaX = panGesture.DeltaX / 25.0f;
            float deltaY = panGesture.DeltaY / 25.0f;
            //Vector3 pos = Earth.transform.position;
            //pos.x += deltaX;
            //pos.y += deltaY;
            // Earth.transform.position = pos;

            GameObject player_object = _spawn_manager._player_object;

            //SpawnManager.Instance.player_object.transform.position = SpawnManager.Instance.player_object.transform.position + new Vector3(deltaX,0, deltaX);
            player_object.transform.position = player_object.transform.position + new Vector3(deltaX, 0, deltaY);


            current_pos = new Vector2(gesture.FocusX, gesture.FocusY);
            float swipe_angle = _spawn_manager.GetAngle(begin_pos, current_pos);



            player_object.transform.rotation = Quaternion.Euler(0, swipe_angle, 0);

            // 歩きモーション
            _spawn_manager._player_animator.SetBool("is_walk", true);
        }
        else if (gesture.State == GestureRecognizerState.Ended)
        {
            _spawn_manager._player_animator.SetBool("is_walk", false);
        }
    }






    int anim_index = 0;

    private void TapGestureCallback(DigitalRubyShared.GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {

            _spawn_manager._player_animator.SetInteger("anim_index", anim_index);

            bool stand_fire = _spawn_manager._player_animator.GetBool("stand_fire");
            _spawn_manager._player_animator.SetBool("stand_fire", !stand_fire);

            anim_index++;

            if (anim_index >= 3)
            {
                anim_index = 0;
            }

            print("anim_index = " + anim_index);


            // タップして攻撃するときに自動でエネミーの向きにプレイヤーの方向を向ける
            SetPlayerAutoAimForEnemy();

            _spawn_manager._enemy_animator.SetBool("is_damage", true);

            StartCoroutine(delayAnimation(1.5f));

            DebugText("Tapped at {0}, {1}", gesture.FocusX, gesture.FocusY);
            // CreateAsteroid(gesture.FocusX, gesture.FocusY);
        }
    }


    IEnumerator delayAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);

        print(" delay false");

        _spawn_manager._enemy_animator.SetBool("is_damage", false);
    }


    void SetPlayerAutoAimForEnemy()
    {

        Vector3 temp_pos = _spawn_manager._player_object.transform.position;
        Vector2 player_pos = new Vector2(temp_pos.x, temp_pos.z);


        temp_pos = _spawn_manager._partner_object.transform.position;
        Vector2 enemy_pos = new Vector2(temp_pos.x, temp_pos.z);

        float target_angle = _spawn_manager.GetAngle(player_pos, enemy_pos);

        _spawn_manager._player_object.transform.rotation = Quaternion.Euler(0, target_angle, 0);
    }


    private void DebugText(string text, params object[] format)
    {
        //bottomLabel.text = string.Format(text, format);
        Debug.Log(string.Format(text, format));
    }
}
