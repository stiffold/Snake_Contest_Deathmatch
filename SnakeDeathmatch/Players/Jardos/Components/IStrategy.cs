using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Debugger;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Jardos.Components
{
    /// <summary>
    /// strategie
    /// </summary>
     public interface IStrategy : IDebuggable
    {
        /*1 krok     = 1bod   */
        /*zabití     = 100bodů*/
        /*zabednění  = 50bodů */
        int EvaluateScore();

        /// <summary>
        /// počet kroků k provedení
        /// </summary>
        int Alocation { get; }

        /// <summary>
        /// update vnitřního stavu stretegie - per frame
        /// </summary>
        /// <param name="fact"></param>
        void Update(Fact fact);

        /// <summary>
        /// pokud se jedná o vybranou strategii, provolává se per frame
        /// </summary>
        /// <returns></returns>
        bool CanContinue();

        /// <summary>
        /// vrať krok
        /// </summary>
        /// <returns></returns>
        Move GetMove();
    }
}
