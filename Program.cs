namespace FolderSync
{
    internal static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                // アプリケーション構成をカスタマイズするには（高DPI設定やデフォルトフォントの設定など）、
                // https://aka.ms/applicationconfiguration を参照してください。
                ApplicationConfiguration.Initialize();
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                // エラーログをファイルに出力
                var logPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "KoFolderSync",
                    "error.log"
                );
                
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(logPath)!);
                    File.WriteAllText(logPath, 
                        $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]\n" +
                        $"Error: {ex.Message}\n" +
                        $"StackTrace:\n{ex.StackTrace}\n\n" +
                        $"InnerException: {ex.InnerException?.Message}\n" +
                        $"InnerStackTrace:\n{ex.InnerException?.StackTrace}");
                }
                catch { }
                
                // ユーザーにエラーを表示
                MessageBox.Show(
                    $"アプリケーションの起動中にエラーが発生しました。\n\n" +
                    $"エラー: {ex.Message}\n\n" +
                    $"詳細はログファイルを確認してください:\n{logPath}",
                    "KoFolderSync - エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}