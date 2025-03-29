using Command.Input;
using Command.Main;
using Command.Player;
using UnityEngine;
using Command.Commands;

namespace Command.Actions
{
    public class MeditateAction : IAction
    {
        private UnitController actorUnit;
        private UnitController targetUnit;
        private bool isSuccessful;
        public TargetType TargetType => TargetType.Self;

        public void PerformAction(UnitController actorUnit, UnitController targetUnit, bool isSuccessful)
        {
            this.actorUnit = actorUnit;
            this.targetUnit = targetUnit;
            this.isSuccessful = isSuccessful;

            actorUnit.PlayBattleAnimation(CommandType.Meditate, CalculateMovePosition(targetUnit), OnActionAnimationCompleted);
        }

        public void OnActionAnimationCompleted()
        {
            GameService.Instance.SoundService.PlaySoundEffects(Sound.SoundType.MEDITATE);

            if (IsSuccessful())
            {
                var healthToIncrease = (int)(targetUnit.CurrentMaxHealth * 0.2f);
                targetUnit.CurrentMaxHealth += healthToIncrease;
                targetUnit.RestoreHealth(healthToIncrease);
            }
            else
                GameService.Instance.UIService.ActionMissed();
        }

        public bool IsSuccessful() => true;

        public Vector3 CalculateMovePosition(UnitController targetUnit) => targetUnit.GetEnemyPosition();
    }
}