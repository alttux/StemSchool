using System.Diagnostics;
using System.Windows;
using System.Windows.Shapes;
using Microsoft.Win32;
using System;
using System.IO;

namespace StemSchool;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private string globalProxyAddr;
    private int globalProxyPort;
    const string msiName = "cert_install_v2.msi";

    public MainWindow()
    {
        InitializeComponent();
    }

    // Заглушки для кнопок
    private void SetProxy(object sender, RoutedEventArgs e)
    {
        globalProxyAddr = ProxyTextBox.Text;
        globalProxyPort = Convert.ToInt32(PortTextBox.Text);
        ConfProxy.SetProxy("10.0.50.52", 3128, 1);
        MessageBox.Show("Прокси настроен");
    }

    private void Cert_Click(object sender, RoutedEventArgs e)
    {
        ConfProxy.RunMsiInstaller(msiName);
    }
    private void All_Click(object sender, RoutedEventArgs e)
    {
        globalProxyAddr = ProxyTextBox.Text;
        globalProxyPort = Convert.ToInt32(PortTextBox.Text);
        ConfProxy.SetProxy("10.0.50.52", 3128, 1);
        ConfProxy.RunMsiInstaller(msiName);
        MessageBox.Show("ESPD настроен");
    }
    private void GitHub_Click(object sender, RoutedEventArgs e)
    {
        Process.Start(new ProcessStartInfo("https://github.com/CodeCraftsman89")
        {
            UseShellExecute = true
        });
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
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
            MessageBox.Show("Прокси настроен");
        }
        catch (Exception ex)
        {
             MessageBox.Show("Ошибка настройки прокси: " + ex.Message);
        }
    }

    public static void RunMsiInstaller(string msiName)
    {
        string msiPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, msiName);

        if (File.Exists(msiPath))
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "msiexec.exe",
                Arguments = $"/i \"{msiPath}\" /qb", // /qn - тихая установка без окон
                UseShellExecute = false
            });

            Console.WriteLine($"Запущена установка: {msiPath}");
        }
        else
        {
            Console.WriteLine($"Файл не найден: {msiPath}");
        }
    }
}