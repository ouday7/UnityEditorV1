using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Templates
{
    public class ChoiceButton : MonoBehaviour
    {
        [BoxGroup("Unselected ")][LabelText("Color")] [SerializeField] private Color unselectedColor;
        [BoxGroup("Selected ")][LabelText("Color")] [SerializeField] private Color selectedColor;
        
        private ChoiceButton lastChoise;

        public void OnClickChoiceButton()
        {
            if (lastChoise != null)
            {
                lastChoise.Unselect();
                Select();
            }
                Select();
                lastChoise = this;
        }
        private void Unselect()
        {
            transform.GetComponent<Image>().DOColor(unselectedColor,0.1f);
            transform.DOScale(1, 0.1f);
            transform.GetComponent<Button>().interactable = true;
        }
        private void Select()
        {
            transform.DOScale(0.6f, 0.25f).OnComplete(()=>
            {
                transform.DOScale(1.2f, 0.25f);
                transform.GetComponent<Image>().DOColor(selectedColor,0.1f);
                transform.GetComponent<Button>().interactable = false;
            });
        }
    }
}
