using System.Linq;
using EditorMenu;
using UnityEngine;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class ExercisesHolder : EntryPointSystemBase
    {
        public static ExercisesHolder instance;//todo: remove singletons with parents

        [SerializeField] private GameObject exerciseHolder;
        [SerializeField] private Button exitConfigBtn;
        public override void Begin()
        {
            if (instance != null) return;
            instance = this;

            exitConfigBtn.onClick.AddListener(OnExitSubmitChapter);
            var selectChapterId = GameDataManager.instance.GetSelectedChapter().id;
            var exercises = GameDataManager.instance.Data.exercises
                .Where(ex => ex.chapterId == selectChapterId).ToList();

            foreach (var ex in exercises)
            {
                var newExBtn = PoolSystem.instance.Spawn<ExerciseBtn>(ObjectToPoolType.Exercise);
                newExBtn.Transform.SetParent(exerciseHolder.transform);
                newExBtn.Transform.localScale = Vector3.one;
                newExBtn.Initialize();
                newExBtn.BindData(ex);

                if (ex.questions.Count == 0) continue;

                foreach (var qst in ex.questions)
                {
                    var newQstBtn = PoolSystem.instance.Spawn<QuestionBtn>(ObjectToPoolType.Question);
                    newQstBtn.Initialize(newExBtn);
                    newQstBtn.BindData(qst);
                    newExBtn.AddQuestionChild(newQstBtn, false);
                    newQstBtn.UpdateName();
                }
            }

            MenuController.instance.UpdateExercisesHolderSize(exerciseHolder.transform.childCount);
            MenuController.instance.UpdateLayout();
        }

        private void OnExitSubmitChapter()
        {
            SceneHandler.instance.LoadScene(SceneNames.GameEditor);
        }
    }
}
