using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "Characters/Character Stats")]
public class SO_CharacterStats : ScriptableObject
{
    [Header("Сила")]
    [SerializeField] private string _nameSTR = "Strength";
    [SerializeField] private StatTag _tagSTR = StatTag.Strength;
    [SerializeField] private float _baseValueSTR = 5f;
    [SerializeField] private bool _alarmBZ_STR = true;

    [Header("Health")]
    [SerializeField] private string _nameHP = "HP";
    [SerializeField] private StatTag _tagHP = StatTag.Health;
    [SerializeField] private float _baseValueHP = 100f;
    [SerializeField] private bool _alarmBZ_HP = true;

    [Header("Energy")]
    [SerializeField] private string _nameEP = "EP";
    [SerializeField] private StatTag _tagEP = StatTag.Energy;
    [SerializeField] private float _baseValueEP = 100f;
    [SerializeField] private bool _alarmBZ_EP = true;

    public string NameSTR => _nameSTR;
    public StatTag TagSTR => _tagSTR;
    public float BaseValueSTR => _baseValueSTR;
    public bool AlarmBZ_STR => _alarmBZ_STR;

    public string NameHP { get => _nameHP; }
    public StatTag TagHP { get => _tagHP; }
    public float BaseValueHP { get => _baseValueHP; }
    public bool AlarmBZ_HP { get => _alarmBZ_HP; }

    public string NameEP { get => _nameEP; }
    public StatTag TagEP { get => _tagEP; }
    public float BaseValueEP { get => _baseValueEP; }
    public bool AlarmBZ_EP { get => _alarmBZ_EP; }



    //public SceneObjectTag SceneObjectTag => _sceneObjectTag;
    //Stats.Add(StatTag.Strength, new Stat("Strength", StatTag.Strength, 5f,true));
}
