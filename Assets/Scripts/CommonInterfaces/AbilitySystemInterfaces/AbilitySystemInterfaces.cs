using System.Collections.Generic;
using Unity.Android.Gradle;
using UnityEditor.Playables;
using UnityEngine;


public interface IAbility
{
    bool TryActivateAbility(ICharacter character, out int outcome) {   outcome = 0; return false; }
    bool HasTag(string tag) {  return false; }
    string GetAbilityName() { return ""; }
    IAbility Clone();
    string GetName {  get; }
}

public interface IAbilityController
{
    void Initialize();
    void TryExecuteAttackAbility();
    void TryExecuteDegenceAbility();
    string DefenceAbilityName {  get; } 

}

public interface IAction
{
    int ExecuteAction(ICharacter character) { return 0; }
    int ExecuteAction(ICharacter character, float myDC) { return 0; }
}

