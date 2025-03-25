using Command.Player;

public abstract class UnitCommand : ICommand
{
    public int ActorUnitID;
    public int TargetUnitID;
    public int ActorPlayerID;
    public int TargetPlayerID;

    protected UnitController actorUnit;
    protected UnitController targetUnit;

    
    public abstract void Execute();

    public abstract bool WillHitTarget();
}