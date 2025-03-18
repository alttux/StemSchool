using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace StemSchool;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    string proxyAddr = ProxyTextBox.Text;
    public MainWindow()
    {
        InitializeComponent();
    }

    // Заглушки для кнопок
    private void SetProxy(object sender, RoutedEventArgs e)
    {
        ConfProxy.SetProxy("10.0.50.52", 3128, 1);
        MessageBox.Show("Нажата Кнопка 1");
    }

    private void Button2_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Нажата Кнопка 2");
    }

    private void Button3_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Нажата Кнопка 3");
    }

    private void Button4_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Нажата Кнопка 4");
    }


}

public class ConfProxy
{
    public static void SetProxy(string proxyAddress, int port, int proxyEnable)
    {
        const string registryKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings";
        try
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registryKey, true))
            {
                if (key != null)
                {
                    key.SetValue("ProxyEnable", proxyEnable);
                    key.SetValue("ProxyServer", $"{proxyAddress}:{port}");
                }
            }
            Console.WriteLine("Proxy settings updated successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error updating proxy settings: " + ex.Message);
        }
    }
}