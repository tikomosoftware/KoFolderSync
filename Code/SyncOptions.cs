namespace FolderSync.Core
{
    /// <summary>
    /// フォルダ同期のオプション設定
    /// </summary>
    public class SyncOptions
    {
        /// <summary>
        /// ソースフォルダのパス
        /// </summary>
        public string SourcePath { get; set; } = string.Empty;

        /// <summary>
        /// 同期先フォルダのパス
        /// </summary>
        public string DestinationPath { get; set; } = string.Empty;

        /// <summary>
        /// 同期先にのみ存在するファイルを削除するか（完全同期）
        /// </summary>
        public bool DeleteExtraFiles { get; set; }

        /// <summary>
        /// テストモード（ドライラン）: trueの場合、実際のファイル操作を行わずログのみ出力
        /// </summary>
        public bool DryRun { get; set; }

        /// <summary>
        /// 並列処理の最大並列度（デフォルト: CPUコア数）
        /// 1を指定すると並列処理を無効化（シングルスレッド）
        /// </summary>
        public int MaxDegreeOfParallelism { get; set; } = Environment.ProcessorCount;

        /// <summary>
        /// 進捗報告のコールバック（0-100の値）
        /// ※並列処理時は複数スレッドから呼ばれるため、スレッドセーフな実装が必要
        /// </summary>
        public Action<int>? ProgressCallback { get; set; }

        /// <summary>
        /// ログ出力のコールバック
        /// ※並列処理時は複数スレッドから呼ばれるため、スレッドセーフな実装が必要
        /// </summary>
        public Action<string>? LogCallback { get; set; }

        /// <summary>
        /// キャンセルトークン
        /// </summary>
        public CancellationToken CancellationToken { get; set; }
    }
}
