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

        // Создаем экземпляр нашего триггера
        noEnemyTrigger = ScriptableObject.CreateInstance<NoSelectedEnemy_NPCTrigger>();

        // Настраиваем триггер через reflection
        ReflectionHelper.SetPrivateField(noEnemyTrigger, "_targetTag", SceneObjectTag.Hero);
        ReflectionHelper.SetPrivateField(noEnemyTrigger, "logging", false);
    }

    [TearDown]
    public override void TearDown()
    {
        if (noEnemyTrigger != null) Object.DestroyImmediate(noEnemyTrigger);
        base.TearDown();
    }

    // Вспомогательный метод для настройки тега триггера
    private void SetTriggerTargetTag(SceneObjectTag tag)
    {
        ReflectionHelper.SetPrivateField(noEnemyTrigger, "_targetTag", tag);
    }

    // Вспомогательный метод для прямой установки цели (обходя проверки SetTargetEnemy)
    private void SetTargetDirectly(GameObject target)
    {
        ReflectionHelper.SetPrivateField(testCharacterTargets, "_selectedTarget", target);
    }

    // Вспомогательный метод для получения текущей цели
    private GameObject GetTargetDirectly()
    {
        return ReflectionHelper.GetPrivateField<GameObject>(testCharacterTargets, "_selectedTarget");
    }

    // Тест 1: Нет цели вообще - должен вернуть false
    [Test]
    public void CheckTrigger_NoTarget_ReturnsFalse()
    {
        // Arrange - убеждаемся, что цели нет
        Assert.IsNull(GetTargetDirectly(), "Предусловие: не должно быть цели");

        // Act
        bool result = noEnemyTrigger.CheckTrigger(testCharacter);

        // Assert
        Assert.IsFalse(result, "Должен вернуть false, когда нет цели");
    }

    // Тест 2: Есть цель-враг - должен вернуть true
    [Test]
    public void CheckTrigger_HasEnemyTarget_ReturnsTrue()
    {
        // Arrange
        GameObject enemyTarget = CreateTestTarget(SceneObjectTag.Enemy, "EnemyTarget");
        SetTargetThroughNativeMethod(enemyTarget); // Используем нативный метод для врага

        // Act
        bool result = noEnemyTrigger.CheckTrigger(testCharacter);

        // Assert
        Assert.IsTrue(result, "Должен вернуть true, когда есть цель-враг");

        // Cleanup
        Object.DestroyImmediate(enemyTarget);
    }

    // Тест 3: Есть цель-герой (совпадающий тег) - должен вернуть true
    [Test]
    public void CheckTrigger_HasHeroTarget_MatchingTag_ReturnsTrue()
    {
        // Arrange
        GameObject heroTarget = CreateTestTarget(SceneObjectTag.Hero, "HeroTarget");
        SetTargetDirectly(heroTarget); // Прямая установка через reflection

        // Act
        bool result = noEnemyTrigger.CheckTrigger(testCharacter);

        // Assert
        Assert.IsTrue(result, "Должен вернуть true, когда есть цель с совпадающим тегом");

        // Cleanup
        Object.DestroyImmediate(heroTarget);
    }

    // Тест 4: Есть цель-нейтрал (не совпадающий тег) - должен вернуть false
    [Test]
    public void CheckTrigger_HasNeutralTarget_NonMatchingTag_ReturnsFalse()
    {
        // Arrange
        GameObject neutralTarget = CreateTestTarget(SceneObjectTag.Neutral, "NeutralTarget");
        SetTargetDirectly(neutralTarget); // Прямая установка через reflection

        // Act
        bool result = noEnemyTrigger.CheckTrigger(testCharacter);

        // Assert
        Assert.IsFalse(result, "Должен вернуть false, когда есть цель с несовпадающим тегом");

        // Cleanup
        Object.DestroyImmediate(neutralTarget);
    }

    // Тест 5: Изменение целевого тега триггера
    [Test]
    public void CheckTrigger_DifferentTargetTag_BehaviorChanges()
    {
        // Arrange
        GameObject neutralTarget = CreateTestTarget(SceneObjectTag.Neutral, "NeutralTarget");
        SetTargetDirectly(neutralTarget); // Прямая установка через reflection

        // Проверяем с тегом триггера по умолчанию (Hero) - должен вернуть false
        bool resultWithHeroTag = noEnemyTrigger.CheckTrigger(testCharacter);
        Assert.IsFalse(resultWithHeroTag, "Должен вернуть false для Neutral с тегом триггера Hero");

        // Act - меняем тег триггера на Neutral
        SetTriggerTargetTag(SceneObjectTag.Neutral);

        // Assert - теперь должен вернуть true
        bool resultWithNeutralTag = noEnemyTrigger.CheckTrigger(testCharacter);
        Assert.IsTrue(resultWithNeutralTag, "Должен вернуть true для Neutral с тегом триггера Neutral");

        // Cleanup
        Object.DestroyImmediate(neutralTarget);
    }

    // Тест 6: Проверка приоритета - сначала проверка HasTargetEnemy()
    [Test]
    public void CheckTrigger_EnemyTargetTakesPriority_OverTagCheck()
    {
        // Arrange
        GameObject enemyTarget = CreateTestTarget(SceneObjectTag.Enemy, "EnemyTarget");
        SetTargetThroughNativeMethod(enemyTarget); // Используем нативный метод для врага

        // Act - даже если тег не совпадает, должен вернуть true из-за HasTargetEnemy()
        SetTriggerTargetTag(SceneObjectTag.Neutral); // Устанавливаем несовпадающий тег
        bool result = noEnemyTrigger.CheckTrigger(testCharacter);

        // Assert
        Assert.IsTrue(result, "Должен вернуть true, если есть цель-враг, независимо от тега");

        // Cleanup
        Object.DestroyImmediate(enemyTarget);
    }

    // Тест 7: Граничный случай - цель уничтожена, но ссылка осталась
    [Test]
    public void CheckTrigger_TargetDestroyed_ReturnsFalse()
    {
        // Arrange
        GameObject tempTarget = CreateTestTarget(SceneObjectTag.Hero, "TempTarget");
        SetTargetDirectly(tempTarget); // Прямая установка через reflection

        // Уничтожаем цель, но ссылка в CharacterTargets остается
        Object.DestroyImmediate(tempTarget);

        // Act
        bool result = noEnemyTrigger.CheckTrigger(testCharacter);

        // Assert
        Assert.IsFalse(result, "Должен вернуть false, если цель уничтожена");
    }
}