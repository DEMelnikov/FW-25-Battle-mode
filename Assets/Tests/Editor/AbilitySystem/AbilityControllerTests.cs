// Assets/Tests/Editor/AbilitySystem/AbilityControllerTests.cs
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using AbilitySystem.AbilityComponents;

namespace AbilitySystem.Tests
{
    [TestFixture]
    public class AbilityControllerTests
    {
        private GameObject testGameObject;
        private AbilityController abilityController;
        private TestCharacter testCharacter;

        [SetUp]
        public void SetUp()
        {
            testGameObject = new GameObject();
            abilityController = testGameObject.AddComponent<AbilityController>();
            testCharacter = testGameObject.AddComponent<TestCharacter>();
        }

        [TearDown]
        public void TearDown()
        {
            // Очищаем все созданные объекты
            var abilities = abilityController.GetAllAbilities();
            foreach (var ability in abilities)
            {
                if (ability is TestAbility testAbility)
                {
                    Object.DestroyImmediate(testAbility);
                }
            }

            Object.DestroyImmediate(testGameObject);
        }

        // Вспомогательный метод для вызова private методов
        private T CallPrivateMethod<T>(string methodName, params object[] parameters)
        {
            var methodInfo = typeof(AbilityController).GetMethod(methodName,
                BindingFlags.NonPublic | BindingFlags.Instance);
            return (T)methodInfo.Invoke(abilityController, parameters);
        }

        // ТЕСТЫ

