﻿using System.Linq;
using EditorMenu;
using UnityEngine;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class ExercisesHolder : EntryPointSystemBase
    {
        public static ExercisesHolder instance;

        [SerializeField] private GameObject exerciseHolder;
        [SerializeField] private Button exitConfigBtn;
        public override void Begin()
        {
            if(instance!=null) return;
             instance = this;
            
            exitConfigBtn.onClick.AddListener(OnExitSubmitChapter);
            var selectChapterId = GameDataManager.instance.GetSelectedChapter().id;
            var exercises = GameDataManager.instance.Data.exercises
                .Where(ex => ex.chapterId == selectChapterId).ToList();
            
            foreach (var ex in exercises)
            {
                var newExBtn = PoolSystem.instance.Spawn<ExerciseBtn>(ObjectToPoolType.Exercise);
                newExBtn.transform.SetParent(exerciseHolder.transform);
                newExBtn.Initialize();
                newExBtn.BindData(ex);
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

            MenuController.instance.UpdateExercisesHolder();
            MenuController.instance.UpdateExercisesHolderSize(exerciseHolder.transform.childCount);
        }

        private void OnExitSubmitChapter()
        {
            SceneHandler.instance.LoadScene(SceneNames.GameEditor);
        }
    }
}