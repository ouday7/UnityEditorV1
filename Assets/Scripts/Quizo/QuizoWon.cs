using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QuizoWon : MonoBehaviour
{
    [SerializeField] private Button ClosePopUp;
    
    public static GameObject lastTemplate;

    private void Start()
    {
        ClosePopUp.onClick.AddListener(()=>
        {
            this.gameObject.SetActive(false);
            lastTemplate.SetActive(true);
        });
    }
    
}
