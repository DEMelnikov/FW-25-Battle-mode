using UnityEngine;

[System.Serializable]
public class Transition
{
    [SerializeField] public Decision decision;
    [SerializeField] public State trueState;
}