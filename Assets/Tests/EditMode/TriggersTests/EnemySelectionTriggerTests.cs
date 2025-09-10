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
        Assert.IsTrue(result, "Должен вернуть true для валидного врага");
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
        Assert.IsFalse(result, "Должен вернуть false для не-врага");
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
        Assert.IsFalse(result, "Должен вернуть false когда у цели нет Character компонента");
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
        Assert.IsFalse(result, "Должен вернуть false когда нет цели");
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
        Assert.IsFalse(result, "Должен вернуть false когда цель уничтожена");
    }

    [Test]
    public void CheckTrigger_ReturnsFalse_WhenCharacterTargetsComponentNull()
    {
        // === ARRANGE ===
        ReflectionHelper.SetPrivateField(testCharacter, "_targets", null);

        // === ACT ===
        bool result = trigger.CheckTrigger(testCharacter);

        // === ASSERT ===
        Assert.IsFalse(result, "Должен вернуть false когда CharacterTargets компонент null");
    }

    [Test]
    public void CheckTrigger_WorksWithNativeSetTargetEnemy_Method()
    {
        // === ARRANGE ===
        // Используем родной метод установки цели (не прямой доступ через reflection)
        var validEnemy = CreateTestTarget(SceneObjectTag.Enemy);
        SetTargetThroughNativeMethod(validEnemy); // Используем testTargets.SetTargetEnemy()

        // === ACT ===
        bool result = trigger.CheckTrigger(testCharacter);

        // === ASSERT ===
        Assert.IsTrue(result, "Должен работать с родным методом установки целей");
        Object.DestroyImmediate(validEnemy);
    }

    [Test]
    public void CheckTrigger_RespectsCharacterTargetsValidation()
    {
        // === ARRANGE ===
        // Пытаемся установить не-врага через родной метод
        var heroTarget = CreateTestTarget(SceneObjectTag.Hero);
        SetTargetThroughNativeMethod(heroTarget); // Должен быть отклонен по логике CharacterTargets

        // === ACT ===
        bool hasTarget = HasTargetEnemy(); // Проверяем валидацию CharacterTargets
        bool triggerResult = trigger.CheckTrigger(testCharacter);

        // === ASSERT ===
        Assert.IsFalse(hasTarget, "CharacterTargets должен отвергать не-врагов");
        Assert.IsFalse(triggerResult, "Триггер должен вернуть false когда CharacterTargets отвергает цель");
        Object.DestroyImmediate(heroTarget);
    }

    [Test]
    public void CheckTrigger_WorksWithDifferentTagCombinations()
    {
        // Тестируем различные комбинации тегов
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

            // Восстанавливаем настройки
            SetWhoIsEnemy(SceneObjectTag.Enemy);
            SetTriggerTargetTag(SceneObjectTag.Enemy);
        }
    }

    [Test]
    public void CheckTrigger_ReturnsFalse_WhenTargetInactive()
    {
        // === ARRANGE ===
        var target = CreateTestTarget(SceneObjectTag.Enemy);
        target.SetActive(false); // Делаем цель неактивной
        SetDirectTestTarget(target);

        // === ACT ===
        bool result = trigger.CheckTrigger(testCharacter);

        // === ASSERT ===
        Assert.IsFalse(result, "Должен вернуть false когда цель неактивна");
        Object.DestroyImmediate(target);
    }
}