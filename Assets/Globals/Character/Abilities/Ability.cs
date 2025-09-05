using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewAbility", menuName = "Abilities/Ability")]
public class Ability : ScriptableObject
{
    [Header("Basic Info")]
    public string abilityName = "New Ability";
    [TextArea] public string description = "Ability description";
    public Sprite icon;
    public AbilityType type = AbilityType.Active;

    [Header("Tags & Categories")]
    public List<string> tags = new List<string>();

    [Header("Timing Settings")]
    public float cooldown = 5f;
    public float duration = 0f;
    public bool isCyclical = false;
    public float cycleInterval = 1f;
    public int maxCycles = -1; // -1 = ����������

    [Header("Activation Requirements")]
    public List<AbilityRequirement> requirements = new List<AbilityRequirement>();
    public List<ConditionType> activationConditions = new List<ConditionType>();

    [Header("Resource Costs")]
    public float healthCost = 0f;
    public float energyCost = 0f;

    [Header("Visual & Audio")]
    public GameObject visualEffect;
    public AudioClip activationSound;

    // Runtime data
    [System.NonSerialized] public AbilityState state = AbilityState.Ready;
    [System.NonSerialized] public bool isActive = false;
    [System.NonSerialized] public TimerTrigger cooldownTimer;
    [System.NonSerialized] public TimerTrigger durationTimer;
    [System.NonSerialized] public TimerTrigger cycleTimer;
    [System.NonSerialized] private Character owner;

    // �������
    public event System.Action OnAbilityActivated;
    public event System.Action OnAbilityDeactivated;
    public event System.Action OnCooldownStarted;
    public event System.Action OnCooldownEnded;
    public event System.Action OnCycleTick;
    public event System.Action OnDurationEnded;

    public void Initialize(Character character)
    {
        owner = character;

        // ������������� �������� � ����� TimerTrigger
        cooldownTimer = new TimerTrigger(
            duration: cooldown,
            onTick: null,
            onStart: HandleCooldownStart,
            onComplete: HandleCooldownEnd,
            looped: false
        );

        durationTimer = new TimerTrigger(
            duration: duration,
            onTick: null,
            onStart: null,
            onComplete: HandleDurationEnd,
            looped: false
        );

        if (isCyclical && cycleInterval > 0)
        {
            cycleTimer = new TimerTrigger(
                duration: cycleInterval,
                onTick: HandleCycleTick,
                onStart: null,
                onComplete: null,
                looped: true,
                maxLoops: maxCycles
            );
        }
    }

    public virtual bool CanActivate(Character character)
    {
        if (state != AbilityState.Ready)
        {
            Debug.Log($"{abilityName}: Not ready (state: {state})");
            return false;
        }

        //if (character.Health.Current <= healthCost)
        //{
        //    Debug.Log($"{abilityName}: Not enough health");
        //    return false;
        //}

        // �������� ����������
        foreach (var requirement in requirements)
        {
            if (requirement != null && !requirement.CheckRequirement(character))
            {
                Debug.Log($"{abilityName}: Requirement not met: {requirement.requirementName}");
                return false;
            }
        }

        // �������� ������� ���������
        foreach (var condition in activationConditions)
        {
            if (condition != null && !condition.CheckCondition(character))
            {
                Debug.Log($"{abilityName}: Condition not met: {condition.conditionName}");
                return false;
            }
        }

        return true;
    }

    public virtual bool Activate(Character character)
    {
        if (!CanActivate(character))
        {
            Debug.Log($"{abilityName}: Cannot activate");
            return false;
        }

        //// ������ ���������
        //if (healthCost > 0)
        //{
        //    character.Health.TakeDamage(healthCost, null);
        //    Debug.Log($"{abilityName}: Paid {healthCost} health");
        //}

        // ������ ��������
        if (cooldown > 0)
        {
            state = AbilityState.Cooldown;
            cooldownTimer.Restart();
            Debug.Log($"{abilityName}: Cooldown started");
        }

        // ������ ������������ ��� ��������� ������������
        if (duration > 0)
        {
            state = AbilityState.Active;
            isActive = true;
            durationTimer.Restart();
            Debug.Log($"{abilityName}: Duration started");
        }
        else
        {
            // ���������� ����������� ����� ������������ � Ready
            state = AbilityState.Ready;
        }

        // ������ ������������ �������
        if (isCyclical && cycleTimer != null)
        {
            cycleTimer.Restart();
            Debug.Log($"{abilityName}: Cycle timer started");
        }

        // ����� �������� ������ ���������
        bool activationResult = OnActivate(character);

        if (activationResult)
        {
            OnAbilityActivated?.Invoke();
            Debug.Log($"{abilityName}: Activated successfully");

            // ��������� � �������� ����������� ���� ��� �� ����������
            if (duration > 0)
            {
                AbilityManager.RegisterActiveAbility(this);
            }
        }

        return activationResult;
    }

    protected virtual bool OnActivate(Character character)
    {
        // ������� ���������� - �������������� � �����������
        return true;
    }

    public virtual void UpdateAbility(float deltaTime)
    {
        // ���������� ���� ��������
        cooldownTimer?.Update(deltaTime);
        durationTimer?.Update(deltaTime);
        cycleTimer?.Update(deltaTime);
    }

