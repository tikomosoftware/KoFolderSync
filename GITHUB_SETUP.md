# GitHub へのプッシュ手順

## 前提条件

✓ Gitリポジトリの初期化完了
✓ 最初のコミット完了

## 手順

### 1. GitHubでリポジトリを作成

1. https://github.com にアクセス
2. 右上の「+」→「New repository」をクリック
3. リポジトリ情報を入力：
   - **Repository name**: `KoFolderSync`
   - **Description**: シンプルで使いやすいフォルダ同期アプリケーション
   - **Public** または **Private** を選択
   - **README, .gitignore, license は追加しない**（既にローカルにあるため）
4. 「Create repository」をクリック

### 2. リモートリポジトリを追加

GitHubで作成したリポジトリのURLをコピーして、以下のコマンドを実行：

```powershell
# HTTPSの場合
git remote add origin https://github.com/YOUR_USERNAME/KoFolderSync.git

# SSHの場合（推奨）
git remote add origin git@github.com:YOUR_USERNAME/KoFolderSync.git
```

**YOUR_USERNAME** を自分のGitHubユーザー名に置き換えてください。

### 3. プッシュ

```powershell
git push -u origin main
```

初回プッシュ時に認証が求められる場合があります：
- **HTTPS**: GitHubのユーザー名とPersonal Access Token
- **SSH**: SSH鍵の設定が必要

### 4. 確認

GitHubのリポジトリページをブラウザで開いて、ファイルがアップロードされているか確認。

---

## トラブルシューティング

### 認証エラー（HTTPS）

Personal Access Tokenが必要です：

1. GitHub → Settings → Developer settings → Personal access tokens → Tokens (classic)
2. 「Generate new token」→「Generate new token (classic)」
3. スコープで「repo」にチェック
4. トークンを生成してコピー
5. パスワードの代わりにトークンを使用

### SSH鍵の設定

```powershell
# SSH鍵を生成（まだない場合）
ssh-keygen -t ed25519 -C "your_email@example.com"

# 公開鍵をコピー
Get-Content ~/.ssh/id_ed25519.pub | clip

# GitHubに公開鍵を追加
# Settings → SSH and GPG keys → New SSH key
```

### ブランチ名が違う場合

```powershell
# ブランチ名を確認
git branch

# mainでない場合（例: master）
git branch -M main
git push -u origin main
```

---

## 今後の更新手順

ファイルを変更した後：

```powershell
# 変更を確認
git status

# 変更をステージング
git add .

# コミット
git commit -m "Update: 変更内容の説明"

# プッシュ
git push
```

---

## リリースタグの作成

バージョンをリリースする場合：

```powershell
# タグを作成
git tag -a v1.0.0 -m "Release v1.0.0"

# タグをプッシュ
git push origin v1.0.0
```

GitHubでタグからReleaseを作成し、`dist\KoFolderSync-1.0.0-release.zip` をアップロードできます。

---

## 便利なコマンド

```powershell
# リモートリポジトリの確認
git remote -v

# コミット履歴の確認
git log --oneline

# 最後のコミットを修正
git commit --amend

# 変更を取り消す
git restore <file>
```
