using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;

namespace KinectCamera
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try {
                InitializeComponent();

                // Kinectの接続確認
                if (KinectSensor.KinectSensors.Count == 0) {
                    throw new Exception("Kinectが接続されていません");
                }

                // Kinectの動作を開始
                StartKinect(KinectSensor.KinectSensors[0]);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                Close();
            }
        }


        /**
         * Kinectの動作を開始する 
         * 
         * @param KinectSensor.KinectSensors[0]
         * 
         */
        private void StartKinect(KinectSensor kinect)
        {
            // カメラを有効にする
            kinect.ColorStream.Enable();
            // カメラフレームイベント
            kinect.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(kinect_ColorFrameReady);

            // Kinectの動作開始
            kinect.Start();
        }

        /*
         * カメラのフレーム更新イベント
         * 
         */
        void kinect_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            try
            {
                using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
                {
                    if (colorFrame != null)
                    {
                        // カメラのピクセルデータ取得
                        byte[] colorPixcel = new byte[colorFrame.PixelDataLength];
                        colorFrame.CopyPixelDataTo(colorPixcel);

                        // ピクセルデータをビットマップに変換
                        cameraImg.Source = BitmapSource.Create(colorFrame.Width, colorFrame.Height, 96, 96, PixelFormats.Bgr32, null, colorPixcel, colorFrame.Width * colorFrame.BytesPerPixel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /**
         * Kinectの動作を停止する 
         * 
         * @param KinectSensor.KinectSensors[0]
         * 
         */
        private void StoptKinect(KinectSensor kinect)
        {
            if (kinect == null)
            {
                if (kinect.IsRunning)
                {
                    // フレーム更新イベントの削除
                    kinect.ColorFrameReady -= kinect_ColorFrameReady;

                    // Kinectの動作を停止
                    kinect.Stop();
                    // ネイティブソースの解放
                    kinect.Dispose();

                    cameraImg.Source = null;
                }
            }       
        }


        /*
         * 終了時のイベント
         * 
         */
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs ce)
        {
            StoptKinect(KinectSensor.KinectSensors[0]);
        }
    }
}
