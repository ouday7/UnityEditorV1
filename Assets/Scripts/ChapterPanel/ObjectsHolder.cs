using System;
using ChapterPanel;
using UnityEngine;


public class ObjectsHolder : EntryPointSystemBase
{
    [SerializeField] private ObjectsHolder holder;
    [SerializeField] private RectTransform quizfieldsHolder;
    private RectTransform _rt;
    private float HeightToAdd = 400;
    public override void Begin()
    {
        SelectTemplateDialog.instance.OnSubmitTemplate += MaximiseHolderHeight;
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
        var holderSize = holder.RectTransform.sizeDelta;
        holder.RectTransform.sizeDelta = new Vector2(holderSize.x, holderSize.y + HeightToAdd);
    }
}
