using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Game
{
    public class PlayerBehaviourWrapper
    {
        public IPlayerBehavior PlayerBehaviour1 { get; private set; }
        public IPlayerBehaviour2 PlayerBehaviour2 { get; private set; }

        public PlayerBehaviourWrapper(object playerBehaviour)
        {
            var knownInterfaces = new[] { typeof(IPlayerBehaviour2), typeof(IPlayerBehavior) };

            PlayerBehaviour2 = (playerBehaviour as IPlayerBehaviour2);
            PlayerBehaviour1 = (playerBehaviour as IPlayerBehavior);

            if (PlayerBehaviour2 == null && PlayerBehaviour1 == null)
            {
                throw new Exception(string.Format("Object {0} must implement at least one of these interfaces: {1}.",
                    playerBehaviour.GetType().Name, string.Join(", ", knownInterfaces.Select(x => x.Name).ToArray())));
            }
        }

        public void Init(int identifier, int playgroundSize, int x, int y, Direction direction)
        {
            if (PlayerBehaviour2 != null)
                PlayerBehaviour2.Init(identifier, playgroundSize, x, y, direction);
            else if (PlayerBehaviour1 != null)
                PlayerBehaviour1.Init((int)direction, identifier);
        }

        public Move GetNextMove(int[,] playground)
        {
            if (PlayerBehaviour2 != null)
                return PlayerBehaviour2.GetNextMove(playground);
            else if (PlayerBehaviour1 != null)
                return (Move)PlayerBehaviour1.NextMove(playground);
            throw new Exception();
        }

        public string Name
        {
            get
            {
                if (PlayerBehaviour2 != null)
                    return PlayerBehaviour2.Name;
                else if (PlayerBehaviour1 != null)
                    return PlayerBehaviour1.MyName();
                throw new Exception();
            }
        }
    }
}
