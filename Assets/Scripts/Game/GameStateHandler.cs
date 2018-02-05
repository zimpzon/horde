using System.Collections;

namespace HordeEngine
{
    public enum GameState { None, Boot, StartScreen, Settings, InHub, InLevel };

    public abstract class GameStateHandler
    {
        public bool RequestPopState;
        public GameState RequestPushState;
        public GameState RequestGoToState;

        protected GameManager GM; // Shortcut

        public GameStateHandler()
        {
            GM = Global.GameManager;
        }

        public void ResetChangeRequests()
        {
            RequestPopState = false;
            RequestPushState = GameState.None;
            RequestGoToState = GameState.None;
        }

        // Pop back to previous state
        protected void PopState()
        {
            RequestPopState = true;
        }

        // Push this state on top of state stack
        protected void PushState(GameState state)
        {
            RequestPushState = state;
        }

        // Jump directly to this state regardless of state stack
        protected void JumpToState(GameState newState)
        {
            RequestGoToState = newState;
        }

        // Only check input in this method. Will only be called when state has focus.
        public virtual void CheckInput() { }

        // This will be called repeatedly until returning true
        public virtual bool TryEnterState() { return true; }

        // Will be called after TryEnter succeeded and until TryLeave takes over.
        // Only exactly one of TryEnter, UpdateState or TryLeave will be called in a frame.
        public virtual void UpdateState(bool hasFocus) { }

        // This will be called repeatedly until returning true
        public virtual bool TryLeaveState() { return true; }
    }
}
