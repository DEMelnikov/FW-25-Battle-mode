using NUnit.Framework;
using UnityEngine;
using AbilitySystem.AbilityComponents;

public class EnemySelectionTriggerTests : TriggerTestBase
{
    [Test]
    public void CheckTrigger_ReturnsTrue_WhenValidEnemyTarget()
    {
        // === ARRANGE ===
        var target = CreateTestTarget(SceneObjectTag.Enemy);
        SetDirectTestTarget(target);

        // === ACT ===
        bool result = trigger.CheckTrigger(testCharacter);

        // === ASSERT ===
        Assert.IsTrue(result, "������ ������� true ��� ��������� �����");
        Object.DestroyImmediate(target);
    }

    [Test]
    public void CheckTrigger_ReturnsFalse_WhenTargetIsNotEnemy()
    {
        // === ARRANGE ===
        var target = CreateTestTarget(SceneObjectTag.Hero);
        SetDirectTestTarget(target);

        // === ACT ===
        bool result = trigger.CheckTrigger(testCharacter);

        // === ASSERT ===
        Assert.IsFalse(result, "������ ������� false ��� ��-�����");
        Object.DestroyImmediate(target);
    }

    [Test]
    public void CheckTrigger_ReturnsFalse_WhenTargetHasNoCharacterComponent()
    {
        // === ARRANGE ===
        var target = new GameObject("TargetWithoutCharacter");
        SetDirectTestTarget(target);

        // === ACT ===
        bool result = trigger.CheckTrigger(testCharacter);

        // === ASSERT ===
        Assert.IsFalse(result, "������ ������� false ����� � ���� ��� Character ����������");
        Object.DestroyImmediate(target);
    }

    [Test]
    public void CheckTrigger_ReturnsFalse_WhenNoTarget()
    {
        // === ARRANGE ===
        SetDirectTestTarget(null);

        // === ACT ===
        bool result = trigger.CheckTrigger(testCharacter);

        // === ASSERT ===
        Assert.IsFalse(result, "������ ������� false ����� ��� ����");
    }

    [Test]
    public void CheckTrigger_ReturnsFalse_WhenTargetDestroyed()
    {
        // === ARRANGE ===
        var target = CreateTestTarget(SceneObjectTag.Enemy);
        SetDirectTestTarget(target);
        Object.DestroyImmediate(target);


        // === ACT ===
        bool result = trigger.CheckTrigger(testCharacter);

        // === ASSERT ===
        Assert.IsFalse(result, "������ ������� false ����� ���� ����������");
    }

    [Test]
    public void CheckTrigger_ReturnsFalse_WhenCharacterTargetsComponentNull()
    {
        // === ARRANGE ===
        ReflectionHelper.SetPrivateField(testCharacter, "_targets", null);

        // === ACT ===
        bool result = trigger.CheckTrigger(testCharacter);

        // === ASSERT ===
        Assert.IsFalse(result, "������ ������� false ����� CharacterTargets ��������� null");
    }

    [Test]
    public void CheckTrigger_WorksWithNativeSetTargetEnemy_Method()
    {
        // === ARRANGE ===
        // ���������� ������ ����� ��������� ���� (�� ������ ������ ����� reflection)
        var validEnemy = CreateTestTarget(SceneObjectTag.Enemy);
        SetTargetThroughNativeMethod(validEnemy); // ���������� testTargets.SetTargetEnemy()

        // === ACT ===
        bool result = trigger.CheckTrigger(testCharacter);

        // === ASSERT ===
        Assert.IsTrue(result, "������ �������� � ������ ������� ��������� �����");
        Object.DestroyImmediate(validEnemy);
    }

    [Test]
    public void CheckTrigger_RespectsCharacterTargetsValidation()
    {
        // === ARRANGE ===
        // �������� ���������� ��-����� ����� ������ �����
        var heroTarget = CreateTestTarget(SceneObjectTag.Hero);
        SetTargetThroughNativeMethod(heroTarget); // ������ ���� �������� �� ������ CharacterTargets

        // === ACT ===
        bool hasTarget = HasTargetEnemy(); // ��������� ��������� CharacterTargets
        bool triggerResult = trigger.CheckTrigger(testCharacter);

        // === ASSERT ===
        Assert.IsFalse(hasTarget, "CharacterTargets ������ ��������� ��-������");
        Assert.IsFalse(triggerResult, "������� ������ ������� false ����� CharacterTargets ��������� ����");
        Object.DestroyImmediate(heroTarget);
    }

    [Test]
    public void CheckTrigger_WorksWithDifferentTagCombinations()
    {
        // ��������� ��������� ���������� �����
        var testCases = new[]
        {
            new { target = SceneObjectTag.Enemy, targetsEnemy = SceneObjectTag.Enemy, triggerTag = SceneObjectTag.Enemy, expected = true },
            new { target = SceneObjectTag.Hero, targetsEnemy = SceneObjectTag.Enemy, triggerTag = SceneObjectTag.Enemy, expected = false },
            //new { target = SceneObjectTag.NPC, targetsEnemy = SceneObjectTag.NPC, triggerTag = SceneObjectTag.NPC, expected = true },
            //new { target = SceneObjectTag.Enemy, targetsEnemy = SceneObjectTag.NPC, triggerTag = SceneObjectTag.Enemy, expected = false }
        };

        foreach (var testCase in testCases)
        {
            // === ARRANGE ===
            var target = CreateTestTarget(testCase.target);
            SetDirectTestTarget(target);
            SetWhoIsEnemy(testCase.targetsEnemy);
            SetTriggerTargetTag(testCase.triggerTag);

            // === ACT ===
            bool result = trigger.CheckTrigger(testCharacter);

            // === ASSERT ===
            Assert.AreEqual(testCase.expected, result,
                $"Failed for: target={testCase.target}, " +
                $"targetsEnemy={testCase.targetsEnemy}, " +
                $"triggerTag={testCase.triggerTag}");

            // === CLEANUP ===
            Object.DestroyImmediate(target);

            // ��������������� ���������
            SetWhoIsEnemy(SceneObjectTag.Enemy);
            SetTriggerTargetTag(SceneObjectTag.Enemy);
        }
    }

    [Test]
    public void CheckTrigger_ReturnsFalse_WhenTargetInactive()
    {
        // === ARRANGE ===
        var target = CreateTestTarget(SceneObjectTag.Enemy);
        target.SetActive(false); // ������ ���� ����������
        SetDirectTestTarget(target);

        // === ACT ===
        bool result = trigger.CheckTrigger(testCharacter);

        // === ASSERT ===
        Assert.IsFalse(result, "������ ������� false ����� ���� ���������");
        Object.DestroyImmediate(target);
    }
}