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

	 void PanGestureCallback(DigitalRubyShared.GestureRecognizer gesture)
     {
            if (gesture.State == GestureRecognizerState.Executing)
            {
                //DebugText("Panned, Location: {0}, {1}, Delta: {2}, {3}", gesture.FocusX, gesture.FocusY, gesture.DeltaX, gesture.DeltaY);

                float deltaX = panGesture.DeltaX / 25.0f;
                float deltaY = panGesture.DeltaY / 25.0f;
                //Vector3 pos = Earth.transform.position;
                //pos.x += deltaX;
                //pos.y += deltaY;
              // Earth.transform.position = pos;

			  //SpawnManager.Instance.player_object.transform.position = SpawnManager.Instance.player_object.transform.position + new Vector3(deltaX,0, deltaX);
			  _spawn_manager.player_object.transform.position = _spawn_manager.player_object.transform.position + new Vector3(deltaX,0, deltaX);
			  
            }
     }

	         private void TapGestureCallback(DigitalRubyShared.GestureRecognizer gesture)
        {
            if (gesture.State == GestureRecognizerState.Ended)
            {
               // DebugText("Tapped at {0}, {1}", gesture.FocusX, gesture.FocusY);
               // CreateAsteroid(gesture.FocusX, gesture.FocusY);
            }
        }

	 private void DebugText(string text, params object[] format)
    {
            //bottomLabel.text = string.Format(text, format);
            Debug.Log(string.Format(text, format));
     }
}
