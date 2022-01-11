using System;
using ChapterPanel;
using UnityEngine;


public class ObjectsHolder : EntryPointSystemBase
{
    [SerializeField] private ObjectsHolder holder;
    [SerializeField] private RectTransform quizfieldsHolder;
    private RectTransform _rt;
    private float HeightToAdd = 150;
    private Vector2 holderSize;
    public override void Begin()
    {
        holderSize = holder.RectTransform.sizeDelta;
        SelectTemplateDialog.instance.OnSubmitTemplate += MaximiseHolderHeight;
        QuestionBtn.OnClickQuestion += MaximiseHolderQuestionClick;
    }
    private RectTransform RectTransform
    {
        get
        {
            if (_rt == null) _rt = GetComponent<RectTransform>();
            return _rt;
        }
    }
    private void MaximiseHolderHeight(TemplateData inTemplate)
    {
        var quizfieldChilds = quizfieldsHolder.childCount;
        if(quizfieldChilds==0)
        {
            holder.RectTransform.sizeDelta=holderSize;
            return;
        }
        holder.RectTransform.sizeDelta = new Vector2(holderSize.x, holderSize.y +(quizfieldChilds* HeightToAdd));
    }
    private void MaximiseHolderQuestionClick(QuestionBtn btn)
    {
        var quizfieldChilds = quizfieldsHolder.childCount;
        
        if(quizfieldChilds==0)
        {
          holder.RectTransform.sizeDelta=holderSize;
          return;
        }
        holder.RectTransform.sizeDelta = new Vector2(holderSize.x, holderSize.y +(quizfieldChilds* HeightToAdd));
    } 
}
