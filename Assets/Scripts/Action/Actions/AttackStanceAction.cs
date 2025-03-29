using Command.Player;
using Command.Input;
using Command.Main;
using UnityEngine;
using Command.Commands;

namespace Command.Actions
{
    public class AttackStanceAction : IAction
    {
        private UnitController actorUnit;
        private UnitController targetUnit;
        private bool isSuccessful;
        TargetType IAction.TargetType { get => TargetType.Self; }

        public void PerformAction(UnitController actorUnit, UnitController targetUnit, bool isSuccessful)
        {
            this.actorUnit = actorUnit;
            this.targetUnit = targetUnit;
            this.isSuccessful = isSuccessful;

            actorUnit.PlayBattleAnimation(CommandType.AttackStance, CalculateMovePosition(targetUnit), OnActionAnimationCompleted);
        }

        public void OnActionAnimationCompleted()
        {
            GameService.Instance.SoundService.PlaySoundEffects(Sound.SoundType.ATTACK_STANCE);

            if (IsSuccessful())
                targetUnit.CurrentPower += (int)(targetUnit.CurrentPower * 0.2f);
            else
                GameService.Instance.UIService.ActionMissed();
        }

        public bool IsSuccessful() => true;

        public Vector3 CalculateMovePosition(UnitController targetUnit) => targetUnit.GetEnemyPosition();
    }
}