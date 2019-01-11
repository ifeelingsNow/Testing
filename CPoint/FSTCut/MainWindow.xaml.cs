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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using CommonFunction;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Globalization;
using System.Threading;
using System.IO;
using Microsoft.Win32;
using System.Collections.ObjectModel;

namespace FSTCut
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private FSTSeg xMin = new FSTSeg("x最小值:", "-0.04000000","mm");
        private FSTSeg xMax = new FSTSeg("x最大值:", "5.04000000", "mm");
        private FSTSeg dltX = new FSTSeg("每圈deltaX:", "0.02000000", "mm");
        private FSTSeg cycNum = new FSTSeg("每圈点数:", "360", "个");
        private FSTSeg zMin = new FSTSeg("Z最小值:", "0", "mm");
        private FSTSeg zMax = new FSTSeg("Z最大值:", "0", "mm");
        private FSTSeg zOffset = new FSTSeg("Z偏置(仅凹结构需要):", "0", "mm");
        private FSTSeg zScale = new FSTSeg("Z缩放:", "1", "倍");
        private FSTSeg fSurggest = new FSTSeg("F建议最大值:", "1", "mm/min");
        private List<Vertex> zList = new List<Vertex>();
        private double dZMin = 0;
        private double dZMax = 0;
        private double dXMin = 0;
        private double dXMax = 0;
        private double dDltX = 0;
        private int iCycNum = 0;
        ObservableCollection<FSTSeg> setList = new ObservableCollection<FSTSeg>(); 
        //{
        //    new FSTSeg("x最小值:",xMin.ToString()),
        //    new FSTSeg("x最大值:","5.04000000"),
        //    new FSTSeg("每圈deltaX:","0.03000000"),
        //    new FSTSeg("每圈点数:","360")
        //};

        public MainWindow()
        {
            InitializeComponent();
            setList.Add(xMin);
            setList.Add(xMax);
            setList.Add(dltX);
            setList.Add(cycNum);
            setList.Add(zMin);
            setList.Add(zMax);
            setList.Add(zOffset);
            setList.Add(zScale);
            setList.Add(fSurggest);
            SegData.ItemsSource = setList;
        }
        private void Button_Click_Input(object sender, RoutedEventArgs e)
        {
            string fileExt = "";
            string fileP = filePath.Text;
            bool test = ComFunc.InputFile(ref fileP, ref fileExt);
            filePath.Text = fileP;
            zList.Clear();
            if (fileExt == "nc")
            {
                DiaCut.NcFilePhrase(fileP, zList);
                //计算最大值和最小值
                GetSegVlue();
                MessageBox.Show("成功导入");
            }

            Trace.WriteLine("1");
        }
        private void GetSegVlue()
        {
            int zListlen = zList.Count;
            dZMax = dZMin = zList[0].z;
            dXMax = dXMin = zList[0].y;
            for (int i = 1; i < zListlen; i++)
            {
                if (dZMax < zList[i].z)
                {
                    dZMax = zList[i].z;
                }
                if (dZMin > zList[i].z)
                {
                    dZMin = zList[i].z;
                }
                if (dXMax < zList[i].y)
                {
                    dXMax = zList[i].y;
                }
                if (dXMin > zList[i].y)
                {
                    dXMin = zList[i].y;
                }
            }
            //计算每圈的点数和每圈X变化
            double cFirst = zList[0].x;
            double xFirst = zList[0].y;
            for (int i = 1; i < zListlen; i++)
            {
                if (zList[i].x == cFirst)
                {
                    iCycNum = i;
                    dDltX = xFirst - zList[i].y;
                    break;
                }
            }
            double fMax = 4000.0/iCycNum * dDltX * 60;
            cycNum.SegValue = iCycNum.ToString();
            dltX.SegValue = dDltX.ToString("F8");
            zMax.SegValue = dZMax.ToString("F8");
            zMin.SegValue = dZMin.ToString("F8");
            xMax.SegValue = dXMax.ToString("F8");
            xMin.SegValue = dXMin.ToString("F8");
            fSurggest.SegValue = fMax.ToString("F8");
        }
        private List<double> GetTransZList()
        {
            List<double> transList = new List<double>();
            int zListlen = zList.Count;
            if (zListlen == 0)
            {
                return transList;
            }
            //偏置
            double dZOfs = Convert.ToDouble(zOffset.SegValue);
            foreach (Vertex zValue in zList)
            {
                transList.Add(zValue.z + dZOfs);
            }
            //缩放
            double dZScale = Convert.ToDouble(zScale.SegValue);
            if (dZScale != 0 && dZScale != 1)
            {
                for (int i = 0; i < zListlen; i++)
                {
                    transList[i] *= dZScale;
                }
            }
            return transList;
            //计算最大值和最小值
        }
        private void Button_Click_OutPutCmp(object sender, RoutedEventArgs e)
        {
            if (zList.Count == 0)
            {
                return;
            }
            dZMax += Convert.ToDouble(zOffset.SegValue);
            dZMin += Convert.ToDouble(zOffset.SegValue);
            zMax.SegValue = dZMax.ToString("F8");
            zMin.SegValue = dZMin.ToString("F8");

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "cmp Files|*.cmp";
            saveFileDialog1.Title = "保存NC文件";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                string filePath = saveFileDialog1.FileName;
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);

                string Head16Line = make16HeadLine();
                sw.Write(Head16Line);
                //写入列表
                //写入前2圈
                int cNum = iCycNum * 2;
                for (int i = 0; i < cNum; i++)
                {
                    string temp = "0.00000000";
                    sw.WriteLine(temp);
                }
                //写入NCZ值
                //List<double> transList = GetTransZList();
                //获得要写入的Z值

                int listlen = zList.Count;
                for (int i = 0; i < listlen; i++)
                {
                    //Z值偏移
                    double dZOfs = Convert.ToDouble(zOffset.SegValue);
                    double zValue = dZOfs + zList[i].z;
                    //Z值缩放
                    double dZScale = Convert.ToDouble(zScale.SegValue);
                    if (dZScale != 0 && dZScale != 1)
                    {
                        zValue *= dZScale;
                    }
                    string temp = zValue.ToString("F8");
                    sw.WriteLine(temp);
                }
                //写入后2圈
                for (int i = 0; i < cNum; i++)
                {
                    string temp = "0.00000000";
                    sw.WriteLine(temp);
                }
                sw.Close();
                MessageBox.Show("成功保存至" + filePath);
            }
            Trace.WriteLine("1");
            //ComFunc.savestringToFile(a);
        }

        private string make16HeadLine()
        {
            string headStr = "; STREAMX" + "\r\n" + "; NanoCAM Version : 2.7 Build : 55" + "\r\n" + "; CREATED : ";
            DateTime time1 = DateTime.Now;
            string timeStr = time1.DayOfWeek.ToString() + " " + time1.ToString() + "\r\n";
            headStr += timeStr;
            headStr += ("; OPERATOR : NanoCAM Operator"  + "\r\n" + 
                "; INPUT FILE : Not Defined"  + "\r\n" + 
                "; C AXIS ENCODER USED : 3200"  + "\r\n" +
                "; SPINDLE MODE : M03 - X : POSITIVE"  + "\r\n" +
                "; --------------------------------------------------------"  + "\r\n" +
                "; RMax with wrap around (mm) : ");
            headStr += ((dXMax + 2 * dDltX).ToString(" F8") + "\r\n");
            headStr += ("; RMin on Surface (mm) : " + dXMin.ToString("F8") + "\r\n");
            headStr += ("; RMax on Surface (mm) : " + dXMax.ToString("F8") + "\r\n");
            headStr += ("; X = " + (dXMax + 2 * dDltX).ToString("F8") + ", " + (dXMin - 2 * dDltX).ToString("F8") + ", " + dDltX.ToString("F8") + "\r\n");
            headStr += ("; Z = " + dZMin.ToString("F8") +", " + dZMax.ToString("F8") + "\r\n");
            headStr += ("; C = " + iCycNum.ToString() + ", 360.00000000\r\n");
            int nCyc = (int)(Math.Abs(dXMax - dXMin + 1e-9) / Math.Abs(dDltX));
            headStr += ("; --------------------------------------------------------" + "\r\n" +
                        "; #1 DEFINE COMP " + iCycNum.ToString() + "." + nCyc.ToString() + ",#3,#4,#5," +
                        Math.Abs(dXMax - dXMin).ToString("F8") + ",360.00000000" + "\r\n");
            return headStr;
        }

        private void Button_Click_CXTOXYZ(object sender, RoutedEventArgs e)
        {
            if (zList.Count == 0)
            {
                return;
            }
            double zOfs, zScl;
            bool zor = double.TryParse(zOffset.SegValue, out zOfs);
            bool zsr = double.TryParse(zScale.SegValue, out zScl);
            List<Vertex> xyzList;
            if (zor && zsr)
            {
                xyzList = DiaCut.CXZToXYZ(zList, zScl, zOfs);
            }
            else
            {
                xyzList = DiaCut.CXZToXYZ(zList);
            }
            ComFunc.Save3DPtToTxt(xyzList);
        }

        private void Button_Click_Test(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("1");
        }

        //private void Button_Click_DefFST(object sender, RoutedEventArgs e)
        //{
        //    FSTDef dlg = new FSTDef();
        //    dlg.ShowDialog(); 
        //}

    }


    public class FSTSeg:INotifyPropertyChanged
    {
        private string _SegName;
        private string _SegValue;
        private string _SegUnit;
        public FSTSeg()
        {
            _SegName = "";
            _SegValue = "";
            _SegUnit = "";
        }
        public FSTSeg(string segName, string segValue, string segUnit)
        {
            _SegName = segName;
            _SegValue = segValue;
            _SegUnit = segUnit;
        }
        public string SegName
        {
            get
            {
                return _SegName;
            }
            set
            {
                if (value != this._SegName)
                {
                    this._SegName = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string SegUnit
        {
            get
            {
                return _SegUnit;
            }
            set
            {
                if (value != this._SegUnit)
                {
                    this._SegUnit = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string SegValue
        {
            get
            {
                return _SegValue;
            }
            set
            {
                if (value != this._SegValue)
                {
                    this._SegValue = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
