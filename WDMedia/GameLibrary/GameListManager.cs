/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WDMedia.Game;

namespace WDMedia.GameLibrary
{
    public class GameListManager
    {
        public struct GameInfo
        {
            public string group, name;
            public NewGameDelegate getNewGame;

            public GameInfo(string group, string name, NewGameDelegate getNewGame)
            {
                this.group = group;
                this.name = name;
                this.getNewGame = getNewGame;
            }
        }
        public delegate IGame NewGameDelegate();
        public delegate void menuHandelerDelegate(NewGameDelegate d, string name);

        public menuHandelerDelegate currentMenuDelegate = null;
        
        public List<GameInfo> Games { get; protected set; }

        public GameListManager()
        {
            Games = new List<GameInfo>();

            /*games.Add(new GameInfo("", "4 in a row", delegate() { Console.WriteLine("4 row"); return new connect4Game(canvasSize.Width, canvasSize.Height); }));
            games.Add(new GameInfo("", "Code Breaker", delegate() { Console.WriteLine("vode breaker"); return new codeBreaker(); }));
            games.Add(new GameInfo("", "Sudoku", delegate() { Console.WriteLine("sudoku"); return new sudokuGame(); }));
            games.Add(new GameInfo("", "15 Tiles", delegate() { return new tileMover(); }));
            games.Add(new GameInfo("", "Chomp", delegate() { return new chompGame(); }));
            games.Add(new GameInfo("", "Noughts & Crosses", delegate() { return new noughtsAndCrossesGame(); }));
            games.Add(new GameInfo("Action", "Figher Pilot", delegate() { return new fighterPilotGame(); }));*/
        }

        public void populateMenu(MenuItem menu)
        {
            foreach (GameInfo info in Games)
            {
                MenuItem m = new MenuItem();
                m.Text = info.name;
                m.Click += new EventHandler(delegate(object sender, EventArgs args) {
                    currentMenuDelegate(info.getNewGame, info.name); });
                menu.MenuItems.Add(m);
            }
        }

        public void populateMenu(ToolStripMenuItem menu)
        {
            foreach (GameInfo info in Games)
            {
                ToolStripMenuItem m = new ToolStripMenuItem();
                Console.WriteLine("Regostering " + info.name);
                m.Text = info.name;
                m.Click += new EventHandler(delegate(object sender, EventArgs args)
                {
                    //Console.WriteLine("event handeler " + info.name);
                    //currentMenuDelegate(info.getNewGame, info.name);
                    foreach (GameInfo foo in Games)
                    {
                        if (foo.name == ((ToolStripMenuItem)sender).Text)
                        {
                            currentMenuDelegate(foo.getNewGame, foo.name);
                        }
                    }
                });
                menu.DropDownItems.Add(m);
            }
        }

        internal void registerMenuHandeler(menuHandelerDelegate onNewGame)
        {
            currentMenuDelegate = onNewGame;
        }
    }
}
