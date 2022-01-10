using System.Collections;
using System.Collections.Generic;
using ChapterPanel;
using UnityEngine;

public class SubmitController : MonoBehaviour
{
    [SerializeField] private EditManager ed;
    [SerializeField] private ContentController cc;
    [SerializeField] private MenuController mc;
    [SerializeField]private GameDataManager gm;
    [SerializeField]private TemplatesHandler  th;
    [SerializeField] private SelectTemplateDialog td;
    [SerializeField] private QuizFieldsHandler qh;
    [SerializeField] private SelectTemplateButton st;

    private void Awake()
    {
        mc.Begin();
        cc.Begin();
        ed.Begin();
        gm.Begin();
        th.Begin();
        st.Begin();
        td.Begin();
        qh.Begin();

    }
}
