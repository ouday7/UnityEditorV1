using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizLost : MonoBehaviour
{
    [SerializeField] private Button ClosePopUp;
    [SerializeField] public static GameObject lastTemplate;

    private void Start()
    {
        ClosePopUp.onClick.AddListener(()=>
        {
            this.gameObject.SetActive(false);
            lastTemplate.SetActive(true);
        });
    }
}
