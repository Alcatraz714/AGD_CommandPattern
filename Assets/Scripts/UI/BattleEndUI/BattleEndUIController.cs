using Command.Main;
using UnityEngine.SceneManagement;

namespace Command.UI
{
    public class BattleEndUIController : IUIController
    {
        private BattleEndUIView battleEndView;

        public BattleEndUIController(BattleEndUIView battleEndView)
        {
            this.battleEndView = battleEndView;
            battleEndView.SetController(this);
        }

        public void OnReplayButtonClicked()
        {
            GameService.Instance.ReplayService.SetReplayState(Replay.ReplayState.ACTIVE);
            GameService.Instance.InputService.SetInputState(Input.InputState.INACTIVE);
            GameService.Instance.EventService.OnReplayButtonClicked.InvokeEvent();
        }

        public void Show() => battleEndView.EnableView();

        public void Hide() => battleEndView.DisableView();

        public void SetWinner(int winnerId) => battleEndView.SetResultText($"Player {winnerId} Won!");

        public void OnHomeButtonClicked() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}