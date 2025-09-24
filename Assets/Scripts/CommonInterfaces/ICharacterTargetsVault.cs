using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public interface ICharacterTargetsVault
{
    public bool HasTargetEnemy();
    public bool TryGetTargetEnemy(out GameObject targetEnemy);
    public bool TryGetTargetCharacter(out ICharacter targetCharacter);
    public bool TryGetTargetEnemyTransform(out Transform _transform);
    public GameObject GetTargetEnemy();
    public float ActualDistance { get; set; }
    public void UpdateDistances();
    public void SetTargetEnemyCharacter(ICharacter target);
    Vector3 GetWayPoint();
    public void SetWayPoint(Vector3 point);
    Vector3 GetCoordinates();

}
