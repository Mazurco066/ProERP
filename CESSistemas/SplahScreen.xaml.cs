using System;
using System.Windows;
using System.Windows.Threading;
using Promig.Utils;

namespace Promig
{
    /// <summary>
    /// Lógica interna para SplahScreen.xaml
    /// </summary>
    public partial class SplahScreen : Window
    {

        private const int TEMP = 8000;
        public SplahScreen()
        {
            InitializeComponent();
            //carregarprogressBar();
            DataBaseCommand.createDataBaseEF();
            DataBaseCommand.createUserPassword();
        }

        private delegate void ProgressBarDelegate();

        private void criarConstrucao() {
            PB.IsIndeterminate = false;
            PB.Maximum = TEMP;
            PB.Value = 0;

            for (int i = 0; i < TEMP; i++) {
                PB.Dispatcher.Invoke(new ProgressBarDelegate(UpdateProgress), DispatcherPriority.Background);
            }
        }
        private void UpdateProgress() {
            PB.Value += 1;
        }


        private void carregarprogressBar() {
                criarConstrucao();
            try {
                    Login janela = new Login();
                    janela.Show();
                    this.Hide();
                    Close();
            }
            catch (Exception a) {
                MessageBox.Show(a.Message);
            }
        }

        private void Window_Activated(object sender, EventArgs e) {
            carregarprogressBar();
        }
    }
}
