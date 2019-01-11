using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Runtime.CompilerServices;
using System.Globalization;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using CommonFunction;
using System.IO;

namespace FSTCut
{
    /// <summary>
    /// FSTDef.xaml 的交互逻辑
    /// </summary>
    public partial class FSTDef : Window
    {
        //工作坐标系的选项
        public static List<string> workAxiaStrArr = new List<string>()
        {
            "G54",
            "G55",
            "G56",
            "G57",
            "G58",
            "G59"
        };
        //刀补号
        public static List<string> cutCpst = new List<string>()
        {
            "T0101",
            "T0202",
            "T0303",
            "T0404",
            "T0505",
            "T0606",
            "T0707",
            "T0808",
            "T0909"
        };
        //主轴旋转方向
        public static List<string> mainAxiaDir = new List<string>()
        {
            "顺时针",//M03
            "逆时针"//M04
        };
        //喷气号
        public static List<string> jetNo = new List<string>()
        {
            "M26",
            "M27"
        };
        public fnlSegments fnlSeg = new fnlSegments();
        public FSTDef()
        {
            InitializeComponent();
            fnlSeg.NCFilePath = "Untitled.NC";
            c_WorkAxis.ItemsSource = workAxiaStrArr;
            c_WorkAxis.SelectedIndex = 0;
            c_CutCmp.ItemsSource = cutCpst;
            c_CutCmp.SelectedIndex = 0;
            c_MainCutDir.ItemsSource = mainAxiaDir;
            c_MainCutDir.SelectedIndex = 0;
            c_JetNo.ItemsSource = jetNo;
            c_JetNo.SelectedIndex = 0;
            //绑定数据
            //工作坐标系
            Binding bd_WorkAxis = new Binding("WorkCoord");
            bd_WorkAxis.Source = fnlSeg;
            bd_WorkAxis.Converter = FindResource("SPC") as IValueConverter;
            c_WorkAxis.SetBinding(ComboBox.SelectedIndexProperty, bd_WorkAxis);

            //刀补号
            Binding bd_CutCmp = new Binding("CutCpst");
            bd_CutCmp.Source = fnlSeg;
            bd_CutCmp.Converter = FindResource("CCC") as IValueConverter;
            c_CutCmp.SetBinding(ComboBox.SelectedIndexProperty, bd_CutCmp);

            //主轴旋转方向
            Binding bd_MainCutDir = new Binding("SpdDir");
            bd_MainCutDir.Source = fnlSeg;
            bd_MainCutDir.Converter = FindResource("SPDC") as IValueConverter;
            c_MainCutDir.SetBinding(ComboBox.SelectedIndexProperty, bd_MainCutDir);

            //喷气号
            Binding bd_JetNo = new Binding("JetNo");
            bd_JetNo.Source = fnlSeg;
            bd_JetNo.Converter = FindResource("JNC") as IValueConverter;
            c_JetNo.SetBinding(ComboBox.SelectedIndexProperty, bd_JetNo);

            //工件尺寸
            Binding bd_PartSize = new Binding("PartSize");
            bd_PartSize.Source = fnlSeg;
            c_PartSize.SetBinding(TextBox.TextProperty, bd_PartSize);

            //加工循环次数
            Binding bd_TotalLoops = new Binding("TotalLoops");
            bd_TotalLoops.Source = fnlSeg;
            c_TotalLoops.SetBinding(TextBox.TextProperty, bd_TotalLoops);

            //主轴转速
            Binding bd_SpindleSpeed = new Binding("SpindleSpd");
            bd_SpindleSpeed.Source = fnlSeg;
            c_SpindleSpeed.SetBinding(TextBox.TextProperty, bd_SpindleSpeed);

            //进给速度
            Binding bd_FeedRate = new Binding("FeedRate");
            bd_FeedRate.Source = fnlSeg;
            c_FeedRate.SetBinding(TextBox.TextProperty, bd_FeedRate);

            //切深
            Binding bd_DepthOfCut = new Binding("CutDepth");
            bd_DepthOfCut.Source = fnlSeg;
            c_DepthOfCut.SetBinding(TextBox.TextProperty, bd_DepthOfCut);

            //LeadInX
            Binding bd_LeadInX = new Binding("LeadInX");
            bd_LeadInX.Source = fnlSeg;
            c_LeadInX.SetBinding(TextBox.TextProperty, bd_LeadInX);

            //LeadInZ
            Binding bd_LeadInZ = new Binding("LeadInZ");
            bd_LeadInZ.Source = fnlSeg;
            c_LeadInZ.SetBinding(TextBox.TextProperty, bd_LeadInZ);

            //安全出去距离
            Binding bd_LeadOutZ = new Binding("LeadOutZ");
            bd_LeadOutZ.Source = fnlSeg;
            c_LeadOutZ.SetBinding(TextBox.TextProperty, bd_LeadOutZ);

            //等等Z
            Binding bd_HZ = new Binding("HZ");
            bd_HZ.Source = fnlSeg;
            c_HZ.SetBinding(TextBox.TextProperty, bd_HZ);

            //文件名称
            Binding bd_NCFilePath = new Binding("NCFilePath");
            bd_NCFilePath.Source = fnlSeg;//UntitledFtsFC.NC
            fnlSeg.NCFilePath = "Untitled.NC";
            c_NCFilePath.SetBinding(TextBox.TextProperty, bd_NCFilePath);

            //文件保存位置
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string fileName = "\\" + (fnlSeg.NCFilePath).Replace("led", "led-fnl");
            c_OFilePath.Text = dir + fileName;

            ////初始化时间
            //if (hsSeg.FeedRate > 0 && sSeg.FeedRate > 0)
            //{
            //    hsTime = TimeSpan.FromMinutes(Math.Abs(hsSeg.CutXStart - hsSeg.CutXEnd) / hsSeg.FeedRate * hsSeg.TotalLoops);
            //    c_hsTime.Text = "用时 " + hsTime.ToString(@"hh\:mm\:ss");

            //    sTime = TimeSpan.FromMinutes(Math.Abs(sSeg.CutXStart - sSeg.CutXEnd) / sSeg.FeedRate * sSeg.TotalLoops);
            //    c_sTime.Text = "用时 " + sTime.ToString(@"hh\:mm\:ss");

            //    c_AllTime.Text = "总用时 " + (hsTime + sTime).ToString(@"hh\:mm\:ss");
            //}

            ////初始化粗糙度
            //if (hsSeg.CutR >= 0 && hsSeg.SpdSpeed >= 0 && sSeg.CutR >=0 && sSeg.SpdSpeed >=0)
            //{
            //    c_hsRa.Text = (hsSeg.FeedRate * hsSeg.FeedRate * 1000000 / hsSeg.SpdSpeed / hsSeg.SpdSpeed * 0.032 / hsSeg.CutR).ToString("F8");
            //    c_Ra.Text = (sSeg.FeedRate * sSeg.FeedRate * 1000000 / sSeg.SpdSpeed / sSeg.SpdSpeed * 0.032 / sSeg.CutR).ToString("F8");
            //}
        }

