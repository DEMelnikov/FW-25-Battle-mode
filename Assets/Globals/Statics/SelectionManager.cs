using UnityEngine;
using System;

/// <summary>
/// ����������� �������� ������ �������� (� ������������� Unity)
/// </summary>
public static class SelectionManager
{
    #region �������
    public static event Action OnSelectionChanged;
    public static event Action<GameObject> OnOpponentChanged;
    #endregion

    private static GameObject _selectedObject;
    private static GameObject _opponentObject;
    private static GameObject _lastSelected;

    #region ��������� ��������
    public static GameObject SelectedObject => _selectedObject;
    public static GameObject OpponentObject => _opponentObject;
    public static GameObject LastSelected => _lastSelected;
    public static bool HasSelection => _selectedObject != null;
    public static bool HasOpponent => _opponentObject != null;
    #endregion

    #region ��������� ������
    /// <summary>
    /// ������� ����� ������
    /// </summary>
    public static void Select(GameObject newSelection)
    {
        if (_selectedObject == newSelection) return;

        ProcessSelection(newSelection);
    }

    private static void ProcessSelection(GameObject selected)
    {
        // ���� ������ Hero � ������������� ����� (Enemy)
        if (selected.TryGetComponent<Character>(out var character) &&
            character.SceneObjectTag == SceneObjectTag.Hero)
        {
            var target = character.GetSelectedTarget();
            if (target != null && target.TryGetComponent<Character>(out var targetChar) &&
                targetChar.SceneObjectTag == SceneObjectTag.Enemy)
            {
                SetSelectionWithOpponent(selected, target);
                return;
            }
        }

        // ���� ������ Enemy � ������������� ����� (Hero)
        if (selected.TryGetComponent<Character>(out var enemy) &&
            enemy.SceneObjectTag == SceneObjectTag.Enemy)
        {
            var target = enemy.GetSelectedTarget();
            if (target != null && target.TryGetComponent<Character>(out var targetChar) &&
                targetChar.SceneObjectTag == SceneObjectTag.Hero)
            {
                SetSelectionWithOpponent(target, selected);
                return;
            }
        }

        // ������� ����� �������
        SetSimpleSelection(selected);
    }

    private static void SetSimpleSelection(GameObject selected)
    {
        _selectedObject = selected;
        _opponentObject = null;
        OnSelectionChanged?.Invoke();
        Debug.Log($"Selected: {selected.name}");
    }

    private static void SetSelectionWithOpponent(GameObject selected, GameObject opponent)
    {
        _selectedObject = selected;
        _opponentObject = opponent;
        OnSelectionChanged?.Invoke();
        Debug.Log($"Selected: {selected.name}, Opponent: {opponent.name}");
    }

    /// <summary>
    /// ���������� ����������
    /// </summary>
    public static void SetOpponent(GameObject newOpponent)
    {
        if (_opponentObject == newOpponent) return;

        _opponentObject = newOpponent;
        //OnOpponentChanged?.Invoke(_opponentObject);
        Debug.Log($" SelectionManager: Selected new opponent: {_opponentObject.name} ");
    }

    /// <summary>
    /// �������� �����
    /// </summary>
    public static void ClearSelections()
    {
        _selectedObject = null;
        _opponentObject = null;
        OnSelectionChanged?.Invoke();
    }

    /// <summary>
    /// �������� ��������� � ���������� �������
    /// </summary>
    public static T GetComponentFromSelected<T>() where T : Component
    {
        return _selectedObject?.GetComponent<T>();
    }

    /// <summary>
    /// ��������� �������������� ������� � ������� ������
    /// </summary>
    public static bool IsSelectedOrOpponent(GameObject obj)
    {
        return obj == _selectedObject || obj == _opponentObject;
    }
    #endregion

    #region �������������
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatic()
    {
        // ���������� ����������� ��������� ��� ������������ ������
        _selectedObject = null;
        _opponentObject = null;
        _lastSelected = null;
        OnSelectionChanged = null;
        OnOpponentChanged = null;
    }
    #endregion
}