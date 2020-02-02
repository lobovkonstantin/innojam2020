using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextBoxBehaviour : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EventSystem.current.SetSelectedGameObject(GetComponent<InputField>().gameObject, null);
        GetComponent<InputField>().OnPointerClick(new PointerEventData(EventSystem.current));

        validateAnswer();
    }

    void validateAnswer()
    {
         if (GetComponent<InputField>().isFocused && GetComponent<InputField>().text != "" && Input.GetKey(KeyCode.Return))
         {
             String userAnswer = GetComponent<InputField>().text;
             if (WorldVariablesHandler.Instance.nameList.Contains(userAnswer))
             {
                 Debug.Log("Item has been repaired!");


                 WorldVariablesHandler.Instance.itemDictionary[userAnswer].DestroyObject();

                 WorldVariablesHandler.Instance.itemDictionary.Remove(userAnswer);

                 WorldVariablesHandler.Instance.nameList.Remove(userAnswer);

             }
             else
             {
                 Debug.Log("NOOOOOOOB!!!!!!!!!!!!! MEOOOOOOOOOOW!!!!!!!!!");
             }
             GetComponent<InputField>().text = "";
         }
    }
}
