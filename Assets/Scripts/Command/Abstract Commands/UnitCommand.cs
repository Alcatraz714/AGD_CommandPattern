using Command.Player;

public abstract class UnitCommand : ICommand
{
    public int ActorUnitID;
    public int TargetUnitID;
    public int ActorPlayerID;
    public int TargetPlayerID;

    public CommandData(int ActorUnitID, int TargetUnitID,int ActorPlayerID, int TargetPlayerID)
        {
            this.ActorUnitID = ActorUnitID;
            this.TargetUnitID = TargetUnitID;
            this.ActorPlayerID = ActorPlayerID;
            this.TargetPlayerID = TargetPlayerID;
        }

    protected UnitController actorUnit;
    protected UnitController targetUnit;

    
    public abstract void Execute();

    public abstract bool WillHitTarget();
}