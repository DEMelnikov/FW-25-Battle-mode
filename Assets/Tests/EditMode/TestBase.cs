using NUnit.Framework;
using UnityEngine;

public class TestBase
{
    protected GameObject testGameObject;
    //protected HealthSystem healthSystem;

    [SetUp]
    public virtual void SetUp()
    {
        // Базовая настройка для всех тестов
        testGameObject = new GameObject("TestObject");
    }

    [TearDown]
    public virtual void TearDown()
    {
        // Базовая очистка для всех тестов
        if (testGameObject != null)
        {
            Object.DestroyImmediate(testGameObject);
        }
    }

    //protected HealthSystem CreateHealthSystem(int maxHealth = 100)
    //{
    //    var hs = testGameObject.AddComponent<HealthSystem>();
    //    // Здесь можно добавить дополнительную настройку если нужно
    //    return hs;
    //}
}