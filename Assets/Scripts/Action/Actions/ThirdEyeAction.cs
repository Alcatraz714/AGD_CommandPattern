using Command.Input;
using Command.Main;
using Command.Player;
using UnityEngine;

namespace Command.Actions
{
    public class ThirdEyeAction : IAction
    {
        private UnitController actorUnit;
        private UnitController targetUnit;
        public TargetType TargetType => TargetType.Self;

        public void PerformAction(UnitController actorUnit, UnitController targetUnit)
        {
            this.actorUnit = actorUnit;
            this.targetUnit = targetUnit;

            actorUnit.PlayBattleAnimation(CommandType.BerserkAttack, CalculateMovePosition(targetUnit), OnActionAnimationCompleted);
        }

        public void OnActionAnimationCompleted()
        {
            if (IsSuccessful())
            {
                int healthToConvert = (int)(targetUnit.CurrentHealth * 0.25f);
                targetUnit.TakeDamage(healthToConvert);
                targetUnit.CurrentPower += healthToConvert;
            }
            else
                GameService.Instance.UIService.ActionMissed();
        }

        public bool IsSuccessful() => true;

        public Vector3 CalculateMovePosition(UnitController targetUnit) => targetUnit.GetEnemyPosition();
    }
}