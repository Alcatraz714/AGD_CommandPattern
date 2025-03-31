using Command.Input;
using Command.Main;
using UnityEngine;

namespace Command.UI
{
    public class GameplayUIController : IUIController
    {
        private GameplayUIView gameplayView;

        public GameplayUIController(GameplayUIView gameplayView)
        {
            this.gameplayView = gameplayView;
            this.gameplayView.SetController(this);
        }

        public void Show() => gameplayView.EnableView();

        public void SetTurnNumber(int turnNumber) => gameplayView.SetTurnText($"Turn: {turnNumber}");

        public void ShowActionOverlay(int activePlayer)
        {
            ResetBattleBackgroundOverlay();
            gameplayView.ShowPlayerOverlay(activePlayer, OverlayColorType.Neutral);
        }

        public void ShowTargetOverlay(int activePlayer, TargetType targetType)
        {
            ResetBattleBackgroundOverlay();

            switch (activePlayer)
            {
                case 1:
                    if (targetType == TargetType.Enemy)
                        gameplayView.ShowPlayerOverlay(2, OverlayColorType.Enemy);
                    else
                        gameplayView.ShowPlayerOverlay(1, OverlayColorType.Friendly);
                    break;
                case 2:
                    if (targetType == TargetType.Enemy)
                        gameplayView.ShowPlayerOverlay(1, OverlayColorType.Enemy);
                    else
                        gameplayView.ShowPlayerOverlay(2, OverlayColorType.Friendly);
                    break;
            }
        }

        public void ResetBattleBackgroundOverlay() => gameplayView.ResetBackgroundOverlay();
        
        public void OnUndoButtonClicked() => GameService.Instance.CommandInvoker.Undo();

        public void ShowMissedAction() => gameplayView.ShowMissedText();

        public void SetBattleBackgroundImage(Sprite bgSprite) => gameplayView.SetBattleBackgroundImage(bgSprite);
    }
}