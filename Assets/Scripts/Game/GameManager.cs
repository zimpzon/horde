using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HordeEngine
{
    /// <summary>
    /// GameManager must be first in script execution order
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public MapRenderer MapRenderer;

        TimeManager timeManager_ = new TimeManager();
        MapResources mapResources_ = new MapResources();
        GameState CurrentGameState { get { return gameStateStack_.Peek(); } }
        Stack<GameState> gameStateStack_ = new Stack<GameState>();
        Dictionary<GameState, GameStateHandler> gameStateHandlers_ = new Dictionary<GameState, GameStateHandler>();

        void Awake()
        {
            Application.logMessageReceived += Application_logMessageReceived;
            Application.lowMemory += Application_lowMemory;

            Global.GameManager = this;
            Global.MapManager = MapRenderer;
            Global.TimeManager = timeManager_;
            Global.MapResources = mapResources_;

            gameStateHandlers_[GameState.Boot] = new GameStateBoot();
            gameStateHandlers_[GameState.InHub] = new GameStateHub();
            gameStateHandlers_[GameState.StartScreen] = new GameStateHub();

            foreach (var handler in gameStateHandlers_.Values)
                handler.Initialize();

            gameStateStack_.Push(GameState.Boot);
        }

        private void Application_lowMemory()
        {
            // TODO
            // Send this to Playfab if possible
        }

        private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
        {
            if (string.IsNullOrEmpty(stackTrace))
                stackTrace = new System.Diagnostics.StackTrace().ToString();

            if (type == LogType.Error)
            {
                // TODO
                // Send this to Playfab if possible. Also see FatalError right below.
            }
        }

        public void FatalError(string message)
        {
            Debug.LogError("FATAL ERROR: " + message);
            Application.Quit();
        }

        void Start()
        {
            StartCoroutine(GameStateLoop());
        }

        void Update()
        {
            // This is the first update to be called in every frame
            timeManager_.UpdateTime(Time.deltaTime);
        }

        IEnumerator EnterState(GameState state)
        {
            var handler = gameStateHandlers_[state];
            while (!handler.TryEnterState())
                yield return null;
        }

        IEnumerator LeaveState(GameState state)
        {
            var handler = gameStateHandlers_[state];
            while (!handler.TryLeaveState())
                yield return null;
        }

        IEnumerator GoToState(GameState to)
        {
            while (gameStateStack_.Count > 0)
            {
                var state = gameStateStack_.Peek();
                yield return LeaveState(state);
                gameStateStack_.Pop();
            }

            yield return PushState(to);
        }

        IEnumerator PushState(GameState to)
        {
            gameStateStack_.Push(to);
            OnStateChanged(to);
            yield return EnterState(to);
        }

        IEnumerator PopState()
        {
            var topState = gameStateStack_.Peek();
            yield return LeaveState(topState);
            gameStateStack_.Pop();

            var newTopState = gameStateStack_.Peek();
            OnStateChanged(newTopState);
            yield return EnterState(newTopState);
        }

        void UpdateAllNonFocusedStates()
        {
            var topState = gameStateStack_.Peek();
            foreach (var gameState in gameStateStack_)
            {
                if (gameState != topState)
                {
                    var handler = gameStateHandlers_[gameState];
                    handler.UpdateState(hasFocus: false);
                    if (handler.RequestGoToState != GameState.None || handler.RequestPushState != GameState.None || handler.RequestPopState)
                    {
                        Debug.LogErrorFormat("Non-focused GameState {0} trying to change state: GoTo: {1}, Pop: {2}, Push: {3}", gameState, handler.RequestGoToState, handler.RequestPopState, handler.RequestPushState);
                        handler.ResetChangeRequests();
                    }
                }
            }
        }

        GameState prevState_;
        void OnStateChanged(GameState state)
        {
            Debug.LogFormat("State changed from {0} to {1}", prevState_, state);
            prevState_ = state;
        }

        IEnumerator GameStateLoop()
        {
            yield return GoToState(GameState.Boot);

            while (true)
            {
                UpdateAllNonFocusedStates();

                // Handle focused top state (usually the only one)
                var topState = gameStateStack_.Peek();
                var handler = gameStateHandlers_[topState];

                if (handler.RequestGoToState != GameState.None)
                {
                    var newState = handler.RequestGoToState;
                    handler.ResetChangeRequests();
                    yield return GoToState(newState);
                }
                else if (handler.RequestPushState != GameState.None)
                {
                    var newState = handler.RequestPushState;
                    handler.ResetChangeRequests();
                    yield return PushState(newState);
                }
                else if (handler.RequestPopState)
                {
                    handler.ResetChangeRequests();
                    yield return PopState();
                }
                else
                {
                    handler.UpdateState(hasFocus: true);
                    yield return null;
                }
            }
        }
    }
}
