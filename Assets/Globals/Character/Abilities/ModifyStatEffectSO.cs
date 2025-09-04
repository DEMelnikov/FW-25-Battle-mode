using UnityEngine;

namespace FW25.Abilities 
{
    [CreateAssetMenu(menuName = "FW25/Abilities/Effects/ModifyStat")]
    public class ModifyStatEffectSO : AbilityEffectSO
    {
        [Tooltip("ID вашей характеристики (например MoveSpeed/AttackSpeed/Damage)")]
        public StatTag statTag;
        public float add;
        public float mult = 1f;

        public override void OnStart(AbilityContext ctx)
        {
            // Замените на реальные методы вашего CharacterStatsController
            // Пример, если у вас API поддерживает модификаторы с источником:
            // ctx.Stats.AddModifier(statId, add, mult, ctx.Runtime);
            //
            // Если API другой — напишите небольшой адаптер/переключатель тут.


            //ctx.Stats.AddModifier(statId, add, mult, ctx.Runtime); // TODO: заменить под ваш метод
            Debug.Log("Trying update Stat");
        }

        public override void OnStop(AbilityContext ctx)
        {
            // Снимаем модификатор, привязанный к конкретному источнику (Runtime)
            // Пример:
            // ctx.Stats.RemoveModifierBySource(statId, ctx.Runtime);
            //ctx.Stats.RemoveModifierBySource(statId, ctx.Runtime); // TODO: заменить под ваш метод
            Debug.Log("Stop Trying update Stat");
        }
    }
}

