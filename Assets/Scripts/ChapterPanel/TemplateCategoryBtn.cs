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

        private Button Button
        {
            get
            {
                if (_btn == null) _btn = GetComponent<Button>();
                return _btn;
            }
        }
        public void Initialize(TemplateCategory category)
        {
            _category = category;
            icon.sprite = _category.icon;
            title.text = _category.name;
            Button.onClick.AddListener(OnButtonClicked);
            Unselect();
        }
        private void OnButtonClicked()
        {
            onClick?.Invoke(this, this._category);
        }

        public void Select() => Button.interactable = false;

        public void Unselect() =>Button.interactable=true;
       
        
    }
}