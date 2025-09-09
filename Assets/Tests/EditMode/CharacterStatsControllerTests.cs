using NUnit.Framework;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class CharacterStatsControllerTests
{
    private GameObject _go;
    private CharacterStatsController _ctrl;

    [SetUp]
    public void Setup()
    {
        _go = new GameObject("Test_CharacterStatsController");
        _ctrl = _go.AddComponent<CharacterStatsController>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(_go);
    }

    // Универсальный хелпер для создания и наполнения SO_CharacterStatsConfig через reflection.
    // Ожидает, что в конфиге есть поле/свойство "Stats" (List<...>),
    // а в элементе списка — поля/свойства "Name", "Tag", "BaseValue", "HasAlarm".
    private SO_CharacterStatsConfig CreateConfig(params (string name, StatTag tag, float baseValue, bool hasAlarm)[] items)
    {
        // Создаем экземпляр конфига
        var config = ScriptableObject.CreateInstance<SO_CharacterStatsConfig>();

        // Достаем поле/свойство "Stats"
        var cfgType = config.GetType();
        var statsMember = (MemberInfo)cfgType.GetField("Stats", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                        ?? cfgType.GetProperty("Stats", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        Assert.IsNotNull(statsMember, "В SO_CharacterStatsConfig не найдено поле/свойство 'Stats'.");

        // Получаем текущее значение списка
        object statsValue = statsMember is FieldInfo fi ? fi.GetValue(config) : ((PropertyInfo)statsMember).GetValue(config);
        IList list = statsValue as IList;

        // Если список = null, создаем новый List<T> нужного типа
        if (list == null)
        {
            // Определяем тип элемента T
            System.Type elementType = null;

            // Если поле уже объявлено как List<T>, вытащим T из FieldType/PropertyType
            System.Type listType = statsMember is FieldInfo fi2 ? fi2.FieldType : ((PropertyInfo)statsMember).PropertyType;
            if (listType.IsGenericType)
            {
                elementType = listType.GetGenericArguments()[0];
                var concreteList = System.Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));
                // Присваиваем новый список обратно в config
                if (statsMember is FieldInfo fi3) fi3.SetValue(config, concreteList);
                else ((PropertyInfo)statsMember).SetValue(config, concreteList);
                list = (IList)concreteList;
            }
            else
            {
                Assert.Fail("Поле/свойство 'Stats' не является generic-списком (List<T>).");
            }
        }

        // Тип элемента списка (T)
        var elementTypeFinal = list.GetType().GetGenericArguments()[0];

        // Утилита для установки поля/свойства по имени
        void SetMember(object obj, string memberName, object value)
        {
            var f = elementTypeFinal.GetField(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (f != null) { f.SetValue(obj, value); return; }
            var p = elementTypeFinal.GetProperty(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (p != null) { p.SetValue(obj, value); return; }
            Assert.Fail($"В элементе Stats не найдено поле/свойство '{memberName}'.");
        }

        // Наполняем элементами
        foreach (var it in items)
        {
            var elem = System.Activator.CreateInstance(elementTypeFinal);
            SetMember(elem, "Name", it.name);
            SetMember(elem, "Tag", it.tag);
            SetMember(elem, "BaseValue", it.baseValue);
            SetMember(elem, "HasAlarm", it.hasAlarm);
            list.Add(elem);
        }

        return config;
    }

    [Test]
    public void SOInitialize_Populates_Stats_And_Clears_Previous()
    {
        // Подготовим заранее «старые» данные, чтобы убедиться, что Clear отрабатывает.
        _ctrl.Stats[(StatTag)12345] = new Stat("Old", (StatTag)12345, 999f, true);

        var config = CreateConfig(
            ("Strength", StatTag.Strength, 5f, true)
        );

        _ctrl.SOIntializeStats(config);

        Assert.IsTrue(_ctrl.Stats.ContainsKey(StatTag.Strength));
        Assert.AreEqual(1, _ctrl.Stats.Count, "Должны остаться только статсы из конфига");
        Assert.AreEqual(5f, _ctrl.Stats[StatTag.Strength].Value, 1e-4f);
    }

    [Test]
    public void SOInitialize_Duplicate_Tags_LogWarning_And_Skip_Duplicate()
    {
        var config = CreateConfig(
            ("Strength", StatTag.Strength, 5f, true),
            ("Strength again", StatTag.Strength, 10f, true) // дубликат
        );

        // Ожидаем предупреждение (строка ровно как в коде контроллера)
        LogAssert.Expect(LogType.Warning, $"Duplicate StatTag {StatTag.Strength} in config!");

        _ctrl.SOIntializeStats(config);

        // В словаре только одно значение
        Assert.IsTrue(_ctrl.Stats.ContainsKey(StatTag.Strength));
        Assert.AreEqual(1, _ctrl.Stats.Count);
        Assert.AreEqual(5f, _ctrl.Stats[StatTag.Strength].Value, 1e-4f);
    }

    [Test]
    public void AddTempModifier_Increases_Strength_By_3_And_Fires_OnValueChanged()
    {
        var config = CreateConfig(
            ("Strength", StatTag.Strength, 5f, true)
        );
        _ctrl.SOIntializeStats(config);

        int changed = 0;
        _ctrl.Stats[StatTag.Strength].OnValueChanged += (_, __, ___) => changed++;

        _ctrl.AddTempModifier();

        Assert.AreEqual(8f, _ctrl.Stats[StatTag.Strength].Value, 1e-4f);
        Assert.AreEqual(1, changed, "Должно сработать событие изменения значения");
    }
}