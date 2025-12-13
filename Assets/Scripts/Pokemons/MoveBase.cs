using UnityEngine;

[CreateAssetMenu(
    fileName = "Move",
    menuName = "Pokemon/Create New Move"
)]
public class MoveBase : ScriptableObject
{
    [Header("Basic Info")]

    [SerializeField]
    private string nameMove;

    [TextArea]
    [SerializeField]
    private string description;

    [Header("Battle Data")]

    [SerializeField]
    private PokemonType type;

    [Tooltip("Dano base do golpe. 0 significa que não causa dano direto.")]
    [Min(0)]
    [SerializeField]
    private int power;

    [Tooltip("Chance de acerto de 0 a 100.")]
    [Range(0, 100)]
    [SerializeField]
    private int accuracy = 100;

    [Tooltip("Quantidade de vezes que o golpe pode ser usado.")]
    [Min(0)]
    [SerializeField]
    private int pp = 10;

    public string Name => nameMove;
    public string Description => description;
    public PokemonType Type => type;
    public int Power => power;
    public int Accuracy => accuracy;
    public int PP => pp;
}
