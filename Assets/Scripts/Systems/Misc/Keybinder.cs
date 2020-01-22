using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keybinder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool a_button = Input.GetKey(KeyCode.JoystickButton0);
        bool b_button = Input.GetKey(KeyCode.JoystickButton1);
        bool x_button = Input.GetKey(KeyCode.JoystickButton2);
        bool y_button = Input.GetKey(KeyCode.JoystickButton3);

        bool start_button = Input.GetKey(KeyCode.JoystickButton7);
        bool back_button = Input.GetKey(KeyCode.JoystickButton6);

        bool lb_button = Input.GetKey(KeyCode.JoystickButton4);
        bool rb_button = Input.GetKey(KeyCode.JoystickButton5);

        bool left_stick_button = Input.GetKey(KeyCode.JoystickButton8);
        bool right_stick_button = Input.GetKey(KeyCode.JoystickButton9);
    }
}
