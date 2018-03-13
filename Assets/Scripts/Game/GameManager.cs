using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HordeEngine
{
    /// <summary>
    /// GameManager must be first in script execution order
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        MapResources mapResources_ = new MapResources();
        GameState CurrentGameState { get { return gameStateStack_.Peek(); } }
        Stack<GameState> gameStateStack_ = new Stack<GameState>();
        Dictionary<GameState, GameStateHandler> gameStateHandlers_ = new Dictionary<GameState, GameStateHandler>();

        Dictionary<string, string> debugLines_ = new Dictionary<string, string>();
        public void ShowDebug(object key, object text, params object[] param)
        {
            string formatted = string.Format("{0}: {1} ({2})", key.ToString(), string.Format(text.ToString(), param), Time.frameCount);
            debugLines_[key.ToString()] = formatted;
            Global.SceneAccess.DebugText.text = string.Join(Environment.NewLine, debugLines_.Values.ToArray());
        }

        void Awake()
        {
            Application.logMessageReceived += Application_logMessageReceived;
            Application.lowMemory += Application_lowMemory;

            Global.GameManager = this;
            Global.TimeManager = new TimeManager();
            Global.ComponentUpdater = new ComponentUpdater();
            Global.Crosshair = new CrosshairController();
            Global.MapResources = mapResources_;
            Global.SceneAccess = FindObjectOfType<SceneAccessScript>();

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
            // This is the first Update() to be called in every frame
            Global.TimeManager.UpdateTime(Time.deltaTime);
            Global.ComponentUpdater.DoUpdate();

            Global.SceneAccess.LightDebugView.texture = Global.SceneAccess.LightingCam.targetTexture;
        }

        void LateUpdate()
        {
            // This is the first LateUpdate() to be called in every frame
            Global.ComponentUpdater.DoLateUpdate();
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
                    GameStateHandler handler = GetStatehandler(gameState);
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

        GameStateHandler GetStatehandler(GameState state)
        {
            GameStateHandler handler;
            gameStateHandlers_.TryGetValue(state, out handler);
            return handler;
        }

        IEnumerator GameStateLoop()
        {
            yield return GoToState(GameState.Boot);

            while (true)
            {
                UpdateAllNonFocusedStates();

                // Handle focused top state (usually the only one)
                var topState = gameStateStack_.Peek();
                var handler = GetStatehandler(topState);

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
