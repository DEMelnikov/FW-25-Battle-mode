using Unity.Android.Gradle;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "TimedAbility", menuName = "FW25/Ability System/Ability/Timed Ability")]
public class TimedAbility : Ability
{
    [Header("Timer Settings:")]
    [SerializeField] private float _interval = 1f;
    private TimerTrigger timer;    

    public override void IniTialize(ICharacter character)
    {
        base.IniTialize(character);
        isTimed = true;

        timer = new TimerTrigger(
            duration: _interval,   
            onTick: () => TryActivateAbility(character, out int outcome),
            onStart: null,
            onComplete: null,
            looped: true);
    }

    public override void Unsubscribe(ICharacter character)
    {
        base.Unsubscribe(character);
        timer.UnsubscribeFromPauseEvents();
    }

    public override void OnUpdate(float deltatime)
    {
        base.OnUpdate(deltatime);
        timer.Update(deltatime);
    }
}
