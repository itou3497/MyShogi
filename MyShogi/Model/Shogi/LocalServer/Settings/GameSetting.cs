﻿using MyShogi.Model.Shogi.Core;
using MyShogi.Model.Shogi.Kifu;
using MyShogi.Model.Common.Utility;

namespace MyShogi.Model.Shogi.LocalServer
{

    /// <summary>
    /// LocalGameServer.GameStart()の引数に渡す、対局条件などを一式書いた設定データ
    /// 
    /// 対局ダイアログの設定情報。
    /// GlobalConfigに保存されていて、ここから次回起動時に対局ダイアログの設定を復元もできる。
    /// 
    /// ・BoardSetting
    /// ・MiscSetting
    /// ・TimeSetting
    /// ・PlayerSettings
    /// の集合
    /// 
    /// </summary>
    public class GameSetting
    {
        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public GameSetting()
        {
            // 初回起動時などデシリアライズに失敗した時のためにデフォルト値をきちんと設定しておく。

            Board = new BoardSetting();
            Players = new PlayerSetting[2] { new PlayerSetting(), new PlayerSetting() };

            // 先後入れ替えるので名前が「先手」「後手」がデフォルトだと紛らわしい。
            // 名前を「わたし」と「あなた」にしとく。
            Player(Color.BLACK).PlayerName = "わたし";
            Player(Color.WHITE).PlayerName = "あなた";

            KifuTimeSettings = new KifuTimeSettings();
            MiscSettings = new MiscSettings();
        }

        /// <summary>
        /// Clone()用のコンストラクタ
        /// </summary>
        /// <param name="players"></param>
        /// <param name="board"></param>
        /// <param name="timeSetting"></param>
        private GameSetting(PlayerSetting[] players , BoardSetting board ,
            KifuTimeSettings kifuTimeSettings , MiscSettings miscSettings )
        {
            Players = players;
            Board = board;
            KifuTimeSettings = kifuTimeSettings;
            MiscSettings = miscSettings;
        }

        /// <summary>
        /// このインスタンスのClone()
        /// </summary>
        /// <returns></returns>
        public GameSetting Clone()
        {
            // premitive typeでないとMemberwiseClone()でClone()されないので自前でCloneする。

            return new GameSetting(
                new PlayerSetting[2] { Players[0].Clone(), Players[1].Clone() },
                Board.Clone(),
                KifuTimeSettings.Clone(),
                MiscSettings.Clone()
            );
        }

        /// <summary>
        /// 開始盤面
        /// </summary>
        public BoardSetting Board;

        /// <summary>
        /// c側の設定情報を取得する。
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public PlayerSetting Player(Color c) { return Players[(int)c]; }

        /// <summary>
        /// 先手と後手のプレイヤーを入れ替える。
        /// TimeSettingsのほうも入れ替える。
        /// </summary>
        public void SwapPlayer()
        {
            Utility.Swap(ref Players[0], ref Players[1]);
            KifuTimeSettings.SwapPlayer();
        }

        /// <summary>
        /// 持ち時間設定
        /// </summary>
        public KifuTimeSettings KifuTimeSettings;

        /// <summary>
        /// その他の細かい設定
        /// </summary>
        public MiscSettings MiscSettings;

        /// <summary>
        /// このメンバーには直接アクセスせずに、Player(Color)のほうを用いて欲しい。
        /// XmlSerializerでシリアライズするときにpublicにしておかないとシリアライズ対象とならないので
        /// publicにしてある。
        /// </summary>
        public PlayerSetting[] Players;
    }
}
