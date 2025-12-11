using UnityEngine;

public class Pokemon
{
    private readonly PokemonBase _pokemonBase;
    private readonly int _level;

    public Pokemon(PokemonBase pBase, int pLevel)
    {
        _pokemonBase = pBase;
        _level = pLevel;
    }

    public PokemonBase Base => _pokemonBase;
    public int Level => _level;

    // Fórmulas de stats
    public int Attack =>
        Mathf.FloorToInt((_pokemonBase.Attack * _level) / 100f) + 5;

    public int Defense =>
        Mathf.FloorToInt((_pokemonBase.Defense * _level) / 100f) + 5;

    public int SpAttack =>
        Mathf.FloorToInt((_pokemonBase.SpAttack * _level) / 100f) + 5;

    public int SpDefense =>
        Mathf.FloorToInt((_pokemonBase.SpDefense * _level) / 100f) + 5;

    public int Speed =>
        Mathf.FloorToInt((_pokemonBase.Speed * _level) / 100f) + 5;

    public int MaxHp =>
        Mathf.FloorToInt((_pokemonBase.MaxHp * _level) / 100f) + 10;
}
