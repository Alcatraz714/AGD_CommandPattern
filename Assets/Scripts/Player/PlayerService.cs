using Command.Actions;
using Command.Main;

namespace Command.Player
{
    public class PlayerService
    {
        private PlayerController player1;
        private PlayerController player2;

        private int currentTurnNumber;
        private PlayerController activePlayer;

        public int ActivePlayerID => activePlayer.PlayerID;
        public int ActiveUnitID => activePlayer.ActiveUnitID;

        public void Init(PlayerScriptableObject player1Data, PlayerScriptableObject player2Data)
        {
            CleanPlayers();
            CreatePlayers(player1Data, player2Data);
            StartTurnSequence();
        }

        private void CleanPlayers()
        {
            if(player1 == null || player2 == null)
                return;

            player1.DestroyAllUnits();
            player2.DestroyAllUnits();
        }

        private void CreatePlayers(PlayerScriptableObject player1Data, PlayerScriptableObject player2Data)
        {
            player1 = new PlayerController(this, player1Data);
            player2 = new PlayerController(this, player2Data);
        }

        private void StartTurnSequence()
        {
            currentTurnNumber = 0;
            StartNextTurn();
        }

        private void StartNextTurn()
        {
            SetActivePlayer();
            
            if (activePlayer == player1)
            {
                currentTurnNumber++;
                GameService.Instance.UIService.UpdateTurnNumber(currentTurnNumber);
            }
            
            activePlayer.StartPlayerTurn();
        }

        private void SetActivePlayer()
        {
            if (activePlayer == null)
                activePlayer = player1;
            else 
                activePlayer = activePlayer == player1 ? player2 : player1;
        }

        public void OnPlayerTurnCompleted() => StartNextTurn();

        public void PerformAction(CommandType actionSelected, UnitController targetUnit) => GameService.Instance.ActionService.GetActionByType(actionSelected).PerformAction(activePlayer.GetUnitByID(ActiveUnitID), targetUnit);

        public void PlayerDied(PlayerController deadPlayer)
        {
            int winnerId;

            if (deadPlayer == player1)
                winnerId = player2.PlayerID;
            else
                winnerId = player1.PlayerID;

            GameService.Instance.UIService.ShowBattleEndUI(winnerId);
        }

        private PlayerController GetPlayerById(int playerId) 
        {
            if (player1.PlayerID == playerId)
                return player1;
            else if (player2.PlayerID == playerId)
                return player2;
            else
                throw new System.Exception($"No Player found for the given Player ID: {playerId}");
        }

        public void CheckGameOver()
        {
            if (player1.AllUnitsDead())
                PlayerDied(player1);
            else if (player2.AllUnitsDead())
                PlayerDied(player2);
        }
    }
}