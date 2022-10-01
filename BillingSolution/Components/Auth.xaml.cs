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
        internal User user = null;
        private static Random random = new Random();
        private string secureCode = "";

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
                // Check phone number and password
                user = db.User.FirstOrDefault(
                    p => p.Phone == phone.Text && p.Passoword == password.Password
                );

                if (user == null)
                {
                    ShowError("Неправильный телефон или пароль");
                    return;
                }

                // Send code
                code_text.Visibility = Visibility.Visible;
                code_reload.Visibility = Visibility.Visible;
                code.Visibility = Visibility.Visible;
                auth.Content = "Вход";
                GenerateNewCode();
            } else
            {
                if (code.Text != secureCode)
                {
                    ShowError("Неправильный код");
                    return;
                }
                Auth();
            }
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

        /// <summary>
        /// Handle code reload click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCodeReloadClick(object sender, MouseButtonEventArgs e)
        {
            GenerateNewCode();
        }

        /// <summary>
        /// Вход в аккаунт
        /// </summary>
        private void Auth()
        {
            var role = db.Role.Find(user.Role);
            MessageBox.Show(role.Title, "Role", MessageBoxButton.OK, MessageBoxImage.None);
        }

        /// <summary>
        /// Regenerates new code
        /// </summary>
        private void GenerateNewCode()
        {
            secureCode = new string(
                Enumerable.Repeat("1234567890", 6)
                .Select(s => s[random.Next(s.Length)]).ToArray()
            );
            MessageBox.Show(secureCode, "Код", MessageBoxButton.OK, MessageBoxImage.Information);
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

    }
}
