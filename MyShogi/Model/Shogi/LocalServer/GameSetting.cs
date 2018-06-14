﻿using MyShogi.Model.Shogi.Core;
using MyShogi.Model.Shogi.Player;

namespace MyShogi.Model.Shogi.LocalServer
{
    /// <summary>
    /// 各プレイヤーごとの対局設定
    /// LocalGameServer.GameStart()の引数に渡す、対局条件などを一式書いた設定データの片側のプレイヤー分。
    /// </summary>
    public class PlayerGameSetting
    {
        public PlayerTypeEnum PlayerType;
    }

    /// <summary>
    /// LocalGameServer.GameStart()の引数に渡す、対局条件などを一式書いた設定データ
    /// </summary>
    public class GameSetting
    {
        /// <summary>
        /// c側の設定情報を取得する。
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public PlayerGameSetting Player(Color c) { return players[(int)c]; }

        public PlayerGameSetting[] players = new PlayerGameSetting[2] { new PlayerGameSetting(), new PlayerGameSetting() };
    }
}
