// Assets/Scripts/TestMocks/TestMocks.cs
using System.Collections.Generic;
using UnityEngine;
using AbilitySystem.AbilityComponents;

namespace AbilitySystem.Tests
{
    // Mock Character
    public class TestCharacter : Character
    {
        public bool canAfford = true;
        public bool canPay = true;

        public void SetupForTest(bool afford, bool pay)
        {
            canAfford = afford;
            canPay = pay;
        }
    }

    // Mock Ability
    public class TestAbility : Ability
    {
        public bool canAffordResult = true;
        public bool payCostResult = true;
        public bool triggersReady = true;
        public int executeResult = 1;

        // Важные поля которые были удалены!
        public List<TestAbilityTrigger> testTriggers = new List<TestAbilityTrigger>();
        public TestAbilityAction testAction;
        public List<TestAbilityResolve> testResolves = new List<TestAbilityResolve>();
        public List<string> testTags = new List<string>();

        public TestAbility()
        {
            // Инициализируем базовые поля через reflection
            var tagsField = typeof(Ability).GetField("tags",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (tagsField != null)
            {
                tagsField.SetValue(this, new List<string>());
            }

            var triggersField = typeof(Ability).GetField("triggers",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (triggersField != null)
            {
                triggersField.SetValue(this, new List<AbilityTrigger>());
            }
        }

        public override bool CanAfford(Character character) => canAffordResult;
        public override bool PayAllCost(Character character) => payCostResult;

        public override bool HasTag(string tag)
        {
            return testTags.Contains(tag);
        }

        // Методы для копирования тестовых данных в базовые поля
        public void CopyTestDataToBaseFields()
        {
            // Копируем триггеры
            var triggersField = typeof(Ability).GetField("triggers",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (triggersField != null)
            {
                var baseTriggers = (List<AbilityTrigger>)triggersField.GetValue(this);
                baseTriggers.Clear();
                foreach (var trigger in testTriggers)
                {
                    baseTriggers.Add(trigger);
                }
            }

            // Копируем action
            var actionField = typeof(Ability).GetField("action",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (actionField != null && testAction != null)
            {
                actionField.SetValue(this, testAction);
            }

            // Копируем resolves
            var resolvesField = typeof(Ability).GetField("resolves",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (resolvesField != null)
            {
                var baseResolves = (List<AbilityResolve>)resolvesField.GetValue(this);
                baseResolves.Clear();
                foreach (var resolve in testResolves)
                {
                    baseResolves.Add(resolve);
                }
            }
        }
    }

    // Mock AbilityTrigger
    public class TestAbilityTrigger : AbilityTrigger
    {
        public bool checkResult = true;
        public Character lastCharacterChecked;

        public override bool CheckTrigger(Character character)
        {
            lastCharacterChecked = character;
            return checkResult;
        }
    }

    // Mock AbilityAction
    public class TestAbilityAction : AbilityAction
    {
        public int executeResult = 1;
        public Character lastCharacterExecuted;

        public override int ExecuteAction(Character character)
        {
            lastCharacterExecuted = character;
            return executeResult;
        }
    }

    // Mock AbilityResolve
    public class TestAbilityResolve : AbilityResolve
    {
        public bool applyCalled = false;
        public Character lastCharacter;
        public int lastOutcome;

        public override void ApplyResolve(Character character, int outcome)
        {
            applyCalled = true;
            lastCharacter = character;
            lastOutcome = outcome;
        }
    }
}