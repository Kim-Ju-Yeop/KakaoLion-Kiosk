using KakaoLion.csv.repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace KakaoLion.csv.repositoryImpl
{
    class StatsRepositoryImpl1 : StatsRepository1
    {
        public void exportStats(List<string> statsList)
        {
            string fileName;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "메뉴별 판매 수 및 총액";
            saveFileDialog.Filter = "Excel File (*.csv)|*.csv";

            if (DialogResult.Cancel == saveFileDialog.ShowDialog())
            {
                return;
            } 
            else if (saveFileDialog.FileName == "")
            {
                return;
            }
            else
            {
                fileName = saveFileDialog.FileName;
            }

            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                StreamWriter sw = new StreamWriter(fileName, false, Encoding.Unicode);

                string columnHeader = "메뉴\t수량\t할인율\t총 매출액\t순수 매출액";
                sw.WriteLine(columnHeader);

                foreach (string stats in statsList)
                {
                    sw.WriteLine(stats);
                }

                sw.Close();
                MessageBox.Show("파일 추출을 성공했습니다.");
            }
            catch (Exception e)
            {
                MessageBox.Show("파일 추출을 실패했습니다.");
            }
        }
    }
}
