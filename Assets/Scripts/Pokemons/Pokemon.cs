using System.Collections.Generic;
using UnityEngine;

public class Pokemon
{
    private readonly PokemonBase _pokemonBase;
    private readonly int _level;

    public Pokemon(PokemonBase pBase, int pLevel)
    {
        _pokemonBase = pBase;
        _level = pLevel;

        HP = MaxHp;

        Moves = new List<Move>();
        foreach (var learnable in _pokemonBase.LearnableMoves)
        {
            if (learnable.Level <= _level)
                Moves.Add(new Move(learnable.Base));

            if (Moves.Count >= 4)
                break;
        }
    }

    public PokemonBase Base => _pokemonBase;
    public int Level => _level;

    public int HP { get; set; }
    public List<Move> Moves { get; set; }

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
