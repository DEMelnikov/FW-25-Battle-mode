using NUnit.Framework;
using UnityEngine;

public class TestBase
{
    protected GameObject testGameObject;
    //protected HealthSystem healthSystem;

    [SetUp]
    public virtual void SetUp()
    {
        // ������� ��������� ��� ���� ������
        testGameObject = new GameObject("TestObject");
    }

    [TearDown]
    public virtual void TearDown()
    {
        // ������� ������� ��� ���� ������
        if (testGameObject != null)
        {
            Object.DestroyImmediate(testGameObject);
        }
    }

    //protected HealthSystem CreateHealthSystem(int maxHealth = 100)
    //{
    //    var hs = testGameObject.AddComponent<HealthSystem>();
    //    // ����� ����� �������� �������������� ��������� ���� �����
    //    return hs;
    //}
}