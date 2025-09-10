using NUnit.Framework;
using UnityEngine;
using AbilitySystem.AbilityComponents;

public class TriggerTestBase
{
    protected GameObject testGameObject;
    protected Character testCharacter;
    protected CharacterTargets testCharacterTargets; // Изменено с Targets на CharacterTargets
    protected EnemySelectionTriggerSO trigger;

    [SetUp]
    public virtual void SetUp()
    {
        // 1. Создаем GameObject для тестового персонажа
        testGameObject = new GameObject("TestCharacter");

        // 2. Добавляем компонент Character
        testCharacter = testGameObject.AddComponent<Character>();

        // 3. Добавляем реальный компонент CharacterTargets
        testCharacterTargets = testGameObject.AddComponent<CharacterTargets>();

        // 4. Настраиваем CharacterTargets через reflection:
        ReflectionHelper.SetPrivateField(testCharacterTargets, "_whoIsYourEnemy", SceneObjectTag.Enemy);
        ReflectionHelper.SetPrivateField(testCharacterTargets, "logging", false);

        // 5. Связываем CharacterTargets с Character через reflection
        ReflectionHelper.SetPrivateField(testCharacter, "_targets", testCharacterTargets);

        // 6. Создаем экземпляр триггера
        trigger = ScriptableObject.CreateInstance<EnemySelectionTriggerSO>();

        // 7. Настраиваем триггер через reflection:
        ReflectionHelper.SetPrivateField(trigger, "_targetTag", SceneObjectTag.Enemy);
        ReflectionHelper.SetPrivateField(trigger, "_logging", false);
    }

    [TearDown]
    public virtual void TearDown()
    {
        if (trigger != null) Object.DestroyImmediate(trigger);
        if (testGameObject != null) Object.DestroyImmediate(testGameObject);
    }

    // Вспомогательный метод для создания тестовой цели
    protected GameObject CreateTestTarget(SceneObjectTag tag, string name = "TestTarget")
    {
        var go = new GameObject(name);
        var character = go.AddComponent<Character>();
        ReflectionHelper.SetPrivateField(character, "_sceneObjectTag", tag);
        return go;
    }

    // Вспомогательный метод для установки цели в систему CharacterTargets
    protected void SetTestTarget(GameObject target)
    {
        ReflectionHelper.SetPrivateField(testCharacterTargets, "_selectedTarget", target);
    }

    // Вспомогательный метод для получения текущей цели
    protected GameObject GetTestTarget()
    {
        return ReflectionHelper.GetPrivateField<GameObject>(testCharacterTargets, "_selectedTarget");
    }

    // Вспомогательный метод для изменения настройки "кто является врагом"
    protected void SetWhoIsEnemy(SceneObjectTag enemyTag)
    {
        ReflectionHelper.SetPrivateField(testCharacterTargets, "_whoIsYourEnemy", enemyTag);
    }

    // Вспомогательный метод для изменения целевого тега триггера
    protected void SetTriggerTargetTag(SceneObjectTag tag)
    {
        ReflectionHelper.SetPrivateField(trigger, "_targetTag", tag);
    }

    // Вспомогательный метод для проверки наличия цели через CharacterTargets
    protected bool HasTargetEnemy()
    {
        return testCharacterTargets.HasTargetEnemy();
    }

    // Вспомогательный метод для использования родного метода установки цели
    protected void SetTargetThroughNativeMethod(GameObject target)
    {
        testCharacterTargets.SetTargetEnemy(target);
    }
}