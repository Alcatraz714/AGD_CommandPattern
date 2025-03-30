using Command.Main;

namespace Command.Commands
{
    public class HealCommand : UnitCommand
    {
        private bool willHitTarget;

        public HealCommand(CommandData commandData)
        {
            this.commandData = commandData;
            willHitTarget = WillHitTarget();
        }

        public override void Execute() => GameService.Instance.ActionService.GetActionByType(CommandType.Heal).PerformAction(actorUnit, targetUnit, willHitTarget);
        public override void Undo()
        {
            if (willHitTarget)
            {
                targetUnit.TakeDamage(actorUnit.CurrentPower);
                actorUnit.Owner.ResetCurrentActiveUnit();
            }
        }

        public override bool WillHitTarget() => true;
    }
}