    private void HandleCooldownStart()
    {
        OnCooldownStarted?.Invoke();
    }

    private void HandleCooldownEnd()
    {
        state = AbilityState.Ready;
        OnCooldownEnded?.Invoke();
        Debug.Log($"{abilityName}: Cooldown ended");
    }

    private void HandleDurationEnd()
    {
        Deactivate(owner);
        OnDurationEnded?.Invoke();
        Debug.Log($"{abilityName}: Duration ended");
    }

    private void HandleCycleTick()
    {
        if (isActive && owner != null)
        {
            OnCycle(owner);
            OnCycleTick?.Invoke();
            Debug.Log($"{abilityName}: Cycle tick");
        }
    }

    protected virtual void OnCycle(Character character)
    {
        // ������ ��� ����������� �������� - �������������� � �����������
    }

    public virtual void Deactivate(Character character)
    {
        if (!isActive && state != AbilityState.Active) return;

        isActive = false;
        state = AbilityState.Ready;

        // ��������� ��������
        durationTimer?.Stop();
        cycleTimer?.Stop();

        // ����� ������ �����������
        OnDeactivate(character);

        OnAbilityDeactivated?.Invoke();

        // �������� �� �������� ������������
        AbilityManager.UnregisterActiveAbility(this);

        Debug.Log($"{abilityName}: Deactivated");
    }

    protected virtual void OnDeactivate(Character character)
    {
        // ������ ������� - �������������� � �����������
    }

    // ������ ��� UI � �������� �������
    public float GetCooldownProgress() => cooldownTimer?.Progress ?? 0f;
    public float GetDurationProgress() => durationTimer?.Progress ?? 0f;
    public float GetRemainingCooldown() => cooldownTimer?.RemainingTime ?? 0f;
    public float GetRemainingDuration() => durationTimer?.RemainingTime ?? 0f;
    public float GetElapsedInCycle() => cycleTimer?.ElapsedInCycle ?? 0f;
    public float GetCycleProgress() => cycleTimer?.Progress ?? 0f;
    public int GetCyclesCompleted() => cycleTimer?.LoopsCompleted ?? 0;

    public void PauseAbility()
    {
        durationTimer?.Pause();
        cycleTimer?.Pause();
    }

    public void ResumeAbility()
    {
        durationTimer?.Resume();
        cycleTimer?.Resume();
    }

    public void CancelAbility()
    {
        Deactivate(owner);
    }

    // ������ ��� �������� �������
    public bool IsOnCooldown() => state == AbilityState.Cooldown;
    public bool IsActive() => state == AbilityState.Active;
    public bool IsReady() => state == AbilityState.Ready;
    public bool IsCycling() => cycleTimer?.IsRunning ?? false;

    // ��������� ���������� �� ����
    public void SetCooldown(float newCooldown)
    {
        if (cooldownTimer != null)
        {
            cooldownTimer.SetDuration(newCooldown);
        }
        cooldown = newCooldown;
    }

    public void SetDuration(float newDuration)
    {
        if (durationTimer != null)
        {
            durationTimer.SetDuration(newDuration);
        }
        duration = newDuration;
    }

    public void SetCycleInterval(float newInterval)
    {
        if (cycleTimer != null)
        {
            cycleTimer.SetDuration(newInterval);
        }
        cycleInterval = newInterval;
    }

    // ������� ��� �����������
    public void Cleanup()
    {
        cooldownTimer = null;
        durationTimer = null;
        cycleTimer = null;
        owner = null;

        // ������� �������
        OnAbilityActivated = null;
        OnAbilityDeactivated = null;
        OnCooldownStarted = null;
        OnCooldownEnded = null;
        OnCycleTick = null;
        OnDurationEnded = null;
    }
}

// �������� �������� ������������
public static class AbilityManager
{
    private static List<Ability> activeAbilities = new List<Ability>();

    public static void RegisterActiveAbility(Ability ability)
    {
        if (ability != null && !activeAbilities.Contains(ability))
        {
            activeAbilities.Add(ability);
        }
    }

    public static void UnregisterActiveAbility(Ability ability)
    {
        if (ability != null)
        {
            activeAbilities.Remove(ability);
        }
    }

    public static void UpdateAbilities(float deltaTime)
    {
        for (int i = activeAbilities.Count - 1; i >= 0; i--)
        {
            var ability = activeAbilities[i];
            if (ability != null && ability.isActive)
            {
                ability.UpdateAbility(deltaTime);
            }
            else
            {
                activeAbilities.RemoveAt(i);
            }
        }
    }

    public static List<Ability> GetActiveAbilities() => new List<Ability>(activeAbilities);
    public static int GetActiveAbilityCount() => activeAbilities.Count;

    public static void PauseAllAbilities()
    {
        foreach (var ability in activeAbilities)
        {
            ability?.PauseAbility();
        }
    }

    public static void ResumeAllAbilities()
    {
        foreach (var ability in activeAbilities)
        {
            ability?.ResumeAbility();
        }
    }

    public static void CancelAllAbilities()
    {
        foreach (var ability in activeAbilities)
        {
            ability?.CancelAbility();
        }
        activeAbilities.Clear();
    }
}