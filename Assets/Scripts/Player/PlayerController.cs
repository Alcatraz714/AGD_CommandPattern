using Command.Commands;
using System.Collections.Generic;
using UnityEngine;

namespace Command.Player
{
    public class PlayerController
    {
        private PlayerService playerService;

        public int PlayerID { get; private set; }
        private List<UnitController> units;
        private int activeUnitIndex;
        public int ActiveUnitID => units[activeUnitIndex].UnitID;

        public PlayerController(PlayerService playerService, PlayerScriptableObject playerScriptableObject)
        {
            this.playerService = playerService;
            PlayerID = playerScriptableObject.PlayerID;
            CreateUnits(playerScriptableObject.UnitData, playerScriptableObject.UnitPositions);
        }

        private void CreateUnits(List<UnitScriptableObject> unitScriptableObjects, List<Vector3> unitPositions)
        {
            units = new List<UnitController>();

            for(int i=0; i<unitScriptableObjects.Count; i++)
            {
                units.Add(new UnitController(this, unitScriptableObjects[i], unitPositions[i]));
            }
        }

        public void StartPlayerTurn()
        {
            activeUnitIndex = 0;
            ResetAllUnitStates();
            TryStaringUnitTurn();
        }

        private void TryStaringUnitTurn()
        {
            if (IsCurrentUnitAlive())
                units[activeUnitIndex].StartUnitTurn();
            else
                OnUnitTurnEnded();
        }

        public void OnUnitTurnEnded()
        {
            if(AllUnitsUsed())
            {
                // TODO:    Need to check here if any of the players are dead. Not only the active one.

                if (AllUnitsDead())
                    playerService.PlayerDied(this);
                else 
                    EndPlayerTurn();
            }
            else
            {
                playerService.CheckGameOver();

                activeUnitIndex++;
                TryStaringUnitTurn();
            }
        }

        private void ResetAllUnitStates() => units.ForEach(unit => unit.SetUsedState(UnitUsedState.NOT_USED));

        private bool IsCurrentUnitAlive() => units[activeUnitIndex].IsAlive();

        private bool AllUnitsUsed() => units.TrueForAll(unit => unit.UsedState == UnitUsedState.USED || !unit.IsAlive());

        public bool AllUnitsDead() => units.TrueForAll(unit => !unit.IsAlive());

        private void EndPlayerTurn() => playerService.OnPlayerTurnCompleted();

        public UnitController GetUnitByID(int unitId) => units.Find(unit => unit.UnitID == unitId);

        public void DestroyAllUnits()
        {
            units.ForEach(unit => unit.Destroy());
            units.Clear();
        }

        public void ProcessUnitCommand(UnitCommand commandToProcess) => GetUnitByID(commandToProcess.commandData.ActorUnitID).ProcessUnitCommand(commandToProcess);

        public void ResetCurrentActivePlayer()
        {
            units[activeUnitIndex].ResetUnitIndicator();
            activeUnitIndex--;
            units[activeUnitIndex].StartUnitTurn();
        }

        public void ResetCurrentActiveUnit()
        {
            //1. Reset the unit indicator (Arrow) for the currently active unit.
            units[activeUnitIndex].ResetUnitIndicator();
            
            //2. Move to the previous unit in the list.
            activeUnitIndex--;
            
            // Continue searching for a valid, living unit to make active.
            while (activeUnitIndex >= 0)
            {
                //3. Check if the unit at the current index is not alive (i.e., it's defeated).
                if (!units[activeUnitIndex].IsAlive())
                {
                    // Move to the previous unit in the list.
                    activeUnitIndex--;
                }
                else
                {
                    //4. Activate the next living unit in the list and start its turn.
                    units[activeUnitIndex].StartUnitTurn();
                    break; // Exit the loop once an active unit is found.
                }
            }
        }
    }
}