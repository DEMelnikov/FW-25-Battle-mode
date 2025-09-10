using NUnit.Framework;
using UnityEngine;
using AbilitySystem.AbilityComponents;

[TestFixture]
public class NoSelectedEnemyNPCTriggerTests : TriggerTestBase
{
    private NoSelectedEnemy_NPCTrigger noEnemyTrigger;

    [SetUp]
    public override void SetUp()
    {
        base.SetUp();

        // ������� ��������� ������ ��������
        noEnemyTrigger = ScriptableObject.CreateInstance<NoSelectedEnemy_NPCTrigger>();

        // ����������� ������� ����� reflection
        ReflectionHelper.SetPrivateField(noEnemyTrigger, "_targetTag", SceneObjectTag.Hero);
        ReflectionHelper.SetPrivateField(noEnemyTrigger, "logging", false);
    }

    [TearDown]
    public override void TearDown()
    {
        if (noEnemyTrigger != null) Object.DestroyImmediate(noEnemyTrigger);
        base.TearDown();
    }

    // ��������������� ����� ��� ��������� ���� ��������
    private void SetTriggerTargetTag(SceneObjectTag tag)
    {
        ReflectionHelper.SetPrivateField(noEnemyTrigger, "_targetTag", tag);
    }

    // ��������������� ����� ��� ������ ��������� ���� (������ �������� SetTargetEnemy)
    private void SetTargetDirectly(GameObject target)
    {
        ReflectionHelper.SetPrivateField(testCharacterTargets, "_selectedTarget", target);
    }

    // ��������������� ����� ��� ��������� ������� ����
    private GameObject GetTargetDirectly()
    {
        return ReflectionHelper.GetPrivateField<GameObject>(testCharacterTargets, "_selectedTarget");
    }

    // ���� 1: ��� ���� ������ - ������ ������� false
    [Test]
    public void CheckTrigger_NoTarget_ReturnsFalse()
    {
        // Arrange - ����������, ��� ���� ���
        Assert.IsNull(GetTargetDirectly(), "�����������: �� ������ ���� ����");

        // Act
        bool result = noEnemyTrigger.CheckTrigger(testCharacter);

        // Assert
        Assert.IsFalse(result, "������ ������� false, ����� ��� ����");
    }

    // ���� 2: ���� ����-���� - ������ ������� true
    [Test]
    public void CheckTrigger_HasEnemyTarget_ReturnsTrue()
    {
        // Arrange
        GameObject enemyTarget = CreateTestTarget(SceneObjectTag.Enemy, "EnemyTarget");
        SetTargetThroughNativeMethod(enemyTarget); // ���������� �������� ����� ��� �����

        // Act
        bool result = noEnemyTrigger.CheckTrigger(testCharacter);

        // Assert
        Assert.IsTrue(result, "������ ������� true, ����� ���� ����-����");

        // Cleanup
        Object.DestroyImmediate(enemyTarget);
    }

    // ���� 3: ���� ����-����� (����������� ���) - ������ ������� true
    [Test]
    public void CheckTrigger_HasHeroTarget_MatchingTag_ReturnsTrue()
    {
        // Arrange
        GameObject heroTarget = CreateTestTarget(SceneObjectTag.Hero, "HeroTarget");
        SetTargetDirectly(heroTarget); // ������ ��������� ����� reflection

        // Act
        bool result = noEnemyTrigger.CheckTrigger(testCharacter);

        // Assert
        Assert.IsTrue(result, "������ ������� true, ����� ���� ���� � ����������� �����");

        // Cleanup
        Object.DestroyImmediate(heroTarget);
    }

    // ���� 4: ���� ����-������� (�� ����������� ���) - ������ ������� false
    [Test]
    public void CheckTrigger_HasNeutralTarget_NonMatchingTag_ReturnsFalse()
    {
        // Arrange
        GameObject neutralTarget = CreateTestTarget(SceneObjectTag.Neutral, "NeutralTarget");
        SetTargetDirectly(neutralTarget); // ������ ��������� ����� reflection

        // Act
        bool result = noEnemyTrigger.CheckTrigger(testCharacter);

        // Assert
        Assert.IsFalse(result, "������ ������� false, ����� ���� ���� � ������������� �����");

        // Cleanup
        Object.DestroyImmediate(neutralTarget);
    }

    // ���� 5: ��������� �������� ���� ��������
    [Test]
    public void CheckTrigger_DifferentTargetTag_BehaviorChanges()
    {
        // Arrange
        GameObject neutralTarget = CreateTestTarget(SceneObjectTag.Neutral, "NeutralTarget");
        SetTargetDirectly(neutralTarget); // ������ ��������� ����� reflection

        // ��������� � ����� �������� �� ��������� (Hero) - ������ ������� false
        bool resultWithHeroTag = noEnemyTrigger.CheckTrigger(testCharacter);
        Assert.IsFalse(resultWithHeroTag, "������ ������� false ��� Neutral � ����� �������� Hero");

        // Act - ������ ��� �������� �� Neutral
        SetTriggerTargetTag(SceneObjectTag.Neutral);

        // Assert - ������ ������ ������� true
        bool resultWithNeutralTag = noEnemyTrigger.CheckTrigger(testCharacter);
        Assert.IsTrue(resultWithNeutralTag, "������ ������� true ��� Neutral � ����� �������� Neutral");

        // Cleanup
        Object.DestroyImmediate(neutralTarget);
    }

    // ���� 6: �������� ���������� - ������� �������� HasTargetEnemy()
    [Test]
    public void CheckTrigger_EnemyTargetTakesPriority_OverTagCheck()
    {
        // Arrange
        GameObject enemyTarget = CreateTestTarget(SceneObjectTag.Enemy, "EnemyTarget");
        SetTargetThroughNativeMethod(enemyTarget); // ���������� �������� ����� ��� �����

        // Act - ���� ���� ��� �� ���������, ������ ������� true ��-�� HasTargetEnemy()
        SetTriggerTargetTag(SceneObjectTag.Neutral); // ������������� ������������� ���
        bool result = noEnemyTrigger.CheckTrigger(testCharacter);

        // Assert
        Assert.IsTrue(result, "������ ������� true, ���� ���� ����-����, ���������� �� ����");

        // Cleanup
        Object.DestroyImmediate(enemyTarget);
    }

    // ���� 7: ��������� ������ - ���� ����������, �� ������ ��������
    [Test]
    public void CheckTrigger_TargetDestroyed_ReturnsFalse()
    {
        // Arrange
        GameObject tempTarget = CreateTestTarget(SceneObjectTag.Hero, "TempTarget");
        SetTargetDirectly(tempTarget); // ������ ��������� ����� reflection

        // ���������� ����, �� ������ � CharacterTargets ��������
        Object.DestroyImmediate(tempTarget);

        // Act
        bool result = noEnemyTrigger.CheckTrigger(testCharacter);

        // Assert
        Assert.IsFalse(result, "������ ������� false, ���� ���� ����������");
    }
}