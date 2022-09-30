using System;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using BillingSolution.Models;

namespace BillingSolution
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal EthosaBillingTestEntities db = new EthosaBillingTestEntities();
        private static Random random = new Random();
        private string c = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handle auth button click and
        /// sends code to phone or log in system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAuthClick(object sender, RoutedEventArgs e)
        {
            if (code_text.Visibility == Visibility.Collapsed)
            {
                // Send code
                code_text.Visibility = Visibility.Visible;
                code.Visibility = Visibility.Visible;
                auth.Content = "Вход";
                c = new string(
                    Enumerable.Repeat("1234567890", 6)
                    .Select(s => s[random.Next(s.Length)]).ToArray()
                );
                MessageBox.Show(c, "Код", MessageBoxButton.OK, MessageBoxImage.Information);
            } else
            {
                if (code.Text != c)
                {
                    ShowError("Неправильный код");
                    return;
                }
                // Check phone number and password
                User user = db.User.FirstOrDefault(
                    p => p.Phone == phone.Text && p.Passoword == password.Password
                );
                if (user != null)
                    Auth();
                else
                    ShowError("Неправильный телефон или пароль");
            }
        }

        private void Auth()
        {

        }

        /// <summary>
        /// Shortcut to >MessageBox.Show
        /// </summary>
        /// <param name="error"></param>
        private void ShowError(string error)
        {
            MessageBox.Show(
                error,
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Warning
            );
        }

        /// <summary>
        /// Handle text input and allow only digits.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCodePreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }
    }
}
