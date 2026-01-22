# KoFolderSync ビルドガイド

## クイックスタート

### 推奨: フレームワーク依存版（軽量）

```powershell
.\build.ps1
```

**出力:**
- `dist\KoFolderSync.exe` - 約172KB
- `dist\FolderSync.dll` - 約42KB
- `dist\README.md`
- `dist\KoFolderSync-1.0.0-release.zip` - リリース用ZIPパッケージ（約112KB）

**必要環境:**
- .NET 10.0 Desktop Runtime (x64)

---

## バージョン番号の変更

`build.ps1` の先頭でバージョン番号を変更できます：

```powershell
$version = "1.0.0"  # ここを変更
```

ZIPファイル名は自動的に `KoFolderSync-{version}-release.zip` になります。

---

## ビルド方法の比較

### 方法1: フレームワーク依存版（推奨）

**特徴:**
- ✓ 超軽量（EXE: 172KB, DLL: 42KB）
- ✓ ダウンロード・配布が高速
- ✗ .NET 10.0 Desktop Runtime が必要

**ビルド:**
```powershell
.\build.ps1
```

または

```powershell
dotnet publish FolderSync.csproj -c Release -r win-x64 --self-contained false -o dist
```

---

### 方法2: 自己完結版（大容量）

**特徴:**
- ✓ .NET ランタイム不要
- ✓ 単体で動作
- ✗ 約110MB（約640倍大きい）

**ビルド:**
```powershell
dotnet publish FolderSync.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=true -o dist
```

---

## ファイルサイズ比較

| ビルド方法 | EXEサイズ | 合計サイズ | .NET Runtime |
|-----------|----------|-----------|--------------|
| フレームワーク依存版 | 172KB | 約220KB | 必要 |
| 自己完結版（単一ファイル） | 110MB | 110MB | 不要 |

**サイズ比: 自己完結版は約500倍大きい**

---

## 推奨配布方法

### 一般ユーザー向け（推奨）
**フレームワーク依存版**
- 超軽量でダウンロードが一瞬
- README.md に .NET Runtime のインストール方法を記載
- ほとんどのWindows PCには既に.NETがインストール済み

### 特殊な環境向け
**自己完結版**
- インターネット接続がない環境
- ランタイムのインストールが困難な環境
- ただし、ファイルサイズが非常に大きい

---

## 配布用ZIPの作成

ZIPファイルは `build.ps1` を実行すると自動的に作成されます。

**手動で作成する場合:**
```powershell
# フレームワーク依存版
Compress-Archive -Path dist\* -DestinationPath KoFolderSync-1.0.0-release.zip

# 自己完結版
Compress-Archive -Path dist\KoFolderSync.exe, dist\README.md -DestinationPath KoFolderSync-1.0.0-standalone.zip
```

---

## .NET Runtime のインストール

ユーザーが .NET 10.0 Desktop Runtime をインストールする必要があります。

**ダウンロード:**
https://dotnet.microsoft.com/download/dotnet/10.0

**インストール手順:**
1. 上記URLにアクセス
2. 「.NET Desktop Runtime 10.0.x」の「x64」をダウンロード
3. インストーラーを実行
4. KoFolderSync.exe を実行

---

## トラブルシューティング

### ビルドが失敗する
```powershell
dotnet --version
```
.NET 10.0 SDK がインストールされているか確認

### EXEが実行できない
.NET 10.0 Desktop Runtime (x64) をインストール

### ファイルサイズを更に小さくしたい
フレームワーク依存版が既に最小サイズです（172KB）

---

## リリースチェックリスト

- [ ] バージョン番号を更新（Form1.Designer.cs, FolderSync.csproj）
- [ ] README.md を更新
- [ ] `.\build.ps1` を実行
- [ ] 動作確認（Windows 10/11）
- [ ] ZIPファイルを作成
- [ ] リリースノートを作成
- [ ] GitHubにタグを作成
- [ ] リリースを公開
