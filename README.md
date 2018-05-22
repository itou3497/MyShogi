﻿
# About this project

MyShogi is an open source GUI for computer Shogi engine.

MyShogiは、オープンソースの将棋ソフト用GUIです。
2018年にマイナビさんから発売する商用版のやねうら王用のGUIとして開発が始まりました。

# 使い方

USIプロトコル対応の将棋エンジンが使えます。// 以下、書きかけ

# 初回リリースで実装する予定の機能

- わかりやすいエンジン設定
- CPU自動判定
- 通常対局機能
- 詰将棋エンジンの利用
- 検討機能
- 形勢のグラフ表示
- KIF/KI2/CSA/SFEN/PSN/[PSN2](MyShogi/docs/PSN2format.md)形式の棋譜の入出力機能。分岐棋譜対応。
- 入玉宣言勝ちの条件変更(24点法、27点法、トライルール)

現在の作業状況は、[こちら](MyShogi/docs/progress.md)にあります。

# 商用版について

## 商用版にのみ搭載される機能

- エンジンバナー
- 指し手の読み上げ(音声)、秒読み(音声)
- 1文字駒、2文字駒、英文字駒

## 商用版に搭載される予定のエンジン

- tanuki-(2018年版) : WCSC28版からR50ぐらいup
- tanuki-(SDT5 優勝バージョン) 
- Qhapaq(2018年版)
- 読み太(2018年版)
- やねうら王(2018年版) : KPP_KKPT型 , depth 12の教師50億局面から学習

# 将来的に実装するかも知れない機能

- ベンチマーク
- 多言語対応
- 定跡編集機能
- MultiPonder(ponderを複数のインスタンスに対して同時に行う)
- 並列自動対局(ソフトの自己対局を複数インスタンスで同時に行う)
- 蠱毒(複数のソフトの自動対局、自動レーティング算出)
- クラスター探索機能
- 通信対局(1対1)
- floodgate対応
- AWSにエンジンを配置しての利用
- Mac対応。ニーズ、作業量を検討した上で。
- 定跡編集機能
- [USI2.0](docs/USI2.0.md)対応
- オンライン対局
- スマホ版(iOS/Android)

# 本プロジェクトが提案するフォーマット

- [PSN2format](MyShogi/docs/PSN2format.md) : PSN形式から改良された棋譜ファイルフォーマット
- [USI2.0](MyShogi/docs/USI2.0.md) : USIプロトコルから改良されたプロトコル

# 謝辞

皆様、ありがとうございます。

- tanuki-のエンジン開発者の野田さん、那須さん、たぬきチームのメンバー
- WhaleWatcherのソースコードを提供してくれた、えびふらいさん
- GUIの開発を協力してくれたMizarさん
- やねうら王に貢献してくださっている方々
- Stockfishの開発チーム

# ライセンス

// 考え中

- ソースコードはGPLv3
- 画面素材、音声素材の単体配布(二次利用等)は禁止

