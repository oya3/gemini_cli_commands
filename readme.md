# 実験内容

- [実験内容](#実験内容)
  - [gemini cli に追加した拡張コマンド](#gemini-cli-に追加した拡張コマンド)
    - [docx2pdf](#docx2pdf)
    - [/read\_word](#read_word)
  - [Wordファイルの解析・変換手法 比較](#wordファイルの解析変換手法-比較)
    - [結論](#結論)
  - [WindowsをMCP経由で操作するための主要ツール3選](#windowsをmcp経由で操作するための主要ツール3選)
    - [1. Windows-MCP (by CursorTouch)](#1-windows-mcp-by-cursortouch)
    - [2. mcp-windows-desktop-automation](#2-mcp-windows-desktop-automation)
    - [3. MCP on Windows (Microsoft公式)](#3-mcp-on-windows-microsoft公式)
    - [導入と使い分けのイメージ](#導入と使い分けのイメージ)
  - [MCP 実験](#mcp-実験)
    - [1. Windows-MCP (by CursorTouch)](#1-windows-mcp-by-cursortouch-1)
      - [ビルド手順](#ビルド手順)
  - [参考](#参考)
    - [URL](#url)
    - [AI履歴](#ai履歴)
  - [メモ](#メモ)


## gemini cli に追加した拡張コマンド

```
.gemini
└── commands
    ├── docx2pdf.toml
    └── read_word.toml
```

### docx2pdf

指定したdocxファイルをpdfファイルに変換する
gemini から docx2pdf.ps1 が実行される

```ps1
/docx2pdf sample2020.docx
```

### /read_word

**失敗。この方法だとうまくいかない。。。**
wordを読み込んでbase64にして送信するサンプル
gemini から read_word.py が実行される

```ps1
/read_word sample2020.docx
```


## Wordファイルの解析・変換手法 比較

| 手法 | 内容 | メリット | デメリット |
| :--- | :--- | :--- | :--- |
| **PDF変換 (PowerShell/COM)** | Wordの機能でPDFに書き出し、そのバイナリをBase64で送信 | **図表・レイアウトが完璧に維持される**。人間が見るのと同じ状態でGeminiが視覚解析できるため、業務フローの理解に最適。 | Windows環境（Wordインストール済）が必要。変換処理に数秒のオーバーヘッドがある。 |
| **python-docx** | Pythonライブラリでxml構造からテキストのみを抽出 | 動作が高速で、Officeが未インストールの環境（Linux等）でも動く。テキスト情報の検索には強い。 | **図表やレイアウト情報が欠落する**。図の中の文字や、文章と図の位置関係がわからなくなるため、仕様書の解析には不向き。 |
| **Pandoc** | ドキュメント変換ツールを使用して形式変換 | 多くのフォーマットに対応している。 | 図表の再現性が低く、PDF化にはLaTeX等の複雑な環境構築が必要。日本語の文字化けリスクが高い。 |
| **テキスト直接貼り付け** | Wordを全選択してコピー＆ペースト | 最も手軽。 | 図表が一切反映されない。ページ数が多いとプロンプトの制限に抵触しやすく、構造が崩れる。 |

### 結論
**業務仕様書からテストケースを作成する場合**は、図表のコンテキストが不可欠なため、**「PowerShellによるPDF変換」**が最も精度が高く、推奨される手法です。


## WindowsをMCP経由で操作するための主要ツール3選

Windows OSやその上のアプリ（C#、VB6等）をGemini CLIなどのAIから操作する場合、以下の3つが主要な選択肢となります。それぞれの役割と強みは以下の通りです。

---

### 1. Windows-MCP (by CursorTouch)
**【役割：今すぐ使える「万能な手」】**
* **特徴:** Windows標準の**UI Automation (UIA)** をフル活用するためのサーバー。
* **強み:** 画面上の要素（ボタン、入力欄、ラベル）を「名前」や「ID」で探し出す能力に長けています。
* **活用シーン:** 独自に作成したC#アプリの特定のテキストボックスに値を入力させるといった、GUI操作に最も向いています。
* GitHub: https://github.com/CursorTouch/Windows-MCP

### 2. mcp-windows-desktop-automation
**【役割：古いアプリに強い「職人の手」】**
* **特徴:** 自動化の老舗ツール「AutoIt」をベースにしたMCPサーバー。
* **強み:** 最新のツールでは認識しにくい**VB6などの古いウィンドウ構造**を特定し、操作するのが非常に得意です。
* **活用シーン:** `Windows-MCP` でうまくコントロールを掴めない古い業務アプリや、特殊なウィンドウ操作が必要な場合に威力を発揮します。
* GitHub: https://github.com/mario-andreschak/mcp-windows-desktop-automation

### 3. MCP on Windows (Microsoft公式)
**【役割：OSが提供する「公式の窓口」】**
* **特徴:** MicrosoftがWindows 11にネイティブに組み込み始めている公式基盤。
* **強み:** ファイル操作（エクスプローラー連携）やOSの設定変更など、システムの中核機能とAIを安全につなぎます。
* **注意点:** 現在はプレビュー版（Windows Insider）での展開が主ですが、将来的にはWindowsの標準機能となります。
* GitHub: https://github.com/mario-andreschak/mcp-windows-desktop-automation

---

### 導入と使い分けのイメージ

これら3つはGemini CLIの設定ファイル（`config.json` 等）に同時に登録でき、AIが目的に応じて使い分けることが可能です。



* **独自アプリの入力欄を操作したい:** まずは `Windows-MCP` を試す。
* **VB6等の古いアプリが動かない:** `mcp-windows-desktop-automation` を使う。
* **OSのファイル操作やシステム設定を変えたい:** `MCP on Windows` の公式コネクタを使う。

---
**次のステップ：**
実際にこれらをGemini CLIに登録するための**設定ファイル（json）の書き方**や、**インストールコマンド**を具体的に提示しましょうか？

## MCP 実験

### 1. Windows-MCP (by CursorTouch)

`windows-mcp` を使用して、生成した C# アプリ (`sample.exe`) の GUI を自動操作する実験。

#### ビルド手順

**通常のビルド（開発用）**

```powershell
cd sample_app
dotnet build -c Release
```

生成場所: `sample_app\bin\Release\net10.0-windows\` （複数のファイルが出力されます）

**単一実行ファイルの生成（配布用・軽量版）**

ビルドサイズを抑えるため、`.csproj` に設定を追加した上で、ランタイムを含めない形式で公開します。

```powershell
cd sample_app
dotnet publish -c Release -r win-x64 --self-contained false
```

生成場所: `sample_app\bin\Release\net10.0-windows\win-x64\publish\sample.exe` （約170KB）
※ 実行環境に .NET 10 デスクトップランタイムがインストールされている必要があります。

Windows-MCP を使って sample_app.exe を操作する実験
以下のプロンプトで自由にアプリを操作することができる

```
`sample_app` フォルダにある `sample.exe` を起動して、IPアドレスに `192.168.1.1`、ポートに `8888` を入力して保存ボタンを押してください。最後に `setting.json` が生成されたことを確認してください。
```

## 参考
### URL
- google gemini 公式 拡張サイト  
  https://geminicli.com/extensions/
- AIエージェントがWindowsを操作する！｜MCP on Windowsのスゴさとは  
  https://www.openbridge.jp/column/mcp-on-windows-intro
- WindowsでMCPホスト  
  https://zenn.dev/robon/articles/6c91a6c14c0618
- Windows on MCPがパブリックプレビューされました！  
  https://qiita.com/nwwn25/items/6f43d8c05907d94db455
- 自動UIテストやってみた: Appium,Windows Application Drive,WPF,.NET Framework4.7.2  
  このやり方古い。mcpで対応できる  
  https://zenn.dev/tigrebiz/articles/aca5c2030ef721  
  https://www.cresco.co.jp/blog/entry/3893.html  
- microsoft/WinAppDriver  
  https://github.com/microsoft/WinAppDriver/commits/master/

### AI履歴
- https://www.openbridge.jp/column/mcp-on-windows-intro の内容を誰でもわかるように説明して  
  https://gemini.google.com/share/e1d9780d6428

## メモ

```
 > windows-mcp MCP Server　を使ってnotepadを起動して本文に`test`を記入するための、プロンプト文を教えてください

✦ windows-mcp MCP Server を使ってメモ帳を起動し、本文に「test」と入力するためのプロンプト文は、以下のようになります。

   1 windows-mcp を使ってメモ帳 (notepad) を起動し、本文に "test" と入力してください。

  もし、より詳細な手順を指定したい場合は、以下のように記述することもできます。

   1 windows-mcp を使って以下の操作を行ってください：
   2 1. メモ帳 (notepad) を起動する。
   3 2. 起動したメモ帳のウィンドウにフォーカスを合わせる。
   4 3. 本文に "test" と入力する。

  これらのプロンプトを使えば、私は適切にツールを選択して実行することができます。
```