using System;
using System.Windows;
using System.Windows.Threading;
using Promig.Utils;
using Promig.View;

namespace Promig {
    
    public partial class SplashScreen : Window {

        private const int TEMP = 8000;

        public SplashScreen() {
            InitializeComponent();
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
                // Criando arquivo de preferências
                if (!CompanyData.PreferencesExists())
                    CompanyData.CreatePreferences();

                // Abrindo janela de login
                Login janela = new Login();
                janela.Show();
                this.Hide();
                Close();
            }
            catch (Exception a) {
                MessageBox.Show("Erro", 
                                a.Message,
                                MessageBoxButton.OK,
                                MessageBoxImage.Error
                );
            }
        }

        private void Window_Activated(object sender, EventArgs e) {
            carregarprogressBar();
        }
    }
}
