using System.Linq;
using EditorMenu;
using UnityEngine;

namespace ChapterPanel
{
    public class ContentController : MonoBehaviour
    {
        [SerializeField] private GameObject exerciseHolder;
        private void Start()
        {
            var selectChapterId = GameDataManager.instance.GetSelectedChapter().id;
            var exercises = GameDataManager.instance.Data.exercises
                .Where(ex => ex.chapterId == selectChapterId).ToList();
            
            foreach (var ex in exercises)
            {
                var newExBtn = PoolSystem.instance.Spawn<ExerciseBtn>(ObjectToPoolType.Exercise);
                newExBtn.Initialize();
                newExBtn.BindData(ex);
                newExBtn.transform.SetParent(exerciseHolder.transform);
                newExBtn.transform.localScale = Vector3.one;
                
                if(ex.questions.Count==0)continue;
                
                foreach (var qst in ex.questions)
                {
                    var newqstBtn = PoolSystem.instance.Spawn<QuestionBtn>(ObjectToPoolType.Question);
                    newqstBtn.Initialize(newExBtn);
                    newqstBtn.BindData(qst);
                    newExBtn.AddQuestionChild(newqstBtn);
                    newqstBtn.transform.localScale = Vector3.one;
                    newqstBtn.UpdateName();
                }
            }
        }

    }
}
