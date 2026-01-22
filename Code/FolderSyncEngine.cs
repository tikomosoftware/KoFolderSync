namespace FolderSync.Core
{
    /// <summary>
    /// フォルダ同期エンジン（マルチスレッド対応）
    /// </summary>
    public class FolderSyncEngine
    {
        private readonly object _lockObject = new object();
        private int _processedFiles = 0;

        /// <summary>
        /// フォルダ同期を実行
        /// </summary>
        /// <param name="options">同期オプション</param>
        /// <returns>同期結果</returns>
        public SyncResult Synchronize(SyncOptions options)
        {
            var result = new SyncResult();
            var startTime = DateTime.Now;
            _processedFiles = 0;

            try
            {
                // 入力検証
                if (!ValidateOptions(options, result))
                {
                    result.Success = false;
                    result.Duration = DateTime.Now - startTime;
                    return result;
                }

                // テストモードの表示
                if (options.DryRun)
                {
                    Log(options, "========================================");
                    Log(options, "【テストモード】実際のファイル操作は行いません");
                    Log(options, "========================================");
                }

                // 同期先フォルダの作成
                if (!Directory.Exists(options.DestinationPath))
                {
                    if (options.DryRun)
                    {
                        Log(options, $"[テスト] 同期先フォルダを作成: {options.DestinationPath}");
                    }
                    else
                    {
                        Directory.CreateDirectory(options.DestinationPath);
                        Log(options, $"同期先フォルダを作成: {options.DestinationPath}");
                    }
                }

                Log(options, $"並列処理モード: 最大{options.MaxDegreeOfParallelism}スレッド");

                // ファイルのコピー処理
                CopyFiles(options, result);

                // 余分なファイルの削除処理
                if (options.DeleteExtraFiles)
                {
                    Log(options, "--- 不要なファイルのチェック開始 ---");
                    DeleteExtraFiles(options, result);
                    Log(options, "--- 不要なファイルのチェック終了 ---");
                }

                ReportProgress(options, 100);
                result.Success = true;

                if (options.DryRun)
                {
                    Log(options, "========================================");
                    Log(options, "【テストモード完了】実際のファイル操作は行われませんでした");
                    Log(options, "========================================");
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                result.Errors.Add($"致命的なエラー: {ex.Message}");
                Log(options, $"エラー: {ex.Message}");
            }
            finally
            {
                result.Duration = DateTime.Now - startTime;
            }

            return result;
        }

        /// <summary>
        /// オプションの検証
        /// </summary>
        private bool ValidateOptions(SyncOptions options, SyncResult result)
        {
            if (string.IsNullOrEmpty(options.SourcePath))
            {
                result.ErrorMessage = "ソースフォルダが指定されていません。";
                result.Errors.Add(result.ErrorMessage);
                return false;
            }

            if (string.IsNullOrEmpty(options.DestinationPath))
            {
                result.ErrorMessage = "同期先フォルダが指定されていません。";
                result.Errors.Add(result.ErrorMessage);
                return false;
            }

            if (!Directory.Exists(options.SourcePath))
            {
                result.ErrorMessage = "ソースフォルダが存在しません。";
                result.Errors.Add(result.ErrorMessage);
                return false;
            }

            return true;
        }

        /// <summary>
        /// ファイルのコピー処理（並列処理版）
        /// </summary>
        private void CopyFiles(SyncOptions options, SyncResult result)
        {
            var allFiles = Directory.GetFiles(options.SourcePath, "*", SearchOption.AllDirectories);
            var totalFiles = allFiles.Length;

            if (totalFiles == 0)
            {
                Log(options, "コピーするファイルはありません。");
                return;
            }

            Log(options, $"合計 {totalFiles} ファイルを同期します。");

            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = options.MaxDegreeOfParallelism,
                CancellationToken = options.CancellationToken
            };

            try
            {
                Parallel.ForEach(allFiles, parallelOptions, (sourceFile) =>
                {
                    try
                    {
                        var relativePath = Path.GetRelativePath(options.SourcePath, sourceFile);
                        var destinationFile = Path.Combine(options.DestinationPath, relativePath);

                        // ディレクトリの作成（スレッドセーフ）
                        var destinationDir = Path.GetDirectoryName(destinationFile);
                        if (!string.IsNullOrEmpty(destinationDir))
                        {
                            lock (_lockObject)
                            {
                                if (!Directory.Exists(destinationDir))
                                {
                                    if (!options.DryRun)
                                    {
                                        Directory.CreateDirectory(destinationDir);
                                    }
                                }
                            }
                        }

                        bool shouldCopy = false;

                        if (!File.Exists(destinationFile))
                        {
                            shouldCopy = true;
                        }
                        else
                        {
                            var sourceInfo = new FileInfo(sourceFile);
                            var destInfo = new FileInfo(destinationFile);

                            if (sourceInfo.LastWriteTime > destInfo.LastWriteTime || sourceInfo.Length != destInfo.Length)
                            {
                                shouldCopy = true;
                            }
                        }

                        if (shouldCopy)
                        {
                            if (options.DryRun)
                            {
                                Log(options, $"[テスト] コピー: {relativePath}");
                            }
                            else
                            {
                                File.Copy(sourceFile, destinationFile, true);
                                Log(options, $"コピー: {relativePath}");
                            }
                            
                            lock (_lockObject)
                            {
                                result.CopiedFiles++;
                            }
                        }
                        else
                        {
                            Log(options, $"スキップ (最新): {relativePath}");
                            
                            lock (_lockObject)
                            {
                                result.SkippedFiles++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var errorMsg = $"ファイルコピーエラー: {sourceFile} - {ex.Message}";
                        
                        lock (_lockObject)
                        {
                            result.Errors.Add(errorMsg);
                        }
                        
                        Log(options, errorMsg);
                    }

                    // 進捗報告（スレッドセーフ）
                    int processed = Interlocked.Increment(ref _processedFiles);
                    var progress = (int)(processed * (options.DeleteExtraFiles ? 50.0 : 100.0) / totalFiles);
                    ReportProgress(options, progress);
                });
            }
            catch (OperationCanceledException)
            {
                result.ErrorMessage = "同期がキャンセルされました。";
                Log(options, result.ErrorMessage);
            }
        }

        /// <summary>
        /// 余分なファイルの削除処理（並列処理版）
        /// </summary>
        private void DeleteExtraFiles(SyncOptions options, SyncResult result)
        {
            var sourceFiles = Directory.GetFiles(options.SourcePath, "*", SearchOption.AllDirectories)
                .Select(f => Path.GetRelativePath(options.SourcePath, f))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var destFiles = Directory.GetFiles(options.DestinationPath, "*", SearchOption.AllDirectories);

            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = options.MaxDegreeOfParallelism,
                CancellationToken = options.CancellationToken
            };

            try
            {
                Parallel.ForEach(destFiles, parallelOptions, (destFile) =>
                {
                    try
                    {
                        var relativePath = Path.GetRelativePath(options.DestinationPath, destFile);

                        if (!sourceFiles.Contains(relativePath))
                        {
                            if (options.DryRun)
                            {
                                Log(options, $"[テスト] 削除: {relativePath}");
                            }
                            else
                            {
                                File.Delete(destFile);
                                Log(options, $"削除: {relativePath}");
                            }
                            
                            lock (_lockObject)
                            {
                                result.DeletedFiles++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var errorMsg = $"削除失敗: {destFile} - {ex.Message}";
                        
                        lock (_lockObject)
                        {
                            result.Errors.Add(errorMsg);
                        }
                        
                        Log(options, errorMsg);
                    }
                });
            }
            catch (OperationCanceledException)
            {
                Log(options, "削除処理がキャンセルされました。");
            }

            // 空のディレクトリを削除（シングルスレッドで実行）
            if (!options.DryRun)
            {
                DeleteEmptyDirectories(options.DestinationPath, options, result);
            }
            else
            {
                DeleteEmptyDirectoriesDryRun(options.DestinationPath, options, result);
            }

            if (result.DeletedFiles > 0)
            {
                Log(options, $"合計 {result.DeletedFiles} ファイルを削除しました。");
            }
            else
            {
                Log(options, "削除するファイルはありませんでした。");
            }
        }

        /// <summary>
        /// 空のディレクトリを削除
        /// </summary>
        private void DeleteEmptyDirectories(string directory, SyncOptions options, SyncResult result)
        {
            foreach (var subDir in Directory.GetDirectories(directory))
            {
                DeleteEmptyDirectories(subDir, options, result);

                if (!Directory.EnumerateFileSystemEntries(subDir).Any())
                {
                    try
                    {
                        Directory.Delete(subDir);
                        Log(options, $"空フォルダ削除: {Path.GetRelativePath(directory, subDir)}");
                        result.DeletedDirectories++;
                    }
                    catch (Exception ex)
                    {
                        var errorMsg = $"フォルダ削除失敗: {subDir} - {ex.Message}";
                        result.Errors.Add(errorMsg);
                        Log(options, errorMsg);
                    }
                }
            }
        }

        /// <summary>
        /// 空のディレクトリを削除（テストモード）
        /// </summary>
        private void DeleteEmptyDirectoriesDryRun(string directory, SyncOptions options, SyncResult result)
        {
            foreach (var subDir in Directory.GetDirectories(directory))
            {
                DeleteEmptyDirectoriesDryRun(subDir, options, result);

                if (!Directory.EnumerateFileSystemEntries(subDir).Any())
                {
                    try
                    {
                        Log(options, $"[テスト] 空フォルダ削除: {Path.GetRelativePath(directory, subDir)}");
                        result.DeletedDirectories++;
                    }
                    catch (Exception ex)
                    {
                        var errorMsg = $"フォルダ削除失敗: {subDir} - {ex.Message}";
                        result.Errors.Add(errorMsg);
                        Log(options, errorMsg);
                    }
                }
            }
        }

        /// <summary>
        /// ログ出力
        /// </summary>
        private void Log(SyncOptions options, string message)
        {
            options.LogCallback?.Invoke(message);
        }

        /// <summary>
        /// 進捗報告
        /// </summary>
        private void ReportProgress(SyncOptions options, int percentage)
        {
            options.ProgressCallback?.Invoke(percentage);
        }
    }
}
