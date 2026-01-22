namespace FolderSync.Core
{
    /// <summary>
    /// フォルダ同期の結果
    /// </summary>
    public class SyncResult
    {
        /// <summary>
        /// 同期が正常に終了したかどうか
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// コピーされたファイル数
        /// </summary>
        public int CopiedFiles { get; set; }

        /// <summary>
        /// スキップされたファイル数
        /// </summary>
        public int SkippedFiles { get; set; }

        /// <summary>
        /// 削除されたファイル数
        /// </summary>
        public int DeletedFiles { get; set; }

        /// <summary>
        /// 削除された空フォルダ数
        /// </summary>
        public int DeletedDirectories { get; set; }

        /// <summary>
        /// エラーメッセージ（エラーが発生した場合）
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// 同期にかかった時間
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// エラーの詳細リスト
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();
    }
}
