using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;

// TapGestureRecognizer用の宣言
using DigitalRubyShared;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationY = 0F;


    [SerializeField]
    SpawnManager _spawn_manager = null;

    [SerializeField]
    GameObject[] _ui_canvas_list;


    [SerializeField]
    Camera _main_camera;

    [SerializeField]
    GameObject _reticle_cross = null;

    [SerializeField]
    TextMeshProUGUI _score_text = null;

    public bool _is_boost = false;


    [SerializeField]
    Image _player_hp_ui;

    [SerializeField]
    public TextMeshProUGUI _missile_num_ui = null;


    public float _player_hp = -300;


    bool is_move = true;

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
        _spawn_manager._player_object = _main_camera.gameObject;

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


        PinchInit();
    }



    // Update is called once per frame
    void Update()
    {
        if (_spawn_manager._enemy_dead_num >= 10)
        {
            GameClear();
        }


        if (is_move == true)
        {
            //SetMainCameraRotationByMouse();

            float move_add = 0.05f;

            if(_is_boost)
            {
                move_add = 0.5f;
            }

            //float current_pos = _main_camera.transform.position;
            _main_camera.transform.position += new Vector3(0, 0, move_add) ;

            _score_text.text = "Score " + _spawn_manager._enemy_dead_num;
        }


        //PinchZoom();
    }


    void SetMainCameraRotationByMouse()
    {
        float rotationX = _main_camera.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

        _main_camera.transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
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
            is_move = true;
            begin_pos = new Vector2(gesture.FocusX, gesture.FocusY);
        }
        else if (gesture.State == GestureRecognizerState.Executing)
        {
            //DebugText("Panned, Location: {0}, {1}, Delta: {2}, {3}", gesture.FocusX, gesture.FocusY, gesture.DeltaX, gesture.DeltaY);

            float deltaX = panGesture.DeltaX / 25.0f;
            float deltaY = panGesture.DeltaY / 25.0f;
            //Vector3 pos = Earth.transform.position;
            //pos.x += deltaX;
            //pos.y += deltaY;
            // Earth.transform.position = pos;

            current_pos = new Vector2(gesture.FocusX, gesture.FocusY);

            Vector2 pos_delta = current_pos - begin_pos;

            GameObject player_object = _spawn_manager._player_object;



            // 照準用の＋画像を非表示する.
            //_reticle_cross.SetActive(false);

            // プレイヤーを移動させる
            //player_object.transform.position += new Vector3(pos_delta.x, 0, pos_delta.y) /3000;
            //_main_camera.transform.position = _spawn_manager._player_object.transform.position + new Vector3(0, 1.5f, -3);

            _main_camera.transform.position += new Vector3(pos_delta.x, pos_delta.y, 0) / 200;
            //_main_camera.transform.rotation = Quaternion.Euler(0, 0, 0);

            float swipe_angle = _spawn_manager.GetAngle(begin_pos, current_pos);

            // player_object.transform.rotation = Quaternion.Euler(0, swipe_angle, 0);

            // 歩きモーション
            //_spawn_manager._player_animator.SetBool("is_walk", true);
        }
        else if (gesture.State == GestureRecognizerState.Ended)
        {
            //_spawn_manager._player_animator.SetBool("is_walk", false);
        }
    }



    int anim_index = 0;

    private void TapGestureCallback(DigitalRubyShared.GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {

            if (is_move == true)
            {
                _main_camera.transform.rotation = Quaternion.Euler(0, 0, 0);

            }

            _spawn_manager.SetGunBullet();


            //_main_camera.transform.position = _spawn_manager._player_forward.transform.position;
            //_main_camera.transform.rotation = euler_angle_player;


            //StartCoroutine(delayAnimation(0.5f));

            //DebugText("Tapped at {0}, {1}", gesture.FocusX, gesture.FocusY);
            // CreateAsteroid(gesture.FocusX, gesture.FocusY);
        }
    }



    void GameClear()
    {

        if (_ui_canvas_list[2].activeSelf == false)
        {
            _ui_canvas_list[2].SetActive(true);

            return;
        }

    }




    IEnumerator delayAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);

        //_spawn_manager._player_animator.SetBool("stand_fire", false);
        //_spawn_manager._enemy_animator.SetBool("is_damage", false);
    }


    Quaternion SetPlayerAutoAimForEnemy(GameObject target_object)
    {
        if (target_object == null)
        {
            return _spawn_manager._player_object.transform.rotation;
        }

        Vector3 temp_pos = target_object.transform.position;
        Vector2 player_pos = new Vector2(temp_pos.x, temp_pos.z);


        temp_pos = _spawn_manager._partner_object.transform.position;
        Vector2 enemy_pos = new Vector2(temp_pos.x, temp_pos.z);

        float target_angle = _spawn_manager.GetAngle(enemy_pos, player_pos);

        _spawn_manager._player_object.transform.rotation = Quaternion.Euler(0, target_angle, 0);

        return _spawn_manager._player_object.transform.rotation;
    }


    private void DebugText(string text, params object[] format)
    {
        //bottomLabel.text = string.Format(text, format);
        Debug.Log(string.Format(text, format));
    }


    void SetAttackMotion()
    {
        if (_spawn_manager._player_animator == null)
        {
            return;
        }
        _spawn_manager._player_animator.SetInteger("anim_index", anim_index);

        bool stand_fire = _spawn_manager._player_animator.GetBool("stand_fire");
        _spawn_manager._player_animator.SetBool("stand_fire", true);

        anim_index++;

        if (anim_index >= 3)
        {
            anim_index = 0;
        }


        _spawn_manager._enemy_animator.SetBool("is_damage", true);

    }


    float wid, hei, diag;//スクリーンサイズ
    float tx, ty; //変数

    float vMin = 0.01f, vMax = 4.0f; // 倍率制限
    float sDist = 0.01f, nDist = 0.0f;// 距離変数
    Vector3 initScale;//最初の大きさ
    float v = 1.0f;//現在の倍率


    Vector3 _initialize_camera_position;

    void PinchInit()
    {
        wid = Screen.width;
        hei = Screen.height;
        diag = Mathf.Sqrt(Mathf.Pow(wid, 2) + Mathf.Pow(hei, 2));
        _initialize_camera_position = _main_camera.transform.position;
    }


    void PinchZoom()
    {
        if(Input.touchCount >=2)
        {
            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);

            if(t2.phase == UnityEngine.TouchPhase.Began)
            {
                sDist = Vector2.Distance(t1.position, t2.position);
            }
            else if ((t1.phase == UnityEngine.TouchPhase.Moved || t1.phase == UnityEngine.TouchPhase.Stationary) &&
                     (t2.phase == UnityEngine.TouchPhase.Moved || t2.phase == UnityEngine.TouchPhase.Stationary)
                )
            {
                nDist = Vector2.Distance(t1.position, t2.position);
                v = v + (nDist - sDist) / diag;
                sDist = nDist;
                if (v > vMax) v = vMax;
                if (v < vMin) v = vMin;

                _main_camera.transform.position = _initialize_camera_position / v;
                _main_camera.fieldOfView = v;
            }
        }
    }


    public void SetHPbarValue(float player_damage)
    {
        
        RectTransform rt = _player_hp_ui.gameObject.GetComponent(typeof(RectTransform)) as RectTransform;
        rt.sizeDelta = new Vector2(rt.sizeDelta.x - player_damage, rt.sizeDelta.y);

    }



    public void SetLockOn()
    {

        float lockon_x = _spawn_manager.GetTargetEnemy().transform.position.x;

        float lockon_y = _spawn_manager.GetTargetEnemy().transform.position.y;

        _main_camera.transform.position = new Vector3(lockon_x, lockon_y, _main_camera.transform.position.z);
    }
}

