using Command.Input;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Command.UI
{
    public class GameplayUIView : MonoBehaviour, IUIView
    {
        private GameplayUIController controller;
        [SerializeField] private TextMeshProUGUI turnText;
        [SerializeField] private TextMeshProUGUI missedText;
        [SerializeField] private Image Player1BackgroundOverlay;
        [SerializeField] private Image Player2BackgroundOverlay;
        [SerializeField] private Color FriendlyOverlayColor;
        [SerializeField] private Color EnemyOverlayColor;
        [SerializeField] private Color ActionSelectionOverlayColor;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Button undoButton;

        public void SetController(GameplayUIController controllerToSet) 
        {
            controller = controllerToSet;
            undoButton.onClick.AddListener(controller.OnUndoButtonClicked);
            missedText.canvasRenderer.SetAlpha(0);
        }

        public void DisableView() => gameObject.SetActive(false);

        public void EnableView() => gameObject.SetActive(true);

        public void SetTurnText(string turnText) => this.turnText.SetText(turnText);

        public void ShowPlayerOverlay(int targetPlayer, OverlayColorType overlayColorType)
        {
            switch(targetPlayer)
            {
                case 1:
                    Player1BackgroundOverlay.enabled = true;
                    SetOverlayColor(Player1BackgroundOverlay, overlayColorType);
                    break;
                case 2:
                    Player2BackgroundOverlay.enabled = true;
                    SetOverlayColor(Player2BackgroundOverlay, overlayColorType);
                    break;
                default:
                    break;
            }
        }

        public void ResetBackgroundOverlay()
        {
            Player1BackgroundOverlay.enabled = false;
            Player2BackgroundOverlay.enabled = false;
        }

        public void ShowMissedText()
        {
            missedText.SetText("Missed!");
            missedText.canvasRenderer.SetAlpha(1);
            missedText.CrossFadeAlpha(0, 2, false);
        }

        public void SetOverlayColor(Image overlayImage, OverlayColorType colorType)
        {
            switch(colorType)
            {
                case OverlayColorType.Friendly:
                    overlayImage.color = FriendlyOverlayColor;
                    break;
                case OverlayColorType.Enemy:
                    overlayImage.color = EnemyOverlayColor;
                    break;
                case OverlayColorType.Neutral:
                    overlayImage.color = ActionSelectionOverlayColor;
                    break;
                default:
                    break;
            }
        }

        public void SetBattleBackgroundImage(Sprite bgSprite)
        {
            backgroundImage.gameObject.SetActive(true);
            backgroundImage.sprite = bgSprite;
        } 
    }

    [Serializable]
    public enum OverlayColorType
    {
        Friendly,
        Enemy,
        Neutral
    }
}