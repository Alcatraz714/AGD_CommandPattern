using Command.Main;

namespace Command.Commands
{
    public class ThirdEyeCommand : UnitCommand
    {
        private bool willHitTarget;
        private int previousHealth;

        public ThirdEyeCommand(CommandData commandData)
        {
            this.commandData = commandData;
            willHitTarget = WillHitTarget();
        }

        public override void Execute()
        {
            previousHealth = targetUnit.CurrentHealth;
            GameService.Instance.ActionService.GetActionByType(CommandType.ThirdEye).PerformAction(actorUnit, targetUnit, willHitTarget);
        }

        public override void Undo()
        {
            if(!targetUnit.IsAlive())
                targetUnit.Revive();
            // we restore the damage done based on target health
            int healthToRestore = (int)(previousHealth * 0.25f);
            targetUnit.RestoreHealth(healthToRestore);
            // we then reduce the extra power as well which was a side effect of this action
            targetUnit.CurrentPower -= healthToRestore;
            actorUnit.Owner.ResetCurrentActiveUnit();
        }

        public override bool WillHitTarget() => true;
    } 
}