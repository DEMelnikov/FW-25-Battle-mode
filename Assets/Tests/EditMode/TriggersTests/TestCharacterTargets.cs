using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SimpleTestTargets : CharacterTargets
{
    // ������� ������� ������ ��������� Targets ��� ������������
    // ����� ������������ ���������� ������ ������������

    private CharacterTargets realTargets;

    public SimpleTestTargets(GameObject gameObject)
    {
        realTargets = gameObject.AddComponent<CharacterTargets>();
    }

    public void SetTestTargetDirectly(GameObject target)
    {
        ReflectionHelper.SetPrivateField(realTargets, "_selectedTarget", target);
    }

    public GameObject GetTestTargetDirectly()
    {
        return ReflectionHelper.GetPrivateField<GameObject>(realTargets, "_selectedTarget");
    }

    public void SetWhoIsYourEnemy(SceneObjectTag enemyTag)
    {
        ReflectionHelper.SetPrivateField(realTargets, "_whoIsYourEnemy", enemyTag);
    }

    public void SetLogging(bool enableLogging)
    {
        ReflectionHelper.SetPrivateField(realTargets, "logging", enableLogging);
    }

    // ���������� ������ � ��������� �������
    public GameObject GetTargetEnemy() => realTargets.GetTargetEnemy();
    public bool HasTargetEnemy() => realTargets.HasTargetEnemy();
    public bool TryGetTargetCharacter(out Character targetCharacter) =>
        realTargets.TryGetTargetCharacter(out targetCharacter);
    public void SetTargetEnemy(GameObject target) => realTargets.SetTargetEnemy(target);
}