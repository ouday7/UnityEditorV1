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
        /*leftStar.transform.DORotate(new Vector3(0, 0, 300), 0.8f).OnComplete(()=>
        {
            rightStar.transform.DORotate(new Vector3(0, 0, 300), 0.8f).OnComplete(() =>
            {
                middletStar.transform.DORotate(new Vector3(0, 0, 300), 0.8f);
            });

        });*/
        
        ClosePopUp.onClick.AddListener(()=>
        {
            this.gameObject.SetActive(false);
            lastTemplate.SetActive(true);
        });
    }
    
}
