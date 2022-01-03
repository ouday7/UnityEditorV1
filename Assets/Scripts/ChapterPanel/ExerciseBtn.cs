using EditorMenu;
using UnityEngine;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class ExerciseBtn : PoolableObject
    {
        [SerializeField] private Button showQstsBtn;
        [SerializeField] private Button addQstBtn;
        [SerializeField] private GameObject exScrollRect;
        [SerializeField] private Text titleTxt;
        [SerializeField] private DraggBtn dragBtn;

        private ExerciseBtn _targetBtn;
        private Color _startColor;
        private Color _endColor;

        private bool _isDraggable;
        private bool _isDragging;

        private int _startIndex;
        private int _endIndex;

        private bool _isRemove;
        private bool _isInitialized;
        private static int _exerciseNbr = 1;
        private const string ExName = "  تمرين  ";

        public ExerciseData data;

        public void Initialize()
        {
            if (_isInitialized) return;
            this.titleTxt.text = ExName + _exerciseNbr;
            _exerciseNbr++;
            _isInitialized = true;
            dragBtn.Initialize(this);

            addQstBtn.onClick.AddListener(() => { MenuController.Instance.AddNewQst(addQstBtn); });
            showQstsBtn.onClick.AddListener(ShowQsts);
        }

        public void BindData(ExerciseData newExerciseData)
        {
            data = newExerciseData;
            data.chapterId = newExerciseData.chapterId;
            data.exerciseId = newExerciseData.exerciseId;
            data.questions = newExerciseData.questions;
        }

        private void ShowQsts()
        {
            if (exScrollRect.activeInHierarchy)
            {
                exScrollRect.SetActive(false);
                return;
            }

            exScrollRect.SetActive(true);
        }

        public void OnStartDrag()
        {
            _startIndex = transform.GetSiblingIndex();
            transform.SetAsLastSibling();
        }

        public void OnDrag(Vector3 pos)
        {
            var transform1 = transform;
            transform1.position = new Vector2(transform1.position.x, pos.y);
        }

        public void OnEndDrag()
        {
            if (this._targetBtn != null)
            {
                PermutationOrder();
                _startColor = Color.black;
            }

            else
            {
                transform.SetSiblingIndex(_startIndex);
            }
        }

        private void PermutationOrder()
        {
        }

        private void Remove()
        {
            PoolSystem.instance.DeSpawn(this.transform);
            _exerciseNbr--;
            MenuController.Instance.currentExList.Remove(transform.GetComponent<ExerciseBtn>());
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Remove"))
            {
                transform.GetComponent<Image>().color = Color.red;
            }

            ;
            if (!other.gameObject.CompareTag("GroupQuestionMenuBtn")) return;
            this._targetBtn = other.GetComponent<ExerciseBtn>();
            Remove();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Remove"))
            {
                _isRemove = false;
                Remove();
            }

            ;
            if (!other.gameObject.CompareTag("GroupQuestionMenuBtn")) return;
            this._targetBtn = null;
        }
    }
}