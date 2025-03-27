using UnityEngine;
using Command.Main;
using Command.Actions;
using System.Collections;
using System;
using Object = UnityEngine.Object;

namespace Command.Player
{
    public class UnitController
    {
        public PlayerController Owner { get; private set; }
        private UnitScriptableObject unitScriptableObject;
        private UnitView unitView;

        public int UnitID { get; private set; }
        public UnitType UnitType => unitScriptableObject.UnitType;
        public int CurrentHealth { get; private set; }
        public UnitUsedState UsedState { get; private set; }
        
        private UnitAliveState aliveState;
        private Vector3 originalPosition;
        public int CurrentPower;
        public int CurrentMaxHealth;

        public UnitController(PlayerController owner, UnitScriptableObject unitScriptableObject, Vector3 unitPosition)
        {
            Owner = owner;
            this.unitScriptableObject = unitScriptableObject;
            UnitID = unitScriptableObject.UnitID;
            originalPosition = unitPosition;

            InitializeView(unitPosition);
            InitializeVariables();
        }

        private void InitializeView(Vector3 positionToSet)
        {
            unitView = Object.Instantiate(unitScriptableObject.UnitPrefab);
            unitView.Controller = this;
            unitView.transform.position = positionToSet;
            unitView.SetUnitIndicator(false);
        }

        private void InitializeVariables()
        {
            CurrentMaxHealth = CurrentHealth = unitScriptableObject.MaxHealth;
            CurrentPower = unitScriptableObject.Power;
            SetAliveState(UnitAliveState.ALIVE);
            SetUsedState(UnitUsedState.NOT_USED);
        }

        public void StartUnitTurn()
        {
            unitView.SetUnitIndicator(true);
            GameService.Instance.UIService.ShowActionOverlay(Owner.PlayerID);
            GameService.Instance.UIService.ShowActionSelectionView(unitScriptableObject.executableCommands);
            GameService.Instance.UIService.SetActionContainerAlignment(Owner.PlayerID);
        }

        private void SetAliveState(UnitAliveState stateToSet) => aliveState = stateToSet;

        public void SetUsedState(UnitUsedState stateToSet) => UsedState = stateToSet;

        public bool IsAlive() => aliveState == UnitAliveState.ALIVE;

        public void TakeDamage(int damageToTake)
        {
            CurrentHealth -= damageToTake;

            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                UnitDied();
            }
            else
                unitView.PlayAnimation(UnitAnimations.HIT);

            unitView.UpdateHealthBar((float) CurrentHealth / CurrentMaxHealth);
        }

        public void RestoreHealth(int healthToRestore)
        {
            CurrentHealth = CurrentHealth + healthToRestore > CurrentMaxHealth ? CurrentMaxHealth : CurrentHealth + healthToRestore;
            unitView.UpdateHealthBar((float)CurrentHealth / CurrentMaxHealth);
        }

        private void UnitDied()
        {
            SetAliveState(UnitAliveState.DEAD);
            unitView.PlayAnimation(UnitAnimations.DEATH);
        }

        public void PlayBattleAnimation(CommandType actionType, Vector3 battlePosition, Action callback)
        {
            GameService.Instance.UIService.ResetBattleBackgroundOverlay();
            MoveToBattlePosition(battlePosition, callback, true, actionType);
        }

        private void MoveToBattlePosition(Vector3 battlePosition, Action callback = null,  bool shouldPlayActionAnimation = true, CommandType actionTypeToExecute = CommandType.None)
        {
            float moveTime = Vector3.Distance(unitView.transform.position, battlePosition) / unitScriptableObject.MovementSpeed;
            unitView.StartCoroutine(MoveToPositionOverTime(battlePosition, moveTime, callback, shouldPlayActionAnimation, actionTypeToExecute));
        }

        private IEnumerator MoveToPositionOverTime(Vector3 targetPosition, float time, Action callback, bool shouldPlayActionAnimation, CommandType actionTypeToExecute)
        {
            float elapsedTime = 0;
            Vector3 startingPosition = unitView.transform.position;

            while (elapsedTime < time)
            {
                unitView.transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / time);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            unitView.transform.position = targetPosition;

            if (shouldPlayActionAnimation)
                PlayActionAnimation(actionTypeToExecute);

            if (callback != null)
                callback.Invoke();
        }

        private void PlayActionAnimation(CommandType actionType)
        {
            if (actionType == CommandType.None)
                return;
            
            if (actionType == unitScriptableObject.executableCommands[0])
                unitView.PlayAnimation(UnitAnimations.ACTION1);
            else if (actionType == unitScriptableObject.executableCommands[1])
                unitView.PlayAnimation(UnitAnimations.ACTION2);
            else
                throw new System.Exception($"No Animation found for the action type : {actionType}");
        }

        public void OnActionExecuted()
        {
            MoveToBattlePosition(originalPosition, null, false);
            SetUsedState(UnitUsedState.USED);
            Owner.OnUnitTurnEnded();
            unitView.SetUnitIndicator(false);
        }

        public void ResetStats() => CurrentPower = unitScriptableObject.Power;

        public void Revive() => SetAliveState(UnitAliveState.ALIVE);

        public void Destroy() => UnityEngine.Object.Destroy(unitView.gameObject);

        public void ResetUnitIndicator() => unitView.SetUnitIndicator(false);

        public Vector3 GetEnemyPosition() 
        {
            if (Owner.PlayerID == 1)
                return unitView.transform.position + unitScriptableObject.EnemyBattlePositionOffset;
            else
                return unitView.transform.position - unitScriptableObject.EnemyBattlePositionOffset;
        }
    }

    public enum UnitUsedState
    {
        USED,
        NOT_USED
    }

    public enum UnitAliveState
    {
        ALIVE,
        DEAD
    }
}