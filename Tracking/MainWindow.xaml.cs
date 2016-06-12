using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace Tracking
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        //private void button_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    Label btn = (Label)sender;
        //    btn.Background = new ImageBrush(new BitmapImage(new Uri("e:\\ico.png")));
        //    //btn.Foreground = new ImageBrush(new BitmapImage(new Uri("e:\\ico.png")));
        //}

        //private void button_MouseLeave(object sender, MouseEventArgs e)
        //{

        //    Label btn = (Label)sender;
        //    //btn.Foreground = new SolidColorBrush(Colors.LightBlue);
        //    btn.Background = new ImageBrush(new BitmapImage(new Uri("e:\\ico.png")));
        //    //btn.Background = new SolidColorBrush(Colors.White);
        //}

        //private void button_company_Click(object sender, MouseButtonEventArgs e)
        //{
        //    window_company win_com = new window_company(this);
        //    win_com.Show();
        //    this.Hide();
        //}

        //private void button_person_Click(object sender, MouseButtonEventArgs e)
        //{
        //    Label btn = (Label)sender;
        //    btn.Background = new ImageBrush(new BitmapImage(new Uri("e:\\ico.png")));
        //}

        private void button_company_MouseDwon(object sender, MouseButtonEventArgs e)
        {
            IntPtr bitmap = global::Tracking.Properties.Resources.company_click.GetHbitmap();
            ImageSource source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            button_company.Background = new ImageBrush(source);
            
        }

        private void button_company_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //button_company.Background = new ImageBrush(new BitmapImage(
            //    new Uri("res\\button\\company-hover.png", System.UriKind.Relative)));
            IntPtr bitmap = global::Tracking.Properties.Resources.company_hover.GetHbitmap();
            ImageSource source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            button_company.Background = new ImageBrush(source);
            window_company win_com = new window_company(this);
            win_com.Show();
            this.Hide();
        }

        private void button_person_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //button_person.Background = new ImageBrush(new BitmapImage(
            //    new Uri("res\\button\\person-click.png", System.UriKind.Relative)));
            IntPtr bitmap = global::Tracking.Properties.Resources.person_click.GetHbitmap();
            ImageSource source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            button_person.Background = new ImageBrush(source);
        }

        private void button_person_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //button_person.Background = new ImageBrush(new BitmapImage(
            //    new Uri("res\\button\\person-hover.png", System.UriKind.Relative)));
            IntPtr bitmap = global::Tracking.Properties.Resources.person_hover.GetHbitmap();
            ImageSource source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            button_person.Background = new ImageBrush(source);
            Window_person win_per = new Window_person(this);
            win_per.Show();
            this.Hide();
        }

    }
}