        [Test]
        public void TryActivateAbility_WithNullAbility_ReturnsFalse()
        {
            // Act
            bool result = abilityController.TryActivateAbility(null);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void TryActivateAbility_WhenCannotAfford_ReturnsFalse()
        {
            // Arrange
            TestAbility ability = ScriptableObject.CreateInstance<TestAbility>();
            ability.canAffordResult = false;

            // Act
            bool result = abilityController.TryActivateAbility(ability);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void TryActivateAbility_WhenTriggersNotReady_ReturnsFalse()
        {
            // Arrange
            TestAbility ability = ScriptableObject.CreateInstance<TestAbility>();
            TestAbilityTrigger trigger = ScriptableObject.CreateInstance<TestAbilityTrigger>();
            trigger.checkResult = false;
            ability.testTriggers.Add(trigger);
            //ability.

            // Act
            bool result = abilityController.TryActivateAbility(ability);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void TryActivateAbility_WhenCannotPayCost_ReturnsFalse()
        {
            // Arrange
            TestAbility ability = ScriptableObject.CreateInstance<TestAbility>();
            ability.payCostResult = false;

            // Добавляем триггер который проходит
            TestAbilityTrigger trigger = ScriptableObject.CreateInstance<TestAbilityTrigger>();
            trigger.checkResult = true;
            ability.testTriggers.Add(trigger);

            // Act
            bool result = abilityController.TryActivateAbility(ability);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void TryActivateAbility_WithValidAbility_ReturnsTrueAndExecutes()
        {
            // Arrange
            TestAbility ability = ScriptableObject.CreateInstance<TestAbility>();

            // Триггер
            TestAbilityTrigger trigger = ScriptableObject.CreateInstance<TestAbilityTrigger>();
            trigger.checkResult = true;
            ability.testTriggers.Add(trigger);

            // Действие
            TestAbilityAction action = ScriptableObject.CreateInstance<TestAbilityAction>();
            action.executeResult = 5;
            ability.testAction = action;

            // Результат
            TestAbilityResolve resolve = ScriptableObject.CreateInstance<TestAbilityResolve>();
            ability.testResolves.Add(resolve);

            // Act
            bool result = abilityController.TryActivateAbility(ability);

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(resolve.applyCalled);
            Assert.AreEqual(testCharacter, resolve.lastCharacter);
            Assert.AreEqual(5, resolve.lastOutcome);
            Assert.AreEqual(testCharacter, action.lastCharacterExecuted);
        }

        [Test]
        public void CanActivateAbility_WithNullAbility_ReturnsFalse()
        {
            // Act
            bool result = abilityController.CanActivateAbility(null);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void CanActivateAbility_WhenCanAfford_ReturnsTrue()
        {
            // Arrange
            TestAbility ability = ScriptableObject.CreateInstance<TestAbility>();
            ability.canAffordResult = true;

            // Act
            bool result = abilityController.CanActivateAbility(ability);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanActivateAbility_WhenCannotAfford_ReturnsFalse()
        {
            // Arrange
            TestAbility ability = ScriptableObject.CreateInstance<TestAbility>();
            ability.canAffordResult = false;

            // Act
            bool result = abilityController.CanActivateAbility(ability);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void CheckTriggersReady_WithAllTriggersReady_ReturnsTrue()
        {
            // Arrange
            TestAbility ability = ScriptableObject.CreateInstance<TestAbility>();

            TestAbilityTrigger trigger1 = ScriptableObject.CreateInstance<TestAbilityTrigger>();
            trigger1.checkResult = true;

            TestAbilityTrigger trigger2 = ScriptableObject.CreateInstance<TestAbilityTrigger>();
            trigger2.checkResult = true;

            ability.testTriggers.Add(trigger1);
            ability.testTriggers.Add(trigger2);

            // Act
            bool result = CallPrivateMethod<bool>("CheckTriggersReady", ability);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(testCharacter, trigger1.lastCharacterChecked);
            Assert.AreEqual(testCharacter, trigger2.lastCharacterChecked);
        }

        [Test]
        public void TryActivateAbility_WithOneTriggerNotReady_ReturnsFalse()
        {
            // Arrange
            TestAbility ability = ScriptableObject.CreateInstance<TestAbility>();
            ability.canAffordResult = true;
            ability.payCostResult = true;

            TestAbilityTrigger trigger1 = ScriptableObject.CreateInstance<TestAbilityTrigger>();
            trigger1.checkResult = true;

            TestAbilityTrigger trigger2 = ScriptableObject.CreateInstance<TestAbilityTrigger>();
            trigger2.checkResult = false; // Этот триггер не готов!

            ability.triggers.Add(trigger1);
            ability.triggers.Add(trigger2);

            // Копируем в базовое поле
            CopyTriggersToBaseField(ability);

            // Проверяем что попало в базовое поле
            var baseTriggersField = typeof(Ability).GetField("triggers",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var baseTriggers = (List<AbilityTrigger>)baseTriggersField.GetValue(ability);

            Debug.Log($"Test triggers count: {ability.triggers.Count}");
            Debug.Log($"Base triggers count: {baseTriggers.Count}");
            Debug.Log($"First trigger result: {baseTriggers[0].CheckTrigger(testCharacter)}");

            // Act
            bool result = CallPrivateMethod<bool>("CheckTriggersReady", ability);

            Debug.Log($"Method result: {result}");

            // Assert
            Assert.IsFalse(result); // Должен вернуть false из-за триггера
        }

        // Вспомогательный метод
        private void CopyTriggersToBaseField(TestAbility ability)
        {
            var baseTriggersField = typeof(Ability).GetField("triggers",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var baseTriggers = (List<AbilityTrigger>)baseTriggersField.GetValue(ability);
            baseTriggers.Clear();
            baseTriggers.AddRange(ability.triggers);
        }

        [Test]
        public void PayAbilityCost_WhenCanPay_ReturnsTrue()
        {
            // Arrange
            TestAbility ability = ScriptableObject.CreateInstance<TestAbility>();
            ability.payCostResult = true;

            // Act
            bool result = CallPrivateMethod<bool>("PayAbilityCost", ability);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void PayAbilityCost_WhenCannotPay_ReturnsFalse()
        {
            // Arrange
            TestAbility ability = ScriptableObject.CreateInstance<TestAbility>();
            ability.payCostResult = false;

            // Act
            bool result = CallPrivateMethod<bool>("PayAbilityCost", ability);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void AddAbility_WithNewAbility_AddsToAvailableAbilities()
        {
            // Arrange
            TestAbility ability = ScriptableObject.CreateInstance<TestAbility>();

            // Act
            abilityController.AddAbility(ability);

            // Assert
            Assert.AreEqual(1, abilityController.GetAllAbilities().Count);
            Assert.Contains(ability, abilityController.GetAllAbilities());
        }

        [Test]
        public void AddAbility_WithNullAbility_DoesNotAdd()
        {
            // Act
            abilityController.AddAbility(null);

            // Assert
            Assert.AreEqual(0, abilityController.GetAllAbilities().Count);
        }

        [Test]
        public void AddAbility_WithDuplicateAbility_DoesNotAddDuplicate()
        {
            // Arrange
            TestAbility ability = ScriptableObject.CreateInstance<TestAbility>();

            // Act
            abilityController.AddAbility(ability);
            abilityController.AddAbility(ability);

            // Assert
            Assert.AreEqual(1, abilityController.GetAllAbilities().Count);
        }

        [Test]
        public void RemoveAbility_WithExistingAbility_RemovesIt()
        {
            // Arrange
            TestAbility ability = ScriptableObject.CreateInstance<TestAbility>();
            abilityController.AddAbility(ability);

            // Act
            abilityController.RemoveAbility(ability);

            // Assert
            Assert.AreEqual(0, abilityController.GetAllAbilities().Count);
        }

        [Test]
        public void GetAbilitiesByTag_WithMatchingTag_ReturnsFilteredList()
        {
            // Arrange
            TestAbility ability1 = ScriptableObject.CreateInstance<TestAbility>();
            TestAbility ability2 = ScriptableObject.CreateInstance<TestAbility>();

            // Используем reflection для установки тегов
            var tagField = typeof(Ability).GetField("tags", BindingFlags.NonPublic | BindingFlags.Instance);
            tagField.SetValue(ability1, new List<string> { "attack", "combat" });
            tagField.SetValue(ability2, new List<string> { "defense", "combat" });

            abilityController.AddAbility(ability1);
            abilityController.AddAbility(ability2);

            // Act
            var attackAbilities = abilityController.GetAbilitiesByTag("attack");
            var defenseAbilities = abilityController.GetAbilitiesByTag("defense");
            var combatAbilities = abilityController.GetAbilitiesByTag("combat");
            var healAbilities = abilityController.GetAbilitiesByTag("heal");

            // Assert
            Assert.AreEqual(1, attackAbilities.Count);
            Assert.AreEqual(1, defenseAbilities.Count);
            Assert.AreEqual(2, combatAbilities.Count);
            Assert.AreEqual(0, healAbilities.Count);
        }
    }
}