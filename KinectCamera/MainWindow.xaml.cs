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
        private void StartKinect(KinectSensor kinect) {
            // Kinectの動作開始
            kinect.Start();
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
                    // Kinectの動作を停止
                    kinect.Stop();
                    // ネイティブソースの解放
                    kinect.Dispose();
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
