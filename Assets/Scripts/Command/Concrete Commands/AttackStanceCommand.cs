using Command.Main;
using Command.Actions;

namespace Command.Commands
{
    public class AttackStanceCommand : UnitCommand
    {
        private bool willHitTarget;

        public AttackStanceCommand(CommandData commandData)
        {
            this.commandData = commandData;
            willHitTarget = WillHitTarget();
        }

        public override void Execute() => GameService.Instance.ActionService.GetActionByType(CommandType.AttackStance).PerformAction(actorUnit, targetUnit, willHitTarget);
        public override void Undo()
        {
            if(willHitTarget)
            {
                targetUnit.CurrentPower -= (int)(targetUnit.CurrentPower * 0.2f); // reduce the power increase since we are no longer in attack stance
                actorUnit.Owner.ResetCurrentActiveUnit();
            }
        }

        public override bool WillHitTarget() => true;
    }
}