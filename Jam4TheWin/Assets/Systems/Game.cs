using System;
using System.Linq;
using SystemBase;
using Systems.GameState;
using Utils;

namespace Systems
{
    public class Game : GameBase
    {
        public BasicGameStateContext GameStateMachine;
        private void Awake()
        {
            IoC.RegisterSingleton(this);

            #region System Registration

            foreach (var t in from a in AppDomain.CurrentDomain.GetAssemblies()
                              from t in a.GetTypes()
                              where Attribute.IsDefined(t, typeof(GameSystemAttribute))
                              select t)
            {
                RegisterSystem(Activator.CreateInstance(t) as IGameSystem);
            }

            #endregion System Registration

            Init();

            var start = new StartScreen();
            GameStateMachine = new BasicGameStateContext(start);
            start.Enter(GameStateMachine);
        }
    }
}