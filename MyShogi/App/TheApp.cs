﻿using System;
using System.Windows.Forms;
using MyShogi.Model.Common.ObjectModel;
using MyShogi.Model.Common.Utility;
using MyShogi.Model.Resource.Images;
using MyShogi.Model.Resource.Sounds;
using MyShogi.Model.Shogi.LocalServer;

// とりま、Windows用
// あとで他環境用を用意する
using MyShogi.View.Win2D;
using MyShogi.ViewModel;

namespace MyShogi.App
{
    /// <summary>
    /// このアプリケーション
    /// singletonで生成
    /// </summary>
    public class TheApp
    {
        /// <summary>
        /// ここが本アプリのエントリーポイント
        /// </summary>
        public void Run()
        {
            // -- 開発時のテストコード

            // 駒素材画像の変換
            //ImageConverter.ConvertPieceImage();
            //ImageConverter.ConvertBoardNumberImage();

            // -- global configの読み込み

            config = GlobalConfig.CreateInstance();

            // -- 各インスタンスの生成と、それぞれのbind作業

            // -- 画像の読み込み

            {
                imageManager = new ImageManager();
                imageManager.Update(); // ここでconfigに従い、画像が読み込まれる。

                // GlobalConfigのプロパティ変更に対して、このimageManagerが呼び出されるようにbindしておく。

                config.AddPropertyChangedHandler("BoardImageVersion", imageManager.UpdateBoardImage);
                config.AddPropertyChangedHandler("TatamiImageVersion", imageManager.UpdateBoardImage);
                config.AddPropertyChangedHandler("KomadaiImageVersion", imageManager.UpdateBoardImage);
                config.AddPropertyChangedHandler("InTheBoardEdit", imageManager.UpdateBoardImage);

                config.AddPropertyChangedHandler("PieceImageVersion", imageManager.UpdatePieceImage);
                config.AddPropertyChangedHandler("PieceAttackImageVersion", imageManager.UpdatePieceAttackImage);
                
                config.AddPropertyChangedHandler("LastMoveFromColorType", imageManager.UpdatePieceMoveImage);
                config.AddPropertyChangedHandler("LastMoveToColorType", imageManager.UpdatePieceMoveImage);
                config.AddPropertyChangedHandler("PickedMoveFromColorType", imageManager.UpdatePieceMoveImage);
                config.AddPropertyChangedHandler("PickedMoveToColorType", imageManager.UpdatePieceMoveImage);

                config.AddPropertyChangedHandler("BoardNumberImageVersion", imageManager.UpdateBoardNumberImage);
            }

            // -- メインの対局ウィンドゥ

            var mainDialog = new MainDialog();
            mainForm = mainDialog;

            mainDialogViewModel = new MainDialogViewModel();
            mainDialog.ViewModel = mainDialogViewModel;

            // -- 対局controllerを1つ生成して、メインの対局ウィンドゥのViewModelに加える

            var gameServer = new LocalGameServer();
            mainDialogViewModel.gameServer = gameServer;

            // LocalGameServerの対局情報と棋譜ウィンドウが更新されたときにメインウインドウの盤面・棋譜ウィンドウに
            // 更新がかかるようにしておく。

            gameServer.AddPropertyChangedHandler("KifuList", mainDialog.gameScreen.kifuControl.OnListChanged , mainDialog);
            gameServer.AddPropertyChangedHandler("Position", mainDialog.gameScreen.PositionChanged);
            gameServer.AddPropertyChangedHandler("TurnChanged", mainDialog.gameScreen.TurnChanged , mainDialog);
            gameServer.AddPropertyChangedHandler("InTheGame", mainDialog.gameScreen.InTheGameChanged , mainDialog);
            gameServer.AddPropertyChangedHandler("InTheGame", mainDialog.UpdateMenuItems , mainDialog);
            gameServer.AddPropertyChangedHandler("EngineInitializing", mainDialog.gameScreen.EngineInitializingChanged , mainDialog);
            gameServer.AddPropertyChangedHandler("RestTimeChanged", mainDialog.gameScreen.RestTimeChanged);
            gameServer.AddPropertyChangedHandler("BoardReverse", mainDialog.UpdateMenuItems , mainDialog);
            gameServer.AddPropertyChangedHandler("GameServerStarted", mainDialog.UpdateMenuItems , mainDialog);
            gameServer.AddPropertyChangedHandler("SetKifuListIndex", mainDialog.gameScreen.SetKifuListIndex , mainDialog);

            // 盤・駒が変更されたときにMainDialogのメニューの内容を修正しないといけないので更新がかかるようにしておく。

            config.AddPropertyChangedHandler("BoardImageVersion", mainDialog.UpdateMenuItems , mainDialog);
            config.AddPropertyChangedHandler("TatamiImageVersion", mainDialog.UpdateMenuItems, mainDialog);
            config.AddPropertyChangedHandler("PieceImageVersion", mainDialog.UpdateMenuItems, mainDialog);
            config.AddPropertyChangedHandler("PromotePieceColorType", mainDialog.UpdateMenuItems, mainDialog);
            config.AddPropertyChangedHandler("PieceAttackImageVersion", mainDialog.UpdateMenuItems, mainDialog);
            config.AddPropertyChangedHandler("BoardNumberImageVersion", mainDialog.UpdateMenuItems, mainDialog);
            config.AddPropertyChangedHandler("LastMoveFromColorType", mainDialog.UpdateMenuItems, mainDialog);
            config.AddPropertyChangedHandler("LastMoveToColorType", mainDialog.UpdateMenuItems, mainDialog);
            config.AddPropertyChangedHandler("PickedMoveFromColorType", mainDialog.UpdateMenuItems, mainDialog);
            config.AddPropertyChangedHandler("PickedMoveToColorType", mainDialog.UpdateMenuItems, mainDialog);
            config.AddPropertyChangedHandler("TurnDisplay", mainDialog.UpdateMenuItems, mainDialog);
            config.AddPropertyChangedHandler("PieceSoundInTheGame", mainDialog.UpdateMenuItems, mainDialog);
            config.AddPropertyChangedHandler("KifuReadOut", mainDialog.UpdateMenuItems, mainDialog);
            config.AddPropertyChangedHandler("ReadOutSenteGoteEverytime", mainDialog.UpdateMenuItems, mainDialog);
            config.AddPropertyChangedHandler("MemoryLoggingEnable", mainDialog.UpdateMenuItems, mainDialog);
            config.AddPropertyChangedHandler("FileLoggingEnable", mainDialog.UpdateMenuItems, mainDialog);
            config.AddPropertyChangedHandler("InTheBoardEdit", mainDialog.UpdateMenuItems, mainDialog);

            // 盤面編集時などに棋譜ウィンドウを消す

            config.AddPropertyChangedHandler("InTheBoardEdit", mainDialog.gameScreen.UpdateKifuControlVisibility , mainDialog);

            // -- ロギング用のハンドラをセット

            var MemoryLoggingEnableHandler = new PropertyChangedEventHandler((args) =>
            {
                if (config.MemoryLoggingEnable)
                    Log.log1 = new MemoryLog();
                else
                {
                    if (Log.log1 != null)
                        Log.log1.Dispose();
                    Log.log1 = null;
                }
            });
            var FileLoggingEnable = new PropertyChangedEventHandler((args) =>
            {
                if (config.FileLoggingEnable)
                {
                    var now = DateTime.Now;
                    Log.log2 = new FileLog($"log{now.ToString("yyyyMMddHHmm")}.txt");
                }
                else
                {
                    if (Log.log2 != null)
                        Log.log2.Dispose();
                    Log.log2 = null;
                }
            });

            config.AddPropertyChangedHandler("MemoryLoggingEnable", MemoryLoggingEnableHandler);
            config.AddPropertyChangedHandler("FileLoggingEnable", FileLoggingEnable);

            // 上のハンドラを呼び出して、必要ならばロギングを開始しておいてやる。
            MemoryLoggingEnableHandler(null);
            FileLoggingEnable(null);

            // 初期化が終わったのでgameServerの起動を行う。
            gameServer.Start();

            // サウンド
            soundManager = new SoundManager();
            soundManager.Start();

            // 終了するときに設定ファイルに書き出すコード
            Application.ApplicationExit += new EventHandler((sender,e) =>
            {
                config.Save();
                soundManager.Dispose();
            });

            Application.Run(mainDialog);
        }

