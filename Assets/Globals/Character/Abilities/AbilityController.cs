using System.Collections.Generic;
using UnityEngine;

namespace FW25.Abilities
{
    public class AbilityController : MonoBehaviour
    {
        [SerializeField] private Character _owner;
        [SerializeField] private CharacterStatsController _stats; // <— добавили
        [SerializeField] private List<AbilityDefinition> startingAbilities;
         private StateMaschine _stateMachine;

        private readonly Dictionary<string, AbilityRuntime> _learned = new();

        public Character Owner => _owner;
        public CharacterStatsController Stats => _stats;

        private void Reset()
        {
            _owner = GetComponent<Character>();
            _stats = GetComponent<CharacterStatsController>(); // <—
            _stateMachine = _owner.StateMaschine;
        }

        private void Awake()
        {
            if (_owner == null) _owner = GetComponent<Character>();
            if (_stats == null) _stats = GetComponent<CharacterStatsController>(); // <—

            //if (_stateMachine != null)
            //{
            //    _stateMachine.OnStateEnter += HandleStateEnter;
            //    _stateMachine.OnStateExit += HandleStateExit;
            //}

            if (startingAbilities != null)
            {
                foreach (var def in startingAbilities)
                    if (def != null) Learn(def);
            }
        }

        public bool Learn(AbilityDefinition def)
        {
            if (def == null || string.IsNullOrEmpty(def.id) || _learned.ContainsKey(def.id))
                return false;

            var rt = new AbilityRuntime
            {
                def = def,
                owner = _owner,
                stats = _stats // <—
            };
            _learned.Add(def.id, rt);

            if (def.kind == AbilityKind.Passive || def.trigger == AbilityTriggerType.OnLearn)
            {
                foreach (var eff in def.effects) eff?.OnStart(rt.Ctx);
            }
            return true;
        }

        public bool Forget(string id)
        {
            if (!_learned.TryGetValue(id, out var rt)) return false;

            foreach (var eff in rt.def.effects) eff?.OnStop(rt.Ctx);
            _learned.Remove(id);
            return true;
        }

        private void Update()
        {
            float now = Time.time;
            float dt = Time.deltaTime;

            foreach (var kv in _learned)
            {
                var rt = kv.Value;
                var def = rt.def;

                if (!rt.isActive) continue;

                // Тики
                if (def.tickPeriod > 0f && now >= rt.nextTickTime)
                {
                    if (!SpendCosts(def, perTick: true))
                    {
                        StopAbility(rt);
                        continue;
                    }

                    foreach (var eff in def.effects) eff?.OnTick(rt.Ctx, def.tickPeriod);
                    rt.nextTickTime = now + def.tickPeriod;
                }

                // Длительность
                if (def.duration > 0f)
                {
                    rt.timeRemaining -= dt;
                    if (rt.timeRemaining <= 0f)
                    {
                        StopAbility(rt);
                    }
                }
            }
        }

        // Интеграция со стейт‑машиной
        //private void HandleStateEnter(CharacterState state)
        //{
        //    // TODO: если у вас другой enum — адаптируйте
        //    if (state == CharacterState.Attack)
        //    {
        //        var rt = ResolveAttackAbility();
        //        if (rt != null) TryActivate(rt.def.id, triggerCall: true);
        //    }
        //}

        //private void HandleStateExit(CharacterState state)
        //{
        //    if (state == CharacterState.Attack)
        //    {
        //        var rt = ResolveAttackAbility();
        //        if (rt != null && rt.isActive && rt.def.looping) StopAbility(rt);
        //    }
        //}

        // Если нет событий, вызывайте эти методы из вашей Attack‑логики
        public void TriggerAttackStart()
        {
            var rt = ResolveAttackAbility();
            if (rt != null) TryActivate(rt.def.id, triggerCall: true);
        }

        public void TriggerAttackStop()
        {
            var rt = ResolveAttackAbility();
            if (rt != null && rt.isActive && rt.def.looping) StopAbility(rt);
        }

