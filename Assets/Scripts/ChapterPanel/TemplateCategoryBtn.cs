using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;

public class TemplateCategoryBtn : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private RtlText title;
    private TemplateCategory _category;
    public TemplateCategory Category => _category;

     private Button _btn;
     
     
     
     
     
     
     public delegate void CategoriesEvents(TemplateCategoryBtn btn, TemplateCategory category);
     public static event CategoriesEvents onClick;
     

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
            this._category = category;

            this.icon.sprite = this._category.icon;
            this.title.text = this._category.name;
            Button.onClick.AddListener(OnButtonClicked);
            Unselect();
            
        }
        
    
        
        
        private void OnButtonClicked()
        {
            onClick?.Invoke(this, this._category);
        }

        public void Select()
        {
            _btn.interactable = false;
        }

        public void Unselect()
        {
            _btn.interactable = true;
        }
 
}
