
using UnityEngine;


//Constant copair mode

public abstract class CheckStatValue : Trigger
{
    [SerializeField] protected StatTag _statToCheck = StatTag.ENERGY;
    [SerializeField] protected RelationType _relationType = RelationType.Lesser;
    [SerializeField] protected bool isInPercantage = true;
    [SerializeField] protected float _equalityTolerance = 0.5f; // Допуск для сравнения на равенство
}
