using Command.Actions;
using Command.Main;
using System.Collections.Generic;

namespace Command.UI
{
    public class ActionSelectionUIController : IUIController
    {
        private ActionSelectionUIView actionSelectionView;
        private ActionButtonView actionButtonPrefab;
        private List<ActionButtonView> actionButtons;

        public ActionSelectionUIController(ActionSelectionUIView actionSelectionView, ActionButtonView actionButtonPrefab)
        {
            this.actionSelectionView = actionSelectionView;
            this.actionButtonPrefab = actionButtonPrefab;
            InitializeController();
        }

        private void InitializeController()
        {
            actionButtons = new List<ActionButtonView>();
            Hide();
        }

        public void Show(List<CommandType> executableCommands)
        {
            ResetActionButtons();
            actionSelectionView.EnableView();
            CreateActionButtons(executableCommands);
        }

        public void Hide()
        {
            ResetActionButtons();
            actionSelectionView.DisableView();
        }

        private void ResetActionButtons()
        {
            actionButtons.ForEach(button => actionSelectionView.RemoveButton(button));
            actionButtons.Clear();
        }

        private void CreateActionButtons(List<CommandType> executableCommands)
        {
            foreach(CommandType commandType in executableCommands)
            {
                var button = actionSelectionView.AddButton(actionButtonPrefab);
                button.SetOwner(this);
                button.SetCommandType(commandType);
                actionButtons.Add(button);
            }
        }

        // To Learn more about Events and Observer Pattern, check out the course list here: https://outscal.com/courses
        public void OnActionSelected(CommandType actionType)
        {
            GameService.Instance.EventService.OnActionSelected.InvokeEvent(actionType);
            Hide();
        }

        public void SetActionContainerAlignment(int activePlayerID)
        {
            switch(activePlayerID)
            {
                case 1:
                    actionSelectionView.SetActionContainerAlignment(ActionContainerAlignment.Left);
                    break;
                case 2:
                    actionSelectionView.SetActionContainerAlignment(ActionContainerAlignment.Right);
                    break;
                default:
                    break;
            }
        }

    }
}