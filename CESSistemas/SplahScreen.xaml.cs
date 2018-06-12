using System;
using System.Windows;
using System.Windows.Threading;

namespace CESSistemas
{
    /// <summary>
    /// Lógica interna para SplahScreen.xaml
    /// </summary>
    public partial class SplahScreen : Window
    {

        private const int TEMP = 1000;
        public SplahScreen()
        {
            InitializeComponent();
            //carregarprogressBar();
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
            int cont = 0;
            while (cont < 5) {

                criarConstrucao();
                cont++;
            }
            try {
                if (cont >= 5) {
                    Login janela = new Login();
                    janela.Show();
                    this.Hide();
                    Close();
                }
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
