public class Move
{
    /// <summary>
    /// Dados base do golpe (tipo, poder, precisão, etc.).
    /// </summary>
    public MoveBase Base { get; set; }

    /// <summary>
    /// Quantidade atual de PP disponível para este golpe.
    /// </summary>
    public int PP { get; set; }

    public Move(MoveBase moveBase)
    {
        Base = moveBase;
        PP = moveBase.PP;
    }
}
