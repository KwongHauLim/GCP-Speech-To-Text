using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Speech_To_Text.View.Manual
{
    /// <summary>
    /// ManualUI.xaml 的互動邏輯
    /// </summary>
    public partial class ManualUI : Window
    {
        public bool IsPressed { get; set; }

        public ManualUI()
        {
            InitializeComponent();
        }

        private void uiMove_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void uiMic_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                IsPressed = true;
            }
        }

        private void uiMic_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released && IsPressed)
            {
                IsPressed = false;

                //Task.Run(() =>
                //{
                //    Thread.Sleep(2000);
                //    var input = new WindowsInput.InputSimulator();
                //    input.Keyboard.TextEntry("Hello");
                //});
            }
        }
    }
}
