namespace FolderSync
{
    public partial class Form1 : Form
    {
        //private const int FixedWidth = 400;
        private string sourceFolderPath = string.Empty;
        private string destinationFolderPath = string.Empty;
        private const string SettingsFileName = "FolderSyncSettings.json";
        private CancellationTokenSource? _cancellationTokenSource;

        public Form1()
        {
            InitializeComponent();
        }

        private void LoadIconSafely()
        {
            try
            {
                // まず埋め込みリソースから読み込みを試みる
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                var resourceName = "FolderSync.Resources.app-icon.ico";
                
                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        this.Icon = new Icon(stream);
                        return;
                    }
                }
                
                // 埋め込みリソースが見つからない場合、ファイルパスから読み込み
                var iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "app-icon.ico");
                if (File.Exists(iconPath))
                {
                    this.Icon = new Icon(iconPath);
                }
            }
            catch
            {
                // アイコンの読み込みに失敗してもアプリは続行
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.Width = FixedWidth;  // ウィンドウ幅を固定する場合
            LoadSettings();
        }

        private void LoadSettings()
        {
            try
            {
                string settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SettingsFileName);
                if (File.Exists(settingsPath))
                {
                    string json = File.ReadAllText(settingsPath);
                    var settings = System.Text.Json.JsonSerializer.Deserialize<AppSettings>(json);
                    
                    if (settings != null)
                    {
                        if (!string.IsNullOrEmpty(settings.LastSourceFolder) && Directory.Exists(settings.LastSourceFolder))
                        {
                            sourceFolderPath = settings.LastSourceFolder;
                            textBoxSource.Text = sourceFolderPath;
                        }
                        
                        if (!string.IsNullOrEmpty(settings.LastDestinationFolder) && Directory.Exists(settings.LastDestinationFolder))
                        {
                            destinationFolderPath = settings.LastDestinationFolder;
                            textBoxDestination.Text = destinationFolderPath;
                        }

                        AddLog("前回の設定を読み込みました。");
                    }
                }
            }
            catch (Exception ex)
            {
                AddLog($"設定の読み込みに失敗: {ex.Message}");
            }
        }

        private void SaveSettings()
        {
            try
            {
                var settings = new AppSettings
                {
                    LastSourceFolder = sourceFolderPath,
                    LastDestinationFolder = destinationFolderPath
                };

                string settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SettingsFileName);
                string json = System.Text.Json.JsonSerializer.Serialize(settings, new System.Text.Json.JsonSerializerOptions 
                                { 
                    WriteIndented = true 
                });
                File.WriteAllText(settingsPath, json);
            }
            catch (Exception ex)
            {
                AddLog($"設定の保存に失敗: {ex.Message}");
            }
        }

        private void buttonBrowseSource_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "ソースフォルダを選択してください";
                
                // 前回選択したフォルダから開始
                if (!string.IsNullOrEmpty(sourceFolderPath) && Directory.Exists(sourceFolderPath))
                {
                    folderDialog.InitialDirectory = sourceFolderPath;
                }
                
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    sourceFolderPath = folderDialog.SelectedPath;
                    textBoxSource.Text = sourceFolderPath;
                    AddLog($"ソースフォルダを選択: {sourceFolderPath}");
                    SaveSettings();
                }
            }
        }

        private void buttonBrowseDestination_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "同期先フォルダを選択してください";
                
                // 前回選択したフォルダから開始
                if (!string.IsNullOrEmpty(destinationFolderPath) && Directory.Exists(destinationFolderPath))
                {
                    folderDialog.InitialDirectory = destinationFolderPath;
                }
                
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    destinationFolderPath = folderDialog.SelectedPath;
                    textBoxDestination.Text = destinationFolderPath;
                    AddLog($"同期先フォルダを選択: {destinationFolderPath}");
                    SaveSettings();
                }
            }
        }

        private void buttonClearLog_Click(object sender, EventArgs e)
        {
            textBoxLog.Clear();
            AddLog("ログをクリアしました。");
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // 幅を固定にする場合はここで制御可能
        }

        private async void buttonSync_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(sourceFolderPath) || string.IsNullOrEmpty(destinationFolderPath))
            {
                MessageBox.Show("ソースフォルダと同期先フォルダの両方を選択してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Directory.Exists(sourceFolderPath))
            {
                MessageBox.Show("ソースフォルダが存在しません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string confirmMessage = $"以下のフォルダ同期を実行しますか？\n\nソース: {sourceFolderPath}\n同期先: {destinationFolderPath}\n\n同期先フォルダにファイルをコピーします。";
            
            if (checkBoxDryRun.Checked)
            {
                confirmMessage += "\n\n【テストモード】実際のファイル操作は行いません。";
            }
            else if (checkBoxDeleteExtra.Checked)
            {
                confirmMessage += "\n\n【注意】同期先にのみ存在するファイルは削除されます！";
            }

            var result = MessageBox.Show(
                confirmMessage,
                "確認",
                MessageBoxButtons.YesNo,
                checkBoxDryRun.Checked ? MessageBoxIcon.Information : 
                (checkBoxDeleteExtra.Checked ? MessageBoxIcon.Warning : MessageBoxIcon.Question));

            if (result != DialogResult.Yes)
            {
                return;
            }

            // キャンセルトークンを作成
            _cancellationTokenSource = new CancellationTokenSource();

            buttonSync.Enabled = false;
            buttonCancel.Enabled = true;
            buttonBrowseSource.Enabled = false;
            buttonBrowseDestination.Enabled = false;
            checkBoxDeleteExtra.Enabled = false;
            checkBoxDryRun.Enabled = false;
            progressBar.Value = 0;

            try
            {
                AddLog("=== 同期開始 ===");
                
                // 新しいコアエンジンを使用
                var engine = new Core.FolderSyncEngine();
                var options = new Core.SyncOptions
                {
                    SourcePath = sourceFolderPath,
                    DestinationPath = destinationFolderPath,
                    DeleteExtraFiles = checkBoxDeleteExtra.Checked,
                    DryRun = checkBoxDryRun.Checked,
                    MaxDegreeOfParallelism = Environment.ProcessorCount, // CPUコア数を使用
                    LogCallback = (msg) => AddLog(msg),
                    ProgressCallback = (progress) => UpdateProgress(progress),
                    CancellationToken = _cancellationTokenSource.Token
                };

                var syncResult = await Task.Run(() => engine.Synchronize(options));

                if (_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    AddLog("=== 同期が中断されました ===");
                    AddLog($"処理時間: {syncResult.Duration.TotalSeconds:F2}秒");
                    AddLog($"コピー: {syncResult.CopiedFiles}ファイル, スキップ: {syncResult.SkippedFiles}ファイル");
                    
                    MessageBox.Show("同期が中断されました。", "中断", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (syncResult.Success)
                {
                    var completionMessage = checkBoxDryRun.Checked ? "=== テスト完了 ===" : "=== 同期完了 ===";
                    AddLog(completionMessage);
                    AddLog($"処理時間: {syncResult.Duration.TotalSeconds:F2}秒");
                    AddLog($"コピー: {syncResult.CopiedFiles}ファイル, スキップ: {syncResult.SkippedFiles}ファイル");
                    
                    if (syncResult.DeletedFiles > 0)
                    {
                        AddLog($"削除: {syncResult.DeletedFiles}ファイル, {syncResult.DeletedDirectories}フォルダ");
                    }

                    var dialogMessage = checkBoxDryRun.Checked ? 
                        "テストが完了しました。\n実際のファイル操作は行われませんでした。" : 
                        "フォルダ同期が完了しました。";
                    MessageBox.Show(dialogMessage, "完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    AddLog($"=== 同期失敗 ===");
                    AddLog($"エラー: {syncResult.ErrorMessage}");
                    
                    if (syncResult.Errors.Count > 0)
                    {
                        AddLog($"エラー詳細: {syncResult.Errors.Count}件");
                    }

                    MessageBox.Show($"同期中にエラーが発生しました:\n{syncResult.ErrorMessage}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                AddLog($"エラー: {ex.Message}");
                MessageBox.Show($"同期中にエラーが発生しました:\n{ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                buttonSync.Enabled = true;
                buttonCancel.Enabled = false;
                buttonBrowseSource.Enabled = true;
                buttonBrowseDestination.Enabled = true;
                checkBoxDeleteExtra.Enabled = true;
                checkBoxDryRun.Enabled = true;
                progressBar.Value = 0;
                
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (_cancellationTokenSource != null && !_cancellationTokenSource.Token.IsCancellationRequested)
            {
                AddLog("同期の停止を要求しています...");
                _cancellationTokenSource.Cancel();
                buttonCancel.Enabled = false;
            }
        }

        private void AddLog(string message)
        {
            if (textBoxLog.InvokeRequired)
            {
                textBoxLog.Invoke(new Action(() =>
                {
                    textBoxLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\r\n");
                    textBoxLog.SelectionStart = textBoxLog.Text.Length;
                    textBoxLog.ScrollToCaret();
                }));
            }
            else
            {
                textBoxLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\r\n");
                textBoxLog.SelectionStart = textBoxLog.Text.Length;
                textBoxLog.ScrollToCaret();
            }
        }

        private void UpdateProgress(int percentage)
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new Action(() => progressBar.Value = percentage));
            }
            else
            {
                progressBar.Value = percentage;
            }
        }

        // ドラッグ&ドロップ: ソースフォルダ
        private void textBoxSource_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true)
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void textBoxSource_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data?.GetData(DataFormats.FileDrop) is string[] files && files.Length > 0)
            {
                var path = files[0];
                
                // ファイルの場合は親ディレクトリを取得
                if (File.Exists(path))
                {
                    path = Path.GetDirectoryName(path) ?? path;
                }

                if (Directory.Exists(path))
                {
                    sourceFolderPath = path;
                    textBoxSource.Text = sourceFolderPath;
                    AddLog($"ソースフォルダを設定: {sourceFolderPath}");
                    SaveSettings();
                }
                else
                {
                    MessageBox.Show("有効なフォルダをドロップしてください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        // ドラッグ&ドロップ: 同期先フォルダ
        private void textBoxDestination_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true)
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void textBoxDestination_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data?.GetData(DataFormats.FileDrop) is string[] files && files.Length > 0)
            {
                var path = files[0];
                
                // ファイルの場合は親ディレクトリを取得
                if (File.Exists(path))
                {
                    path = Path.GetDirectoryName(path) ?? path;
                }

                if (Directory.Exists(path))
                {
                    destinationFolderPath = path;
                    textBoxDestination.Text = destinationFolderPath;
                    AddLog($"同期先フォルダを設定: {destinationFolderPath}");
                    SaveSettings();
                }
                else
                {
                    MessageBox.Show("有効なフォルダをドロップしてください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }

    public class AppSettings
    {
        public string LastSourceFolder { get; set; } = string.Empty;
        public string LastDestinationFolder { get; set; } = string.Empty;
    }
}