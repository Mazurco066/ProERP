using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Excel = Microsoft.Office.Interop.Excel;

namespace Promig.Utils
{
   public class Util {

     // exportar para o exel uma datagrid
     public static void exportarExcel(DataGrid dg){
            Excel.Application excel = new Excel.Application();
            excel.Visible = true; 
            Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Worksheet sheet1 = (Worksheet)workbook.Sheets[1];

            for (int j = 0; j < dg.Columns.Count; j++) 
            {
                Range myRange = (Range)sheet1.Cells[1, j + 1];
                sheet1.Cells[1, j + 1].Font.Bold = true; 
                sheet1.Columns[j + 1].ColumnWidth = 15; 
                myRange.Value2 = dg.Columns[j].Header;
            }
            for (int i = 0; i < dg.Columns.Count; i++) { 
                for (int j = 0; j < dg.Items.Count; j++) {
                    TextBlock b = dg.Columns[i].GetCellContent(dg.Items[j]) as TextBlock;
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 2, i + 1];
                    myRange.Value2 = b.Text;
                }
            }
     }

        // estados
        public static List<string> ListaEstado() {
            List<string> listaEstado = new List<string>();
            string acre = "AC";
            listaEstado.Add(acre);
            string alagoas = "AL";
            listaEstado.Add(alagoas);
            string amapa = "AP";
            listaEstado.Add(amapa);
            string amazonas = "AM";
            listaEstado.Add(amazonas);
            string bahia = "BA";
            listaEstado.Add(bahia);
            string ceara = "CE";
            listaEstado.Add(ceara);
            string df = "DF";
            listaEstado.Add(df);
            string es = "ES";
            listaEstado.Add(es);
            string goias = "GO";
            listaEstado.Add(goias);
            string maranhao = "MA";
            listaEstado.Add(maranhao);
            string mg = "MT";
            listaEstado.Add(mg);
            string mgs = "MS";
            listaEstado.Add(mgs);
            string minas = "MG";
            listaEstado.Add(minas);
            string para = "PA";
            listaEstado.Add(para);
            string paraiba = "PB";
            listaEstado.Add(paraiba);
            string parana = "PR";
            listaEstado.Add(parana);
            string pernanbuco = "PE";
            listaEstado.Add(pernanbuco);
            string piaui = "PI";
            listaEstado.Add(piaui);
            string rio = "RJ";
            listaEstado.Add(rio);
            string rgn = "RN";
            listaEstado.Add(rgn);
            string rgs = "RS";
            listaEstado.Add(rgs);
            string rondonia = "RO";
            listaEstado.Add(rondonia);
            string roraima = "RR";
            listaEstado.Add(roraima);
            string sc = "SC";
            listaEstado.Add(sc);
            string spaulo = "SP";
            listaEstado.Add(spaulo);
            string sergipe = "SE";
            listaEstado.Add(sergipe);
            string tocantins = "TO";
            listaEstado.Add(tocantins);

            return listaEstado;
        }
    }
}
