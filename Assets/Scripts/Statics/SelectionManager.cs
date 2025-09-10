using UnityEngine;
using System;

/// <summary>
/// Статический менеджер выбора объектов (с ограничениями Unity)
/// </summary>
public static class SelectionManager
{
    #region События
    public static event Action OnSelectionChanged;
    public static event Action OnOpponentChanged;
    #endregion

    private static GameObject _selectedObject;
    private static GameObject _opponentObject;
    private static GameObject _lastSelected;

    #region Публичные свойства
    public static GameObject SelectedObject => _selectedObject;
    public static GameObject OpponentObject => _opponentObject;
    public static GameObject LastSelected => _lastSelected;
    public static bool HasSelection => _selectedObject != null;
    public static bool HasOpponent => _opponentObject != null;
    #endregion

    #region Публичные методы
    /// <summary>
    /// Выбрать новый объект
    /// </summary>
    public static void Select(GameObject newSelection)
    {
        if (_selectedObject == newSelection) return;

        ProcessSelection(newSelection);
    }

    private static void ProcessSelection(GameObject selected)
    {
        // Если выбран Hero с установленной целью (Enemy)
        if (selected.TryGetComponent<ISelectableCharacter>(out var character) &&
            character.SceneObjectTag == SceneObjectTag.Hero)
        {
            var target = character.GetSelectedTarget();
            if (target != null && target.TryGetComponent<ISelectableCharacter>(out var targetChar) &&
                targetChar.SceneObjectTag == SceneObjectTag.Enemy)
            {
                SetSelectionWithOpponent(selected, target);
                return;
            }
        }

        // Если выбран Enemy с установленной целью (Hero)
        if (selected.TryGetComponent<ISelectableCharacter>(out var enemy) &&
            enemy.SceneObjectTag == SceneObjectTag.Enemy)
        {
            var target = enemy.GetSelectedTarget();
            if (target != null && target.TryGetComponent<ISelectableCharacter>(out var targetChar) &&
                targetChar.SceneObjectTag == SceneObjectTag.Hero)
            {
                SetSelectionWithOpponent(target, selected);
                return;
            }
        }

        // Обычный выбор объекта
        SetSimpleSelection(selected);
    }

    private static void SetSimpleSelection(GameObject selected)
    {

        if (selected.TryGetComponent<ISelectableCharacter>(out var selectedChar))
        {
            if (selectedChar.SceneObjectTag == SceneObjectTag.Hero)
            {
                _selectedObject = selected;
                _opponentObject = null;
                OnSelectionChanged?.Invoke();
                OnOpponentChanged?.Invoke();
                Debug.Log($"Selected: {selected.name}");
            }
            if (selectedChar.SceneObjectTag == SceneObjectTag.Enemy)
            {
                _selectedObject = null;
                _opponentObject = selected;
                OnSelectionChanged?.Invoke();
                OnOpponentChanged?.Invoke();
                Debug.Log($"Selected: {selected.name}");
            }
        }
    }

    private static void SetSelectionWithOpponent(GameObject selected, GameObject opponent)
    {
        _selectedObject = selected;
        _opponentObject = opponent;
        OnSelectionChanged?.Invoke();
        OnOpponentChanged?.Invoke();
        Debug.Log($"Selected: {selected.name}, Opponent: {opponent.name}");
    }

    /// <summary>
    /// Установить противника
    /// </summary>
    public static void SetOpponent(GameObject newOpponent)
    {
        if (_opponentObject == newOpponent) return;

        _opponentObject = newOpponent;
        OnOpponentChanged?.Invoke();
        Debug.Log($" SelectionManager: Selected new opponent: {_opponentObject.name} ");
    }

    /// <summary>
    /// Очистить выбор
    /// </summary>
    public static void ClearSelections()
    {
        _selectedObject = null;
        _opponentObject = null;
        OnSelectionChanged?.Invoke();
        OnOpponentChanged?.Invoke();
    }

    /// <summary>
    /// Получить компонент с выбранного объекта
    /// </summary>
    public static T GetComponentFromSelected<T>() where T : Component
    {
        return _selectedObject?.GetComponent<T>();
    }

    /// <summary>
    /// Проверить принадлежность объекта к системе выбора
    /// </summary>
    public static bool IsSelectedOrOpponent(GameObject obj)
    {
        return obj == _selectedObject || obj == _opponentObject;
    }
    #endregion

    #region Инициализация
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatic()
    {
        // Сбрасываем статическое состояние при перезагрузке домена
        _selectedObject    = null;
        _opponentObject    = null;
        _lastSelected      = null;
        OnSelectionChanged = null;
        OnOpponentChanged  = null;
    }
    #endregion
}