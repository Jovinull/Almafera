using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Create new pokemon")]
public class PokemonBase : ScriptableObject
{
    [Header("Infos básicas")]
    [SerializeField] private string pokemonName;

    [TextArea]
    [SerializeField] private string description;

    [Header("Sprites")]
    [SerializeField] private Sprite frontSprite;
    [SerializeField] private Sprite backSprite;

    [Header("Tipos")]
    [SerializeField] private PokemonType type1;
    [SerializeField] private PokemonType type2;

    [Header("Base Stats")]
    [SerializeField] private int maxHp;
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField] private int spAttack;
    [SerializeField] private int spDefense;
    [SerializeField] private int speed;

    public string Name => pokemonName;
    public string Description => description;

    public Sprite FrontSprite => frontSprite;
    public Sprite BackSprite => backSprite;

    public PokemonType Type1 => type1;
    public PokemonType Type2 => type2;

    public int MaxHp => maxHp;
    public int Attack => attack;
    public int Defense => defense;
    public int SpAttack => spAttack;
    public int SpDefense => spDefense;
    public int Speed => speed;
}

public enum PokemonType
{
    None,
    Normal,
    Fire,
    Water,
    Electric,
    Grass,
    Ice,
    Fighting,
    Poison,
    Ground,
    Flying,
    Psychic,
    Bug,
    Rock,
    Ghost,
    Dragon
}
