namespace FW25.Abilities
{
    public class AbilityRuntime
    {
        public AbilityDefinition def;
        public Character owner;
        public CharacterStatsController stats; // <Ч добавили

        public bool isActive;
        public float timeRemaining;
        public float nextTickTime;
        public float cooldownEndsAt;

        private AbilityContext _ctx;
        public AbilityContext Ctx => _ctx ??= new AbilityContext
        {
            Owner = owner,
            Stats = stats,
            Definition = def,
            Runtime = this
        };

        public void ResetRuntime()
        {
            isActive = false;
            timeRemaining = 0f;
            nextTickTime = 0f;
            cooldownEndsAt = 0f;
        }
    }
}