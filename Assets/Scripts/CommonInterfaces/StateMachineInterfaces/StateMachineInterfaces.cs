using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public interface IStateMachine
{
    void Initialize();
    void SetState(FSMStates newState);
    IStateContext Context { get; }
    CharacterGlobalState CharacterGoal { get; set;    }
    //void SetInitialState();
    GameObject GetGameObject();
}

public interface IStateContext
{
    GameObject Owner { get; }
    ICharacter GetCharacter();
    IAbilityController GetAbilityController();

}

//public interface ITransition
//{
//    Decision decision { get; }
//    State trueState { get; }
//}

//public interface IState
//{
//    string name {  get; }
//    // Для доступа к списку переходов
//    List<ITransition> GetTransitions();

//    // Методы жизненного цикла состояния
//    void OnEnter(IStateMachine machine);
//    void OnUpdate(IStateMachine machine);
//    void OnFixedUpdate(IStateMachine machine);
//    void OnExit(IStateMachine machine);

//    // Метод проверки переходов (можно сделать protected в классе, но в интерфейсе public)
//    void CheckTransitions(IStateMachine machine);
//}

