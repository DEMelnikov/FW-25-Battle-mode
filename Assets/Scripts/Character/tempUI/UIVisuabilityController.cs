using UnityEngine;

public class UIVisuabilityController : MonoBehaviour
{
    [SerializeField] private GameObject _targetObject;
    [SerializeField] private SceneObjectTag _targetTag;
    [SerializeField] private bool logging = false;

    private void Start()
    {
        if (_targetObject == null) return;

        SetterActive();

    }

    private void OnEnable()
    {
        //// Подписываемся на события изменения выбора
        //SelectionManager.Instance.OnSelectedObjectChanged += UpdateVisibility;
        //SelectionManager.Instance.OnOpponentObjectChanged += UpdateVisibility;
        SelectionManager.OnSelectionChanged += UpdateVisibility;
       if(logging) Debug.Log("UIVisuabilityController:  Subscribe to OnSelectionChanged is on");
        //UpdateVisibility(null); // Инициализация
    }

    private void OnDisable()
    {
        // Отписываемся от событий
        //SelectionManager.Instance.OnSelectedObjectChanged -= UpdateVisibility;
        //SelectionManager.Instance.OnOpponentObjectChanged -= UpdateVisibility;
        //SelectionManager.OnSelectionChanged -= UpdateVisibility;
        //Debug.Log("UIVisuabilityController:  UnSubscribe to OnSelectionChanged");
    }

    private void OnDestroy()
    {
        // Отписываемся от событий
        //SelectionManager.Instance.OnSelectedObjectChanged -= UpdateVisibility;
        //SelectionManager.Instance.OnOpponentObjectChanged -= UpdateVisibility;
        SelectionManager.OnSelectionChanged -= UpdateVisibility;
        if (logging) Debug.Log("UIVisuabilityController:  UnSubscribe to OnSelectionChanged");
    }

    private void UpdateVisibility()
    {
        if (logging) Debug.Log("UIVisuabilityController:  Got invoke - start cheking");
        if (_targetObject == null) return;

        SetterActive();

        if (logging) Debug.Log("Selection Hit in UpdateVisibility");
    }


    private bool CheckTagMatch()
    {
        switch (_targetTag)
        {
            case SceneObjectTag.Hero:
                if (SelectionManager.HasSelection)
                {
                    if(SelectionManager.SelectedObject.GetComponent<Character>().SceneObjectTag  == SceneObjectTag.Hero) return true;
                }
                return false;
                //break;
            case SceneObjectTag.Enemy:
                if (SelectionManager.HasOpponent)
                {
                    if (SelectionManager.OpponentObject.GetComponent<Character>().SceneObjectTag == SceneObjectTag.Enemy) return true;
                }
                return false;
            default: return false;
        }
    }

    private void SetterActive()
    {
        if (CheckTagMatch())
        {
            _targetObject.SetActive(true);
        }
        else { _targetObject.SetActive(false); }
    }
}