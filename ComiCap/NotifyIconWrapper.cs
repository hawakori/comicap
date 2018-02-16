using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;

using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing.Imaging;

namespace ComiCap {
    using System;
    using System.ComponentModel;
    using System.Windows;

    /// <summary>
    /// タスクトレイ通知アイコン
    /// </summary>
    public partial class NotifyIconWrapper : Component {
        MainWindow main_window = null;

        /// <summary>
        /// NotifyIconWrapper クラス を生成、初期化します。
        /// </summary>
        public NotifyIconWrapper() {
            // コンポーネントの初期化
            this.InitializeComponent();

            // コンテキストメニューのイベントを設定
            this.toolStripMenuItem_Open.Click += this.toolStripMenuItem_Open_Click;
            this.toolStripMenuItem_Exit.Click += this.toolStripMenuItem_Exit_Click;
            this.toolStripMenuItem_Capture.Click += this.toolStripMenuItem_Capture_Click;
        }

        /// <summary>
        /// コンテナ を指定して NotifyIconWrapper クラス を生成、初期化します。
        /// </summary>
        /// <param name="container">コンテナ</param>
        public NotifyIconWrapper(IContainer container) {
            container.Add(this);

            this.InitializeComponent();
        }

        /// <summary>
        /// コンテキストメニュー "表示" を選択したとき呼ばれます。
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void toolStripMenuItem_Open_Click(object sender, EventArgs e) {
            // MainWindow を生成、表示
            main_window = new MainWindow();
            main_window.Show();
        }

        /// <summary>
        /// コンテキストメニュー "終了" を選択したとき呼ばれます。
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void toolStripMenuItem_Exit_Click(object sender, EventArgs e) {
            // 現在のアプリケーションを終了
            Application.Current.Shutdown();
        }

        /// <summary>
        /// コンテキストメニュー "Capture" を選択したとき呼ばれます。
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void toolStripMenuItem_Capture_Click(object sender, EventArgs e) {
            //Bitmapの作成
            Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            //Graphicsの作成
            Graphics g = Graphics.FromImage(bmp);
            //画面全体をコピーする
            g.CopyFromScreen(new System.Drawing.Point(0, 0), new System.Drawing.Point(0, 0), bmp.Size);
            //解放
            g.Dispose();

            using (Stream stream = new MemoryStream()) {
                bmp.Save(stream, ImageFormat.Png);
                stream.Seek(0, SeekOrigin.Begin);
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.StreamSource = stream;
                img.EndInit();

                main_window.image.Source = img;
            }
        }
    }
}
