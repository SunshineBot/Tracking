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
            button_company.Background = new ImageBrush(new BitmapImage(
                new Uri("res\\button\\company-click.png", System.UriKind.Relative)));
        }

        private void button_company_MouseUp(object sender, MouseButtonEventArgs e)
        {
            button_company.Background = new ImageBrush(new BitmapImage(
                new Uri("res\\button\\company-hover.png", System.UriKind.Relative)));
            window_company win_com = new window_company(this);
            win_com.Show();
            this.Hide();
        }

        private void button_person_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //button_person.Background = new ImageBrush(new BitmapImage(
            //    new Uri("res\\button\\person-click.png", System.UriKind.Relative)));
        }

        private void button_person_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //button_person.Background = new ImageBrush(new BitmapImage(
            //    new Uri("res\\button\\person-hover.png", System.UriKind.Relative)));
            Window_person win_per = new Window_person(this);
            win_per.Show();
            this.Hide();
        }

    }
}