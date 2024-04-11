using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public abstract class ScoreReferee : GameReferee
    {
        [SerializeField, Min(0)] private int scoreForWin;
        
        private Dictionary<BasePlayer, int> playersScore;

        public event Action<BasePlayer, int, int> ScoreChanged;
        public int ScoreForWin
        {
            get => scoreForWin;
            set => scoreForWin = Mathf.Max(0, value);
        }

        public override void Initialize(Player player, IEnumerable<BasePlayer> enemies)
        {
            base.Initialize(player, enemies);
            playersScore = new Dictionary<BasePlayer, int>
            (enemies
                .Concat(new[] {player})
                .Select(x => new KeyValuePair<BasePlayer, int>(x, 0))
            );
        }
        
        public int GetScoreForPlayer()
        {
            return GetScoreForPlayer(Player);
        }
        
        public int GetScoreForEnemies()
        {
            return GetScoreForPlayer(Enemies.First());
        }


        public int GetScoreForPlayer(BasePlayer basePlayer)
        {
            return playersScore.TryGetValue(basePlayer, out var score) ? score : 0;
        }

        protected void SetScoreForPlayer([NotNull] BasePlayer basePlayer, int score)
        {
            if (basePlayer == null) throw new ArgumentNullException(nameof(basePlayer));
            
            playersScore[basePlayer] = score;
            ScoreChanged?.Invoke(basePlayer, score, scoreForWin);

            if (GetScoreForPlayer(basePlayer) >= scoreForWin)
            {
                if (basePlayer == Player)
                    Win();
                else
                    Lose();
            }
        }
    }
}