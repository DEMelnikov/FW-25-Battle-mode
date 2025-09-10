using NUnit.Framework;
using UnityEngine;
using AbilitySystem.AbilityComponents;

public class TriggerTestBase
{
    protected GameObject testGameObject;
    protected Character testCharacter;
    protected CharacterTargets testCharacterTargets; // �������� � Targets �� CharacterTargets
    protected EnemySelectionTriggerSO trigger;

    [SetUp]
    public virtual void SetUp()
    {
        // 1. ������� GameObject ��� ��������� ���������
        testGameObject = new GameObject("TestCharacter");

        // 2. ��������� ��������� Character
        testCharacter = testGameObject.AddComponent<Character>();

        // 3. ��������� �������� ��������� CharacterTargets
        testCharacterTargets = testGameObject.AddComponent<CharacterTargets>();

        // 4. ����������� CharacterTargets ����� reflection:
        ReflectionHelper.SetPrivateField(testCharacterTargets, "_whoIsYourEnemy", SceneObjectTag.Enemy);
        ReflectionHelper.SetPrivateField(testCharacterTargets, "logging", false);

        // 5. ��������� CharacterTargets � Character ����� reflection
        ReflectionHelper.SetPrivateField(testCharacter, "_targets", testCharacterTargets);

        // 6. ������� ��������� ��������
        trigger = ScriptableObject.CreateInstance<EnemySelectionTriggerSO>();

        // 7. ����������� ������� ����� reflection:
        ReflectionHelper.SetPrivateField(trigger, "_targetTag", SceneObjectTag.Enemy);
        ReflectionHelper.SetPrivateField(trigger, "_logging", false);
    }

    [TearDown]
    public virtual void TearDown()
    {
        if (trigger != null) Object.DestroyImmediate(trigger);
        if (testGameObject != null) Object.DestroyImmediate(testGameObject);
    }

    // ��������������� ����� ��� �������� �������� ����
    protected GameObject CreateTestTarget(SceneObjectTag tag, string name = "TestTarget")
    {
        var go = new GameObject(name);
        var character = go.AddComponent<Character>();
        ReflectionHelper.SetPrivateField(character, "_sceneObjectTag", tag);
        return go;
    }

    // ��������������� ����� ��� ��������� ���� � ������� CharacterTargets
    protected void SetTestTarget(GameObject target)
    {
        ReflectionHelper.SetPrivateField(testCharacterTargets, "_selectedTarget", target);
    }

    // ��������������� ����� ��� ��������� ������� ����
    protected GameObject GetTestTarget()
    {
        return ReflectionHelper.GetPrivateField<GameObject>(testCharacterTargets, "_selectedTarget");
    }

    // ��������������� ����� ��� ��������� ��������� "��� �������� ������"
    protected void SetWhoIsEnemy(SceneObjectTag enemyTag)
    {
        ReflectionHelper.SetPrivateField(testCharacterTargets, "_whoIsYourEnemy", enemyTag);
    }

    // ��������������� ����� ��� ��������� �������� ���� ��������
    protected void SetTriggerTargetTag(SceneObjectTag tag)
    {
        ReflectionHelper.SetPrivateField(trigger, "_targetTag", tag);
    }

    // ��������������� ����� ��� �������� ������� ���� ����� CharacterTargets
    protected bool HasTargetEnemy()
    {
        return testCharacterTargets.HasTargetEnemy();
    }

    // ��������������� ����� ��� ������������� ������� ������ ��������� ����
    protected void SetTargetThroughNativeMethod(GameObject target)
    {
        testCharacterTargets.SetTargetEnemy(target);
    }
}