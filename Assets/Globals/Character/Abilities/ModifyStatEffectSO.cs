using UnityEngine;

namespace FW25.Abilities 
{
    [CreateAssetMenu(menuName = "FW25/Abilities/Effects/ModifyStat")]
    public class ModifyStatEffectSO : AbilityEffectSO
    {
        [Tooltip("ID ����� �������������� (�������� MoveSpeed/AttackSpeed/Damage)")]
        public StatTag statTag;
        public float add;
        public float mult = 1f;

        public override void OnStart(AbilityContext ctx)
        {
            // �������� �� �������� ������ ������ CharacterStatsController
            // ������, ���� � ��� API ������������ ������������ � ����������:
            // ctx.Stats.AddModifier(statId, add, mult, ctx.Runtime);
            //
            // ���� API ������ � �������� ��������� �������/������������� ���.


            //ctx.Stats.AddModifier(statId, add, mult, ctx.Runtime); // TODO: �������� ��� ��� �����
            Debug.Log("Trying update Stat");
        }

        public override void OnStop(AbilityContext ctx)
        {
            // ������� �����������, ����������� � ����������� ��������� (Runtime)
            // ������:
            // ctx.Stats.RemoveModifierBySource(statId, ctx.Runtime);
            //ctx.Stats.RemoveModifierBySource(statId, ctx.Runtime); // TODO: �������� ��� ��� �����
            Debug.Log("Stop Trying update Stat");
        }
    }
}

