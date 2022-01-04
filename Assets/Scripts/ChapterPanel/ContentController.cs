
using EditorMenu;
using Envast.Components.GridLayout;
using Envast.Layouts;
using UnityEngine;

namespace ChapterPanel
{
    public class ContentController : MonoBehaviour
    {
        private void Start()
        {
            foreach (var ex in GameDataManager.Instance.Data.exercises )
            {
                if(ex.chapterId!=EditorButtonsManager.instance._selectedChapter.Data.id) continue;
                var newExBtn = PoolSystem.instance.Spawn<ExerciseBtn>(ObjectToPoolType.Exercise);
                newExBtn.Initialize();
                newExBtn.BindData(ex);
                newExBtn.transform.SetParent(MenuController.instance.ExerciseHolder.transform);
                newExBtn.transform.localScale = Vector3.one;
                
                if(ex.questions.Count==0)continue;
                
                foreach (var qst in ex.questions)
                {
                    var newqstBtn = PoolSystem.instance.Spawn<QuestionBtn>(ObjectToPoolType.Question);
                    newqstBtn.Initialize();
                    newqstBtn.UpdateName();
                    newqstBtn.BindData(qst);
                    newExBtn.AddQuestionChild(newqstBtn);
                    newqstBtn.transform.localScale = Vector3.one;
                }
            }
        }

    }
}