        /// <summary>
        /// 最前面に来るようにしてMessageBox.Show(text)を呼び出す。
        /// </summary>
        /// <param name="text"></param>
        public void MessageShow(string text)
        {
            if (mainForm != null && mainForm.IsHandleCreated && !mainForm.IsDisposed)
            {
                if (mainForm.InvokeRequired)
                    mainForm.Invoke(new Action(() => { MessageBox.Show(mainForm, text); }));
                else
                    MessageBox.Show(mainForm, text);
            }
            else
                MessageBox.Show(text);
        }

        /// <summary>
        /// 最前面に来るようにしてMessageBox.Show(text,caption)を呼び出す。
        /// </summary>
        public void MessageShow(string text , string caption)
        {
            if (mainForm != null)
                MessageBox.Show(mainForm, text , caption);
            else
                MessageBox.Show(text , caption);
        }

        // -- それぞれのViewModel
        // 他のViewModelにアクションが必要な場合は、これを経由して通知などを行えば良い。
        // 他のViewに直接アクションを起こすことは出来ない。必ずViewModelに通知などを行い、
        // そのViewModelのpropertyをsubscribeしているViewに間接的に通知が行くという仕組みを取る。

        /// <summary>
        /// MainDialogのViewModel
        /// </summary>
        public MainDialogViewModel mainDialogViewModel { get; private set; }

        /// <summary>
        /// 画像の読み込み用。本GUIで用いる画像はすべてここから取得する。
        /// </summary>
        public ImageManager imageManager { get; private set; }

        /// <summary>
        /// GUIの全体設定
        /// </summary>
        public GlobalConfig config { get; private set; }

        /// <summary>
        /// サウンドマネージャー
        /// </summary>
        public SoundManager soundManager { get; private set; }

        /// <summary>
        /// メインのForm
        /// これがないとMessageBox.Show()などで親を指定できなくて困る。
        /// </summary>
        public Form mainForm { get; private set; }

        /// <summary>
        /// singletonなinstance。それぞれのViewModelなどにアクセスしたければ、これ経由でアクセスする。
        /// </summary>
        public static TheApp app = new TheApp();
    }
}
