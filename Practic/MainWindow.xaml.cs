using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WordLinkOrder;
using WordLinkOrderChecker;

namespace Practic
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private System.Windows.Forms.OpenFileDialog openFileDialog;
        private RegistryKey registryKey;
        System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
        public MainWindow()
        {
            InitializeComponent();

            //чтобы сохраняло предудущий выбор в регистре
            registryKey = Registry.CurrentUser.OpenSubKey("Software", true).CreateSubKey("WordSearch", true);
            this.txtPath.Text = registryKey.GetValue("Path", "") as string;
            System.Windows.Forms.MessageBox.Show("MainWindow запущен");
        }

        void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Word files(*.doc;*.docx)|*.doc;*.docx|All files(*.*)|*.*";
            if (this.openFileDialog == default)//Чтобы менялся выбраный файл
                this.openFileDialog = new System.Windows.Forms.OpenFileDialog();

            if (this.openFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            this.txtPath.Text = this.openFileDialog.FileName;
        }

        void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string path = txtPath.Text;

            if (String.IsNullOrEmpty(path))
            {
                System.Windows.MessageBox.Show("No search path was provided. Please enter or select a path to search in.", "Missing search path", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            registryKey.SetValue("Path", path);//Register rewriting

            //passing to path function
            Check check = new Check();
            this.Hide(); // Hide current MainWindow
            check.Checker(path);//После проверки результат возвращается сюда и здесь программа продолжается

            System.Windows.Forms.MessageBox.Show("Программа обработала результаты поиска");
        }
    }
}
