using ChapterPanel;
using UnityEngine;
using UnityEngine.UI;

namespace QuizFields
{
    public class ToggleField : QuizFieldBase
    {
        [SerializeField] public Toggle myToggle;
        public override void Initialize()
        {
            myToggle.onValueChanged.AddListener(ToggleValue);
        }
        private void ToggleValue(bool newValue)
        {
            myToggle.isOn = newValue;
        }

        public void BinData()
        {
            
        }
        public override void BindData(QuizFieldData inData)
        {
            _data = inData;
        }
    }
}
