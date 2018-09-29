using iTextSharp.text;
using iTextSharp.text.pdf;
using Promig.Model;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Promig.Utils {
    class PdfModel {

        #region Consts

        private static string estimateDirectoryPath = "C:\\ProERP\\Generated-Documents\\Orçamentos\\";
        private static string mapsDirectoryPath = $"C:\\ProERP\\Generated-Documents\\PDF-Maps\\";

        #endregion

        #region Static-Methods

        /// <summary>
        /// Método estatico para exportar pdf de mapas
        /// </summary>
        public static string ExportMapPdf(string location, Microsoft.Maps.MapControl.WPF.Location loc) {

            //Criando um novo documento com margem 40
            Document document = new Document(PageSize.A4);
            document.SetMargins(40, 40, 40, 40);
            document.AddCreationDate();

            //Definindo o caminho que será salvo o arquivo PDF
            string oldPath = Directory.GetCurrentDirectory();
            if (!Directory.Exists(mapsDirectoryPath)) Directory.CreateDirectory(mapsDirectoryPath);
            Directory.SetCurrentDirectory(mapsDirectoryPath);

            //Configurando arquivo a ser salvo
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"{DateTime.Now.ToString("ddMMyyyyhhmmss")}roadmap.pdf");

            //Criando arquivo em branco para testes
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(path, FileMode.Create));
            writer.CompressionLevel = PdfStream.NO_COMPRESSION;

            //Editando o documento
            document.Open();

            //Recuperando imagem do mapa
            string mapURl = $"https://dev.virtualearth.net/REST/v1/Imagery/Map/Road/{loc.Latitude.ToString().Replace(',', '.')},{loc.Longitude.ToString().Replace(',', '.')}/15?mapSize=700,700&pp={loc.Latitude.ToString().Replace(',', '.')},{loc.Longitude.ToString().Replace(',', '.')};21;Destino&key=AsHgFB0MOC02SgIYNbIwV9WOuo94eLp3brN5PvlD9Vu-p9DSjVUYfUZZIS5jfOeb";
            BitmapImage src = new BitmapImage();
            Uri uri = new Uri(mapURl);
            src.BeginInit();
            src.UriSource = uri;
            src.EndInit();

            var width = Convert.ToInt32(Math.Round(document.PageSize.Width - 40));
            var height = Convert.ToInt32(Math.Round(((float)src.Height / (float)src.Width) * (width)));

            //Adicionando conteudo
            Paragraph title = new Paragraph("Visualização em Mapa", new Font(Font.NORMAL, 20));
            title.Alignment = Element.ALIGN_CENTER;
            Paragraph adress = new Paragraph($"\n{location}\n\n", new Font(Font.NORMAL, 14));
            adress.Alignment = Element.ALIGN_CENTER;
            Paragraph footer = new Paragraph($"\n\n\nEmitido dia {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")} - ProERP", new Font(Font.NORMAL, 12));
            footer.Alignment = Element.ALIGN_CENTER;

            Image image = Image.GetInstance(uri);
            image.Alignment = Element.ALIGN_CENTER;
            image.SetDpi(600, 600);
            image.ScaleToFit(width, height);

            document.Add(title);
            document.Add(adress);
            document.Add(image);
            document.Add(footer);

            //Finalizando Edições
            document.Close();
            System.Diagnostics.Process.Start(path);

            //Sinalizando usuario sobre criação do arquivo e restaurando diretorio
            Directory.SetCurrentDirectory(oldPath);
            return $"Arquivo PDF do mapa salvo no diretorio {path}";
        }

        /// <summary>
        /// Método estatico para exportar pdf de orçamento
        /// </summary>
        /// <param name="estimate"></param>
        public static void ExportEstimatePdf(Estimate estimate) {

            // Definição das margens do documento
            Document doc = new Document(PageSize.A4);
            doc.SetMargins(40, 40, 40, 80);
            doc.AddCreationDate();

            // Criação do diiretório se não existir
            if (!Directory.Exists(estimateDirectoryPath)) Directory.CreateDirectory(estimateDirectoryPath);
            string oldPath = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(estimateDirectoryPath);

            //Configurando arquivo a ser salvo
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"Orçamento-{DateTime.Now.ToString("ddMMyyyyhhmmss")}.pdf");

            // Definição de escrita do documento
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
            writer.CompressionLevel = PdfStream.NO_COMPRESSION;

            // Adicionando paragráfos
            var img = iTextSharp.text.Image.GetInstance(estimate.ImgPath);
            doc.Open();
            doc.Add(img);
            Paragraph p1 = new Paragraph("PROPOSTA COMERCIAL", new Font(Font.NORMAL, 14, (int)System.Drawing.FontStyle.Bold));
            doc.Add(p1);
            Paragraph p2 = new Paragraph($"Cliente: {estimate.NameCustomer}", new Font(Font.NORMAL, 12));
            doc.Add(p2);
            Paragraph p3 = new Paragraph($"Data: {estimate.Date}", new Font(Font.NORMAL, 12));
            doc.Add(p3);
            Paragraph p4 = new Paragraph("\n");
            doc.Add(p4);
            Paragraph p5 = new Paragraph(estimate.Description, new Font(Font.NORMAL, 12));
            doc.Add(p5);
            Paragraph p6 = new Paragraph("\n");
            doc.Add(p6);
            Paragraph p7 = new Paragraph("Inclusos encargos com mão de obra, material e equipamentos para execução dos serviços," +
            "encargos trabalhistas e impostos municipais, estaduais e federais." +
            "Obs: Itens não relacionados nesse documento orçamentário e escopo dos serviços e materiais " +
            "serão faturados como aditivos.", new Font(Font.NORMAL, 12));
            doc.Add(p7);
            Paragraph p8 = new Paragraph("\n");
            doc.Add(p8);
            Paragraph p9 = new Paragraph($"Número do Documento: {estimate.DocNo}", new Font(Font.NORMAL, 12));
            doc.Add(p9);
            Paragraph p10 = new Paragraph("\n");
            doc.Add(p10);
            Paragraph p11 = new Paragraph($"Condição de Pagamento: {estimate.PayCondition}", new Font(Font.NORMAL, 12));
            doc.Add(p11);
            Paragraph p12 = new Paragraph($"Execução em até: {estimate.DaysExecution}", new Font(Font.NORMAL, 12));
            doc.Add(p12);
            Paragraph p13 = new Paragraph($"Valor Total dos serviços R${estimate.TotalValue}", new Font(Font.NORMAL, 12));
            doc.Add(p13);
            Paragraph p14 = new Paragraph("\n");
            doc.Add(p14);
            Paragraph p15 = new Paragraph("DESCRIÇÃO DOS SERVIÇOS", new Font(Font.NORMAL, 14, (int)System.Drawing.FontStyle.Bold));
            doc.Add(p15);
            Paragraph p16 = new Paragraph("\n");
            doc.Add(p16);
            int cont = 1;
            foreach (ItemEstimate item in estimate.Items) {
                Paragraph p17 = new Paragraph($"Descrição {cont}: {item.Service.Task} - SUBTOTAL: R${item.SubTotal}", new Font(Font.NORMAL, 10));
                doc.Add(p17);
                cont++;
            }
            Paragraph p18 = new Paragraph("\n");
            doc.Add(p18);
            Paragraph p19 = new Paragraph("\n");
            doc.Add(p19);
            Paragraph p20 = new Paragraph("\n");
            doc.Add(p20);
            Model.Json.CompanyModel data = CompanyData.GetPreferencesData();
            Paragraph p21 = new Paragraph($"Atenciosamente: {data.name}", new Font(Font.NORMAL, 9, (int)System.Drawing.FontStyle.Bold));
            doc.Add(p21);
            Paragraph p22 = new Paragraph(CompanyData.GetPdfFooterData(), new Font(Font.NORMAL, 8));
            doc.Add(p22);

            // Processando documento
            doc.Close();
            System.Diagnostics.Process.Start(path);
        }

        #endregion

        #region Pdf-Utils

        #endregion
    }
}
