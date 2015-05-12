using System;

namespace SnakeGame.Interface
{
    [Obsolete("Use IPlayerBehaviour2 instead.")]
    public interface IPlayerBehavior
    {
        /// <summary>
        /// Inicializace chování hráče
        /// </summary>
        /// <param name="direction">
        ///Top = 1,
        ///TopRight = 2,
        ///Right = 3,
        ///BottomRight = 4,
        ///Bottom = 5,
        ///BottomLeft = 6,
        ///Left = 7,
        ///TopLeft = 8,
        /// </param>
        /// <param name="identificator">číselný identifikátor hráče v poli</param>
        void Init(int direction, int identificator);

        /// <summary>
        /// Vrať směr dalšího tahu
        /// </summary>
        /// <param name="gameSurround">hrací pole 50/50 X/Y</param>
        /// <returns>
        ///Left = 1,
        ///Straight = 2,
        ///Right = 3
        /// </returns>
        int NextMove(int[,] gameSurround);

        /// <summary>
        /// Vrať svoje jméno
        /// </summary>
        /// <returns></returns>
        string MyName();
    }
}
