using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxBehaviour : MonoBehaviour
{
    InputField inputFieldField;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void validateAnswer()
    {
        if (Input.GetKey(KeyCode.KeypadEnter))
        {
            String userAnswer = inputFieldField.text;
            if (WorldVariablesHandler.Instance.nameList.Contains(userAnswer))
            {
                Debug.Log("Item has been repaired!");
            }
        }
    }
}
