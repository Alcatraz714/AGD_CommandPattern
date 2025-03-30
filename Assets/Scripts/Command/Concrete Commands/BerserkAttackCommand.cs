using Command.Main;
using UnityEngine;

namespace Command.Commands
{
    public class BerserkAttackCommand : UnitCommand
    {
        private bool willHitTarget;
        private const float hitChance = 0.66f;

        public BerserkAttackCommand(CommandData commandData)
        {
            this.commandData = commandData;
            willHitTarget = WillHitTarget();
        }

        public override void Execute() => GameService.Instance.ActionService.GetActionByType(CommandType.BerserkAttack).PerformAction(actorUnit, targetUnit, willHitTarget);
        public override void Undo()
        {
            if(willHitTarget)
            {
                if(!targetUnit.IsAlive())
                {
                    targetUnit.Revive();
                }
                // if berserk was succesful we restore enemy health based on the berserk damage
                targetUnit.RestoreHealth(actorUnit.CurrentPower * 2);
            }
            else
            {
                if(!actorUnit.IsAlive())
                {
                    actorUnit.Revive();
                }
                // if berserk was failed we restore this unit's health based on the berserk damage on fail
                actorUnit.RestoreHealth(actorUnit.CurrentPower * 2);
            }
            actorUnit.Owner.ResetCurrentActiveUnit();
        }

        public override bool WillHitTarget() => Random.Range(0f, 1f) < hitChance;
    }
}