        private AbilityRuntime ResolveAttackAbility()
        {
            // Пример под оружие: теги оружия должны совпадать с тегами способности
            //var weaponTag = _owner.Equipment?.EquippedWeapon?.Tags ?? AbilityTag.None;

            //foreach (var rt in _learned.Values)
            //{
            //    var def = rt.def;
            //    if (def.trigger != AbilityTriggerType.OnStateEnter_Attack) continue;
            //    if ((def.tags & AbilityTag.Attack) == 0) continue;

            //    bool weaponOk = (weaponTag == AbilityTag.None) || ((def.tags & weaponTag) != 0);
            //    if (!weaponOk) continue;

            //    if (!IsOnCooldown(rt) && AreRequirementsMet(def))
            //        return rt;
            //}
            return null;
        }

        public bool TryActivate(string abilityId, bool triggerCall = false)
        {
            if (!_learned.TryGetValue(abilityId, out var rt)) return false;
            var def = rt.def;

            if (rt.isActive) return false;
            if (IsOnCooldown(rt)) return false;

            if (!triggerCall && def.trigger != AbilityTriggerType.Manual && def.kind != AbilityKind.Passive)
                return false;

            if (!AreRequirementsMet(def)) return false;

            if (!SpendCosts(def, perTick: false)) return false;

            // Старт
            rt.isActive = true;
            rt.timeRemaining = def.duration;
            rt.nextTickTime = Time.time + Mathf.Max(0.0001f, def.tickPeriod);

            foreach (var eff in def.effects) eff?.OnStart(rt.Ctx);

            // Мгновенные без длительности/лупа — сразу отработать и выключить
            if (!def.looping && def.duration <= 0f)
            {
                foreach (var eff in def.effects) eff?.OnTick(rt.Ctx, 0f);
                StopAbility(rt);
            }

            return true;
        }

        public void StopAbility(string id)
        {
            if (_learned.TryGetValue(id, out var rt)) StopAbility(rt);
        }

        private void StopAbility(AbilityRuntime rt)
        {
            if (!rt.isActive) return;

            foreach (var eff in rt.def.effects) eff?.OnStop(rt.Ctx);
            rt.isActive = false;
            rt.cooldownEndsAt = Time.time + rt.def.cooldown;
        }

        private bool IsOnCooldown(AbilityRuntime rt) => Time.time < rt.cooldownEndsAt;

        private bool AreRequirementsMet(AbilityDefinition def)
        {
            if (def.activateRequirements == null) return true;
            foreach (var req in def.activateRequirements)
            {
                if (req == null) continue;
                if (!req.IsMet(_owner, out _)) return false;
            }
            return true;
        }

        private bool SpendCosts(AbilityDefinition def, bool perTick)
        {
            if (def.costs == null) return true;

            // 1) Проверка достаточности
            for (int i = 0; i < def.costs.Length; i++)
            {
                var c = def.costs[i];
                if (c.perTick != perTick) continue;
                //var r = _owner.Resources.Get(c.type); // TODO: подстроить под ваши ресурсы
                //if (!r.CanSpend(c.amount))
                    return false;
            }

            // 2) Списание
            for (int i = 0; i < def.costs.Length; i++)
            {
                var c = def.costs[i];
                if (c.perTick != perTick) continue;
                //_owner.Resources.Get(c.type).Spend(c.amount);
            }

            return true;
        }

        // API для UI/магазина
        public IReadOnlyCollection<AbilityRuntime> Learned => _learned.Values;
        public bool IsLearned(string id) => _learned.ContainsKey(id);

        public bool PurchaseAndLearn(AbilityDefinition def, int priceCurrency)
        {
            // TODO: интегрируйте с вашей экономикой (деньги/опыт), затем Learn(def)
            // if (!Owner.Wallet.TrySpend(priceCurrency)) return false;
            return Learn(def);
        }


        // Остальной код без изменений (Update/TryActivate/StopAbility/ResolveAttackAbility/SpendCosts/…)
        // …
    }
}