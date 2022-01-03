using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;

namespace ChapterPanel
{
    public class TemplateCategoryBtn : MonoBehaviour
    {
        public delegate void CategoriesEvents(TemplateCategoryBtn btn, TemplateCategory category);
        public static event CategoriesEvents onClick;

        [SerializeField] private Image icon;
        [SerializeField] private RtlText title;
    
        private Button _btn;
        private TemplateCategory _category;
        public TemplateCategory Category => _category;

        private Button button
        {
            get
            {
                if (_btn == null) _btn = GetComponent<Button>();
                return _btn;
            }
        }
        public void Initialize(TemplateCategory category)
        {
            this._category = category;

            this.icon.sprite = this._category.icon;
            this.title.text = this._category.name;
            button.onClick.AddListener(OnButtonClicked);
            Unselect();
        }
        private void OnButtonClicked()
        {
            onClick?.Invoke(this, this._category);
        }

        public void Select()
        {
            button.interactable = false;
        }

        public void Unselect()
        {
            button.interactable = true;
        }

    
    }
}