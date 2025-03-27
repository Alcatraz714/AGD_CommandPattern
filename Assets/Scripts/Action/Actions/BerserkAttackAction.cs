using Command.Input;
using Command.Main;
using Command.Player;
using UnityEngine;

namespace Command.Actions
{
    public class BerserkAttackAction : IAction
    {
        private const float hitChance = 0.66f;
        private UnitController actorUnit;
        private UnitController targetUnit;
        public TargetType TargetType => TargetType.Enemy;

        public void PerformAction(UnitController actorUnit, UnitController targetUnit)
        {
            this.actorUnit = actorUnit;
            this.targetUnit = targetUnit;

            actorUnit.PlayBattleAnimation(CommandType.BerserkAttack, CalculateMovePosition(targetUnit), OnActionAnimationCompleted);
        }

        public void OnActionAnimationCompleted()
        {
            GameService.Instance.SoundService.PlaySoundEffects(Sound.SoundType.BERSERK_ATTACK);

            if (IsSuccessful())
                targetUnit.TakeDamage(actorUnit.CurrentPower * 2);
            else
            {
                actorUnit.TakeDamage(actorUnit.CurrentPower * 2);
                Debug.Log("actor unit must be hit now.");
            }
        }

        public bool IsSuccessful() => Random.Range(0f, 1f) < hitChance;

        public Vector3 CalculateMovePosition(UnitController targetUnit) => targetUnit.GetEnemyPosition();
    }
}