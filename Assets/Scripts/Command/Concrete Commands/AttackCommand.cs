using Command.Main;
using Command.Actions;

namespace Command.Commands
{
    public class AttackCommand : UnitCommand
    {
        private bool willHitTarget;

        public AttackCommand(CommandData commandData)
        {
            this.commandData = commandData;
            willHitTarget = WillHitTarget();
        }

        public override bool WillHitTarget() => true;

        public override void Execute() => GameService.Instance.ActionService.GetActionByType(CommandType.Attack).PerformAction(actorUnit, targetUnit, willHitTarget);

        public override void Undo()
        {
            if (willHitTarget)
            {
                if (!targetUnit.IsAlive())
                {
                    targetUnit.Revive();
                }
            }
            // we revive the unit first if unit was already dead before healing
            targetUnit.RestoreHealth(actorUnit.CurrentPower);
            actorUnit.Owner.ResetCurrentActiveUnit();
        }
    }
}