        private void Button_Click_Test(object sender, RoutedEventArgs e)
        {
            DateTime dt = Convert.ToDateTime("2016/10/1");
            DateTime dt1 = dt.AddMonths(1);
            TimeSpan dtsp = dt1 - dt;
            int days = dtsp.Days;
            Trace.WriteLine("1");
        }

        private void Button_Click_GenCmp(object sender, RoutedEventArgs e)
        {
            MainWindow dlg = new MainWindow();
            dlg.ShowDialog();
            Trace.WriteLine("1");
        }

        private void c_NCFilePath_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Text.EndsWith("-FNL.NC"))
            {
                return;
            }
            else
            {
                tb.Text += "-FNL.NC";
                int lastTag = c_OFilePath.Text.LastIndexOf("\\");
                c_OFilePath.Text = c_OFilePath.Text.Substring(0, lastTag) + "\\" + tb.Text;
            }
        }


        private void c_NCFilePath_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            tb.Text = "";
        }

        private void c_JetNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            c_SpindleSpeed.Focus();
        }

        private void c_NCFilePath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                c_Gen.Focus();
                //c_NCFilePath_LostFocus(sender, e);
            }
        }

        private void c_SpindleSpeed_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //MessageBox.Show("按了回车键");
                c_FeedRate.Focus();
            }
        }

        private void c_FeedRate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //MessageBox.Show("按了回车键");
                c_SpindleSpeed.Focus();
            }
        }
        private void c_PartSize_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //MessageBox.Show("按了回车键");
                c_TotalLoops.Focus();
            }
        }


        private void c_LeadInX_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //MessageBox.Show("按了回车键");
                c_LeadInZ.Focus();
            }
        }

        private void c_HZ_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //MessageBox.Show("按了回车键");
                c_NCFilePath.Focus();
            }
        }

        private void c_LeadInZ_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //MessageBox.Show("按了回车键");
                c_LeadOutZ.Focus();
            }
        }

        private void c_LeadOutZ_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //MessageBox.Show("按了回车键");
                c_HZ.Focus();
            }
        }

        private void c_TotalLoops_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //MessageBox.Show("按了回车键");
                c_DepthOfCut.Focus();
            }
        }

        private void c_DepthOfCut_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //MessageBox.Show("按了回车键");
                c_FeedRate.Focus();
            }
        }

        private void c_Browse_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fd = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = fd.ShowDialog();  
            if (result == System.Windows.Forms.DialogResult.Cancel)  
            {  
                return;  
            }
            string m_Dir = fd.SelectedPath.Trim();
            this.c_OFilePath.Text = m_Dir + "\\" + fnlSeg.NCFilePath;  

        }

        private void c_Gen_Click(object sender, RoutedEventArgs e)
        {
            //string mainTxt = "", SFNC = "";
            //int PathTag = c_OFilePath.Text.LastIndexOf("\\");
            string NumData = "", MainTxt = "", filePath = c_OFilePath.Text;
            //得到数据字符串
            if (File.Exists(c_DataFile.Text) == false)
            {
                MessageBox.Show("数据文件不存在，请重新导入");
                return;
            }
            StreamReader sw = new StreamReader(c_DataFile.Text, Encoding.Default);
            NumData = sw.ReadToEnd();
            sw.Close();
            if (NumData.EndsWith("\n") == false)
            {
                NumData += "\r\n";
            }
            if (CommonFunction.DiaCut.MakeFNLNcFile(fnlSeg, filePath, NumData, ref MainTxt))
            {
                MessageBox.Show("生成NC文件至" + filePath);
            }
        }

        private void c_DataBrowse_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog1 = new Microsoft.Win32.OpenFileDialog();
            string fileFilter = "Txt Files|*.txt|nc Files|*.nc|All Files|*.*";
            openFileDialog1.Filter = fileFilter;
            //openFileDialog1.Filter = "Txt Files|*.txt|nc Files|*.nc|All Files|*.*";
            openFileDialog1.Title = "选择数据文件";
            if (openFileDialog1.ShowDialog() == true)
            {
                c_DataFile.Text = openFileDialog1.FileName;
                MessageBox.Show("成功导入数据");
            }
            else
            {
                MessageBox.Show("未导入数据");
                return;
            }
        }
    }

    public class WorkCoordsToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string getV = (string)value;
            int sIdx = 0;
            switch (getV)
            {
                case "G54":
                    sIdx = 0;
                    break;
                case "G55":
                    sIdx = 1;
                    break;
                case "G56":
                    sIdx = 2;
                    break;
                case "G57":
                    sIdx = 3;
                    break;
                case "G58":
                    sIdx = 4;
                    break;
                case "G59":
                    sIdx = 5;
                    break;
            }
            return sIdx;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //throw new NotImplementedException();
            int getV = (int)value;
            string ItemStr = "G54";
            switch (getV)
            {
                case 0:
                    ItemStr = "G54";
                    break;
                case 1:
                    ItemStr = "G55";
                    break;
                case 2:
                    ItemStr = "G56";
                    break;
                case 3:
                    ItemStr = "G57";
                    break;
                case 4:
                    ItemStr = "G58";
                    break;
                case 5:
                    ItemStr = "G59";
                    break;
            }
            return ItemStr;
        }
    }
    public class CutCpstToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string getV = (string)value;
            int sIdx = 0;
            switch (getV)
            {
                case "T0101":
                    sIdx = 0;
                    break;
                case "T0202":
                    sIdx = 1;
                    break;
                case "T0303":
                    sIdx = 2;
                    break;
                case "T0404":
                    sIdx = 3;
                    break;
                case "T0505":
                    sIdx = 4;
                    break;
                case "T0606":
                    sIdx = 5;
                    break;
                case "T0707":
                    sIdx = 6;
                    break;
                case "T0808":
                    sIdx = 7;
                    break;
                case "T0909":
                    sIdx = 8;
                    break;
            }
            return sIdx;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //throw new NotImplementedException();
            int getV = (int)value;
            string ItemStr = "T0101";
            switch (getV)
            {
                case 0:
                    ItemStr = "T0101";
                    break;
                case 1:
                    ItemStr = "T0202";
                    break;
                case 2:
                    ItemStr = "T0303";
                    break;
                case 3:
                    ItemStr = "T0404";
                    break;
                case 4:
                    ItemStr = "T0505";
                    break;
                case 5:
                    ItemStr = "T0606";
                    break;
                case 6:
                    ItemStr = "T0707";
                    break;
                case 7:
                    ItemStr = "T0808";
                    break;
                case 8:
                    ItemStr = "T0909";
                    break;
            }
            return ItemStr;
        }
    }
    public class SpdDirToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string getV = (string)value;
            int sIdx = 0;
            switch (getV)
            {
                case "顺时针":
                    sIdx = 0;
                    break;
                case "逆时针":
                    sIdx = 1;
                    break;
            }
            return sIdx;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //throw new NotImplementedException();
            int getV = (int)value;
            string ItemStr = "顺时针";
            switch (getV)
            {
                case 0:
                    ItemStr = "顺时针";
                    break;
                case 1:
                    ItemStr = "逆时针";
                    break;
            }
            return ItemStr;
        }
    }
    public class JetNoToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string getV = (string)value;
            int sIdx = 0;
            switch (getV)
            {
                case "M26":
                    sIdx = 0;
                    break;
                case "M27":
                    sIdx = 1;
                    break;
            }
            return sIdx;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //throw new NotImplementedException();
            int getV = (int)value;
            string ItemStr = "M26";
            switch (getV)
            {
                case 0:
                    ItemStr = "M26";
                    break;
                case 1:
                    ItemStr = "M27";
                    break;
            }
            return ItemStr;
        }
    }
}
