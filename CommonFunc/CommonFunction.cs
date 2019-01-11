using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Runtime.Serialization;  
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Media;
using System.Runtime.CompilerServices;
using System.Net;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

//using NPOI;
//using NPOI.HPSF;
//using NPOI.HSSF;
//using NPOI.SS.Formula.Eval;
//using NPOI.HSSF.UserModel;
//using NPOI.HSSF.Util;
//using NPOI.POIFS;
//using NPOI.SS.UserModel;
//using NPOI.Util;
//using NPOI.SS;
//using NPOI.DDF;
//using NPOI.SS.Util;

namespace CommonFunction
{
    public class ComFunc
    {
        //保存复数列表
        public static void SaveCplxList(List<complex> oriList)
        {
            if (oriList.Count == 0)
            {
                return;
            }
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Txt Files|*.txt";
            saveFileDialog1.Title = "保存文件";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                for (int i = 0; i < oriList.Count; i++)
                {
                    string oneline = oriList[i].x.ToString("F8") + " " + oriList[i].y.ToString("F8");
                    sw.WriteLine(oneline);
                }
                string showTxt = "已将文件保存至" + saveFileDialog1.FileName;
                MessageBox.Show(showTxt);
                sw.Close();
                fs.Close();

            }
        }
        //若数列刚好到x2点，则包括此点，若不能经过，则不包括； y同理
        public static List<complex> MeshGrid(double x1, double x2, double xstep, double y1, double y2, double ystep)
        {
            List<complex> resList = new List<complex>();
            //构造x,y数组
            int cyc = (int)((x2 - x1) / xstep) + 1;
            List<double> xList = new List<double>();
            List<double> yList = new List<double>();
            for (int i = 0; i < cyc; i++)
            {
                xList.Add(x1 + xstep * i);
            }
            cyc = (int)((y2 - y1) / ystep) + 1;
            for (int i = 0; i < cyc; i++)
            {
                yList.Add(y1 + ystep * i);
            }
            for (int i = 0; i < yList.Count; i++)
            {
                for (int j = 0; j < xList.Count; j++)
                {
                    resList.Add(new complex(xList[j], yList[i]));
                }
            }
            return resList;
        }
        public static List<complex> MeshGrid(List<double> xList, List<double> yList)
        {
            List<complex> resList = new List<complex>();
            for (int i = 0; i < yList.Count; i++)
            {
                for (int j = 0; j < xList.Count; j++)
                {
                    resList.Add(new complex(xList[j], yList[i]));
                }
            }
            return resList;
        }
        public static bool Get2Vertex(TextBox tbName1, TextBox tbName2, ref Vertex pt1, ref Vertex pt2)
        {
            int dotPos = tbName1.Text.IndexOf(",");
            string x1Str = "", x2Str = "";
            try
            {
                x1Str = tbName1.Text.Substring(0, dotPos);
            }
            catch (ArgumentOutOfRangeException err)
            {
                MessageBox.Show("x值输入错误，请重新输入，错误类型： " + err.Message);
                return false;
            }
            double x1 = 0, x2 = 0;
            try
            {
                x1 = Convert.ToDouble(x1Str);
            }
            catch (FormatException err)
            {
                MessageBox.Show("x值输入错误，请重新输入，错误类型： " + err.Message);
                return false;
            }
            try
            {
                x2Str = tbName1.Text.Substring(dotPos + 1);

            }
            catch (ArgumentOutOfRangeException err)
            {
                MessageBox.Show("x值输入错误，请重新输入，错误类型： " + err.Message);
                return false;
            }
            try
            {
                x2 = Convert.ToDouble(x2Str);
            }
            catch (FormatException err)
            {
                MessageBox.Show("x值输入错误，请重新输入，错误类型： " + err.Message);
                return false;
            }
            pt1.x = x1;
            pt1.y = x2;

            dotPos = tbName2.Text.IndexOf(",");
            string y1Str = "", y2Str = "";
            try
            {
                y1Str = tbName2.Text.Substring(0, dotPos);
            }
            catch (ArgumentOutOfRangeException err)
            {
                MessageBox.Show("y值输入错误，请重新输入，错误类型： " + err.Message);
                return false;
            }
            double y1 = 0, y2 = 0;
            try
            {
                y1 = Convert.ToDouble(y1Str);
            }
            catch (FormatException err)
            {
                MessageBox.Show("y值输入错误，请重新输入，错误类型： " + err.Message);
                return false;
            }
            try
            {
                y2Str = tbName2.Text.Substring(dotPos + 1);

            }
            catch (ArgumentOutOfRangeException err)
            {
                MessageBox.Show("y值输入错误，请重新输入，错误类型： " + err.Message);
                return false;
            }
            try
            {
                y2 = Convert.ToDouble(y2Str);
            }
            catch (FormatException err)
            {
                MessageBox.Show("y值输入错误，请重新输入，错误类型： " + err.Message);
                return false;
            }
            pt2.x = y1;
            pt2.y = y2;
            return true;
        }
        public static bool GetVertex(TextBox tbName, ref Vertex pt)
        {
            int dotPos = tbName.Text.IndexOf(",");
            string x1Str = "", x2Str = "";
            try
            {
                x1Str = tbName.Text.Substring(0, dotPos);
            }
            catch (ArgumentOutOfRangeException err)
            {
                MessageBox.Show("x值输入错误，请重新输入，错误类型： " + err.Message);
                return false;
            }
            double x1 = 0, x2 = 0;
            try
            {
                x1 = Convert.ToDouble(x1Str);
            }
            catch (FormatException err)
            {
                MessageBox.Show("x值输入错误，请重新输入，错误类型： " + err.Message);
                return false;
            }
            try
            {
                x2Str = tbName.Text.Substring(dotPos + 1);

            }
            catch (ArgumentOutOfRangeException err)
            {
                MessageBox.Show("x值输入错误，请重新输入，错误类型： " + err.Message);
                return false;
            }
            try
            {
                x2 = Convert.ToDouble(x2Str);
            }
            catch (FormatException err)
            {
                MessageBox.Show("x值输入错误，请重新输入，错误类型： " + err.Message);
                return false;
            }
            pt.x = x1;
            pt.y = x2;
            return true;
        }
        public static bool GetVertexFromStr(string oneline, ref Vertex pt)
        {
            int dotPos = 0;
            dotPos = oneline.IndexOf(",");
            if (dotPos == -1)
            {
                dotPos = oneline.IndexOf("  ");
                if (dotPos == -1)
                {
                    dotPos = oneline.IndexOf(" ");
                }
            }
            string x1Str = "", x2Str = "";
            try
            {
                x1Str = oneline.Substring(0, dotPos);
            }
            catch (ArgumentOutOfRangeException err)
            {
                MessageBox.Show("数值解析，错误类型： " + err.Message);
                return false;
            }
            double x1 = 0, x2 = 0;
            try
            {
                x1 = Convert.ToDouble(x1Str);
            }
            catch (FormatException err)
            {
                MessageBox.Show("x值输入错误，请重新输入，错误类型： " + err.Message);
                return false;
            }
            try
            {
                x2Str = oneline.Substring(dotPos + 1);

            }
            catch (ArgumentOutOfRangeException err)
            {
                MessageBox.Show("x值输入错误，请重新输入，错误类型： " + err.Message);
                return false;
            }
            try
            {
                x2 = Convert.ToDouble(x2Str);
            }
            catch (FormatException err)
            {
                MessageBox.Show("x值输入错误，请重新输入，错误类型： " + err.Message);
                return false;
            }
            pt.x = x1;
            pt.y = x2;
            return true;
        }
        public static bool GetVertexFromOneline(string oneline, ref Vertex pt)
        {
            int dotPos = oneline.IndexOf(",");
            string x1Str = "", x2Str = "";
            try
            {
                x1Str = oneline.Substring(0, dotPos);
            }
            catch (ArgumentOutOfRangeException err)
            {
                MessageBox.Show("数值解析，错误类型： " + err.Message);
                return false;
            }
            double x1 = 0, x2 = 0;
            try
            {
                x1 = Convert.ToDouble(x1Str);
            }
            catch (FormatException err)
            {
                MessageBox.Show("x值输入错误，请重新输入，错误类型： " + err.Message);
                return false;
            }
            try
            {
                x2Str = oneline.Substring(dotPos + 1);

            }
            catch (ArgumentOutOfRangeException err)
            {
                MessageBox.Show("x值输入错误，请重新输入，错误类型： " + err.Message);
                return false;
            }
            try
            {
                x2 = Convert.ToDouble(x2Str);
            }
            catch (FormatException err)
            {
                MessageBox.Show("x值输入错误，请重新输入，错误类型： " + err.Message);
                return false;
            }
            pt.x = x1;
            pt.y = x2;
            return true;
        }

        public static Vertex Rotate2DPt(Vertex srcPt, double theta)
        {
            Vertex resPt = new Vertex();
            resPt.x = srcPt.x * Math.Cos(theta / 180 * Math.PI) - srcPt.y * Math.Sin(theta / 180 * Math.PI);
            resPt.y = srcPt.x * Math.Sin(theta / 180 * Math.PI) + srcPt.y * Math.Cos(theta / 180 * Math.PI);
            resPt.z = srcPt.z;
            return resPt;
        }
        public static Vertex Rotate2DPtR(Vertex srcPt, double theta)
        {
            Vertex resPt = new Vertex();
            resPt.x = srcPt.x * Math.Cos(theta) - srcPt.y * Math.Sin(theta);
            resPt.y = srcPt.x * Math.Sin(theta) + srcPt.y * Math.Cos(theta);
            resPt.z = srcPt.z;
            return resPt;
        }
        public static string FindGapString(string oriStr)
        {
            if (oriStr == "")
            {
                return "";
            }
            int pos = oriStr.IndexOf('\t');
            if (pos != -1)
            {
                return "	";
            }
            pos = oriStr.IndexOf(",");
            if (pos != -1)
            {
                return ",";
            }
            pos = oriStr.IndexOf(" ");
            if (pos != -1)
            {
                return " ";
            }
            return "";
        }
        public static bool ResolveFileNumToList(string filePath, ref List<Vertex> dstList)
        {
            if (File.Exists(filePath) == false)
    	    {
                return false;
	        }
            dstList = new List<Vertex>();
            StreamReader sr = new StreamReader(filePath, Encoding.Default);
            string oneline = sr.ReadLine();
            string gapStr = FindGapString(oneline);
            int gap1 = oneline.IndexOf(gapStr);
            int gap2 = oneline.IndexOf(gapStr, gap1 + 1);
            double x = 0, y = 0, z = 0;
            x = Convert.ToDouble(oneline.Substring(0, gap1));
            if (gap2 == -1)//只有2D的数据
            {
                y = Convert.ToDouble(oneline.Substring(gap1 + 1, oneline.Length - gap1 - 1));
                z = 0;
            }
            else
            {
                y = Convert.ToDouble(oneline.Substring(gap1 + 1, gap2 - gap1 - 1));
                z = Convert.ToDouble(oneline.Substring(gap2 + 1, oneline.Length - gap2 - 1));
            }
            Vertex tp1 = new Vertex(x, y, z);
            dstList.Add(tp1);
            //继续添加点
            while ((oneline = sr.ReadLine()) != null)
            {
                gap1 = oneline.IndexOf(gapStr);
                gap2 = oneline.IndexOf(gapStr, gap1 + 1);
                x = Convert.ToDouble(oneline.Substring(0, gap1));
                if (gap2 == -1)//只有2D的数据
                {
                    y = Convert.ToDouble(oneline.Substring(gap1 + 1, oneline.Length - gap1 - 1));
                    z = 0;
                }
                else
                {
                    y = Convert.ToDouble(oneline.Substring(gap1 + 1, gap2 - gap1 - 1));
                    z = Convert.ToDouble(oneline.Substring(gap2 + 1, oneline.Length - gap2 - 1));
                }
                Vertex tp = new Vertex(x, y, z);
                dstList.Add(tp);                
            }
            sr.Close();
            return true;
        }
        public static List<Vertex> MirrorByYAxiaVertex2D(List<Vertex> oriList)
        {
            if (oriList.Count == 0)
            {
                return null;
            }
            List<Vertex> resList = new List<Vertex>();
            if (oriList.Count == 1)
            {
                Vertex tempPt = new Vertex(-oriList[0].x, 0, oriList[0].z);
                resList.Add(tempPt);
                return resList;
            }
            if (oriList[0].x == oriList[1].x)
            {
                return null;
            }
            int oriLen = oriList.Count;
            //先判断X的取值方向
            if (oriList[0].x < oriList[1].x)
            {
                //X正向取值
                //判断X的取值范围
                if (oriList[1].x > 0)
                {
                    //X取值为Y轴右侧,则先镜像再插入
                    for (int i = oriLen - 1; i > 0; i--)
                    {
                        Vertex tempPt = new Vertex(-oriList[i].x, 0, oriList[i].z);
                        resList.Add(tempPt);
                    }
                    for (int i = 0; i < oriLen; i++)
                    {
                        resList.Add(oriList[i]);                        
                    }
                }
                else if (oriList[0].x < 0)
                {
                    //x取值为Y轴左侧，先插入再镜像
                    for (int i = 0; i < oriLen; i++)
                    {
                        resList.Add(oriList[i]);                                                
                    }
                    //镜像
                    for (int i = oriLen - 2; i >= 0; i--)
                    {
                        Vertex tempPt = new Vertex(-oriList[i].x, 0, oriList[i].z);
                        resList.Add(tempPt);
                    }
                }
            }
            else
            {
                //X反向取值
                //全部在Y右边,则先复制再镜像
                if (oriList[0].x > 0)
                {
                    for (int i = 0; i < oriLen; i++)
                    {
                        resList.Add(oriList[i]);
                    }
                    //镜像
                    for (int i = oriLen - 2; i >= 0; i--)
                    {
                        Vertex tempPt = new Vertex(-oriList[i].x, 0, oriList[i].z);
                        resList.Add(tempPt);
                    }
                }
                //全部在Y左边，则先镜像再复制
                if (oriList[1].x < 0)
                {
                    //镜像
                    for (int i = oriLen - 1; i > 0; i--)
                    {
                        Vertex tempPt = new Vertex(-oriList[i].x, 0, oriList[i].z);
                        resList.Add(tempPt);                        
                    }
                    //复制
                    for (int i = 0; i < oriLen; i++)
                    {
                        resList.Add(oriList[i]);
                    }
                }
            }
            return resList;
        }
        public static bool MakeEndOneEnter(string dataTxt)
        {
            if (dataTxt == "")
            {
                return false;
            }
            if (!dataTxt.EndsWith("\r\n"))
            {
                dataTxt += "\r\n";
            }
            else
            {
                while (dataTxt.EndsWith("\r\n"))
                {
                    dataTxt = dataTxt.Remove(dataTxt.Length - 2, 2);
                }
                dataTxt += "\r\n";
            }
            return true;
        }
        public static string getOneLineGap(string oneLineStr)
        {
            int dotPos = oneLineStr.IndexOf(',');
            int gapPos = oneLineStr.IndexOf(' ');
            int tabPos = oneLineStr.IndexOf('\t');
            if (dotPos != -1)
            {
                return ",";
            }
            else
            {
                if (tabPos != -1)
                {
                    return "\t";
                }
                return " ";
            }
        }
        public static List<int> BuildRandomSequence(int low, int high)
        {
            int x = 0, tmp = 0;
            if (low > high)
            {
                tmp = low;
                low = high;
                high = tmp;
            }
            List<int> resList = new List<int>();
            Random ro = new Random();
            for (int i = low; i <= high; i++)
            {
                resList.Add(i);
            }
            for (int i = resList.Count - 1; i > 0; i--)
            {
                x = ro.Next() % (i + 1);
                tmp = resList[i];
                resList[i] = resList[x];
                resList[x] = tmp;
            }
            return resList;
        }
        public static T Clone<T>(T RealObject)   
        {   
            using (Stream objectStream = new MemoryStream())   
            {   
                IFormatter formatter = new BinaryFormatter();   
                formatter.Serialize(objectStream, RealObject);   
                objectStream.Seek(0, SeekOrigin.Begin);   
                return (T)formatter.Deserialize(objectStream);   
            }   
        }
        public static bool ChangeAssDataToFile(string filePath, int eachColLen)
        {
            if (!File.Exists(filePath))
            {
                return false;
            }
            //FileStream fs = new FileStream(filePath, FileMode.Open);

            StreamReader smR = new StreamReader(filePath);
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Txt Files|*.txt";
            saveFileDialog1.Title = "Select a Save File";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                string newFilePath = saveFileDialog1.FileName;
                //System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                FileStream fs = new FileStream(newFilePath, FileMode.Create);
                StreamWriter swW = new StreamWriter(fs, Encoding.Default);
                char[] eachLine = new char[eachColLen];
                int i = 0;
                string oneLine = "";
                while (smR.Read(eachLine, 0, eachColLen) != 0)
                {
                    if (i == 9)
                    {
                        oneLine += new string(eachLine);
                        oneLine += "\r\n";
                        swW.Write(oneLine);
                        oneLine = "";
                        i = 0;
                    }
                    else
                    {
                        oneLine += new string(eachLine);
                        i++;
                    }
                }
                swW.Close();
                fs.Close();
                MessageBox.Show("成功保存至" + newFilePath, "提示");
            }
            smR.Close();
            return true;
        }
        public static bool InputFile(ref string filePath, ref string fileExt, string fileFilter = "Txt Files|*.txt|nc Files|*.nc|All Files|*.*", string openStr = "打开文件")
        {
            if (filePath == "")
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = fileFilter;
                //openFileDialog1.Filter = "Txt Files|*.txt|nc Files|*.nc|All Files|*.*";
                openFileDialog1.Title = openStr;
                if (openFileDialog1.ShowDialog() == true)
                {
                    filePath = openFileDialog1.FileName;
                }
                else
                {
                    return false;
                }
            }
            else if (File.Exists(filePath) == false)
            {
                MessageBox.Show("文件名不合法");
                return false;
            }
            //得到文件后缀名
            int dotPos = filePath.LastIndexOf('.');
            fileExt = filePath.Substring(dotPos + 1);
            return true;
        }
        public static void savestringToFile(string strData, string filePath = "")
        {
            if (strData.Length == 0)
            {
                return;
            }
            if (filePath == "")
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Txt Files|*.txt";
                saveFileDialog1.Title = "Select a Save File";
                saveFileDialog1.ShowDialog();
                if (saveFileDialog1.FileName != "")
                {
                    string newFilePath = saveFileDialog1.FileName;
                    System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                    StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                    sw.Write(strData);
                    string showTxt = "已将文件保存至" + newFilePath;
                    MessageBox.Show(showTxt);
                    sw.Close();
                    fs.Close();
                }                
            }
            else if (File.Exists(filePath) == false)
            {
                MessageBox.Show("文件名不合法");
                return;
            }
            else if (File.Exists(filePath) == true)
            {
                System.IO.FileStream fs = new FileStream(filePath, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                sw.Write(strData);
                sw.Close();
                fs.Close();
            }
        }
        public static void Save1DPtToTxtByCroup(List<double> srcList, int eachLineCount)
        {
            if (srcList.Count == 0 || eachLineCount <=0)
            {
                return;
            }
            int ptNum = srcList.Count;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "cmp Files|*.cmp";
            saveFileDialog1.Title = "保存CMP文件";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                string filePath = saveFileDialog1.FileName;
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                string temp;
                int enterTag = eachLineCount - 1;
                for (int i = 0; i < ptNum; i++)
                {
                    if (i % eachLineCount == enterTag)
                    {
                        temp = string.Format("{0:f8}\r\n", srcList[i]);
                    }
                    else
                    {
                        temp = string.Format("{0:f8} ", srcList[i]);
                    }
                    sw.Write(temp);
                }
                string showTxt = "已将文件保存至" + filePath;
                MessageBox.Show(showTxt);
                sw.Close();
                fs.Close();
            }
        }
        public static void Save1DPtToTxt(List<int> srcList)
        {
            if (srcList.Count == 0)
            {
                return;
            }
            int ptNum = srcList.Count;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Txt Files|*.txt";
            saveFileDialog1.Title = "Select a Cursor File";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                string filePath = saveFileDialog1.FileName;
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                for (int i = 0; i < ptNum; i++)
                {
                    string temp = string.Format("{0}\r\n", srcList[i]);
                    sw.Write(temp);
                }
                string showTxt = "已将文件保存至" + filePath;
                MessageBox.Show(showTxt);
                sw.Close();
                fs.Close();
            }        		
        }
        public static void Save1DValueToTxt<T>(List<T> srcList)
        {
            if (srcList.Count == 0)
            {
                return;
            }
            int ptNum = srcList.Count;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Txt Files|*.txt";
            saveFileDialog1.Title = "Select a Cursor File";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                string filePath = saveFileDialog1.FileName;
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                for (int i = 0; i < ptNum; i++)
                {
                    string temp = string.Format("{0}\r\n", srcList[i]);
                    sw.Write(temp);
                }
                string showTxt = "已将文件保存至" + filePath;
                MessageBox.Show(showTxt);
                sw.Close();
                fs.Close();
            }
        }
        public static void saveVertexToTxt(List<Vertex>srcList)
        {
            if (srcList.Count == 0)
            {
                return;
            }
            int ptNum = srcList.Count;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Txt Files|*.txt";
            saveFileDialog1.Title = "保存文件";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                string filePath = saveFileDialog1.FileName;
                if (ptNum < 200000)
                {
                    System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                    //FileStream fs = new FileStream(filePath, FileMode.CreateNew);
                    StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                    //首先判断是2D点还是3D点
                    bool is2D = true;
                    for (int i = 0; i < ptNum; i++)
                    {
                        if (srcList[i].z != 0)
                        {
                            is2D = false;
                            break;
                        }
                    }
                    for (int i = 0; i < ptNum; i++)
                    {
                        string temp;
                        if (is2D)
                        {
                            temp = string.Format("{0:f8} {1:f8}\r\n", srcList[i].x, srcList[i].y);
                        }
                        else
                        {
                            temp = string.Format("{0:f8} {1:f8} {2:f8}\r\n", srcList[i].x, srcList[i].y, srcList[i].z);
                        }
                        sw.Write(temp);
                    }
                    string showTxt = "已将文件保存至" + filePath;
                    MessageBox.Show(showTxt);
                    sw.Close();
                    fs.Close();
                }
                else
                {
                    MessageBox.Show("点数量太多，请重新输入较少数据");
                }
            }
        }
        public static void Save3DPtToTxt(List<Vertex> srcList)
        {
            if (srcList.Count == 0)
            {
                return;
            }
            int ptNum = srcList.Count;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Txt Files|*.txt";
            saveFileDialog1.Title = "保存文件";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                //如果列表长度大于3000，则分几次保存，否则一次保存
                string filePath = saveFileDialog1.FileName;
                if (ptNum < 200000)
                {
                    System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                    //FileStream fs = new FileStream(filePath, FileMode.CreateNew);
                    StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                    for (int i = 0; i < ptNum; i++)
                    {
                        string temp = string.Format("{0:f8} {1:f8} {2:f8}\r\n", srcList[i].x, srcList[i].y, srcList[i].z);
                        sw.Write(temp);
                    }
                    string showTxt = "已将文件保存至" + filePath;
                    MessageBox.Show(showTxt);
                    sw.Close();
                    fs.Close();
                }
                else
                {
                    int ptArrLen = (int)(ptNum / 200000), ptTag = 0;
                    if (ptArrLen * 200000 != ptNum)
                    {
                        ptArrLen++;
                    }
                    int[] ptArr = new int[ptArrLen];
                    for (int i = 0; i < ptArrLen - 1; i++)
                    {
                        ptArr[i] = 200000;
                        ptTag += 200000;
                    }
                    ptArr[ptArrLen - 1] = ptNum - ptTag;
                    ptTag = 0;
                    System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                    StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                    for (int j = 0; j < ptArr[0]; j++)
                    {
                        string temp = string.Format("{0:f8},{1:f8},{2:f8}\r\n", srcList[ptTag].x, srcList[ptTag].y, srcList[ptTag].z);
                        sw.Write(temp);
                        ptTag++;
                    }
                    sw.Close();
                    fs.Close();
                    for (int i = 1; i < ptArrLen; i++)
                    {
                        //System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                        fs = new FileStream(filePath, FileMode.Append);
                        sw = new StreamWriter(fs, Encoding.Default);
                        for (int j = 0; j < ptArr[i]; j++)
                        {
                            string temp = string.Format("{0:f8},{1:f8},{2:f8}\r\n", srcList[ptTag].x, srcList[ptTag].y, srcList[ptTag].z);
                            sw.Write(temp);
                            ptTag++;
                        }
                        sw.Close();
                        fs.Close();
                    }
                    string showTxt = "已将文件保存至" + filePath;
                    MessageBox.Show(showTxt);
                }
                
            }
        }
        public static bool GetCsvPt(ref string filePath, List<Vertex> resList, List<int> BadIdx, CsvInfo fileInfo)
        {
            if (filePath == "")
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "Txt Files|*.txt|all files(*.*)|*.*";
                openFileDialog1.Title = "Select a Cursor File";
                if (openFileDialog1.ShowDialog() == true)
                {
                    filePath = openFileDialog1.FileName;
                }
                else
                {
                    return false;
                }
                //清空resList列表
                resList.Clear();
                BadIdx.Clear();
            }
            else if (File.Exists(filePath) == false)
            {
                return false;
            }
            //首先找到x-pixels =1025, y-pixels =1025
            System.IO.StreamReader sr = new System.IO.StreamReader(filePath);
            int rowTag = 0, xPix = 0, ListTag = 0, zTag = 0;
            double xRsl = 0, yRsl = 0, zRsl = 0;
            string curStr = sr.ReadLine();
            while (curStr != "Start of Data:" && sr.Peek() != -1)
            {
                //if (String.Compare(curStr, 0, "file:", 0, 5, StringComparison.OrdinalIgnoreCase) == 0)
                if (curStr.IndexOf("x-pixels") >= 0)
                {
                    //找到X有多少个点
                    int firEqual = curStr.IndexOf("=");
                    int secEqual = curStr.IndexOf("=", firEqual + 1);
                    int firstDot = curStr.IndexOf(",");
                    xPix = Convert.ToInt32(curStr.Substring(firEqual + 1, firstDot - firEqual - 1));
                    //yPix = Convert.ToInt32(curStr.Substring(secEqual + 1, curStr.Length - secEqual - 1));
                }
                //x-resolution =2.8541(um), y-resolution =2.8541(um), z-resolution =0.001(um)
                if (curStr.IndexOf("x-resolution") >= 0)
                {
                    //找到X有多少个点
                    int firEqual = curStr.IndexOf("=");
                    int secEqual = curStr.IndexOf("=", firEqual + 1);
                    int thirdEqual = curStr.IndexOf("=", secEqual + 1);
                    int firLB = curStr.IndexOf("(");
                    int firRB = curStr.IndexOf(")");
                    int secLB = curStr.IndexOf("(", firLB + 1);
                    int thirdLB = curStr.IndexOf("(", secLB + 1);
                    //int firRB = curStr.IndexOf(")");
                    string unitXYZ = curStr.Substring(firLB + 1, firRB - firLB - 1);
                    if (unitXYZ == "um")
                    {
                        xRsl = Convert.ToDouble(curStr.Substring(firEqual + 1, firLB - firEqual - 1)) * 0.001;
                        yRsl = Convert.ToDouble(curStr.Substring(secEqual + 1, secLB - secEqual - 1)) * 0.001;
                        zRsl = Convert.ToDouble(curStr.Substring(thirdEqual + 1, thirdLB - thirdEqual - 1)) * 0.001;
                    }
                    else if (unitXYZ == "mm")
                    {
                        xRsl = Convert.ToDouble(curStr.Substring(firEqual + 1, firLB - firEqual - 1));
                        yRsl = Convert.ToDouble(curStr.Substring(secEqual + 1, secLB - secEqual - 1));
                        zRsl = Convert.ToDouble(curStr.Substring(thirdEqual + 1, thirdLB - thirdEqual - 1));
                    }
                }
                curStr = sr.ReadLine();
                Trace.WriteLine("1");
            }
            while (sr.Peek() != -1)
            {
                curStr = sr.ReadLine();
                //循环读取
                int dotPos = -1, lastDotPos = -1, yTag = 0;
                dotPos = curStr.IndexOf(",");
                while (dotPos != -1)
                {
                    Vertex tempPt = new Vertex();
                    if (curStr.Substring(lastDotPos + 1, dotPos - lastDotPos - 1) == "BAD")
                    {
                        tempPt.x = rowTag * xRsl;
                        tempPt.y = yTag * yRsl;
                        tempPt.z = 0;
                        resList.Add(tempPt);
                        BadIdx.Add(ListTag);
                        ListTag++;
                    }
                    else
                    {
                        tempPt.z = Convert.ToDouble(curStr.Substring(lastDotPos + 1, dotPos - lastDotPos - 1));
                        zTag++;
                        if (zTag == 1)
                        {
                            fileInfo.zMin = tempPt.z;
                            fileInfo.zMax = tempPt.z;
                        }
                        else
                        {
                            if (fileInfo.zMin > tempPt.z)
                            {
                                fileInfo.zMin = tempPt.z;                                
                            }
                            if (fileInfo.zMax < tempPt.z)
                            {
                                fileInfo.zMax = tempPt.z;
                            }
                        }
                        tempPt.x = rowTag * xRsl;
                        tempPt.y = yTag * yRsl;
                        resList.Add(tempPt);
                        ListTag++;
                    }
                    if (yTag == (xPix - 1)/2 && rowTag == (xPix - 1)/2)
                    {
                        fileInfo.midPt.x = rowTag * xRsl;
                        fileInfo.midPt.y = yTag * yRsl;
                        if (curStr.Substring(lastDotPos + 1, dotPos - lastDotPos - 1) == "BAD")
                        {
                            tempPt.z = 0;
                        }
                        else
                        {
                            fileInfo.midPt.z = tempPt.z;
                        }
                    }
                    lastDotPos = dotPos;
                    dotPos = curStr.IndexOf(",", lastDotPos + 1);
                    yTag++;
                }
                //最后一个数要插入
                Vertex lastPtTemp = new Vertex();
                if (curStr.Substring(lastDotPos + 1, curStr.Length - lastDotPos - 1) == "BAD")
                {
                    lastPtTemp.z = 0;
                    lastPtTemp.x = rowTag * xRsl;
                    lastPtTemp.y = yTag * yRsl;
                    resList.Add(lastPtTemp);
                    BadIdx.Add(ListTag);
                    ListTag++;
                }
                else
                {
                    lastPtTemp.z = Convert.ToDouble(curStr.Substring(lastDotPos + 1, curStr.Length - lastDotPos - 1));
                    lastPtTemp.x = rowTag * xRsl;
                    lastPtTemp.y = yTag * yRsl;
                    zTag++;
                    if (zTag == 1)
                    {
                        fileInfo.zMin = lastPtTemp.z;
                        fileInfo.zMax = lastPtTemp.z;
                    }
                    else
                    {
                        if (fileInfo.zMin > lastPtTemp.z)
                        {
                            fileInfo.zMin = lastPtTemp.z;
                        }
                        if (fileInfo.zMax < lastPtTemp.z)
                        {
                            fileInfo.zMax = lastPtTemp.z;
                        }
                    }
                    resList.Add(lastPtTemp);
                    ListTag++;
                }
                rowTag++;
            }
						sr.Close();
            fileInfo.ptNum = xPix * xPix;
            fileInfo.xResolution = xRsl;
            fileInfo.yResolution = yRsl;
            fileInfo.zResolution = zRsl;
            return true;
        }

        public static bool offsetCsvPt(CsvInfo fileInfo, List<Vertex> srcList)
        {
            if (fileInfo.ptNum == 0 || srcList.Count == 0)
            {
                return false;
            }
            //找到X,Y平移值
            foreach(Vertex VPt in srcList)
            {
                VPt.x -= fileInfo.midPt.x;
                VPt.y -= fileInfo.midPt.y;
            }
            return true;
        }
        public static bool arrCsvPt(List<Vertex> srcList, ref int ArrNum, double ArrCycle, List<Vertex> OutputList)
        {
            if (srcList.Count == 0)
            {
                return false;
            }
            if (ArrNum <= 0)
            {
                return false;
            }
            if (ArrCycle <= 0)
            {
                return false;
            }
            if (ArrNum % 2 == 1)
            {
                ArrNum++;
            }
            int beginArr = -ArrNum/ 2;
            int endArr = -beginArr;
            for (int i = beginArr; i < endArr; i++)
            {
                foreach(Vertex VT in srcList)
                {
                    //VT.x += i * ArrCycle;
                    Vertex tempPt = new Vertex(VT.x + i*ArrCycle + ArrCycle/2, VT.y, VT.z);
                    OutputList.Add(tempPt);
                }
                Trace.WriteLine("1");
            }
            return true;
        }

        public static void AddPar(double x, double y, ref double res)
        {
            res = x + y;
        }
        public static string QueueToPathData(Queue<double> srcQ, double dltX)
        {
            string strRes = "M ", tempStr;
            int qLen = srcQ.Count;
            for (int i = 0; i < qLen - 1; i++)
            {
                tempStr = string.Format("{0},{1} L ", (i * dltX).ToString(), srcQ.ElementAt(i).ToString());
                strRes += tempStr;
            }
            tempStr = string.Format("{0},{1}", ((qLen - 1) * dltX).ToString(), srcQ.ElementAt(qLen - 1).ToString());
            strRes += tempStr;
            return strRes;
        }
        public static string QueueFloatToPathData(Queue<float> srcQ, double dltX)
        {
            string strRes = "M ", tempStr;
            int qLen = srcQ.Count;
            for (int i = 0; i < qLen - 1; i++)
            {
                tempStr = string.Format("{0},{1} L ", (i * dltX).ToString(), srcQ.ElementAt(i).ToString());
                strRes += tempStr;
            }
            tempStr = string.Format("{0},{1}", ((qLen - 1) * dltX).ToString(), srcQ.ElementAt(qLen - 1).ToString());
            strRes += tempStr;
            return strRes;
        }
        public static string QueueInt16ToPathData(Queue<Int16> srcQ, double dltX)
        {
            if (srcQ.Count == 0)
            {
                return "";
            }
            string strRes = "M ", tempStr;
            int qLen = srcQ.Count;
            for (int i = 0; i < qLen - 1; i++)
            {
                tempStr = string.Format("{0},{1} L ", (i * dltX).ToString(), srcQ.ElementAt(i).ToString());
                strRes += tempStr;
            }
            tempStr = string.Format("{0},{1}", ((qLen - 1) * dltX).ToString(), srcQ.ElementAt(qLen - 1).ToString());
            strRes += tempStr;
            return strRes;
        }
        public static float[] GetDataFromPort(byte[] getData)
        {
            int dataLen = getData.Length;
            int ValueCount = dataLen / sizeof(float);
            int flen = sizeof(float);
            if (dataLen % 4 != 0)
            {
                float[] errList = new float[1];
                return errList;
            }
            float[] fList = new float[ValueCount];
            for (int i = 0; i < ValueCount; i++)
            {
                byte[] tempBtye = new byte[4];
                for (int j = 0; j < flen; j++)
                {
                    tempBtye[j] = getData[i * flen + j];
                }
                fList[i] = BitConverter.ToSingle(tempBtye, 0);
            }
            return fList;
        }

        public static void SaveToExcelFile(MemoryStream ms, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();
                fs.Write(data, 0, data.Length);
                fs.Flush();
                data = null;
            }
        }

        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[-]?\d*[.]?\d*$");
        }
        //是否为操作符
        public static bool IsOper(string value)
        {
            if (value.Length > 1)
            {
                return false;
            }
            if (value == "+" || value == "-" || value == "/" || value == "*" || value == "/" || value == "%")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsInt(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*$");
        }
        public static bool IsUnsign(string value)
        {
            return Regex.IsMatch(value, @"^\d*[.]?\d*$");
        }
        public static bool isTel(string strInput)
        {
            return Regex.IsMatch(strInput, @"\d{3}-\d{8}|\d{4}-\d{7}");
        }
    }
    public class ExpAnly
    {
        private readonly static char[,] prior = new char[8, 8]{{'>','>','<','<','<','<','>','>'},
                                                 {'>','>','<','<','<','<','>','>'},
                                                 {'>','>','>','>','<','<','>','>'},
                                                 {'>','>','>','>','<','<','>','>'},
                                                 {'>','>','>','>','>','<','>','>'},
                                                 {'<','<','<','<','<','<','=','\0'},
                                                 {'>','>','>','>','>','\0','>','>'},
                                                 {'<','<','<','<','<','<','\0','='}};
        private static bool Precede(char t1, char t2, ref char res)
        {
            int idx1 = -1, idx2 = -1;
            switch (t1)
            {
                case '+':
                    idx1 = 0;
                    break;
                case '-':
                    idx1 = 1;
                    break;
                case '*':
                    idx1 = 2;
                    break;
                case '/':
                    idx1 = 3;
                    break;
                case '^':
                    idx1 = 4;
                    break;
                case '(':
                    idx1 = 5;
                    break;
                case ')':
                    idx1 = 6;
                    break;
                case '#':
                    idx1 = 7;
                    break;
                default:
                    break;
            }
            switch (t2)
            {
                case '+':
                    idx2 = 0;
                    break;
                case '-':
                    idx2 = 1;
                    break;
                case '*':
                    idx2 = 2;
                    break;
                case '/':
                    idx2 = 3;
                    break;
                case '^':
                    idx2 = 4;
                    break;
                case '(':
                    idx2 = 5;
                    break;
                case ')':
                    idx2 = 6;
                    break;
                case '#':
                    idx2 = 7;
                    break;
                default:
                    break;
            }
            res = prior[idx1, idx2];
            if (res == '\0')
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private static ExpElem OperateSt(double a, char op, double b)
        {
            ExpNode resTp = new ExpNode("0.0",ExpType.num);
            ExpElem res = new ExpElem(resTp);
            switch (op)
            {
                case '+':
                    {
                        res.num = a + b;
                        break;
                    }
                case '-':
                    {
                        res.num = a - b;
                        break;
                    }
                case '*':
                    {
                        res.num = a * b;
                        break;
                    }
                case '/':
                    {
                        if (b < 1e-9)
                        {
                            res.isValid = false;
                        }
                        else
                        {
                            res.num = a / b;
                        }
                        break;
                    }
                case '^':
                    {
                        res.num = Math.Pow(a, b);
                        if (Double.IsNaN(res.num))
                        {
                            res.isValid = false;
                        }
                        break;
                    }
                default: return res;
            }
            return res;
        }
        //初始化表达式
        //返回的string第一个字符是因变量，剩余的是自变量
        public static string InitExp(List<ExpNode> expList)
        {
            string expstr = "";
            HashSet<string> variset = new HashSet<string>();
            List<ExpElem> eeList = new List<ExpElem>();
            //等号位置
            int equalIdx = 0;
            List<int> variIdx = new List<int>();
            for (int i = 0; i < expList.Count; i++)
            {
                if (expList[i].eType == ExpType.vari)
                {
                    variset.Add(expList[i].eData);
                    variIdx.Add(i);
                }
                if (expList[i].eData == "=")
                {
                    equalIdx = i;
                }
            }
            //构造表达式求值式子
            for (int i = equalIdx + 1; i < expList.Count; i++)
            {
                ExpElem ee = new ExpElem(expList[i]);
            }
            for (int i = 0; i < variset.Count; i++)
            {
                expstr += variset.ElementAt<string>(i);
            }
            return expstr;
        }
        public static string CheckVari(string expstr)
        {
            HashSet<char> a = new HashSet<char>();
            for (int i = 0; i < expstr.Length; i++)
            {
                switch (expstr[i])
                {
                    case 'x':
                        a.Add(expstr[i]);
                        break;
                    case 'y':
                        a.Add(expstr[i]);
                        break;
                    case 'z':
                        a.Add(expstr[i]);
                        break;
                    default:
                        break;
                }
            }
            string vari = "";
            foreach (char item in a)
            {
                string str = new string(item, 1);
                vari += str;
            }
            return vari;
        }
        public static bool Get3DPtFromExp(string expstr, Vertex RangeStep1, Vertex RangeStep2, ref List<Vertex> resList)
        {
            //构造表达式列表
            List<ExpNode> enList = new List<ExpNode>();
            List<ExpElem> eeList = new List<ExpElem>();
            if (!GetExpElem(expstr, ref enList))
            {
                MessageBox.Show("表达式书写错误，请重新检查");
                return false;
            }
            int equalTag = 0;
            for (; equalTag < enList.Count; equalTag++)
            {
                if (enList[equalTag].eData == "=")
                {
                    break;
                }
            }
            equalTag++;
            for (; equalTag < enList.Count; equalTag++)
            {
                eeList.Add(new ExpElem(enList[equalTag]));
            }
            //确定因变量，自变量
            string varis = CheckVari(expstr);
            //根据自变量因变量来确定数值列表
            if (resList == null)
            {
                resList = new List<Vertex>();
            }
            else
            {
                resList.Clear();
            }
            if (varis.Length == 1)
            {
                MessageBox.Show("表达式必须包含至少一个自变量和因变量");
                return false;
            }
            if (varis.Length > 3)
            {
                MessageBox.Show("变量太多");
            }
            int nv1 = (int)(Math.Abs((RangeStep1.x - RangeStep1.z) / RangeStep1.y)) + 1;
            int nv2 = (int)(Math.Abs((RangeStep2.x - RangeStep2.z) / RangeStep2.y)) + 1;
            double step1 = RangeStep1.z > RangeStep1.x ? Math.Abs(RangeStep1.y) : -Math.Abs(RangeStep1.y);
            double step2 = RangeStep2.z > RangeStep2.x ? Math.Abs(RangeStep2.y) : -Math.Abs(RangeStep2.y);
            //3D数据
            if (varis.Length == 3)
            {
                switch (varis[0])
                {
                        //因变量分别为x,y,z时
                    case 'x':
                        {
                            for (int i = 0; i < nv2; i++)
                            {
                                for (int j = 0; j < nv1; j++)
                                {
                                    Vertex tp = new Vertex(0, RangeStep1.x + step1 * j, RangeStep2.x + step2 * i);
                                    double eres = 0;
                                    if (EvaluateExpression(eeList, tp, ref eres))
                                    {
                                        tp.x = eres;
                                        resList.Add(tp);
                                    }
                                }
                            }
                            break;
                        }
                    case 'y':
                        {
                            for (int i = 0; i < nv2; i++)
                            {
                                for (int j = 0; j < nv1; j++)
                                {
                                    Vertex tp = new Vertex(RangeStep1.x + step1 * j, 0, RangeStep2.x + step2 * i);
                                    double eres = 0;
                                    if (Math.Abs(tp.x + 0.56) < 1e-9 && Math.Abs(tp.z) < 1e-9)
                                    {
                                        Trace.WriteLine("1");
                                    }
                                    if (EvaluateExpression(eeList, tp, ref eres))
                                    {
                                        tp.y = eres;
                                        resList.Add(tp);
                                    }
                                }
                            }
                            break;
                        }
                    case 'z':
                        {
                            for (int i = 0; i < nv2; i++)
                            {
                                for (int j = 0; j < nv1; j++)
                                {
                                    Vertex tp = new Vertex(RangeStep1.x + step1 * j, RangeStep2.x + step2 * i, 0);
                                    double eres = 0;
                                    if (EvaluateExpression(eeList, tp, ref eres))
                                    {
                                        tp.z = eres;
                                        resList.Add(tp);
                                    }
                                }
                            } 
                            break;
                        }
                    default:
                        break;
                }
            }
            //2D数据，若为2D数据，则自变量2,RangeStep2忽略
            else 
            {
                switch (varis)
                {
                    case "xy":
                        {
                            for (int i = 0; i < nv1; i++)
                            {
                                double eres = 0;
                                Vertex tp = new Vertex(0, RangeStep1.x + step1 * i);
                                if (EvaluateExpression(eeList, tp, ref eres))
                                {
                                    tp.x = eres;
                                    resList.Add(tp);
                                }
                            }
                            break;
                        }
                    case "xz":
                        {
                            for (int i = 0; i < nv1; i++)
                            {
                                double eres = 0;
                                Vertex tp = new Vertex(0, 0, RangeStep1.x + step1 * i);
                                if (EvaluateExpression(eeList, tp, ref eres))
                                {
                                    tp.x = eres;
                                    resList.Add(tp);
                                }
                            }
                            break;
                        }
                    case "yx":
                        {
                            for (int i = 0; i < nv1; i++)
                            {
                                double eres = 0;
                                Vertex tp = new Vertex(RangeStep1.x + step1 * i, 0);
                                if (EvaluateExpression(eeList, tp, ref eres))
                                {
                                    tp.y = eres;
                                    resList.Add(tp);
                                }
                            }
                            break;
                        }
                    case "yz":
                        {
                            for (int i = 0; i < nv1; i++)
                            {
                                double eres = 0;
                                Vertex tp = new Vertex(0, 0, RangeStep1.x + step1 * i);
                                if (EvaluateExpression(eeList, tp, ref eres))
                                {
                                    tp.y = eres;
                                    resList.Add(tp);
                                }
                            }
                            break;
                        }
                    case "zx":
                        {
                            for (int i = 0; i < nv1; i++)
                            {
                                double eres = 0;
                                Vertex tp = new Vertex(RangeStep1.x + step1 * i, 0);
                                if (EvaluateExpression(eeList, tp, ref eres))
                                {
                                    tp.z = eres;
                                    resList.Add(tp);
                                }
                            }
                            break;
                        }
                    case "zy":
                        {
                            for (int i = 0; i < nv1; i++)
                            {
                                double eres = 0;
                                Vertex tp = new Vertex(0, RangeStep1.x + step1 * i);
                                if (EvaluateExpression(eeList, tp, ref eres))
                                {
                                    tp.z = eres;
                                    resList.Add(tp);
                                }
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
            return true;
        }
        public static bool Get3DPtFromExp(string expstr, List<Vertex> srcPt, ref List<Vertex> resList)
        {
            //构造表达式列表
            List<ExpNode> enList = new List<ExpNode>();
            //eeList为等号右边的部分
            List<ExpElem> eeList = new List<ExpElem>();
            if (!GetExpElem(expstr, ref enList))
            {
                MessageBox.Show("表达式书写错误，请重新检查");
                return false;
            }
            int equalTag = 0;
            for (; equalTag < enList.Count; equalTag++)
            {
                if (enList[equalTag].eData == "=")
                {
                    break;
                }
            }
            equalTag++;
            for (; equalTag < enList.Count; equalTag++)
            {
                eeList.Add(new ExpElem(enList[equalTag]));
            }
            //确定因变量，自变量
            string varis = CheckVari(expstr);
            //根据自变量因变量来确定数值列表
            if (resList == null)
            {
                resList = new List<Vertex>();
            }
            else
            {
                resList.Clear();
            }
            if (varis.Length == 1)
            {
                MessageBox.Show("表达式必须包含至少一个自变量和因变量");
                return false;
            }
            if (varis.Length > 3)
            {
                MessageBox.Show("变量太多");
            }
            //3D数据
            if (varis.Length == 3)
            {
                switch (varis[0])
                {
                    //因变量分别为x,y,z时
                    case 'x':
                        {
                            for (int i = 0; i < srcPt.Count; i++)
                            {
                                Vertex tp = new Vertex(0, srcPt[i].x, srcPt[i].y);
                                double eres = 0;
                                if (EvaluateExpression(eeList, tp, ref eres))
                                {
                                    tp.x = eres;
                                    resList.Add(tp);
                                }
                            }
                            break;
                        }
                    case 'y':
                        {
                            for (int i = 0; i < srcPt.Count; i++)
                            {
                                Vertex tp = new Vertex(srcPt[i].x, 0, srcPt[i].y);
                                double eres = 0;
                                if (EvaluateExpression(eeList, tp, ref eres))
                                {
                                    tp.y = eres;
                                    resList.Add(tp);
                                }
                            }
                            break;
                        }
                    case 'z':
                        {
                            for (int i = 0; i < srcPt.Count; i++)
                            {
                                Vertex tp = new Vertex(srcPt[i].x, srcPt[i].y);
                                double eres = 0;
                                if (EvaluateExpression(eeList, tp, ref eres))
                                {
                                    tp.z = eres;
                                    resList.Add(tp);
                                }
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
            //2D数据，若为2D数据，则自变量2,RangeStep2忽略
            else
            {
                switch (varis)
                {
                    case "xy":
                        {
                            for (int i = 0; i < srcPt.Count; i++)
                            {
                                Vertex tp = new Vertex(0, srcPt[i].x);
                                double eres = 0;
                                if (EvaluateExpression(eeList, tp, ref eres))
                                {
                                    tp.x = eres;
                                    resList.Add(tp);
                                }
                            }
                            break;
                        }
                    case "xz":
                        {
                            for (int i = 0; i < srcPt.Count; i++)
                            {
                                Vertex tp = new Vertex(0, 0, srcPt[i].x);
                                double eres = 0;
                                if (EvaluateExpression(eeList, tp, ref eres))
                                {
                                    tp.x = eres;
                                    resList.Add(tp);
                                }
                            }
                            break;
                        }
                    case "yx":
                        {
                            for (int i = 0; i < srcPt.Count; i++)
                            {
                                Vertex tp = new Vertex(srcPt[i].x, 0);
                                double eres = 0;
                                if (EvaluateExpression(eeList, tp, ref eres))
                                {
                                    tp.y = eres;
                                    resList.Add(tp);
                                }
                            }
                            break;
                        }
                    case "yz":
                        {
                            for (int i = 0; i < srcPt.Count; i++)
                            {
                                Vertex tp = new Vertex(0, 0, srcPt[i].x);
                                double eres = 0;
                                if (EvaluateExpression(eeList, tp, ref eres))
                                {
                                    tp.y = eres;
                                    resList.Add(tp);
                                }
                            }
                            break;
                        }
                    case "zx":
                        {
                            for (int i = 0; i < srcPt.Count; i++)
                            {
                                Vertex tp = new Vertex(srcPt[i].x, 0);
                                double eres = 0;
                                if (EvaluateExpression(eeList, tp, ref eres))
                                {
                                    tp.z = eres;
                                    resList.Add(tp);
                                }
                            }
                            break;
                        }
                    case "zy":
                        {
                            for (int i = 0; i < srcPt.Count; i++)
                            {
                                Vertex tp = new Vertex(0, srcPt[i].x);
                                double eres = 0;
                                if (EvaluateExpression(eeList, tp, ref eres))
                                {
                                    tp.z = eres;
                                    resList.Add(tp);
                                }
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
            return true;
        }
        public static bool EvaluateExpression(List<ExpElem> expList, Vertex vari, ref double res)
        {
            List<ExpElem> expClone = new List<ExpElem>();
            for (int i = 0; i < expList.Count; i++)
            {
                expClone.Add(expList[i]);
            }
            ExpNode tp = new ExpNode("#", ExpType.oper);
            Stack<ExpElem> OPND = new Stack<ExpElem>();
            Stack<ExpElem> OPTR = new Stack<ExpElem>();
            expClone.Add(new ExpElem(tp));
            OPTR.Push(new ExpElem(tp));
            for (int i = 0; i < expClone.Count; i++)
            {
                ExpElem curE = expClone[i];
                if (curE.isValid)
                {
                    switch (curE.etype)
                    {
                        //数字
                        case ExpType.num:
                            {
                                OPND.Push(curE);
                                break;
                            }
                        case ExpType.par:
                        case ExpType.oper:
                            {
                                //查找优先级
                                char cp = '\0';
                                if (Precede(OPTR.Peek().oper, curE.oper, ref cp))
                                {
                                    //如果是<，入栈
                                    switch (cp)
                                    {
                                        case '<'://入栈
                                            {
                                                OPTR.Push(curE);
                                                break;
                                            }
                                        case '>'://出栈
                                            {
                                                char op = OPTR.Pop().oper;
                                                double b = OPND.Pop().num;
                                                double a = OPND.Pop().num;
                                                ExpElem ab = OperateSt(a, op, b);
                                                if (ab.isValid)
                                                {
                                                    OPND.Push(ab);
                                                }
                                                else
                                                {
                                                    //本次表达式解析错误
                                                    return false;
                                                }
                                                i--;
                                                break;
                                            }
                                        case '='://脱括号
                                            {
                                                OPTR.Pop();
                                                break;
                                            }
                                        default:
                                            break;
                                    }
                                } 
                                break;
                            }
                        case ExpType.neg:
                            break;
                        case ExpType.func:
                            {
                                List<ExpElem> funcList = new List<ExpElem>();
                                Stack<char> funcst = new Stack<char>();
                                funcst.Push('(');
                                i+=2;
                                //收集函数内部表达式
                                while (funcst.Count != 0)
                                {
                                    funcList.Add(expList[i]);
                                    if (expList[i].oper == '(')
                                    {
                                        funcst.Push('(');
                                    }
                                    if (expList[i].oper == ')')
                                    {
                                        funcst.Pop();
                                    }
                                }
                                i--;
                                //将最后一个反括号去掉
                                funcList.RemoveAt(funcList.Count - 1);
                                double funcRes = 0;
                                if (EvaluateExpression(funcList, vari, ref funcRes))
                                {
                                    return false;
                                }
                                switch (curE.func)
                                {
                                    case "sin":
                                        {
                                            ExpElem fe = new ExpElem(Math.Sin(funcRes));
                                            OPND.Push(fe);
                                            break;
                                        }
                                    case "cos":
                                        {
                                            ExpElem fe = new ExpElem(Math.Cos(funcRes));
                                            OPND.Push(fe);
                                            break;
                                        }
                                    case "tan":
                                        {
                                            double ifErr = Math.Tan(funcRes);
                                            if (ifErr == System.Double.NaN)
                                            {
                                                return false;
                                            }
                                            ExpElem fe = new ExpElem(ifErr);
                                            OPND.Push(fe);
                                            break;
                                        }
                                    case "ctan":
                                        {
                                            double ifErr = Math.Tan(funcRes);
                                            if (ifErr == System.Double.NaN)
                                            {
                                                return false;
                                            }
                                            ExpElem fe = new ExpElem(1.0/ifErr);
                                            OPND.Push(fe);
                                            break;
                                        }
                                    case "sqrt":
                                        {
                                            double ifErr = Math.Sqrt(funcRes);
                                            if (ifErr == System.Double.NaN)
                                            {
                                                return false;
                                            }
                                            ExpElem fe = new ExpElem(1.0 / ifErr);
                                            OPND.Push(fe);
                                            break;
                                        }
                                    default:
                                        break;
                                }
                                break;
                            }
                        case ExpType.vari:
                            {
                                switch (curE.vari)
                                {
                                    case 'x':
                                        {
                                            ExpElem ve = new ExpElem(vari.x);
                                            OPND.Push(ve);
                                            break;
                                        }
                                    case 'y':
                                        {
                                            ExpElem ve = new ExpElem(vari.y);
                                            OPND.Push(ve);
                                            break;
                                        }
                                    case 'z':
                                        {
                                            ExpElem ve = new ExpElem(vari.z);
                                            OPND.Push(ve);
                                            break;
                                        }
                                    default:
                                        break;
                                }
                                break;
                            }
                        case ExpType.unknow:
                            break;
                        default:
                            break;
                    }
                }
            }
            res = OPND.Peek().num;
            return true;
        }

        //检测一个字符串列表表达的表达式是否有效
        public static bool CheckExpValid(List<string> srcExp)
        {
            if (srcExp.Count == 0)
            {
                return false;
            }
            //判断是否为数字
            if (srcExp.Count == 1)
            {
                if (Regex.IsMatch(srcExp[0], "^[0-9]+$"))
                {
                    return true;
                }
            }
            int srcLen = srcExp.Count;
            //第一个不能为操作符、反括号，但是可以为"-"
            if (Regex.IsMatch(srcExp[0], "^[*+/^).]+$"))
            {
                return false;
            }
            //最后一个不能为操作符，正括号
            if (Regex.IsMatch(srcExp[srcLen - 1], "^[*+/^(.]+$"))
            {
                return false;
            }
            //数字节点，如果第一个字符串是.，则错误
            for (int i = 0; i < srcLen; i++)
            {
                if (srcExp[i][0] == '.')
                {
                    return false;
                }
            }
            //两个操作符不能相邻，除了-号
            if (srcLen >= 3)
            {
                for (int i = 0; i < srcLen - 2; i++)
                {
                    if (srcExp[i].Length == 1 && (!Regex.IsMatch(srcExp[i], "^[0-9]+$")) && (!Regex.IsMatch(srcExp[i], "^[xyz)]+$")))
                    {
                        //符号后面不能接符号(除了负号的"-")
                        if (Regex.IsMatch(srcExp[i + 1], "^[*+/^]+$"))
                        {
                            return false;
                        }
                        //如果符号后面是-，则表示此符号是负号
                        if (srcExp[i + 1] == "-")
                        {
                            double temp = 0;
                            if (!double.TryParse(srcExp[i + 2], out temp))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            //括号匹配检测
            Stack<string> st = new Stack<string>();
            for (int i = 0; i < srcLen; i++)
            {
                if (srcExp[i] == "(")
                {
                    st.Push(srcExp[i]);
                }
                if (srcExp[i] == ")")
                {
                    try
                    {
                        st.Pop();
                    }
                    catch (InvalidOperationException)
                    {
                        MessageBox.Show("括号不配对");
                        return false;
                    }
                }
            }
            if (st.Count != 0)
            {
                return false;
            }
            //三角函数有效性,最后一个不能是三角函数
            if (srcExp[srcLen - 1] == "sin" ||
                srcExp[srcLen - 1] == "cos" ||
                srcExp[srcLen - 1] == "tan" ||
                srcExp[srcLen - 1] == "ctan")
            {
                return false;
            }
            //三角函数后面不能接操作符
            for (int i = 0; i < srcLen - 2; i++)
            {
                if (srcExp[i] == "sin" ||
                    srcExp[i] == "cos" ||
                    srcExp[i] == "tan" ||
                    srcExp[i] == "ctan")
                {
                    if (Regex.IsMatch(srcExp[i], "^[*+/^]+$"))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        //将表达式string，解析成string列表，每个列表中的元素为一个表达式元素
        //这样做的目的是，小数能够识别
        public static bool GetExpElem(string srcstr, ref List<ExpNode> enAList)
        {
            if (srcstr == "")
            {
                return false;
            }
            //string 去空格
            string slimstr = srcstr.Replace(" ", "");
            //变小写
            slimstr = slimstr.ToLower();
            if (enAList == null)
            {
                enAList = new List<ExpNode>();
            }
            else
            {
                enAList.Clear();
            }
            List<string> resList = new List<string>();
            #region 将原string拆分成单个元素，并初步检测有效性
            string RegexNum = "^[0-9.]+$";//判断是否为数字
            string RegexOper = "^[*+-=/^&()]+$";//判断是否为操作符
            string RegexVar = "^[xyz]+$";//判断是否含有变量xyz
            string RegexAll = "^[*+-=/^()0-9xyz]+$";
            string RegexSin = @"sin\(";
            string RegexCos = @"cos\(";
            string RegexTan = @"tan\(";
            string RegexCtan = @"ctan\(";
            string curC = slimstr.Substring(0, 1);
            string lstC = curC;
            string curElem = "";
            int strlen = slimstr.Length;
            //单个字符只能为数字
            if (strlen == 1)
            {
                if (Regex.IsMatch(slimstr, "^[0-9]+$"))
                {
                    return true;
                }
            }
            //所检测的必须为数字，操作符、三角函数、xyz
            bool isNumAndSin = (Regex.IsMatch(slimstr, RegexAll) ||
                Regex.IsMatch(slimstr, RegexSin) ||
                Regex.IsMatch(slimstr, RegexCos) ||
                Regex.IsMatch(slimstr, RegexTan) ||
                Regex.IsMatch(slimstr, RegexCtan));
            bool haveXYZ = ((slimstr.IndexOf("x") != -1) ||
                (slimstr.IndexOf("y") != -1) ||
                (slimstr.IndexOf("z") != -1));
            if (!(isNumAndSin && haveXYZ))
            {
                return false;
            }
            //分类,-是一个元素，.和数字在一起
            for (int i = 0; i < strlen; i++)
            {
                curC = slimstr.Substring(i, 1);
                //如果是数字,包括.号
                if (Regex.IsMatch(curC, RegexNum))
                {
                    curElem += curC;
                }
                else
                {
                    //当前数字入列表
                    if (curElem != "")
                    {
                        resList.Add(curElem);
                        curElem = "";
                    }
                    //如果为操作符
                    if (Regex.IsMatch(curC, RegexOper))
                    {
                        curElem = "";
                        resList.Add(curC);
                    }
                    //如果为变量
                    if (Regex.IsMatch(curC, RegexVar))
                    {
                        curElem = "";
                        resList.Add(curC);
                    }
                    //如果为sin
                    if (curC == "s")
                    {
                        if (i + 3 > strlen || slimstr.Substring(i + 1, 2) != "in")
                        {
                            return false;
                        }
                        resList.Add("sin");
                        i += 2;
                    }
                    //如果为cos
                    if (curC == "c")
                    {
                        if (i + 4 > strlen)
                        {
                            return false;
                        }
                        if (slimstr.Substring(i + 1, 2) == "os")
                        {
                            resList.Add("cos");
                            i += 2;
                        }
                        else if (slimstr.Substring(i + 1, 3) == "tan")
                        {
                            resList.Add("ctan");
                            i += 3;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    //如果为tan
                    if (curC == "t")
                    {
                        if (i + 3 > strlen || slimstr.Substring(i + 1, 2) != "an")
                        {
                            return false;
                        }
                        resList.Add("tan");
                        i += 2;
                    }
                }
            }
            //最后一个数字加入列表
            if (curElem != "")
            {
                resList.Add(curElem);
            }
            //初步检测表达式有效性
            if (!CheckExpValid(resList))
            {
                return false;
            }
            #endregion
            List<ExpNode> enList = new List<ExpNode>();
            for (int i = 0; i < resList.Count; i++)
            {
                //大于1的字符串要么是函数，要么就是数字
                if (resList[i].Length > 1)
                {
                    //数字
                    if (Regex.IsMatch(resList[i], "^[0-9.]+$"))
                    {
                        enList.Add(new ExpNode(resList[i], ExpType.num));
                    }
                    //函数
                    else
                    {
                        enList.Add(new ExpNode(resList[i], ExpType.func));
                    }
                }
                //单字符的均为操作符，除了小于10的单数字
                if (resList[i].Length == 1)
                {
                    //变量
                    if (Regex.IsMatch(resList[i], "^[xyz]+$"))
                    {
                        enList.Add(new ExpNode(resList[i], ExpType.vari));
                        continue;
                    }
                    //括号
                    if (resList[i] == "(" || resList[i] == ")")
                    {
                        enList.Add(new ExpNode(resList[i], ExpType.par));
                        continue;
                    }
                    //单字符
                    if (Regex.IsMatch(resList[i], "^[1-9]+$"))
                    {
                        enList.Add(new ExpNode(resList[i], ExpType.num));
                        continue;
                    }
                    //操作符
                    //此时，除了-未确认是减号还是负号，别的全部为操作符
                    if (resList[i] != "-")
                    {
                        enList.Add(new ExpNode(resList[i], ExpType.oper));
                    }
                    else
                    {
                        enList.Add(new ExpNode(resList[i], ExpType.unknow));
                    }

                }
            }
            #region 判断负号还是减号，负号则与数字合并
            //将x,y,z变量后面的乘号补齐
            //判断-号是负号还是减号
            for (int i = 0; i < enList.Count; i++)
            {
                if (enList[i].eData == "-")
                {
                    if (enList[i - 1].eType == ExpType.num || enList[i - 1].eData == ")" || Regex.IsMatch(enList[i - 1].eData, "^[xyz]+$"))
                    {
                        //减号
                        enList[i].eType = ExpType.oper;
                    }
                    else if (enList[i-1].eType == ExpType.oper)
                    {
                        //负号
                        enList[i].eType = ExpType.neg;
                    }
                }
            }
            //把负号添加到数字中去,
            //因为前面已经判断过最后一个字符串不可能是负号，则不存在i超过下标的情形
            List<ExpNode> enNList = new List<ExpNode>();
            for (int i = 0; i < enList.Count; i++)
            {
                if (enList[i].eType == ExpType.neg)
                {
                    enNList.Add(new ExpNode("-" + enList[i + 1].eData, ExpType.num));
                    i++;
                }
                else
                {
                    enNList.Add(enList[i]);
                }
            }
            #endregion
            #region 原表达式，变量之前没有乘号，将乘号补齐，变量之后直接接数字，则表达式书写错误
            //补齐变量前面的乘号
            for (int i = 0; i < enNList.Count; i++)
            {
                if (Regex.IsMatch(enNList[i].eData, "^[xyz]+$"))
                {
                    //左是变量或数字或者反括号
                    if (i > 0)
                    {
                        if (enNList[i - 1].eType == ExpType.num || enNList[i - 1].eType == ExpType.vari || enNList[i - 1].eData == ")")
                        {
                            //添加乘号
                            enAList.Add(new ExpNode("*", ExpType.oper));
                        }
                        enAList.Add(enNList[i]);
                    }
                    //右边是函数，则补齐乘号
                    if (i < enNList.Count -1)
                    {
                        if (i == 0)
                        {
                            enAList.Add(enNList[i]);
                        }
                        if (enNList[i + 1].eType == ExpType.func)
                        {
                            //添加乘号
                            enAList.Add(new ExpNode("*", ExpType.oper));
                        }
                        //如果右边是数字了，则表达式错误
                        else if (enNList[i + 1].eType == ExpType.num)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    enAList.Add(enNList[i]);
                }
            }
            #endregion
            return true;
        }
        //计算逆波兰式的值
        public static bool GetRPNExp(string exp, ref double res)
        {
            //去空格
            string expNoSpc = exp.Replace(" ", "");
            //检测第一个字符串是否为数字
            string firstc = expNoSpc.Substring(0, 1);
            if (!ComFunc.IsNumeric(firstc) || firstc == "." || firstc == ".")
            {
                return false;
            }
            Stack<string> st = new Stack<string>();
            res = 0;
            string tpstr = firstc;
            int curPos = 0;
            while (curPos < expNoSpc.Length)
            {
                //解析数字
                while ((ComFunc.IsNumeric(tpstr)) && curPos < expNoSpc.Length - 1)
                {
                    tpstr += expNoSpc.Substring(curPos, 1);
                    if (curPos < expNoSpc.Length - 1)
                    {
                        curPos++;
                    }
                }
                tpstr = tpstr.Substring(0, tpstr.Length - 1);
                curPos--;
                //数字入栈
                //try
                //{
                //    curNum = Convert.ToDouble(tpstr);
                //}
                //catch (ArgumentOutOfRangeException err)
                //{
                //    MessageBox.Show("y值输入错误，请重新输入, 错误类型： " + err.Message);
                //    return false;
                //}
                st.Push(tpstr);
                //继续解析
                tpstr = expNoSpc.Substring(curPos, 1);
                //如果为逗号，后面就接着数字,继续解析
                if (tpstr == ",")
                {
                    curPos++;
                    tpstr = expNoSpc.Substring(curPos, 1);
                }
                else
                {
                    if (ComFunc.IsOper(tpstr))
                    {
                        st.Push(tpstr);
                        curPos++;
                        tpstr = expNoSpc.Substring(curPos, 1);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        //表达式求值测试
    }
    public class FTPFunc
    {
        public static void UploadFile(FileInfo fInfo, string targetDir, string hostname, string username, string password)
        {
            //string target;
            //if (targetDir.Trim() == "")
            //{
            //    return;
            //}
            //target = Guid.NewGuid().ToString();
            ////使用临时文件名
            //string URI = "FTP://" + hostname + "/" + targetDir + "/" + target;
            //System.Net.FtpWebRequest ftp = GetRequest(URI, username, password)
        }
    }
    public class DiaCut
    {
        public static bool NegateNcFileYValue(string filePathAndName)
        {
            if (File.Exists(filePathAndName) == false)
            {
                return false;
            }
            StreamReader sr = new StreamReader(filePathAndName);
            List<string> allStr = new List<string>();
            string oneLine = "";
            while ((oneLine = sr.ReadLine()) != null)
            {
                //解析Y值
                int yPos = oneLine.IndexOf('Y');
                int zPos = oneLine.IndexOf('Z');
                int fPos = oneLine.IndexOf('F');
                //如果找到了Y值
                if (yPos != -1 && zPos != -1)
                {
                    double yValue = -Convert.ToDouble(oneLine.Substring(yPos + 1, zPos - yPos - 1));
                    double zValue = Convert.ToDouble(oneLine.Substring(zPos + 1, fPos - zPos - 1));
                    string xStr = oneLine.Substring(0, yPos);
                    string fStr = oneLine.Substring(fPos, oneLine.Length - fPos);
                    string newLine = xStr + "Y" + yValue.ToString("F6") + " Z" + zValue.ToString("F6") + " " + fStr;
                    allStr.Add(newLine);
                }
                else
                {
                    allStr.Add(oneLine);
                }
            }
            sr.Close();
            //输出到文件
            int strLen = allStr.Count;
            StreamWriter sw = new StreamWriter(filePathAndName);
            for (int i = 0; i < strLen; i++)
            {
                sw.WriteLine(allStr[i]);
            }
            sw.Close();
            string showTxt = "已将文件保存至" + filePathAndName;
            MessageBox.Show(showTxt);
            return true;
        }
        public static List<string> GetNcString(List<DiaCutPoint> srcList, ref double usedTime)
        {
            List<string> allstrList = new List<string>();
            usedTime = 0;
            if (srcList.Count == 0)
            {
                return allstrList;
            }
            Vertex lstPt = new Vertex(0, 0, 0);
            for (int i = 0; i < srcList.Count; i++)
			{
                Vertex tp = new Vertex(srcList[i].dcPt.x, srcList[i].dcPt.y, srcList[i].dcPt.z);
                double dis = Math.Sqrt((tp.x - lstPt.x) * (tp.x - lstPt.x) + (tp.y - lstPt.y) * (tp.y - lstPt.y) + (tp.z - lstPt.z) * (tp.z - lstPt.z));
                int fPos = srcList[i].cutSpd.IndexOf("F");
                double spd = Convert.ToDouble(srcList[i].cutSpd.Substring(fPos+1));
                usedTime += (dis / spd * 60.0);
                string oneline = "X" + srcList[i].dcPt.x.ToString("F8") + 
                                 " Y" + srcList[i].dcPt.y.ToString("F8") + 
                                 " Z" + srcList[i].dcPt.z.ToString("F8") + 
                                 " " + srcList[i].cutSpd;
                allstrList.Add(oneline);
			}
            return allstrList;
        }
        public struct Lens_MdlAndMst
        {
            //MICROSTRUCTURE 参数
            public double d1;
            public double l1;
            public double m1;
            public double a1;
            public double a1min;
            public double a1max;
            public double RRuStructMax1;

            //MODULATION 参数
            public double d2;
            public double l2;
            public double m2;
            public double a2;
            public double a2min;
            public double a2max;
            public double RRuStructMax2;
        }
        public static double LinkFunctionWithPar(double phi, double rr, Lens_MdlAndMst LensPar)
        {
            double Y, Z, Z2;
            double xt, y, z;
            double r;

            ///////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////// MICROSTRUCTURE PARAMETERS // /////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////
            double d = LensPar.d1;
            double L = LensPar.l1;
            double m = LensPar.m1;
            double a = LensPar.a1;
            double a1min = LensPar.a1min;
            double a1max = LensPar.a1max;
            double RRuStructMax = LensPar.RRuStructMax1;
            double t = (1.0 / RRuStructMax) * Math.Log(a1min / a1max);
            double a1 = a1max * Math.Exp(((1 - a) * rr + a * rr * Math.Abs(Math.Cos(phi))) * t);   // !!!!! MODIF ATTTENUATION X  !!!!!!!!!!
            double A = 64 * a1 * (2.0 * m + L - 2.0) / (d * d * d);
            double B = 16 * a1 * (3.0 - 2.0 * L - 2.0 * m) / (d * d);
            double C = 4.0 * L * a1 / d;
            double F = (A / 2.0) * Math.Pow((d / 4), 4.0) + (2.0 * B / 3.0) * Math.Pow((d / 4.0), 3.0) + C * Math.Pow((d / 4.0), 2.0);

            ///////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////MODULATION PARAMETERS //////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////
            double d2 = LensPar.d2;
            double L2 = LensPar.l2;
            double m2 = LensPar.m2;
            double a2min = LensPar.a2min;
            double a2max = LensPar.a2max;
            double RRuStructMax2 = LensPar.RRuStructMax2;
            double t2 = (1.0 / RRuStructMax2) * Math.Log(a2min / a2max);
            double a2 = a2max * Math.Exp((rr * Math.Abs(Math.Sin(phi))) * t2);
            double A2 = 64 * a2 * (2.0 * m2 + L2 - 2.0) / (d2 * d2 * d2);
            double B2 = 16 * a2 * (3.0 - 2.0 * L2 - 2.0 * m2) / (d2 * d2);
            double C2 = 4.0 * L2 * a2 / d2;
            double F2 = (A2 / 2.0) * Math.Pow((d2 / 4), 4.0) + (2.0 * B2 / 3.0) * Math.Pow((d2 / 4.0), 3.0) + C2 * Math.Pow((d2 / 4.0), 2.0);
            ///////////////////////////////////////////////////////////////////////////////////////////////////

            y = rr * Math.Cos(phi);
            z = rr * Math.Sin(phi);
            y = y - d / 2.0;
            z = z - d / 2.0;
            Y = Math.Abs(y) - Math.Floor(Math.Abs(y) / d) * d - d / 2.0;
            Z = Math.Abs(z) - Math.Floor(Math.Abs(z) / d) * d - d / 2.0;
            r = Math.Sqrt(Y * Y + Z * Z);


            Z2 = Math.Abs(z - Math.Floor(z / d2) * d2 - d2 / 2);

            ///////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////     EQUATIONS        ///////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////

            if (Math.Abs(z + d / 2.0) > 10)
            {
                //MICROSTRUCTURE
                if (r < (d / 4.0))
                {
                    xt = (A / 4.0) * Math.Pow(r, 4.0) + (B / 3.0) * Math.Pow(r, 3.0) + (C / 2.0) * Math.Pow(r, 2.0) - F;
                }
                else
                {
                    if (r < (d / 2.0))
                    {
                        xt = -(A / 4.0) * Math.Pow((d / 2.0 - r), 4.0) - (B / 3.0) * Math.Pow((d / 2.0 - r), 3.0) - (C / 2.0) * Math.Pow((d / 2.0 - r), 2.0);
                    }
                    else
                    {
                        xt = 0;
                    }
                }
            }
            else
            {
                //MODULATION		
                if (Z2 < (d2 / 4.0))
                {
                    xt = (A2 / 4.0) * Math.Pow(Z2, 4.0) + (B2 / 3.0) * Math.Pow(Z2, 3.0) + (C2 / 2.0) * Math.Pow(Z2, 2.0) - F2;
                }
                else
                {
                    xt = -(A2 / 4.0) * Math.Pow((d2 / 2.0 - Z2), 4.0) - (B2 / 3.0) * Math.Pow((d2 / 2.0 - Z2), 3.0) - (C2 / 2.0) * Math.Pow((d2 / 2.0 - Z2), 2.0);
                }
            }
            return (xt * 1E3); /* output in microns */
        }

        public static double LinkFunction(double phi, double rr)
        {
            double Y, Z, Z2;
            double xt, y, z;
            double r;

            ///////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////// MICROSTRUCTURE PARAMETERS // /////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////
            double d = 0.800;
            double L = 0.75;
            double m = 0.50;
            double a = 0.500;
            double a1min = 0.003;
            double a1max = 0.100;
            double RRuStructMax = 35.0;
            double t = (1.0 / RRuStructMax) * Math.Log(a1min / a1max);
            double a1 = a1max * Math.Exp(((1 - a) * rr + a * rr * Math.Abs(Math.Cos(phi))) * t);   // !!!!! MODIF ATTTENUATION X  !!!!!!!!!!
            double A = 64 * a1 * (2.0 * m + L - 2.0) / (d * d * d);
            double B = 16 * a1 * (3.0 - 2.0 * L - 2.0 * m) / (d * d);
            double C = 4.0 * L * a1 / d;
            double F = (A / 2.0) * Math.Pow((d / 4), 4.0) + (2.0 * B / 3.0) * Math.Pow((d / 4.0), 3.0) + C * Math.Pow((d / 4.0), 2.0);

            ///////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////MODULATION PARAMETERS //////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////
            double d2 = 0.800;
            double L2 = 0.1;
            double m2 = 0.98;
            double a2min = 0.005;
            double a2max = 0.03;
            double RRuStructMax2 = 10.0;
            double t2 = (1.0 / RRuStructMax2) * Math.Log(a2min / a2max);
            double a2 = a2max * Math.Exp((rr * Math.Abs(Math.Sin(phi))) * t2);
            double A2 = 64 * a2 * (2.0 * m2 + L2 - 2.0) / (d2 * d2 * d2);
            double B2 = 16 * a2 * (3.0 - 2.0 * L2 - 2.0 * m2) / (d2 * d2);
            double C2 = 4.0 * L2 * a2 / d2;
            double F2 = (A2 / 2.0) * Math.Pow((d2 / 4), 4.0) + (2.0 * B2 / 3.0) * Math.Pow((d2 / 4.0), 3.0) + C2 * Math.Pow((d2 / 4.0), 2.0);
            ///////////////////////////////////////////////////////////////////////////////////////////////////

            y = rr * Math.Cos(phi);
            z = rr * Math.Sin(phi);
            y = y - d / 2.0;
            z = z - d / 2.0;
            Y = Math.Abs(y) - Math.Floor(Math.Abs(y) / d) * d - d / 2.0;
            Z = Math.Abs(z) - Math.Floor(Math.Abs(z) / d) * d - d / 2.0;
            r = Math.Sqrt(Y * Y + Z * Z);


            Z2 = Math.Abs(z - Math.Floor(z / d2) * d2 - d2 / 2);

            ///////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////     EQUATIONS        ///////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////

            if (Math.Abs(z + d / 2.0) > 10)
            {
                //MICROSTRUCTURE
                if (r < (d / 4.0))
                {
                    xt = (A / 4.0) * Math.Pow(r, 4.0) + (B / 3.0) * Math.Pow(r, 3.0) + (C / 2.0) * Math.Pow(r, 2.0) - F;
                }
                else
                {
                    if (r < (d / 2.0))
                    {
                        xt = -(A / 4.0) * Math.Pow((d / 2.0 - r), 4.0) - (B / 3.0) * Math.Pow((d / 2.0 - r), 3.0) - (C / 2.0) * Math.Pow((d / 2.0 - r), 2.0);
                    }
                    else
                    {
                        xt = 0;
                    }
                }
            }
            else
            {
                //MODULATION		
                if (Z2 < (d2 / 4.0))
                {
                    xt = (A2 / 4.0) * Math.Pow(Z2, 4.0) + (B2 / 3.0) * Math.Pow(Z2, 3.0) + (C2 / 2.0) * Math.Pow(Z2, 2.0) - F2;
                }
                else
                {
                    xt = -(A2 / 4.0) * Math.Pow((d2 / 2.0 - Z2), 4.0) - (B2 / 3.0) * Math.Pow((d2 / 2.0 - Z2), 3.0) - (C2 / 2.0) * Math.Pow((d2 / 2.0 - Z2), 2.0);
                }
            }
            return (xt * 1E3); /* output in microns */
        }
        //计算某点的加工时间
        //需要输入当前点，上一点的坐标、加工速度
        public static bool GetNcOnePtTime(ref TimeSpan ts, Vertex curPt, Vertex lstPt, double speed)
        {
            if (speed < 0 || Math.Abs(speed) < 1e-9)
            {
                return false;
            }
            double dis = Math.Sqrt((curPt.x - lstPt.x) * (curPt.x - lstPt.x) + (curPt.y - lstPt.y) * (curPt.y - lstPt.y) + (curPt.z - lstPt.z) * (curPt.z - lstPt.z));
            ts = TimeSpan.FromMinutes(dis / speed); 
            return true;
        }
        //NC文件单行计算时间
        //curPt返回当前点
        //usedTime返回过去了多少时间，按秒计算
        public static bool rsvNcStr(ref Vertex curPt, ref TimeSpan usedTime, Vertex lstPt, string oneline)
        {
            int xpos = oneline.IndexOf('X');
            int ypos = oneline.IndexOf('Y');
            int zpos = oneline.IndexOf('Z');
            int fpos = oneline.IndexOf('F');
            curPt.x = lstPt.x;
            curPt.y = lstPt.y;
            curPt.z = lstPt.z;
            if (fpos == -1)
            {
                return false;
            }
            if (xpos == -1 && ypos == -1 && zpos == -1)
            {
                return true;
            }
            //解析X
            if (xpos != -1)
            {
                char[] xchar = new char[100];
                int xcharTag = 0;
                for (int i = xpos + 1; i < oneline.Length; i++)
                {
                    if ((oneline[i] <= '9' && oneline[i] >= '0') || oneline[i] == '-' || oneline[i] == '.')
                    {
                        xchar[xcharTag] = oneline[i];
                        xcharTag++;
                    }
                    else
                    {
                        break;
                    }
                }
                //将X转成数字
                //string xstr = new string(xchar);
                curPt.x = Convert.ToDouble(new string(xchar));
            }
            //解析Y
            if (ypos != -1)
            {
                char[] ychar = new char[100];
                int ycharTag = 0;
                for (int i = ypos + 1; i < oneline.Length; i++)
                {
                    if ((oneline[i] <= '9' && oneline[i] >= '0') || oneline[i] == '-' || oneline[i] == '.')
                    {
                        ychar[ycharTag] = oneline[i];
                        ycharTag++;
                    }
                    else
                    {
                        break;
                    }
                }
                //将X转成数字
                //string ystr = new string(ychar);
                curPt.y = Convert.ToDouble(new string(ychar));
            }
            //解析Z
            if (zpos != -1)
            {
                char[] zchar = new char[100];
                int zcharTag = 0;
                for (int i = zpos + 1; i < oneline.Length; i++)
                {
                    if ((oneline[i] <= '9' && oneline[i] >= '0') || oneline[i] == '-' || oneline[i] == '.')
                    {
                        zchar[zcharTag] = oneline[i];
                        zcharTag++;
                    }
                    else
                    {
                        break;
                    }
                }
                //将X转成数字
                //string ystr = new string(ychar);
                curPt.z = Convert.ToDouble(new string(zchar));
            }
            //解析F
            if (fpos != -1)
            {
                char[] fchar = new char[100];
                int fcharTag = 0;
                for (int i = fpos + 1; i < oneline.Length; i++)
                {
                    if ((oneline[i] <= '9' && oneline[i] >= '0') || oneline[i] == '-' || oneline[i] == '.')
                    {
                        fchar[fcharTag] = oneline[i];
                        fcharTag++;
                    }
                    else
                    {
                        break;
                    }
                }
                //将F转成数字
                //string ystr = new string(ychar);
                double speed = Convert.ToDouble(new string(fchar));
                if (Math.Abs(speed) < 1e-9)
                {
                    return false;
                }
                //求两点之间的距离
                double dis = Math.Sqrt((curPt.x - lstPt.x) * (curPt.x - lstPt.x) + (curPt.y - lstPt.y) * (curPt.y - lstPt.y) + (curPt.z - lstPt.z) * (curPt.z - lstPt.z));
                usedTime = TimeSpan.FromMinutes(dis / speed);//表示分钟数
            }
            return true;
        }
        public static TimeSpan GetNcFileUsedtime(string filePath)
        {
            TimeSpan allts = new TimeSpan(0);
            StreamReader sr = new StreamReader(filePath);
            string oneline = "";
            Vertex curPt = new Vertex();
            Vertex lstPt = new Vertex();
            while ((oneline = sr.ReadLine()) != null)
            {
                TimeSpan ts = new TimeSpan(0);
                bool test = rsvNcStr(ref curPt, ref ts, lstPt, oneline);
                if (test == true)
                {
                    lstPt.x = curPt.x;
                    lstPt.y = curPt.y;
                    lstPt.z = curPt.z;
                }
                allts += ts;
            }
            sr.Close();
            return allts;
        }
        public static List<Vertex> SetCXZROIValue(List<Vertex> srcList, Rect givenRC, double zValue)
        {
            List<Vertex> newList = new List<Vertex>();
            for (int i = 0; i < srcList.Count; i++)
            {
                double x = srcList[i].y * Math.Cos(srcList[i].x * Math.PI / 180);
                double y = srcList[i].y * Math.Sin(srcList[i].x * Math.PI / 180);
                bool isX1 = (x > givenRC.X || Math.Abs(x - givenRC.X) < 1e-9);
                bool isX2 = (x < (givenRC.X + givenRC.Width) || Math.Abs(x - givenRC.X - givenRC.Width) < 1e-9);
                bool isY1 = (y > givenRC.Y || Math.Abs(y - givenRC.Y) < 1e-9);
                bool isY2 = (y < (givenRC.Y + givenRC.Height) || Math.Abs(y - givenRC.Y - givenRC.Height) < 1e-9);
                if (isX1 && isX2 && isY1 && isY2)
                {
                    newList.Add(srcList[i]);
                }
                else
                {
                    Vertex tp = new Vertex(srcList[i].x, srcList[i].y, zValue);
                    newList.Add(tp);
                }
            }
            return newList;
        }
        public static List<Vertex> GetHexArrPtList(double hexLen, double width, double height)
        {
            List<Vertex> allList = new List<Vertex>();
            if (hexLen <=0 || width <= 0 || height <= 0)
            {
                return allList;
            }
            double xCur = hexLen / 2, yCur = 0;
            while (yCur < height || Math.Abs(yCur - height) < 1e-9)
            {
                while (xCur < width || Math.Abs(xCur - width) < 1e-9)
                {
                    Vertex tp = new Vertex(xCur, yCur, 0);
                    allList.Add(tp);
                    xCur += hexLen;
                    Vertex tp1 = new Vertex(xCur, yCur, 0);
                    allList.Add(tp1);
                    xCur += (hexLen * 2);
                }
                xCur = 0;
                yCur += hexLen /2 * Math.Sqrt(3);
                while (xCur < width || Math.Abs(xCur - width) < 1e-9)
                {
                    Vertex tp = new Vertex(xCur, yCur, 0);
                    allList.Add(tp);
                    xCur += (hexLen * 2);
                    Vertex tp1 = new Vertex(xCur, yCur, 0);
                    allList.Add(tp1);
                    xCur += hexLen;                    
                }
                xCur = hexLen / 2;
                yCur += hexLen / 2 * Math.Sqrt(3);
            }
            return allList;
        }
        public static List<complex> GetHexArrPtList(double hexLen, Rect lbPos)
        {
            double width = lbPos.Width, height = lbPos.Height;
            List<complex> allList = new List<complex>();
            if (hexLen <= 0 || width <= 0 || height <= 0)
            {
                return allList;
            }
            double xSt = hexLen / 2, ySt = 0;
            double xCur = xSt, yCur = ySt;
            int yTag = 0;
            double yAdd = hexLen / 2 * Math.Sqrt(3);
            while (yCur < height || Math.Abs(yCur - height) < 1e-9)
            {
                int xTag = 0;
                while (xCur < width || Math.Abs(xCur - width) < 1e-9)
                {
                    complex tp = new complex(xCur, yCur);
                    allList.Add(tp);
                    xTag++;
                    xCur = hexLen * xTag + xSt;
                    complex tp1 = new complex(xCur, yCur);
                    allList.Add(tp1);
                    xTag += 2;
                    xCur = hexLen * xTag + xSt;
                }
                xCur = 0;
                yTag++;
                yCur = yAdd * yTag;
                xTag = 0;
                while (xCur < width || Math.Abs(xCur - width) < 1e-9)
                {
                    complex tp = new complex(xCur, yCur);
                    allList.Add(tp);
                    xTag += 2;
                    xCur = hexLen * xTag;
                    complex tp1 = new complex(xCur, yCur);
                    allList.Add(tp1);
                    xTag++;
                    xCur = hexLen * xTag;
                }
                yTag++;
                yCur = yAdd * yTag;
                xCur = xSt;
            }
            //将列表移动到rect点处
            complex ofsPt = new complex(lbPos.X - lbPos.Width / 2, lbPos.Y - lbPos.Height / 2);
            for (int i = 0; i < allList.Count; i++)
            {
                allList[i].x += ofsPt.x;
                allList[i].y += ofsPt.y;
            }
            return allList;
        }
        public static bool HexIntoPiece(List<Vertex> srcList, int nPiece)
        {
            if (srcList == null)
            {
                return false;
            }
            if (srcList.Count == 0 || nPiece <= 0)
            {
                return false;
            }
            int listLen = srcList.Count;
            for (int i = 0; i < listLen; i++)
            {
                double theta = GetThetaValue(srcList[i].x, srcList[i].y);
                int thetaTag = 0;
                if (theta < 2 * Math.PI - Math.PI / nPiece)
                {
                    thetaTag = (int)((theta + Math.PI / nPiece) / (2 * Math.PI / nPiece));
                }
                srcList[i].z = -2 * Math.PI / nPiece * thetaTag;
            }
            return true;
        }
        public static double GetThetaValue(double x, double y)
        {
            if (x == 0 && y == 0)
            {
                return 0;
            }
            double Theta = Math.Acos(x / Math.Sqrt(x * x + y * y));
            //第三四象限
            if (y < 0)
            {
                return Math.PI * 2 - Theta;
            }
            return Theta;
        }
        public static List<Vertex> GetOneThetaPt(double R, int n, double xPrec, double yPrec)
        {
            List<Vertex> ptList = new List<Vertex>();
            double Theta = Math.PI / n ;//在<= -Theta 到 <Theta范围之内的点
            double ATheta = Theta * 180 / Math.PI;
            double xExt = R, xCur = xPrec;
            double yExt = R * Math.Sin(Theta), yCur = 0;
            while (yCur <= R)
            {
                while (xCur <= R)
                {
                    //计算角度
                    double rCur = Math.Sqrt(xCur * xCur + yCur * yCur);
                    double tCur = Math.Asin(yCur / rCur);
                    //在单个范围之内
                    if (tCur >= - Theta && tCur < Theta)
                    {
                        if (rCur <= R)
                        {
                            Vertex tpPt = new Vertex(xCur, yCur, 0);
                            ptList.Add(tpPt);
                        }
                    }
                    xCur += xPrec;
                }
                xCur = 0;
                yCur += yPrec;
            }
            return ptList;
        }
        public static List<List<Vertex>> GetEllipsePt(double a, double b, int n, double xPrec, double yPrec)
        {
            List<List<Vertex>> allPtList = new List<List<Vertex>>();
            for (int i = 0; i < n; i++)
            {
                List<Vertex> ptList = new List<Vertex>();
                allPtList.Add(ptList);
            }
            //先生成所有点
            List<Vertex> allPt = new List<Vertex>();
            double xCur = -a, yCur = -b;
            while (yCur <= b)
            {
                while (xCur <= a)
                {
                    //Z值在取值范围之内
                    if ((1 - xCur * xCur / a / a - yCur * yCur / b / b) >= 0)
                    {
                        //计算角度
                        double Theta = GetThetaValue(xCur, yCur);
                        int thetaTag = 0;
                        if (Theta < 2 * Math.PI - Math.PI / n)
                        {
                            thetaTag = (int)((Theta + Math.PI / n) / (2 * Math.PI / n));
                        }
                        Vertex tpPt = new Vertex(xCur, yCur, -2 * Math.PI / n * thetaTag);
                        allPtList[thetaTag].Add(tpPt);
                        allPt.Add(tpPt);
                    }
                    xCur += xPrec;
                }
                xCur -= xPrec;
                yCur += yPrec;
                if (yCur > b)
                {
                    break;
                }
                while (xCur > -a)
                {
                    if ((1 - xCur * xCur / a / a - yCur * yCur / b / b) >= 0)
                    {
                        //计算角度
                        double Theta = GetThetaValue(xCur, yCur);
                        int thetaTag = 0;
                        if (Theta < 2 * Math.PI - Math.PI / n)
                        {
                            thetaTag = (int)((Theta + Math.PI / n) / (2 * Math.PI / n));
                        }
                        Vertex tpPt = new Vertex(xCur, yCur, -2 * Math.PI / n * thetaTag);
                        allPtList[thetaTag].Add(tpPt);
                        allPt.Add(tpPt);
                    }
                    xCur -= xPrec;
                }
                //最后一个-R点
                xCur = -a;
                if ((1 - xCur * xCur / a / a - yCur * yCur / b / b) >= 0)
                {
                    //计算角度
                    double Theta = GetThetaValue(xCur, yCur);
                    int thetaTag = 0;
                    if (Theta < 2 * Math.PI - Math.PI / n)
                    {
                        thetaTag = (int)((Theta + Math.PI / n) / (2 * Math.PI / n));
                    }
                    Vertex tpPt = new Vertex(xCur, yCur, -2 * Math.PI / n * thetaTag);
                    allPtList[thetaTag].Add(tpPt);
                    allPt.Add(tpPt);
                }
                yCur += yPrec;
            }
            return allPtList;
        }
        public static List<List<Vertex>> GetThetaPt(double R, int n, double xPrec, double yPrec)
        {
            List<List<Vertex>> allPtList = new List<List<Vertex>>();
            for (int i = 0; i < n; i++)
            {
                List<Vertex> ptList = new List<Vertex>();
                allPtList.Add(ptList);
            }
            //先生成所有点
            List<Vertex> allPt = new List<Vertex>();
            double xCur = -R, yCur = -R;
            while (yCur < R || Math.Abs(yCur - R) < 1e-9)
            {
                while (xCur < R || Math.Abs(xCur - R) < 1e-9)
                {
                    if (Math.Sqrt(xCur* xCur + yCur * yCur) <= R)
                    {
                        //计算角度
                        double Theta = GetThetaValue(xCur, yCur);
                        int thetaTag = 0;
                        if (Theta < 2 * Math.PI - Math.PI / n)
                        {
                            thetaTag = (int)((Theta + Math.PI / n) / (2 * Math.PI / n));
                        }
                        Vertex tpPt = new Vertex(xCur, yCur, -2 * Math.PI / n * thetaTag);
                        allPtList[thetaTag].Add(tpPt);
                        allPt.Add(tpPt);
                    }
                    xCur += xPrec;
                }
                xCur -= xPrec;
                yCur += yPrec;
                if (yCur > R)
                {
                    break;
                }
                while (xCur > -R)
                {
                    if (Math.Sqrt(xCur * xCur + yCur * yCur) <= R)
                    {
                        //计算角度
                        double Theta = GetThetaValue(xCur, yCur);
                        int thetaTag = 0;
                        if (Theta < 2 * Math.PI - Math.PI / n)
                        {
                            thetaTag = (int)((Theta + Math.PI / n) / (2 * Math.PI / n));
                        }
                        Vertex tpPt = new Vertex(xCur, yCur, -2 * Math.PI / n * thetaTag);
                        allPtList[thetaTag].Add(tpPt);
                        allPt.Add(tpPt);
                    }
                    xCur -= xPrec;
                }
                //最后一个-R点
                xCur = -R;
                if (Math.Sqrt(xCur * xCur + yCur * yCur) <= R)
                {
                    //计算角度
                    double Theta = GetThetaValue(xCur, yCur);
                    int thetaTag = 0;
                    if (Theta < 2 * Math.PI - Math.PI / n)
                    {
                        thetaTag = (int)((Theta + Math.PI / n) / (2 * Math.PI / n));
                    }
                    Vertex tpPt = new Vertex(xCur, yCur, -2 * Math.PI / n * thetaTag);
                    allPtList[thetaTag].Add(tpPt);
                    allPt.Add(tpPt);
                }
                yCur += yPrec;
            }
            //ComFunc.Save3DPtToTxt(allPt);
            ////保存文件对话框
            //SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            //saveFileDialog1.Filter = "Txt Files|*.txt";
            //saveFileDialog1.Title = "保存txt文件";
            //saveFileDialog1.ShowDialog();
            //if (saveFileDialog1.FileName != "")
            //{
            //    string filePath = saveFileDialog1.FileName;
            //    System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
            //    StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            //    int ptLen = allPtList[0].Count;
            //    for (int i = 0; i < ptLen; i++)
            //    {
            //        string oneLineStr = allPtList[0][i].x.ToString("F8") + " " + allPtList[0][i].y.ToString("F8") + "\r\n";
            //        sw.Write(oneLineStr);
            //    }
            //    string showTxt = "已将文件保存至" + filePath;
            //    MessageBox.Show(showTxt);
            //    sw.Close();
            //    fs.Close();
            //}
            return allPtList;
        }

        //midlen是九宫格中间区域的宽度，即为0区域的宽度，常规设置为120
        //R是九宫格口径的一半，也就是工件的半径
        //此算法计算九宫格右上角的圆弧区域的分割点和2个区域需要旋转的角度
        //返回分割点的X,Y坐标,3D点的Z为下面区域需要旋转的角度alpha，上面区域旋转的角度为90-alpha
        public static Vertex Get9GridSmallHeight(double midLen, double R)
        {
            Vertex splitPt = new Vertex();
            //计算下面的弦与圆弧相交的2点A,B，A点位于B点的下面
            Vertex A = new Vertex(Math.Sqrt(R * R - midLen * midLen / 4), midLen / 2);
            Vertex B = new Vertex();
            //角度theta+角度AOY为通过分割点的直线倾斜角,也为下面区域的旋转角度alpha
            double theta = Math.Atan(A.y / A.x);
            splitPt.z = Math.Asin(midLen / 2 / R) + theta;
            //计算B点的坐标，它为A点绕原点旋转2倍的theta
            B.x = A.x * Math.Cos(theta * 2) - A.y * Math.Sin(theta * 2);
            B.y = A.x * Math.Sin(theta * 2) + A.y * Math.Cos(theta * 2);
            double test = splitPt.z * 180.0 / Math.PI;
            //计算此直线的斜率
            double k = Math.Tan(splitPt.z);
            //直线经过B点，计算直线方程的b值
            double b = B.y - B.x * k;
            //计算splitPt的y值
            splitPt.x = midLen / 2;
            splitPt.y = k * splitPt.x + b;
            return splitPt;
        }
        //网纹镜九宫格算法
        //返回一个9个集合，分别保存了九宫格的每个区域的点
        public static List<List<Vertex>> Get9GridPt(double R, double xPrec, double yPrec, double midLen)
        {
            //此功能是基本写死了
            List<List<Vertex>> allPtList = new List<List<Vertex>>();
            for (int i = 0; i < 9; i++)
            {
                List<Vertex> ptList = new List<Vertex>();
                allPtList.Add(ptList);
            }
            //先生成所有点
            List<Vertex> allPt = new List<Vertex>();
            double xCur = -R, yCur = -R;
            double xSt = -R, ySt = -R;
            int yTimes = (int)(R / yPrec * 2) + 1;
            int xTimes = (int)(R / xPrec * 2) + 1;
            for (int i = 0; i < yTimes; i += 2)
            {
                yCur = ySt + yPrec * i;
                for (int j = 0; j < xTimes; j++)
                {
                    xCur = xSt + xPrec * j;
                    if (Math.Sqrt(xCur * xCur + yCur * yCur) < R || Math.Abs(Math.Sqrt(xCur * xCur + yCur * yCur) - R) < 1e-9)
                    {
                        Vertex tp = new Vertex(xCur, yCur);
                        allPt.Add(tp);
                    }
                }
                //如果xcur!=xSt,即不在边界
                if (Math.Abs(xCur + xSt) > 1e-9)
                {
                    xCur = -xSt;
                    if (Math.Sqrt(xCur * xCur + yCur * yCur) < R || Math.Abs(Math.Sqrt(xCur * xCur + yCur * yCur) - R) < 1e-9)
                    {
                        Vertex tp = new Vertex(xCur, yCur);
                        allPt.Add(tp);
                    }
                }

                yCur = ySt + yPrec * (i + 1);
                for (int j = 0; j < xTimes; j++)
                {
                    xCur = -xSt - xPrec * j;
                    if (Math.Sqrt(xCur * xCur + yCur * yCur) < R || Math.Abs(Math.Sqrt(xCur * xCur + yCur * yCur) - R) < 1e-9)
                    {
                        Vertex tp = new Vertex(xCur, yCur);
                        allPt.Add(tp);
                    }
                }
                //如果x不在边界
                if (Math.Abs(xCur - xSt) > 1e-9)
                {
                    xCur = xSt;
                    if (Math.Sqrt(xCur * xCur + yCur * yCur) < R || Math.Abs(Math.Sqrt(xCur * xCur + yCur * yCur) - R) < 1e-9)
                    {
                        Vertex tp = new Vertex(xCur, yCur);
                        allPt.Add(tp);
                    }
                }
            }
            //#region 查看所有的点是否完整
            //StreamWriter swallpt = new StreamWriter(@"E:\Git\Coding\CSharp\ConvexCurveFly2Step\alllist.txt");
            //for (int i = 0; i < allPt.Count; i++)
            //{
            //    string oneline = allPt[i].x.ToString("F8") + " " + allPt[i].y.ToString("F8");
            //    swallpt.WriteLine(oneline);

            //}
            //swallpt.Close();
            //#endregion
            //将所有的点归类
            int ptLen = allPt.Count;
            DateTime dt1 = DateTime.Now;
            for (int i = 0; i < ptLen; i++)
            {
                //判断是否是九宫格最中间的区域 -midlen/2 <= x <= midlen/2 和 -midlen/2 <= y <= midlen/2
                if ((allPt[i].x > -midLen / 2 || Math.Abs(allPt[i].x + midLen / 2) < 1e-9) &&
                    (allPt[i].x < midLen / 2 || Math.Abs(allPt[i].x - midLen / 2) < 1e-9) &&
                    (allPt[i].y > -midLen / 2 || Math.Abs(allPt[i].y + midLen / 2) < 1e-9) &&
                    (allPt[i].y < midLen / 2 || Math.Abs(allPt[i].y - midLen / 2) < 1e-9))
                {
                    allPtList[0].Add(allPt[i]);
                    allPt[i].z = 1;
                    //allPt.Remove(allPt[i]);
                    //i--;
                    //ptLen--;
                }
            }
            for (int i = 0; i < ptLen; i++)
            {
                if (Math.Abs(allPt[i].z - 1) > 1e-9)
                {
                    //判断是否是为九宫格最右边的区域 x > midlen/2 和 -midlen/2 <= y <= midlen/2
                    //表示为1区域的点，同时应该包括1区域与2区域的边界，1区域和8区域的边界,即为上述表达式中的等号
                    if (allPt[i].x > midLen / 2 && (allPt[i].y > -midLen / 2 || Math.Abs(allPt[i].y + midLen / 2) < 1e-9) && (allPt[i].y < midLen / 2 || Math.Abs(allPt[i].y - midLen / 2) < 1e-9))
                    {
                        allPtList[1].Add(allPt[i]);
                        allPt[i].z = 1;
                    }
                }
            }

            for (int i = 0; i < ptLen; i++)
            {
                if (Math.Abs(allPt[i].z - 1) > 1e-9)
                {
                    //判断是否是为九宫格右上角的区域 x >= midlen/2 和 y > midlen/2
                    //表示为2区域的点，同时应该包括2区域与3区域的边界,即为上述表达式中的等号
                    if ((allPt[i].x > midLen / 2 || Math.Abs(allPt[i].x - midLen / 2) < 1e-9) && allPt[i].y > midLen / 2)
                    {
                        allPtList[2].Add(allPt[i]);
                        allPt[i].z = 1;
                    }
                }
            }

            for (int i = 0; i < ptLen; i++)
            {
                if (Math.Abs(allPt[i].z - 1) > 1e-9)
                {
                    //判断是否是为九宫格中间上面的区域 -midlen/2 <= x < midlen/2 和 y > midlen/2
                    //表示为3区域的点，同时应该包括3区域与4区域的边界,即为上述表达式中的等号
                    if ((allPt[i].x > -midLen / 2 || Math.Abs(allPt[i].x + midLen / 2) < 1e-9) && allPt[i].x < midLen / 2 && allPt[i].y > midLen / 2)
                    {
                        allPtList[3].Add(allPt[i]);
                        allPt[i].z = 1;
                    }
                }
            }

            for (int i = 0; i < ptLen; i++)
            {
                if (Math.Abs(allPt[i].z - 1) > 1e-9)
                {
                    //判断是否是为九宫格左上角的区域 x < - midlen/2 和 y >= midlen/2
                    //表示为4区域的点，同时应该包括4区域与5区域的边界,即为上述表达式中的等号
                    if (allPt[i].x < -midLen / 2 && (allPt[i].y > midLen / 2 || Math.Abs(allPt[i].y - midLen / 2) < 1e-9))
                    {
                        allPtList[4].Add(allPt[i]);
                        allPt[i].z = 1;
                    }
                }
            }

            for (int i = 0; i < ptLen; i++)
            {
                if (Math.Abs(allPt[i].z - 1) > 1e-9)
                {
                    //判断是否是为九宫格最左边的区域 x < - midlen/2 和 -midlen/2 <= y < midlen/2
                    //表示为5区域的点，同时应该包括5区域与6区域的边界,即为上述表达式中的等号
                    if (allPt[i].x < -midLen / 2 && (allPt[i].y > -midLen / 2 || Math.Abs(allPt[i].y + midLen / 2) < 1e-9) && allPt[i].y < midLen / 2)
                    {
                        allPtList[5].Add(allPt[i]);
                        allPt[i].z = 1;
                    }
                }
            }

            for (int i = 0; i < ptLen; i++)
            {
                if (Math.Abs(allPt[i].z - 1) > 1e-9)
                {
                    //判断是否是为九宫格左下角的区域 x <= - midlen/2 和 y < -midlen/2
                    //表示为6区域的点，同时应该包括6区域与7区域的边界,即为上述表达式中的等号
                    if ((allPt[i].x < -midLen / 2 || Math.Abs(allPt[i].x + midLen / 2) < 1e-9) && allPt[i].y < -midLen / 2)
                    {
                        allPtList[6].Add(allPt[i]);
                        allPt[i].z = 1;
                    }
                }
            }

            for (int i = 0; i < ptLen; i++)
            {
                if (Math.Abs(allPt[i].z - 1) > 1e-9)
                {
                    //判断是否是为九宫格中间下面的区域 -midlen/2 < x <= midlen/2 和 y < -midlen/2
                    //表示为7区域的点，同时应该包括7区域与8区域的边界,即为上述表达式中的等号
                    if (allPt[i].x > -midLen / 2 && (allPt[i].x < midLen / 2 || Math.Abs(allPt[i].x - midLen / 2) < 1e-9) && allPt[i].y < -midLen / 2)
                    {
                        allPtList[7].Add(allPt[i]);
                        allPt[i].z = 1;
                    }
                }
            }

            for (int i = 0; i < ptLen; i++)
            {
                if (Math.Abs(allPt[i].z - 1) > 1e-9)
                {
                    //判断是否是为九宫格右下角的区域 x > midlen/2 和 y < -midlen/2
                    if (allPt[i].x > midLen / 2 && allPt[i].y < -midLen / 2)
                    {
                        allPtList[8].Add(allPt[i]);
                        allPt[i].z = 1;
                    }
                }
            }
            //#region 测试分段中的点是否完整
            //for (int i = 0; i < allPtList.Count; i++)
            //{
            //    StreamWriter sw = new StreamWriter(@"E:\Git\Coding\CSharp\ConvexCurveFly2Step\list" + i.ToString() + ".txt");
            //    for (int j = 0; j < allPtList[i].Count; j++)
            //    {
            //        string oneline = allPtList[i][j].x.ToString("F8") + " " + allPtList[i][j].y.ToString("F8") + " " + i.ToString();
            //        sw.WriteLine(oneline);
            //    }
            //    sw.Close();
            //}
            //DateTime dt2 = DateTime.Now;
            //TimeSpan spT = dt2 - dt1;
            //MessageBox.Show(spT.ToString());
            //#endregion
            return allPtList;
        }
        //网纹镜九宫格算法
        //返回一个9个集合，分别保存了九宫格的每个区域的点
        //0和1的区域合并，返回8个列表
        public static List<List<Vertex>> Get901GridPt(double R, double xPrec, double yPrec, double midLen)
        {
            //此功能是基本写死了
            List<List<Vertex>> allPtList = new List<List<Vertex>>();
            for (int i = 0; i < 8; i++)
            {
                List<Vertex> ptList = new List<Vertex>();
                allPtList.Add(ptList);
            }
            //先生成所有点
            List<Vertex> allPt = new List<Vertex>();
            double xCur = -R, yCur = -R;
            double xSt = -R, ySt = -R;
            int yTimes = (int)(R / yPrec * 2) + 1;
            int xTimes = (int)(R / xPrec * 2) + 1;
            for (int i = 0; i < yTimes; i += 2)
            {
                yCur = ySt + yPrec * i;
                for (int j = 0; j < xTimes; j++)
                {
                    xCur = xSt + xPrec * j;
                    if (Math.Sqrt(xCur * xCur + yCur * yCur) < R || Math.Abs(Math.Sqrt(xCur * xCur + yCur * yCur) - R) < 1e-9)
                    {
                        Vertex tp = new Vertex(xCur, yCur);
                        allPt.Add(tp);
                    }
                }
                //如果xcur!=xSt,即不在边界
                if (Math.Abs(xCur + xSt) > 1e-9)
                {
                    xCur = -xSt;
                    if (Math.Sqrt(xCur * xCur + yCur * yCur) < R || Math.Abs(Math.Sqrt(xCur * xCur + yCur * yCur) - R) < 1e-9)
                    {
                        Vertex tp = new Vertex(xCur, yCur);
                        allPt.Add(tp);
                    }
                }

                yCur = ySt + yPrec * (i + 1);
                for (int j = 0; j < xTimes; j++)
                {
                    xCur = -xSt - xPrec * j;
                    if (Math.Sqrt(xCur * xCur + yCur * yCur) < R || Math.Abs(Math.Sqrt(xCur * xCur + yCur * yCur) - R) < 1e-9)
                    {
                        Vertex tp = new Vertex(xCur, yCur);
                        allPt.Add(tp);
                    }
                }
                //如果x不在边界
                if (Math.Abs(xCur - xSt) > 1e-9)
                {
                    xCur = xSt;
                    if (Math.Sqrt(xCur * xCur + yCur * yCur) < R || Math.Abs(Math.Sqrt(xCur * xCur + yCur * yCur) - R) < 1e-9)
                    {
                        Vertex tp = new Vertex(xCur, yCur);
                        allPt.Add(tp);
                    }
                }
            }
            //将所有的点归类
            int ptLen = allPt.Count;
            for (int i = 0; i < ptLen; i++)
            {
                //判断是否是九宫格中间和右边的区域 x >= -midlen/2 <= x 和 -midlen/2 <= y <= midlen/2
                if ((allPt[i].x > -midLen / 2 || Math.Abs(allPt[i].x + midLen / 2) < 1e-9) &&
                    //(allPt[i].x < midLen / 2 || Math.Abs(allPt[i].x - midLen / 2) < 1e-9) &&
                    (allPt[i].y > -midLen / 2 || Math.Abs(allPt[i].y + midLen / 2) < 1e-9) &&
                    (allPt[i].y < midLen / 2 || Math.Abs(allPt[i].y - midLen / 2) < 1e-9))
                {
                    allPtList[0].Add(allPt[i]);
                    allPt[i].z = 1;
                    //allPt.Remove(allPt[i]);
                    //i--;
                    //ptLen--;
                }
            }
            for (int i = 0; i < ptLen; i++)
            {
                if (Math.Abs(allPt[i].z - 1) > 1e-9)
                {
                    //判断是否是为九宫格右上角的区域 x >= midlen/2 和 y > midlen/2
                    //表示为2区域的点，同时应该包括2区域与3区域的边界,即为上述表达式中的等号
                    if ((allPt[i].x > midLen / 2 || Math.Abs(allPt[i].x - midLen / 2) < 1e-9) && allPt[i].y > midLen / 2)
                    {
                        allPtList[1].Add(allPt[i]);
                        allPt[i].z = 1;
                    }
                }
            }

            for (int i = 0; i < ptLen; i++)
            {
                if (Math.Abs(allPt[i].z - 1) > 1e-9)
                {
                    //判断是否是为九宫格中间上面的区域 -midlen/2 <= x < midlen/2 和 y > midlen/2
                    //表示为3区域的点，同时应该包括3区域与4区域的边界,即为上述表达式中的等号
                    if ((allPt[i].x > -midLen / 2 || Math.Abs(allPt[i].x + midLen / 2) < 1e-9) && allPt[i].x < midLen / 2 && allPt[i].y > midLen / 2)
                    {
                        allPtList[2].Add(allPt[i]);
                        allPt[i].z = 1;
                    }
                }
            }

            for (int i = 0; i < ptLen; i++)
            {
                if (Math.Abs(allPt[i].z - 1) > 1e-9)
                {
                    //判断是否是为九宫格左上角的区域 x < - midlen/2 和 y >= midlen/2
                    //表示为4区域的点，同时应该包括4区域与5区域的边界,即为上述表达式中的等号
                    if (allPt[i].x < -midLen / 2 && (allPt[i].y > midLen / 2 || Math.Abs(allPt[i].y - midLen / 2) < 1e-9))
                    {
                        allPtList[3].Add(allPt[i]);
                        allPt[i].z = 1;
                    }
                }
            }

            for (int i = 0; i < ptLen; i++)
            {
                if (Math.Abs(allPt[i].z - 1) > 1e-9)
                {
                    //判断是否是为九宫格最左边的区域 x < - midlen/2 和 -midlen/2 <= y < midlen/2
                    //表示为5区域的点，同时应该包括5区域与6区域的边界,即为上述表达式中的等号
                    if (allPt[i].x < -midLen / 2 && (allPt[i].y > -midLen / 2 || Math.Abs(allPt[i].y + midLen / 2) < 1e-9) && allPt[i].y < midLen / 2)
                    {
                        allPtList[4].Add(allPt[i]);
                        allPt[i].z = 1;
                    }
                }
            }

            for (int i = 0; i < ptLen; i++)
            {
                if (Math.Abs(allPt[i].z - 1) > 1e-9)
                {
                    //判断是否是为九宫格左下角的区域 x <= - midlen/2 和 y < -midlen/2
                    //表示为6区域的点，同时应该包括6区域与7区域的边界,即为上述表达式中的等号
                    if ((allPt[i].x < -midLen / 2 || Math.Abs(allPt[i].x + midLen / 2) < 1e-9) && allPt[i].y < -midLen / 2)
                    {
                        allPtList[5].Add(allPt[i]);
                        allPt[i].z = 1;
                    }
                }
            }

            for (int i = 0; i < ptLen; i++)
            {
                if (Math.Abs(allPt[i].z - 1) > 1e-9)
                {
                    //判断是否是为九宫格中间下面的区域 -midlen/2 < x <= midlen/2 和 y < -midlen/2
                    //表示为7区域的点，同时应该包括7区域与8区域的边界,即为上述表达式中的等号
                    if (allPt[i].x > -midLen / 2 && (allPt[i].x < midLen / 2 || Math.Abs(allPt[i].x - midLen / 2) < 1e-9) && allPt[i].y < -midLen / 2)
                    {
                        allPtList[6].Add(allPt[i]);
                        allPt[i].z = 1;
                    }
                }
            }

            for (int i = 0; i < ptLen; i++)
            {
                if (Math.Abs(allPt[i].z - 1) > 1e-9)
                {
                    //判断是否是为九宫格右下角的区域 x > midlen/2 和 y < -midlen/2
                    if (allPt[i].x > midLen / 2 && allPt[i].y < -midLen / 2)
                    {
                        allPtList[7].Add(allPt[i]);
                        allPt[i].z = 1;
                    }
                }
            }
            //#region 测试分段中的点是否完整
            //for (int i = 0; i < allPtList.Count; i++)
            //{
            //    StreamWriter sw = new StreamWriter(@"E:\Git\Coding\CSharp\ConvexCurveFly2Step\list" + i.ToString() + ".txt");
            //    for (int j = 0; j < allPtList[i].Count; j++)
            //    {
            //        string oneline = allPtList[i][j].x.ToString("F8") + " " + allPtList[i][j].y.ToString("F8") + " " + i.ToString();
            //        sw.WriteLine(oneline);
            //    }
            //    sw.Close();
            //}
            //DateTime dt2 = DateTime.Now;
            //TimeSpan spT = dt2 - dt1;
            //MessageBox.Show(spT.ToString());
            //#endregion
            return allPtList;
        }
        //将九宫格的2，4，6，8分别再分成2分，
        //同时将九宫格的0和1区域合并
        //将四个死角重新归纳
        //一共12个区域，为九宫格的进化版
        public static List<List<Vertex>> Get901GridPlusAllPt(double R, double xPrec, double yPrec, double midLen)
        {
            List<List<Vertex>> Grid9List = Get901GridPt(R, xPrec, yPrec, midLen);
            List<List<Vertex>> Grid9PlusList = new List<List<Vertex>>();
            for (int i = 0; i < 12; i++)
            {
                List<Vertex> ptList = new List<Vertex>();
                Grid9PlusList.Add(ptList);
            }
            //计算九宫格2区域的分割点
            Vertex splidPt = Get9GridSmallHeight(midLen, R);
            //先把0区域拷贝到新列表中
            for (int i = 0; i < Grid9List[0].Count; i++)
            {
                Grid9PlusList[0].Add(Grid9List[0][i]);
            }
            //从1开始，2，4，6，8分割为2份
            int newLTag = 1;
            for (int i = 1; i < 8; i++)
            {
                //不分割列表
                if (i % 2 == 0)
                {
                    for (int j = 0; j < Grid9List[i].Count; j++)
                    {
                        Grid9PlusList[newLTag].Add(Grid9List[i][j]);
                    }
                    newLTag++;
                }
                switch (i)
                {
                    case 1:
                        {
                            //y <= splidPt.y;
                            for (int j = 0; j < Grid9List[i].Count; j++)
                            {
                                if (Grid9List[i][j].y < splidPt.y || Math.Abs(Grid9List[i][j].y - splidPt.y) < 1e-9)
                                {
                                    Grid9PlusList[1].Add(Grid9List[i][j]);
                                }
                                else if (Grid9List[i][j].y > splidPt.y)
                                {
                                    Grid9PlusList[2].Add(Grid9List[i][j]);
                                }
                            }
                            newLTag += 2;
                            break;
                        }
                    case 3:
                        {
                            //y >= splidPt.y;
                            for (int j = 0; j < Grid9List[i].Count; j++)
                            {
                                if (Grid9List[i][j].y > splidPt.y || Math.Abs(Grid9List[i][j].y - splidPt.y) < 1e-9)
                                {
                                    Grid9PlusList[4].Add(Grid9List[i][j]);
                                }
                                else if (Grid9List[i][j].y < splidPt.y)
                                {
                                    Grid9PlusList[5].Add(Grid9List[i][j]);
                                }
                            }
                            newLTag += 2;
                            break;
                        }
                    case 5:
                        {
                            //y >= -splidPt.y;
                            for (int j = 0; j < Grid9List[i].Count; j++)
                            {
                                if (Grid9List[i][j].y > -splidPt.y || Math.Abs(Grid9List[i][j].y + splidPt.y) < 1e-9)
                                {
                                    Grid9PlusList[7].Add(Grid9List[i][j]);
                                }
                                else if (Grid9List[i][j].y < -splidPt.y)
                                {
                                    Grid9PlusList[8].Add(Grid9List[i][j]);
                                }
                            }
                            newLTag += 2;
                            break;
                        }
                    case 7:
                        {
                            //y <= -splidPt.y;
                            for (int j = 0; j < Grid9List[i].Count; j++)
                            {
                                if (Grid9List[i][j].y < -splidPt.y || Math.Abs(Grid9List[i][j].y + splidPt.y) < 1e-9)
                                {
                                    Grid9PlusList[10].Add(Grid9List[i][j]);
                                }
                                else if (Grid9List[i][j].y > -splidPt.y)
                                {
                                    Grid9PlusList[11].Add(Grid9List[i][j]);
                                }
                            }
                            newLTag += 2;
                            break;
                        }
                }
            }
            //重新规划四个死角点
            //得到12个区域的旋转角度列表
            List<double> rotateList = Grid901PlusRotateList(midLen, R);
            #region 从2区域中找出死角点并入区域1
            List<int> errList = new List<int>();
            for (int i = 0; i < Grid9PlusList[2].Count; i++)
            {
                Vertex tp = ComFunc.Rotate2DPtR(Grid9PlusList[2][i], rotateList[2]);
                //此点y > midlen，也就是在0和1区域之外
                if (Math.Abs(tp.y - midLen/2) > 1e-9 && (tp.y > midLen / 2 || tp.y < -midLen / 2))
                {
                    errList.Add(i);
                }
            }
            //把死角点加入到1区域,先把1区域最后一点标记
            Grid9PlusList[1][Grid9PlusList[1].Count - 1].z = -1;
            for (int i = 0; i < errList.Count; i++)
            {
                Grid9PlusList[1].Add(Grid9PlusList[2][errList[i]]);
            }
            //再将区域2中的死角点删掉
            for (int i = 0; i < errList.Count; i++)
            {
                Grid9PlusList[2].RemoveAt(errList[i]- i);
            }
            #endregion
            #region 从4区域找到死角点并入区域5
            //清空死角列表
            errList.Clear();
            for (int i = 0; i < Grid9PlusList[4].Count; i++)
            {
                Vertex tp = ComFunc.Rotate2DPtR(Grid9PlusList[4][i], rotateList[4]);
                //此点y > midlen，也就是在0和1区域之外
                if (Math.Abs(tp.y - midLen / 2) > 1e-9 && (tp.y > midLen / 2 || tp.y < -midLen / 2))
                {
                    errList.Add(i);
                }
            }
            //把死角点加入到5区域
            Grid9PlusList[5][Grid9PlusList[5].Count - 1].z = -1;
            for (int i = 0; i < errList.Count; i++)
            {
                Grid9PlusList[5].Add(Grid9PlusList[4][errList[i]]);
            }
            //再将区域6中的死角点删掉
            for (int i = 0; i < errList.Count; i++)
            {
                Grid9PlusList[4].RemoveAt(errList[i] - i);
            }
            #endregion
            #region 从8区域找到死角点并入7
            //清空死角列表
            errList.Clear();
            for (int i = 0; i < Grid9PlusList[8].Count; i++)
            {
                Vertex tp = ComFunc.Rotate2DPtR(Grid9PlusList[8][i], rotateList[8]);
                //此点y > midlen，也就是在0和1区域之外
                if (Math.Abs(tp.y - midLen / 2) > 1e-9 && (tp.y > midLen / 2 || tp.y < -midLen / 2))
                {
                    errList.Add(i);
                }
            }
            //把死角点加入到7区域
            for (int i = 0; i < errList.Count; i++)
            {
                Grid9PlusList[7].Insert(i, Grid9PlusList[8][errList[i]]);
            }
            //7区域分界点做标记
            Grid9PlusList[7][errList.Count - 1].z = -1;
            //再将区域2中的死角点删掉
            for (int i = 0; i < errList.Count; i++)
            {
                Grid9PlusList[8].RemoveAt(errList[i] - i);
            }
            #endregion
            #region 从10区域找到死角点并入11
            //清空死角列表
            errList.Clear();
            for (int i = 0; i < Grid9PlusList[10].Count; i++)
            {
                Vertex tp = ComFunc.Rotate2DPtR(Grid9PlusList[10][i], rotateList[10]);
                //此点y > midlen，也就是在0和1区域之外
                if (Math.Abs(tp.y - midLen / 2) > 1e-9 && (tp.y > midLen / 2 || tp.y < -midLen / 2))
                {
                    errList.Add(i);
                }
            }
            //把死角点加入到11区域
            for (int i = 0; i < errList.Count; i++)
            {
                Grid9PlusList[11].Insert(i, Grid9PlusList[10][errList[i]]);
            }
            //7区域分界点做标记
            Grid9PlusList[11][errList.Count - 1].z = -1;
            //再将区域10中的死角点删掉
            for (int i = 0; i < errList.Count; i++)
            {
                Grid9PlusList[10].RemoveAt(errList[i] - i);
            }
            #endregion
            return Grid9PlusList;
        }
        //将九宫格的2，4，6，8分别再分成2分，
        //同时将九宫格的0和1区域合并,一共12个区域，为九宫格的进化版
        public static List<List<Vertex>> Get901GridPlusPt(double R, double xPrec, double yPrec, double midLen)
        {
            List<List<Vertex>> Grid9List = Get901GridPt(R, xPrec, yPrec, midLen);
            List<List<Vertex>> Grid9PlusList = new List<List<Vertex>>();
            for (int i = 0; i < 12; i++)
            {
                List<Vertex> ptList = new List<Vertex>();
                Grid9PlusList.Add(ptList);
            }
            //计算九宫格2区域的分割点
            Vertex splidPt = Get9GridSmallHeight(midLen, R);
            //先把0区域拷贝到新列表中
            for (int i = 0; i < Grid9List[0].Count; i++)
            {
                Grid9PlusList[0].Add(Grid9List[0][i]);
            }
            //从1开始，2，4，6，8分割为2份
            int newLTag = 1;
            for (int i = 1; i < 8; i++)
            {
                //不分割列表
                if (i % 2 == 0)
                {
                    for (int j = 0; j < Grid9List[i].Count; j++)
                    {
                        Grid9PlusList[newLTag].Add(Grid9List[i][j]);
                    }
                    newLTag++;
                }
                switch (i)
                {
                    case 1:
                        {
                            //y <= splidPt.y;
                            for (int j = 0; j < Grid9List[i].Count; j++)
                            {
                                if (Grid9List[i][j].y < splidPt.y || Math.Abs(Grid9List[i][j].y - splidPt.y) < 1e-9)
                                {
                                    Grid9PlusList[1].Add(Grid9List[i][j]);
                                }
                                else if (Grid9List[i][j].y > splidPt.y)
                                {
                                    Grid9PlusList[2].Add(Grid9List[i][j]);
                                }
                            }
                            newLTag += 2;
                            break;
                        }
                    case 3:
                        {
                            //y >= splidPt.y;
                            for (int j = 0; j < Grid9List[i].Count; j++)
                            {
                                if (Grid9List[i][j].y > splidPt.y || Math.Abs(Grid9List[i][j].y - splidPt.y) < 1e-9)
                                {
                                    Grid9PlusList[4].Add(Grid9List[i][j]);
                                }
                                else if (Grid9List[i][j].y < splidPt.y)
                                {
                                    Grid9PlusList[5].Add(Grid9List[i][j]);
                                }
                            }
                            newLTag += 2;
                            break;
                        }
                    case 5:
                        {
                            //y >= -splidPt.y;
                            for (int j = 0; j < Grid9List[i].Count; j++)
                            {
                                if (Grid9List[i][j].y > -splidPt.y || Math.Abs(Grid9List[i][j].y + splidPt.y) < 1e-9)
                                {
                                    Grid9PlusList[7].Add(Grid9List[i][j]);
                                }
                                else if (Grid9List[i][j].y < -splidPt.y)
                                {
                                    Grid9PlusList[8].Add(Grid9List[i][j]);
                                }
                            }
                            newLTag += 2;
                            break;
                        }
                    case 7:
                        {
                            //y <= -splidPt.y;
                            for (int j = 0; j < Grid9List[i].Count; j++)
                            {
                                if (Grid9List[i][j].y < -splidPt.y || Math.Abs(Grid9List[i][j].y + splidPt.y) < 1e-9)
                                {
                                    Grid9PlusList[10].Add(Grid9List[i][j]);
                                }
                                else if (Grid9List[i][j].y > -splidPt.y)
                                {
                                    Grid9PlusList[11].Add(Grid9List[i][j]);
                                }
                            }
                            newLTag += 2;
                            break;
                        }
                }
            }
            return Grid9PlusList;
        }
        //将九宫格的2，4，6，8分别再分成2分，一共13个区域，为九宫格的进化版
        //3中死角并入2种，5并6,9并8,11并12
        public static List<List<Vertex>> Get9GridPlusAllPt(double R, double xPrec, double yPrec, double midLen)
        {
            List<List<Vertex>> Grid9List = Get9GridPt(R, xPrec, yPrec, midLen);
            List<List<Vertex>> Grid9PlusList = new List<List<Vertex>>();
            for (int i = 0; i < 13; i++)
            {
                List<Vertex> ptList = new List<Vertex>();
                Grid9PlusList.Add(ptList);
            }
            //计算九宫格2区域的分割点
            Vertex splidPt = Get9GridSmallHeight(midLen, R);
            //先把0区域拷贝到新列表中
            for (int i = 0; i < Grid9List[0].Count; i++)
            {
                Grid9PlusList[0].Add(Grid9List[0][i]);
            }
            //从1开始，2，4，6，8分割为2份
            int newLTag = 1;
            for (int i = 1; i < 9; i++)
            {
                //不分割列表
                if (i % 2 == 1)
                {
                    for (int j = 0; j < Grid9List[i].Count; j++)
                    {
                        Grid9PlusList[newLTag].Add(Grid9List[i][j]);
                    }
                    newLTag++;
                }
                switch (i)
                {
                    case 2:
                        {
                            //y <= splidPt.y;
                            for (int j = 0; j < Grid9List[i].Count; j++)
                            {
                                if (Grid9List[i][j].y < splidPt.y || Math.Abs(Grid9List[i][j].y - splidPt.y) < 1e-9)
                                {
                                    Grid9PlusList[2].Add(Grid9List[i][j]);
                                }
                                else if (Grid9List[i][j].y > splidPt.y)
                                {
                                    Grid9PlusList[3].Add(Grid9List[i][j]);
                                }
                            }
                            newLTag += 2;
                            break;
                        }
                    case 4:
                        {
                            //y >= splidPt.y;
                            for (int j = 0; j < Grid9List[i].Count; j++)
                            {
                                if (Grid9List[i][j].y > splidPt.y || Math.Abs(Grid9List[i][j].y - splidPt.y) < 1e-9)
                                {
                                    Grid9PlusList[5].Add(Grid9List[i][j]);
                                }
                                else if (Grid9List[i][j].y < splidPt.y)
                                {
                                    Grid9PlusList[6].Add(Grid9List[i][j]);
                                }
                            }
                            newLTag += 2;
                            break;
                        }
                    case 6:
                        {
                            //y >= -splidPt.y;
                            for (int j = 0; j < Grid9List[i].Count; j++)
                            {
                                if (Grid9List[i][j].y > -splidPt.y || Math.Abs(Grid9List[i][j].y + splidPt.y) < 1e-9)
                                {
                                    Grid9PlusList[8].Add(Grid9List[i][j]);
                                }
                                else if (Grid9List[i][j].y < -splidPt.y)
                                {
                                    Grid9PlusList[9].Add(Grid9List[i][j]);
                                }
                            }
                            newLTag += 2;
                            break;
                        }
                    case 8:
                        {
                            //y <= -splidPt.y;
                            for (int j = 0; j < Grid9List[i].Count; j++)
                            {
                                if (Grid9List[i][j].y < -splidPt.y || Math.Abs(Grid9List[i][j].y + splidPt.y) < 1e-9)
                                {
                                    Grid9PlusList[11].Add(Grid9List[i][j]);
                                }
                                else if (Grid9List[i][j].y > -splidPt.y)
                                {
                                    Grid9PlusList[12].Add(Grid9List[i][j]);
                                }
                            }
                            newLTag += 2;
                            break;
                        }
                }
            }
            //重新规划四个死角点
            //3中死角并入2种，5并6,9并8,11并12
            //得到13个区域的旋转角度列表
            List<double> rotateList = Grid9PlusRotateList(midLen, R);
            #region 从3区域中找出死角点并入区域2
            List<int> errList = new List<int>();
            for (int i = 0; i < Grid9PlusList[3].Count; i++)
            {
                Vertex tp = ComFunc.Rotate2DPtR(Grid9PlusList[3][i], rotateList[3]);
                //此点y > midlen，也就是在0和1区域之外
                if (Math.Abs(tp.y - midLen / 2) > 1e-9 && (tp.y > midLen / 2 || tp.y < -midLen / 2))
                {
                    errList.Add(i);
                }
            }
            //把死角点加入到2区域,先把2区域最后一点标记
            Grid9PlusList[2][Grid9PlusList[2].Count - 1].z = -1;
            for (int i = 0; i < errList.Count; i++)
            {
                Grid9PlusList[2].Add(Grid9PlusList[3][errList[i]]);
            }
            //再将区域3中的死角点删掉
            for (int i = 0; i < errList.Count; i++)
            {
                Grid9PlusList[3].RemoveAt(errList[i] - i);
            }
            #endregion
            #region 从5区域找到死角点并入区域6
            //清空死角列表
            errList.Clear();
            for (int i = 0; i < Grid9PlusList[5].Count; i++)
            {
                Vertex tp = ComFunc.Rotate2DPtR(Grid9PlusList[5][i], rotateList[5]);
                //此点y > midlen，也就是在0和1区域之外
                if (Math.Abs(tp.y - midLen / 2) > 1e-9 && (tp.y > midLen / 2 || tp.y < -midLen / 2))
                {
                    errList.Add(i);
                }
            }
            //把死角点加入到6区域
            Grid9PlusList[6][Grid9PlusList[6].Count - 1].z = -1;
            for (int i = 0; i < errList.Count; i++)
            {
                Grid9PlusList[6].Add(Grid9PlusList[5][errList[i]]);
            }
            //再将区域5中的死角点删掉
            for (int i = 0; i < errList.Count; i++)
            {
                Grid9PlusList[5].RemoveAt(errList[i] - i);
            }
            #endregion
            #region 从9区域找到死角点并入8
            //清空死角列表
            errList.Clear();
            for (int i = 0; i < Grid9PlusList[9].Count; i++)
            {
                Vertex tp = ComFunc.Rotate2DPtR(Grid9PlusList[9][i], rotateList[9]);
                //此点y > midlen，也就是在0和1区域之外
                if (Math.Abs(tp.y - midLen / 2) > 1e-9 && (tp.y > midLen / 2 || tp.y < -midLen / 2))
                {
                    errList.Add(i);
                }
            }
            //把死角点加入到8区域
            for (int i = 0; i < errList.Count; i++)
            {
                Grid9PlusList[8].Insert(i, Grid9PlusList[9][errList[i]]);
            }
            //8区域分界点做标记
            Grid9PlusList[8][errList.Count - 1].z = -1;
            //再将区域9中的死角点删掉
            for (int i = 0; i < errList.Count; i++)
            {
                Grid9PlusList[9].RemoveAt(errList[i] - i);
            }
            #endregion
            #region 从11区域找到死角点并入12
            //清空死角列表
            errList.Clear();
            for (int i = 0; i < Grid9PlusList[11].Count; i++)
            {
                Vertex tp = ComFunc.Rotate2DPtR(Grid9PlusList[11][i], rotateList[11]);
                //此点y > midlen，也就是在0和1区域之外
                if (Math.Abs(tp.y - midLen / 2) > 1e-9 && (tp.y > midLen / 2 || tp.y < -midLen / 2))
                {
                    errList.Add(i);
                }
            }
            //把死角点加入到12区域
            for (int i = 0; i < errList.Count; i++)
            {
                Grid9PlusList[12].Insert(i, Grid9PlusList[11][errList[i]]);
            }
            //12区域分界点做标记
            Grid9PlusList[12][errList.Count - 1].z = -1;
            //再将区域11中的死角点删掉
            for (int i = 0; i < errList.Count; i++)
            {
                Grid9PlusList[11].RemoveAt(errList[i] - i);
            }
            #endregion

            return Grid9PlusList;
        }

        //将九宫格的2，4，6，8分别再分成2分，一共13个区域，为九宫格的进化版
        public static List<List<Vertex>> Get9GridPlusPt(double R, double xPrec, double yPrec, double midLen)
        {
            List<List<Vertex>> Grid9List = Get9GridPt(R,xPrec,yPrec,midLen);
            List<List<Vertex>> Grid9PlusList = new List<List<Vertex>>();
            for (int i = 0; i < 13; i++)
            {
                List<Vertex> ptList = new List<Vertex>();
                Grid9PlusList.Add(ptList);
            }
            //计算九宫格2区域的分割点
            Vertex splidPt = Get9GridSmallHeight(midLen, R);
            //先把0区域拷贝到新列表中
            for (int i = 0; i < Grid9List[0].Count; i++)
			{
			    Grid9PlusList[0].Add(Grid9List[0][i]);
			}
            //从1开始，2，4，6，8分割为2份
            int newLTag = 1;
            for (int i = 1; i < 9; i++)
            {
                //不分割列表
                if (i % 2 == 1)
                {
                    for (int j = 0; j < Grid9List[i].Count; j++)
                    {
                        Grid9PlusList[newLTag].Add(Grid9List[i][j]);
                    }
                    newLTag++;
                }
                switch(i)
                {
                    case 2:
                        {
                            //y <= splidPt.y;
                            for (int j = 0; j < Grid9List[i].Count; j++)
                            {
                                if (Grid9List[i][j].y < splidPt.y || Math.Abs(Grid9List[i][j].y - splidPt.y) < 1e-9)
                                {
                                    Grid9PlusList[2].Add(Grid9List[i][j]);
                                }
                                else if (Grid9List[i][j].y > splidPt.y)
                                {
                                    Grid9PlusList[3].Add(Grid9List[i][j]);
                                }
                            }
                            newLTag += 2;
                            break; 
                        }
                    case 4:
                        {
                            //y >= splidPt.y;
                            for (int j = 0; j < Grid9List[i].Count; j++)
                            {
                                if (Grid9List[i][j].y > splidPt.y || Math.Abs(Grid9List[i][j].y - splidPt.y) < 1e-9)
                                {
                                    Grid9PlusList[5].Add(Grid9List[i][j]);
                                }
                                else if (Grid9List[i][j].y < splidPt.y)
                                {
                                    Grid9PlusList[6].Add(Grid9List[i][j]);
                                }
                            }
                            newLTag += 2;
                            break;
                        }
                    case 6:
                        {
                            //y >= -splidPt.y;
                            for (int j = 0; j < Grid9List[i].Count; j++)
                            {
                                if (Grid9List[i][j].y > -splidPt.y || Math.Abs(Grid9List[i][j].y + splidPt.y) < 1e-9)
                                {
                                    Grid9PlusList[8].Add(Grid9List[i][j]);
                                }
                                else if (Grid9List[i][j].y < -splidPt.y)
                                {
                                    Grid9PlusList[9].Add(Grid9List[i][j]);
                                }
                            }
                            newLTag += 2;
                            break;
                        }
                    case 8:
                        {
                            //y <= -splidPt.y;
                            for (int j = 0; j < Grid9List[i].Count; j++)
                            {
                                if (Grid9List[i][j].y < -splidPt.y || Math.Abs(Grid9List[i][j].y + splidPt.y) < 1e-9)
                                {
                                    Grid9PlusList[11].Add(Grid9List[i][j]);
                                }
                                else if (Grid9List[i][j].y > -splidPt.y)
                                {
                                    Grid9PlusList[12].Add(Grid9List[i][j]);
                                }
                            }
                            newLTag += 2;
                            break;
                        }
                }
            }
            return Grid9PlusList;
        }
        //计算01进化版九宫格列表的旋转角度列表
        //midlen是九宫格中间区域的宽度，即为0区域的宽度，常规设置为120
        //R是九宫格口径的一半，也就是工件的半径
        public static List<double> Grid901PlusRotateList(double midLen, double R)
        {
            List<double> resList = new List<double>();
            Vertex splitPt = Get9GridSmallHeight(midLen, R);
            resList.Add(0.0);//0
            resList.Add(-splitPt.z);//1
            resList.Add(-Math.PI / 2 + splitPt.z);//2
            resList.Add(-Math.PI / 2);//3
            resList.Add(-Math.PI / 2 - splitPt.z);//4
            resList.Add(-Math.PI + splitPt.z);//5
            resList.Add(-Math.PI);//6
            resList.Add(-Math.PI - splitPt.z);//7
            resList.Add(-1.5 * Math.PI + splitPt.z);//8
            resList.Add(-1.5 * Math.PI);//9
            resList.Add(-1.5 * Math.PI - splitPt.z);//10
            resList.Add(-2.0 * Math.PI + splitPt.z);//11
            return resList;
        }
        //计算进化版九宫格列表的旋转角度列表
        //midlen是九宫格中间区域的宽度，即为0区域的宽度，常规设置为120
        //R是九宫格口径的一半，也就是工件的半径
        public static List<double> Grid9PlusRotateList(double midLen, double R)
        {
            List<double> resList = new List<double>();
            Vertex splitPt = Get9GridSmallHeight(midLen, R);
            resList.Add(0.0);//0
            resList.Add(0.0);//1
            resList.Add( - splitPt.z);//2
            resList.Add(- Math.PI / 2 + splitPt.z);//3
            resList.Add(- Math.PI / 2);//4
            resList.Add(- Math.PI / 2 - splitPt.z);//5
            resList.Add(- Math.PI + splitPt.z);//6
            resList.Add(- Math.PI);//7
            resList.Add(- Math.PI - splitPt.z);//8
            resList.Add(- 1.5 * Math.PI + splitPt.z);//9
            resList.Add(- 1.5 * Math.PI);//10
            resList.Add(- 1.5 * Math.PI - splitPt.z);//11
            resList.Add(- 2.0 * Math.PI + splitPt.z);//12
            return resList;
        }
        //九宫格点的变换函数
        //oriPt:原始的点,gridIdx:点所位于的区域
        //midLen:车床最大加工范围，也就是九宫格的第一个区域（0区域）的宽度
        //sampleR:工件的口径的一半，其实也就是XExt的取值范围
        //tsfList进化版九宫格各区域旋转变换角度列表,列表可根据Grid9PlusRotateList获得，参数是midLen和sampleR
        //构造旋转角度列表，根据idx查找旋转角度，做旋转操作
        //如果有旋转过后的点超出了加工量程，返回false,否则返回true
        public static bool TsfGrid9PlusPt(ref Vertex resPt, Vertex oriPt, int gridIdx, List<double> tsfList, double midLen, double sampleR)
        {
            //若转换之后的点在圆外，或者在0和1区域之外的地方，返回false
            //转换
            resPt = ComFunc.Rotate2DPtR(oriPt, tsfList[gridIdx]);
            if (resPt.y > midLen / 2 || resPt.y < - midLen / 2)
            {
                return false;
            }
            return true;
        }
        //重新处理九宫格加工顺序
        //读取NC文件导入之后重新排序,3行一组，不包括特殊行
        public static List<string> Resort9GridStr(List<string> oriList)
        {
            List<string> resList = new List<string>();
            List<string> oriPtList = new List<string>();
            int sTag = -1;
            string tpStr = "";
            for (int i = 0; i < oriList.Count; i++)
            {
                int xpos = oriList[i].IndexOf('X');
                if (xpos != 0)
                {
                    //当前列为特殊special point行
                    sTag = i;
                    tpStr = oriList[i];
                }
                else
                {
                    oriPtList.Add(oriList[i]);
                }
            }
            //重新输出列表，3个一组
            for (int i = oriPtList.Count - 3; i >= 0; i = i - 3)
            {
                for (int j = 0; j < 3; j++)
                {
                    resList.Add(oriPtList[i + j]);
                }
            }
            //将特殊点插入
            if (sTag != -1)
            {
                resList.Insert(oriList.Count - sTag - 1, tpStr);
            }
            return resList;
        }
        public static List<Vertex> CXZToXYZ(List<Vertex> srcList, double zScale = 1.0, double zOffset = 0.0)
        {
            List<Vertex> xyzList = new List<Vertex>();
            if (srcList.Count == 0)
            {
                return xyzList;                
            }
            foreach (Vertex VT in srcList)
            {
                Vertex tempPt = new Vertex();
                tempPt.x = VT.y * Math.Cos(VT.x * Math.PI / 180);
                tempPt.y = VT.y * Math.Sin(VT.x * Math.PI / 180);
                tempPt.z = VT.z * zScale + zOffset;
                xyzList.Add(tempPt);
            }
            return xyzList;
        }
        public static bool NcFilePhrase(string filePath, List<Vertex> zList)
        {
            try
            {
                StreamReader sr = new System.IO.StreamReader(filePath);
                string line = "";
                zList.Clear();
                while ((line = sr.ReadLine()) != null)
                {
                    //找到了点数据
                    if (line.IndexOf('C') == 0)
                    {
                        //int cPos = line.IndexOf('C');
                        int xPos = line.IndexOf('X');
                        int zPos = line.IndexOf('Z');
                        Vertex tempVT = new Vertex();
                        double x, y, z;
                        bool pxRes = double.TryParse(line.Substring(1,xPos - 1), out x);
                        bool pyRes = double.TryParse(line.Substring(xPos + 1, zPos - xPos- 1), out y);
                        bool pzRes = double.TryParse(line.Substring(zPos + 1), out z);
                        if (pzRes == true && pxRes == true && pyRes == true)
                        {
                            tempVT.x = x;
                            tempVT.y = y;
                            tempVT.z = z;
                            zList.Add(tempVT);
                        }
                    }
                }
                sr.Close();
                return true;
            }
            catch (Exception)
            {
                //MessageBox.Show(err.Message);
                return false;
            }
        }
        //public static bool NCCXZFilePhrase(string filePath, List<Vertex> zList)
        //{

        //}
        public static bool XYZFilePhrase(string filePath, List<Vertex> zList)
        {
            try
            {
                StreamReader sr = new System.IO.StreamReader(filePath);
                string line = "";
                zList.Clear();
                line = sr.ReadLine();
                //确定分隔符号
                string gapStr = ComFunc.FindGapString(line);
                int pos1 = line.IndexOf(gapStr);
                int pos2 = line.IndexOf(gapStr, pos1 + 1);
                double x, y, z;
                bool is2D = true;
                if (pos2 != -1)//3D数据
                {
                    is2D = false;
                    bool pxRes = double.TryParse(line.Substring(0, pos1), out x);
                    bool pyRes = double.TryParse(line.Substring(pos1 + 1, pos2 - pos1 - 1), out y);
                    bool pzRes = double.TryParse(line.Substring(pos2 + 1), out z);
                    if (pxRes == true && pyRes == true && pzRes == true)
                    {
                        Vertex tempVT = new Vertex(x, y, z);
                        zList.Add(tempVT);
                    }
                }
                if (pos2 == -1)//2D数据
                {
                    bool pxRes = double.TryParse(line.Substring(0, pos1), out x);
                    bool pyRes = double.TryParse(line.Substring(pos1 + 1), out y);
                    if (pxRes == true && pyRes == true)
                    {
                        Vertex tempVT = new Vertex(x, y);
                        zList.Add(tempVT);
                    }
                }
                while ((line = sr.ReadLine()) != null)
                {
                    if (is2D)
                    {
                        pos1 = line.IndexOf(gapStr);
                        bool pxRes = double.TryParse(line.Substring(0, pos1), out x);
                        bool pyRes = double.TryParse(line.Substring(pos1 + 1), out y);
                        if (pxRes == true && pyRes == true)
                        {
                            Vertex tempVT = new Vertex(x, y);
                            zList.Add(tempVT);
                        }
                    }
                    else
                    {
                        pos1 = line.IndexOf(gapStr);
                        pos2 = line.IndexOf(gapStr, pos1 + 1);
                        bool pxRes = double.TryParse(line.Substring(0, pos1), out x);
                        bool pyRes = double.TryParse(line.Substring(pos1 + 1, pos2 - pos1 - 1), out y);
                        bool pzRes = double.TryParse(line.Substring(pos2 + 1), out z);
                        if (pxRes == true && pyRes == true && pzRes == true)
                        {
                            Vertex tempVT = new Vertex(x, y, z);
                            zList.Add(tempVT);
                        }
                    }
                }
                sr.Close();
                return true;
            }
            catch (Exception)
            {
                //MessageBox.Show(err.Message);
                return false;
            }
        }
        public static List<Vertex> GetSlimList(List<Vertex> oriList, int slimF)
        {
            List<Vertex> allList = new List<Vertex>();
            if (slimF == 1)
            {
                for (int i = 0; i < oriList.Count; i++)
                {
                    allList.Add(oriList[i]);
                }
                return allList;
            }
            if (oriList.Count == 0 || slimF <= 0)
            {
                return allList;
            }
            int listLen = oriList.Count;
            int x1 = 0, x2 = 0;
            double platZ = oriList[0].z;
            //将2个特征的X1，X2找出
            for (int i = 0; i < listLen; i++)
            {
                if (Math.Abs(oriList[i].z - platZ) > 1e-9)
                {
                    x1 = i;
                    break;
                }
            }
            for (int i = x1 + 1; i < listLen; i++)
            {
                if (Math.Abs(oriList[i].z - platZ) < 1e-9)
                {
                    x2 = i;
                    break;
                }
            }
            for (int i = 0; i < x1 - 1; i+= slimF)
            {
                allList.Add(oriList[i]);
            }
            for (int i = x1 - 1; i <= x2; i++)
            {
                allList.Add(oriList[i]);
            }
            for (int i = x2 + 1; i < listLen; i+= slimF)
            {
                allList.Add(oriList[i]);
            }
            return allList;
        }
        public static List<Vertex> GetArrList(int arrN, double arrT, List<Vertex> oriList)
        {
            List<Vertex> allList = new List<Vertex>();
            if (arrN <= 0 || arrT <=0)
            {
                return allList;
            }
            int listLen = oriList.Count;
            bool isXPlus = true;//表示X的取值方向
            if (oriList[0].x > oriList[1].x)
            {
                isXPlus = false;
            }
            if (arrN == 1)
            {
                for (int i = 0; i < listLen; i++)
                {
                    allList.Add(oriList[i]);
                }
                return allList;
            }
            if (arrN % 2 == 0)
            {
                //偶数个阵列，则原列表平移半个周期
                for (int i = 0; i < listLen; i++)
                {
                    oriList[i].x += arrT / 2;
                }
                //阵列
                int beginN = -arrN / 2;
                if (isXPlus)
                {
                    for (int i = beginN; i < -beginN; i++)
                    {
                        for (int j = 0; j < listLen; j++)
                        {
                            Vertex tp = new Vertex(oriList[j].x + arrT * i, oriList[j].y, oriList[j].z);
                            allList.Add(tp);
                        }
                    }                    
                }
                else
                {
                    for (int i = -beginN - 1; i >= beginN; i--)
                    {
                        for (int j = 0; j < listLen; j++)
                        {
                            Vertex tp = new Vertex(oriList[j].x + arrT * i, oriList[j].y, oriList[j].z);
                            allList.Add(tp);
                        }
                    }
                }
            }
            else
            {
                int beginN = -arrN / 2;
                if (isXPlus)
                {
                    for (int i = beginN; i <= -beginN; i++)
                    {
                        for (int j = 0; j < listLen; j++)
                        {
                            Vertex tp = new Vertex(oriList[j].x + arrT * i, oriList[j].y, oriList[j].z);
                            allList.Add(tp);
                        }
                    }                    
                }
                else
                {
                    for (int i = -beginN; i >= beginN; i--)
                    {
                        for (int j = 0; j < listLen; j++)
                        {
                            Vertex tp = new Vertex(oriList[j].x + arrT * i, oriList[j].y, oriList[j].z);
                            allList.Add(tp);
                        }
                    }
                }
            }
            //将重复的X删去
            if (allList.Count == 0)
            {
                return allList;
            }
            listLen = allList.Count;
            List<Vertex> backList = new List<Vertex>();
            Vertex cTp = allList[0];
            for (int i = 1; i < listLen - 1; i++)
            {
                if (allList[i].x != cTp.x)
                {
                    backList.Add(allList[i]);
                }
                cTp.x = allList[i].x;
                cTp.y = allList[i].y;
                cTp.z = allList[i].z;
            }
            return backList;
        }
        public static List<Vertex> GetMirrorList(List<Vertex> oriList)
        {
            List<Vertex> allList = new List<Vertex>();
            //bool hasZero = true;
            int oriListType = 0;
            if (oriList.Count == 0)
            {
                return allList;
            }
            if (oriList.Count == 1 && oriList[0].x == 0)
            {
                allList.Add(oriList[0]);
                return allList;
            }
            if (oriList.Count == 1)
            {
                Vertex temp = new Vertex(-oriList[0].x, oriList[0].y, oriList[0].z);
                if (oriList[0].x > 0)
                {
                    allList.Add(temp);
                    allList.Add(oriList[0]);
                    return allList;
                }
                else
                {
                    allList.Add(oriList[0]);
                    allList.Add(temp);
                    return allList;
                }
            }
            //判断有零还是无零
            int listLen = oriList.Count;
            //if (allList[0].x != 0 && allList[listLen - 1].x != 0)
            //{
            //    hasZero = false;
            //}
            //else
            //{
            //    hasZero = true;
            //}
            //判断原列表的类型，一共四种
            if (oriList[0].x < 0 && oriList[1].x > oriList[0].x)
            {
                oriListType = 0;
            }
            else if (oriList[0].x == 0 && oriList[1].x > oriList[0].x)
            {
                oriListType = 1;
            }
            else if (oriList[0].x > 0 && oriList[1].x < oriList[0].x)
            {
                oriListType = 2;
            }
            else if (oriList[0].x == 0 && oriList[1].x < oriList[0].x)
            {
                oriListType = 3;
            }
            switch (oriListType)
            {
                case 0:
                    {
                        for (int i = 0; i < listLen; i++)
                        {
                            allList.Add(oriList[i]);
                        }
                        for (int i = listLen - 2; i >= 0; i--)
                        {
                            Vertex tp = new Vertex(-oriList[i].x, oriList[i].y, oriList[i].z);
                            allList.Add(tp);
                        }
                        break;
                    }
                case 1:
                    {
                        for (int i = listLen - 1; i > 0; i--)
                        {
                            Vertex tp = new Vertex(-oriList[i].x, oriList[i].y, oriList[i].z);
                            allList.Add(tp);
                        }
                        for (int i = 0; i < listLen; i++)
                        {
                            allList.Add(oriList[i]);
                        }
                        break;
                    }
                case 2:
                    {
                        for (int i = 0; i < listLen; i++)
                        {
                            allList.Add(oriList[i]);
                        }
                        for (int i = listLen - 2; i >= 0; i--)
                        {
                            Vertex tp = new Vertex(-oriList[i].x, oriList[i].y, oriList[i].z);
                            allList.Add(tp);
                        }
                        break;
                    }
                case 3:
                    {
                        for (int i = listLen - 1; i > 0; i--)
                        {
                            Vertex tp = new Vertex(-oriList[i].x, oriList[i].y, oriList[i].z);
                            allList.Add(tp);
                        }
                        for (int i = 0; i < listLen; i++)
                        {
                            allList.Add(oriList[i]);
                        }
                        break;
                    }
            }
            return allList;
        }
        public static bool NcXZFilePhrase(string filePath, List<Vertex> zList)
        {
            try
            {
                StreamReader sr = new System.IO.StreamReader(filePath);
                string line = "";
                zList.Clear();
                while ((line = sr.ReadLine()) != null)
                {
                    string tpLine = line;
                    //删除空格
                    tpLine = tpLine.Replace(" ", "");
                    //找到了点数据
                    if (tpLine.IndexOf('X') == 0)
                    {
                        //int cPos = line.IndexOf('C');
                        int zPos = line.IndexOf('Z');
                        Vertex tempVT = new Vertex();
                        double x, z;
                        bool pxRes = double.TryParse(line.Substring(1, zPos - 1), out x);
                        bool pzRes = double.TryParse(line.Substring(zPos + 1), out z);
                        if (pzRes == true && pxRes == true)
                        {
                            tempVT.x = x;
                            tempVT.z = z;
                            zList.Add(tempVT);
                        }
                    }
                }
                sr.Close();
                return true;
            }
            catch (Exception)
            {
                //MessageBox.Show(err.Message);
                return false;
            }
        }
        public static bool MakePrfFile(List<Vertex> zList)
        {
            if (zList.Count == 0)
            {
                return false;
            }
            //计算X精度
            double xPrec = Math.Abs(zList[1].x - zList[0].x);
            //Z精度统一为1e-9
            string headTxt = "1 2\r\n" +
                             "Measur 0.00000000000e+000 PRF\r\n" +
                             "CX M " + zList.Count.ToString("e11") + " MM 1.00000000000e+000 D\r\n" +
                             "CZ M " + zList.Count.ToString("e11") + " MM 1.00000000000e-009 L\r\n" +
                             "EOR\r\n" +
                             "STYLUS_RADIUS 0.00000000000e+000 MM\r\n" +
                             "SPACING CX " + xPrec.ToString("e11") + "\r\n" + 
                             "MAP 1.000000e+000 CZ CZ 1.00000000000e+000 1.00000000000e+000\r\n" + 
                             "MAP 2.000000e+000 CZ CX 1.00000000000e+000 0.00000000000e+000\r\n" +
                             "COMMENT TH_DEVELOPMENT MEASUREMENT_CONDITIONS_STYLUS_TIP_RADIUS 2.00000000000e-003\r\n" +
                             "COMMENT TH_DEVELOPMENT MEASUREMENT_CONDITIONS_STYLUS_ARM_LENGTH 6.00000000000e+001\r\n" + 
                             "COMMENT TH_DEVELOPMENT MEASUREMENT_CONDITIONS_GAUGE_RANGE 1.25000000000e+001\r\n" + 
                             "COMMENT TH_DEVELOPMENT MEASUREMENT_CONDITIONS_CALIB_CONSTS_WERE_LINEAR 1.00000000000e+000\r\n" +
                             "EOR\r\n";
            //保存文件对话框
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "prf Files|*.prf";
            saveFileDialog1.Title = "保存PRF文件";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                string filePath = saveFileDialog1.FileName;
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                string temp = "";
                //写入文件头
                sw.Write(headTxt);
                int ptNum = zList.Count;
                for (int i = 0; i < ptNum; i++)
                {
                    temp = (zList[i].z * (1e9)).ToString("f0") + "\r\n";
                    sw.Write(temp);
                }
                sw.WriteLine("EOR");
                sw.WriteLine("EOF");
                string showTxt = "已将文件保存至" + filePath;
                MessageBox.Show(showTxt);
                sw.Close();
                fs.Close();
            }
            return true;
        }
        public static bool MakeModeFile(List<Vertex> zList)
        {
            if (zList.Count == 0)
            {
                return false;
            }
            //计算X长度
            double xLen = 0, maxX = zList[0].x, minX = zList[0].x;
            for (int i = 0; i < zList.Count; i++)
            {
                if (zList[i].x > maxX)
                {
                    maxX = zList[i].x;
                }
                if (zList[i].x < minX)
                {
                    minX = zList[i].x;
                }
            }
            xLen = maxX - minX;
            //文件头
            string headTxt = "1 2\r\n" + 
                             "Aspheric_Utility 1.000000e+000 MOD\r\n" +
                             "CX A " + zList.Count.ToString("e6") + " MM 1.000000e+000 D\r\n" +
                             "CZ A " + zList.Count.ToString("e6") + " MM 1.000000e+000 D\r\n" + 
                             "EOR\r\n" +
                             "FILTER_MODE NO_FILTER\r\n" + 
                             "FORM ASPHERIC\r\n" +
                             "ASSESSMENT_LENGTH " + xLen.ToString("e6") + " MM\r\n" +
                             "UMBER_MOD_POINTS " + zList.Count.ToString("e6") + "\r\n" +
                             "ASPHERIC_COEFF 0.000000e+000 MM -1.000000e+000\r\n" + 
                             "ASPHERIC_COEFF 0.000000e+000 MM -2.000000e+000\r\n" + 
                             "ASPHERIC_COEFF 0.000000e+000 MM -3.000000e+000\r\n" + 
                             "ASPHERIC_COEFF 0.000000e+000 MM -4.000000e+000\r\n" + 
                             "ASPHERIC_COEFF 0.000000e+000 MM -5.000000e+000\r\n" + 
                             "ASPHERIC_COEFF 0.000000e+000 MM -6.000000e+000\r\n" + 
                             "ASPHERIC_COEFF 0.000000e+000 MM -7.000000e+000\r\n" + 
                             "ASPHERIC_COEFF 0.000000e+000 MM -8.000000e+000\r\n" + 
                             "ASPHERIC_COEFF 0.000000e+000 MM -9.000000e+000\r\n" + 
                             "ASPHERIC_COEFF 0.000000e+000 MM -1.000000e+001\r\n" + 
                             "ASPHERIC_COEFF 0.000000e+000 MM -1.100000e+001\r\n" + 
                             "ASPHERIC_COEFF 0.000000e+000 MM -1.200000e+001\r\n" + 
                             "ASPHERIC_COEFF 0.000000e+000 MM -1.300000e+001\r\n" + 
                             "ASPHERIC_COEFF 0.000000e+000 MM -1.400000e+001\r\n" + 
                             "ASPHERIC_COEFF 0.000000e+000 MM -1.500000e+001\r\n" + 
                             "ASPHERIC_COEFF 0.000000e+000 MM -1.600000e+001\r\n" + 
                             "ASPHERIC_COEFF 0.000000e+000 MM -1.700000e+001\r\n" + 
                             "ASPHERIC_COEFF 0.000000e+000 MM -1.800000e+001\r\n" + 
                             "ASPHERIC_COEFF 0.000000e+000 MM -1.900000e+001\r\n" + 
                             "ASPHERIC_COEFF 0.000000e+000 MM -2.000000e+001\r\n" + 
                             "ASPHERIC_RADIUS 9.100000e+000 MM\r\n" + 
                             "ASPHERIC_K 0.000000e+000\r\n" + 
                             "EOR\r\n";
            //保存文件对话框
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "mod Files|*.mod";
            saveFileDialog1.Title = "保存MOD文件";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                string filePath = saveFileDialog1.FileName;
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                string temp = "";
                //写入文件头
                sw.Write(headTxt);
                int ptNum = zList.Count;
                for (int i = 0; i < ptNum; i++)
                {
                    temp = zList[i].x.ToString("e6") + "\r\n";
                    sw.Write(temp);
                }
                for (int i = 0; i < ptNum; i++)
                {
                    temp = zList[i].z.ToString("e6") + "\r\n";
                    sw.Write(temp);
                }
                sw.WriteLine("EOR");
                sw.WriteLine("EOF");
                string showTxt = "已将文件保存至" + filePath;
                MessageBox.Show(showTxt);
                sw.Close();
                fs.Close();
            }

            return true;
        }
        public static bool MakeFNLNcFile(fnlSegments fSeg, string filePath, string numData, ref string MainTxt)
        {
            int findTag = filePath.LastIndexOf("\\");
            if (findTag == -1)
            {
                return false;
            }
            string fileP = filePath.Substring(0, findTag);
            if (!Directory.Exists(fileP))
            {
                return false;
            }
            double xLead = 0;
            string cycStr = "";
            if (fSeg.SpdDir == "顺时针")
            {
                xLead = fSeg.PartSize + fSeg.LeadInX;
                cycStr = "M03";
            }
            else
            {
                xLead = -fSeg.PartSize - fSeg.LeadInX;
                cycStr = "M04";
            }
            string HeadStr = "( Cigit Version - Version 1.40 )\r\n"
                        + "( CREATED : " + DateTime.Now.DayOfWeek.ToString() + " " + DateTime.Now.ToString() + " )\r\n"//时间日期 
                        + "( ========== PART SURFACE INFO ========== )\r\n"
                        + "( NUMBER OF SEGMENTS: 1 )\r\n"
                        + "( SEGMENT 1: Line )"
                        + "( Width = " + fSeg.PartSize.ToString() + " mm )\r\n"
                        + "( Height = 0 mm )\r\n"
                        + "( --- RESET 5XX VARIABLES --- )\r\n"
                        + "#547 = 0\r\n"
                        + "\r\n"
                        + "( ========== SECTION - COMMANDS ========== )\r\n"
                        + "#501 = " + fSeg.TotalLoops.ToString() + "                      ( NUMBER OF LOOPS )\r\n"
                        + "#502 = " + fSeg.CutDepth.ToString() + "                    ( DEPTH OF CUT )\r\n"
                        + "#503 = " + fSeg.FeedRate.ToString() + "                      ( FEEDRATE )\r\n"
                        + "#504 = " + fSeg.SpindleSpd.ToString() + "				( SPINDLE SPEED )\r\n"
                        + "\r\n"
                        + "#506 = 0                      ( LOOP COUNT )\r\n"
                        + "#509 = 0                      ( FINISH CUT COUNT 1 )\r\n"
                        + "#547 = 0				( CUT OFFSET VARIABLE )\r\n"
                        + "\r\n"
                        + "G71 G01 G18 G40 G63 G90 G94 " + fSeg.WorkCoord +  "\r\n"
                        + "T0000                    ( DEACTIVATE ALL TOOL OFFSETS )\r\n"
                        + "\r\n"
                        + fSeg.CutCpst + "\r\n"
                        + "#547 = #547 - #502		( SUBTRACTS DEPTH OF CUT FROM CUT OFFSET VARIABLE)\r\n"
                        + "G52 Z[#547]                   ( INCREMENT LOCAL COORDINATE SYSTEM SETTING BY CUT OFFSET VARIABLE )\r\n"
                        + "G01 X" + xLead.ToString() + " F200                ( PARKING POSITION - X )\r\n"
                        + "Z" + fSeg.LeadInZ.ToString() + "                        ( PARKING POSITION - Z )\r\n"
                        + "\r\n"
                        + "Y0.0              ( SET Y AXIS TO ZERO )\r\n"
                        + cycStr + "S[#504]\r\n"
                        + "\r\n"
                        + "X" + xLead.ToString() + " Z" + fSeg.HZ.ToString() + " F100     ( FROM PARKING POSITION )\r\n"
                        + "( LEAD IN BLOCKS )\r\n"
                        + fSeg.JetNo + "\r\n"
                        + "X" + xLead.ToString() + " Z" + fSeg.HZ.ToString() + " F[#503]\r\n"
                        + "( CUTTING BLOCKS )\r\n";

            string EndStr = "( LEAD OUT BLOCKS )\r\n"
                        + "G04P.1\r\n"
                        + "Z" + fSeg.LeadOutZ.ToString() + "\r\n"
                        + "M29\r\n"
                        + "G94 G01 Z" + fSeg.LeadOutZ.ToString() + " F200                ( PARKING POSITION - Z )\r\n"
                        + "X" + xLead.ToString() + "                         ( PARKING POSITION - X )\r\n"
                        + "\r\n"
                        + "#506 = #506 + 1\r\n"
                        + "END 1\r\n"
                        + "G52 Z0.0 			( SHIFT THE LOCAL COORDINATE SYSTEM TO 0.0)\r\n"
                        + "G94 G01 Z" + fSeg.LeadOutZ.ToString() + " F200                ( PARKING POSITION - Z )\r\n"
                        + "X" + xLead.ToString() + "                         ( PARKING POSITION - X )\r\n"
                        + "\r\n"
                        + "M30                  ( RESET PROGRAM )\r\n";
            System.IO.FileStream fs = new FileStream(filePath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            sw.Write(HeadStr);
            sw.Write(numData);
            sw.Write(EndStr);
            sw.Close();
            fs.Close();
            return true;

        }
        public static bool MakeMainNcFile(CutSegments hcs, CutSegments cs, string filePath, ref string MainTxt)
        {
            int findTag = filePath.LastIndexOf("\\");
            if (findTag == -1)
            {
                return false;
            }
            string fileP = filePath.Substring(0, findTag);
            if (!Directory.Exists(fileP))
            {
                return false;
            }
            MainTxt = "( ============ SECTION - HEADER ====================== )\r\n( Cigit Version : 2.7 Build : 55 )\r\n( CREATED : "
                                + DateTime.Now.DayOfWeek.ToString() + " " + DateTime.Now.ToString() + " )\r\n"//时间日期                               
                                + "( OPERATOR : Cigit Operator )\r\n"
                                + "( SCRIPT: FTS Post.mpyx - MODIFIED: 2016/5/12 )\r\n\r\n"
                                + "( ============ SECTION - INITIALIZATION ============== )\r\n"
                                + "( ***** RESET ALL 5XX VARIABLES ******* )\r\n"
                                + "#500 = 500\r\n"
                                + "WHILE [#500LT599] DO 1\r\n"
                                + "#500 = #500 + 1\r\n"
                                + "#[#500] = 0\r\n"
                                + "END1\r\n"
                                + "#599 = 0\r\n"
                                + "#500 = 0\r\n"
                                + "M79							( DISABLE C AXIS MODE )\r\n"
                                + "\r\n"
                                + "( ============ SECTION - VARIABLE DECLARATION ======== )\r\n"
                                + "( ***** PROGRAM VARIABLES ******* )\r\n"
                                + "#500 = 0                        ( PASS COUNTER )\r\n"
                                + "#501 = 0.00                     ( USER INPUT - OVERALL LENGTH )\r\n"
                                + "#503 = 0.00                     ( USER INPUT - ZORIGIN=FLAT )\r\n"
                                + "#504 = 0.0                      ( USER INPUT - TOOL NOSE RADIUS FOR MACHINE COMP )\r\n"
                                + "#505 = 0.01536108				( POSITIVE FREEFORM DEPARTURE )\r\n"
                                + "#507 = " + cs.JetNo.Replace("M", "") + "                      ( MIST NUMBER )\r\n"//喷气号
                                + "#588 = -0.00340770				( NEGATIVE FREEFORM DEPARTURE )\r\n"
                                + "\r\n"
                                + "( ***** SEMI-FINISH CONSTANTS *** )\r\n"
                                + "#530 = " + hcs.SpdSpeed.ToString() + "	    			( RPM )\r\n"
                                + "#531 = " + hcs.TotalLoops.ToString() + "						( # OF CUTS )\r\n"
                                + "#532 = " + hcs.DepthOfCut.ToString() + "			( DOC )\r\n"
                                + "#533 = " + hcs.FeedRate.ToString() + "			( FEEDRATEConstantSpeed_G94 )\r\n"
                                + "#534 = 1					( 0 = NFTS OFF, 1 = NFTS ON )\r\n"
                                + "\r\n"
                                + "( ***** FINISH CUT CONSTANTS **** )\r\n"
                                + "#540 = " + cs.SpdSpeed.ToString() + "	    			( RPM )\r\n"
                                + "#541 = " + cs.TotalLoops.ToString() + "						( # OF CUTS )\r\n"
                                + "#542 = " + cs.DepthOfCut.ToString() + "			( DOC )\r\n"
                                + "#543 = " + cs.FeedRate.ToString() + "			( FEEDRATEConstantSpeed_G94 )\r\n"
                                + "#544 = 1					( 0 = NFTS OFF, 1 = NFTS ON )\r\n"
                                + "( #550 = TOTAL Z-STOCK REMOVAL )\r\n"
                                + "( #551 = TOTAL X-STOCK REMOVAL )\r\n"
                                + "( #552 = CURRENT DOC )\r\n"
                                + "( #553 = CURRENT FEEDRATE )\r\n"
                                + "( #554 = CURRENT NFTS STATUS )\r\n"
                                + "( #555 = CURRENT SPINDLE SPEED )\r\n"
                                + "\r\n"
                                + "( M60 : REFERENCE ON  - X AND C )\r\n"
                                + "( M61 : REFERENCE OFF - X AND C )\r\n"
                                + "( M62 : FTSN ON-TRACKING, FLOATING )\r\n"
                                + "( M63 : W ON AND REFERENCE )\r\n"
                                + "( M64 : FTS ON,NON TRACKING,HOLD ZERO POSITION )\r\n"
                                + "( M65 : FTS ON,TRACKING, ACTIVE )\r\n"
                                + "( M66 : SYNC WITH FTS READY )\r\n"
                                + "\r\n"
                                + "( ============ SECTION - COMMANDS ==================== )\r\n"
                                + "\r\n"
                                + "G01 G71 G90 G40 G18 " + cs.WorkCoord
                                + " T0000					    ( DEACTIVATE ALL TOOL OFFSETS )\r\n"
                                + "G94				    	( FEED PER MINUTE )\r\n"
                                + "G04 P.4					( DWELL )\r\n"
                                + "M01 						( OPTIONAL STOP )\r\n"
                                + "\r\n"
                                + "#550 = #501 + [#521*#522] + [#531*#532] + [#541*#542] + #503 + #504 - #505  ( TOTAL STOCK REMOVAL )\r\n"
                                + "\r\n"
                                + " ( ----- SEMI-FINISH CUT --- )\r\n"
                                + "\r\n"
                                + "#550 = #550 + #505   ( RETRACT TOOL BY FREEFORM DEPARTURE DISTANCE  )\r\n"
                                + "\r\n"
                                + "( CALL SUB ROUTINE - SEMI-FINISH CUT )\r\n"
                                + "IF[#500EQ#531] GOTO31\r\n"
                                + "\r\n"
                                + cs.CutCpst + "          ( ACTIVATE TOOL OFFSET )\r\n"
                                + "M05            ( ENSURE SPINDLE IS STOPPED COMPLETELY )\r\n"
                                + "M80            ( ORIENT C TO 0 )\r\n"
                                + "G09 C0.0       ( USER INPUT )\r\n"
                                + "G92 C0.0\r\n"
                                + "G04 P1         ( DWELL FOR SETTLING )\r\n"
                                + "X-0 C0 F500    ( MOVE X AND C TO START POSITION )\r\n"
                                + "G04 P5         ( DWELL FOR STOP )\r\n"
                                + "M60            ( REF X AND C                BIT 1 ON, DRY CONTACT ON )\r\n"
                                + "G04 P1         ( DWELL )\r\n"
                                + "M61            ( END REF X- AND C           BIT 1 OFF, DRY CONTACT OFF )\r\n"
                                + "G4P1           ( DWELL )\r\n"
                                + "( M63            ( W ON AND REFERENCE         BIT 2 ON, BIT 3 OFF ) )\r\n"
                                + "G04 P1         ( DWELL )\r\n"
                                + "M66            ( SYNC WITH FTS READY        WAIT FOR INPUT 1 TO TOGGLE )\r\n"
                                + "G04 P1         ( DWELL )\r\n"
                                + "M79            ( EXIT C-AXIS MODE )\r\n"
                                + "G04 P1         ( DWELL )\r\n"
                                + "WHILE[#500LT#531]DO2\r\n"
                                + cs.CutCpst + "          ( ACTIVATE TOOL OFFSET )\r\n"
                                + "#507 = " + cs.JetNo.Replace("M", "") + "                      ( MIST NUMBER )\r\n"//喷气号
                                + "#552 = #532			( ASSIGN DEPTH OF CUT )\r\n"
                                + "#553 = #533			( ASSIGN FEED RATE )\r\n"
                                + "#554 = #534			( SET NFTS TO OFF )\r\n"
                                + "#555 = #530			( ASSIGN SPINDLE SPEED )\r\n"
                                + "M98(" + hcs.NCFilePath + ")\r\n"//半精车
                                + "#500 = #500 + 1\r\n"
                                + "END 2\r\n"
                                + "N31\r\n"
                                + "\r\n"
                                + "#500 = 0                   ( RESET PASS COUNTER )\r\n"
                                + "#554 = 0\r\n"
                                + "G52                        ( RESET G52 OFFSETS )\r\n"
                                + "G04 P.4                    ( DWELL )\r\n"
                                + "T0000                      ( DEACTIVATE ALL TOOL OFFSETS )\r\n"
                                + "M01                        ( OPTIONAL STOP )\r\n"
                                + "\r\n"
                                + "( ----- FINISH CUT -------- )\r\n"
                                + "\r\n"
                                + "( CALL SUB ROUTINE - FINISH CUT )\r\n"
                                + "IF[#500EQ#541] GOTO41\r\n"
                                + "\r\n"
                                + cs.CutCpst + "          ( ACTIVATE TOOL OFFSET )\r\n"
                                + "M05            ( ENSURE SPINDLE IS STOPPED COMPLETELY )\r\n"
                                + "M80            ( ORIENT C TO 0 )\r\n"
                                + "G09 C0.0       ( USER INPUT )\r\n"
                                + "G92 C0.0\r\n"
                                + "G04 P1         ( DWELL FOR SETTLING )\r\n"
                                + "X-0 C0 F500    ( MOVE X AND C TO START POSITION )\r\n"
                                + "G04 P5         ( DWELL FOR STOP )\r\n"
                                + "M60            ( REF X AND C                BIT 1 ON, DRY CONTACT ON )\r\n"
                                + "G04 P1         ( DWELL )\r\n"
                                + "M61            ( END REF X- AND C           BIT 1 OFF, DRY CONTACT OFF )\r\n"
                                + "G4P1           ( DWELL )\r\n"
                                + "( M63            ( W ON AND REFERENCE         BIT 2 ON, BIT 3 OFF ) )\r\n"
                                + "G04 P1         ( DWELL )\r\n"
                                + "M66            ( SYNC WITH FTS READY        WAIT FOR INPUT 1 TO TOGGLE )\r\n"
                                + "G04 P1         ( DWELL )\r\n"
                                + "M79            ( EXIT C-AXIS MODE )\r\n"
                                + "G04 P1         ( DWELL )\r\n"
                                + "WHILE[#500LT#541]DO3\r\n"
                                + cs.CutCpst + "          ( ACTIVATE TOOL OFFSET )\r\n"
                                + "#507 = " + cs.JetNo.Replace("M", "") + "                      ( MIST NUMBER )\r\n"//喷气号
                                + "#552 = #542			( ASSIGN DEPTH OF CUT )\r\n"
                                + "#553 = #543			( ASSIGN FEED RATE )\r\n"
                                + "#554 = #544			( SET NFTS TO OFF )\r\n"
                                + "#555 = #540			( ASSIGN SPINDLE SPEED )\r\n"
                                + "M98(" + cs.NCFilePath + ")\r\n"//精车
                                + "#500 = #500 + 1\r\n"
                                + "END 3\r\n"
                                + "\r\n"
                                + "N41\r\n"
                                + "\r\n"
                                + "#500 = 0                   ( RESET PASS COUNTER )\r\n"
                                + "#554 = 0 \r\n"
                                + "G52                        ( RESET G52 OFFSETS )\r\n"
                                + "G04 P.4                    ( DWELL )\r\n"
                                + "T0000                      ( DEACTIVATE ALL TOOL OFFSETS )\r\n"
                                + "M01                        ( OPTIONAL STOP )\r\n"
                                + "( ============ SECTION - FOOTER ====================== )\r\n"
                                + "\r\n"
                                + "T0000					( CANCEL ALL TOOL OFFSETS )\r\n"
                                + "M05						( STOP SPINDLE )\r\n"
                                + "M30						( RESET PROGRAM )\r\n"
                                + "\r\n";
            System.IO.FileStream fs = new FileStream(filePath,FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            sw.Write(MainTxt);
            sw.Close();
            fs.Close();
            return true;
        }
        public static bool MakeMainRecNcFile(RecCutSegments ccs, RecCutSegments hcs, RecCutSegments cs, string filePath)
        {
            int findTag = filePath.LastIndexOf("\\");
            if (findTag == -1)
            {
                return false;
            }
            string fileP = filePath.Substring(0, findTag);
            string MainTxt = "";
            if (!Directory.Exists(fileP))
            {
                return false;
            }
            MainTxt = "( ============ SECTION - HEADER ====================== )\r\n"
                                + "( Cigit Version : 2.7 Build : 55 )\r\n"
                                + "( CREATED : "
                                + DateTime.Now.DayOfWeek.ToString() + " " + DateTime.Now.ToString() + " )\r\n"//时间日期                               
                                + "( OPERATOR : Cigit Operator ZhuGuoDong)\r\n"
                                + "( SCRIPT: SSS Post.mpyx - MODIFIED: 2016/5/12 )\r\n\r\n"
                                + "( ============ SECTION - INITIALIZATION ============== )\r\n"
                                + "( ***** RESET ALL 5XX VARIABLES ******* )\r\n"
                                + "#500 = 500\r\n"
                                + "WHILE [#500LT599] DO 1\r\n"
                                + "#500 = #500 + 1\r\n"
                                + "#[#500] = 0\r\n"
                                + "END1\r\n"
                                + "#599 = 0\r\n"
                                + "#500 = 0\r\n"
                                + "M79							( DISABLE C AXIS MODE )\r\n"
                                + "\r\n"
                                + "( ============ SECTION - VARIABLE DECLARATION ======== )\r\n"
                                + "\r\n"
                                + "( ***** PROGRAM VARIABLES ******* )\r\n"
                                + "#500 = 0                        ( PASS COUNTER )\r\n"
                                + "#501 = 0.00                     ( USER INPUT - OVERALL LENGTH )\r\n"
                                + "#503 = 0.00                     ( USER INPUT - ZORIGIN=FLAT )\r\n"
                                + "#504 = 0.0                      ( USER INPUT - TOOL NOSE RADIUS FOR MACHINE COMP )\r\n"
                                + "#505 = 0              ( POSITIVE FREEFORM DEPARTURE )\r\n"
                                + "#507 = " + cs.JetNo.Replace("M", "") + "                       ( MIST NUMBER )\r\n"
                                + "#588 = 0              ( NEGATIVE FREEFORM DEPARTURE )\r\n" //喷气号
                                + "\r\n"
                                + "( ***** ROUGH-FINISH CONSTANTS *** )\r\n"
                                + "#521 = " + ccs.TotalLoops.ToString() + "					( # OF CUTS )\r\n"
                                + "#522 = " + ccs.DepthOfCut.ToString("F8") + "			( DOC )\r\n"
                                + "#523 = 10					( FEEDRATE-G94 USER INPUT )\r\n"
                                + "#524 = 0					( 0 = NFTS OFF, 1 = NFTS ON )\r\n"
                                + "\r\n"
                                + "( ***** SEMI-FINISH CONSTANTS *** )\r\n"
                                + "#531 = " + hcs.TotalLoops.ToString() + "						( # OF CUTS )\r\n"
                                + "#532 = " + hcs.DepthOfCut.ToString("F8") + "			( DOC )\r\n"
                                + "#533 = 10					( FEEDRATE-G94 USER INPUT )\r\n"
                                + "#534 = 0					( 0 = NFTS OFF, 1 = NFTS ON )\r\n"
                                + "\r\n"
                                + "( ***** FINISH CUT CONSTANTS **** )\r\n"
                                + "#541 = " + cs.TotalLoops.ToString() + "						( # OF CUTS )\r\n"
                                + "#542 = " + cs.DepthOfCut.ToString("F8") + "			( DOC )\r\n"
                                + "#543 = 10					( FEEDRATE-G94 USER INPUT )\r\n"
                                + "#544 = 0					( 0 = NFTS OFF, 1 = NFTS ON )\r\n"
                                + "\r\n"
                                + "( #550 = TOTAL Z-STOCK REMOVAL )\r\n"
                                + "( #551 = TOTAL X-STOCK REMOVAL )\r\n"
                                + "( #552 = CURRENT DOC )\r\n"
                                + "( #553 = CURRENT FEEDRATE )\r\n"
                                + "( #554 = CURRENT NFTS STATUS )\r\n"
                                + "\r\n"
                                + "( ============ SECTION - COMMANDS ==================== )\r\n"
                                + "\r\n"
                                + "G01 G71 G90 G40 G18 " + cs.WorkCoord + "\r\n"
                                + "T0000					( DEACTIVATE ALL TOOL OFFSETS )\r\n"
                                + "G94					( FEED PER MINUTE )\r\n"
                                + "G04 P.4					( DWELL )\r\n"
                                + "M80                                 ( C AXIS ORIENTATION )\r\n"
                                + "G09 C0.0                            ( USER INPUT )\r\n"
                                + "G92 C0.0                            ( SET COORD SYS )\r\n"
                                + "M01 						( OPTIONAL STOP )\r\n"
                                + "\r\n"
                                + "#550 = #501 + [#521*#522] + [#531*#532] + [#541*#542] + #503 + #504 - #505     ( TOTAL STOCK REMOVAL )\r\n"
                                + "\r\n"
                                + "( ----- ROUGH-FINISH CUT --- )\r\n"
                                + "\r\n"
                                + "#550 = #550 + #505   ( RETRACT TOOL BY FREEFORM DEPARTURE DISTANCE  )\r\n"
                                + "\r\n"
                                + "( CALL SUB ROUTINE - SEMI-FINISH CUT )\r\n"
                                + "IF[#500EQ#521] GOTO21\r\n"
                                + "\r\n"
                                + "WHILE[#500LT#521]DO1\r\n"
                                + ccs.CutCpst + "               ( ACTIVATE TOOL OFFSET )\r\n"
                                + "#507 = " + ccs.JetNo.Replace("M", "") + "           ( MIST NUMBER )\r\n"
                                + "#552 = #522      ( ASSIGN DEPTH OF CUT )\r\n"
                                + "#553 = #523      ( ASSIGN FEED RATE )\r\n"
                                + "#554 = #524      ( SET NFTS TO OFF )\r\n"
                                + "M98(" + ccs.NCFilePath + ")\r\n"
                                + "#500 = #500 + 1\r\n"
                                + "END 1\r\n"
                                + "\r\n"
                                + "N21\r\n"
                                + "\r\n"
                                + "#500 = 0                   ( RESET PASS COUNTER )\r\n"
                                + "#554 = 0\r\n"
                                + "G52                        ( RESET G52 OFFSETS )\r\n"
                                + "G04 P.4                    ( DWELL )\r\n"
                                + "T0000                      ( DEACTIVATE ALL TOOL OFFSETS )\r\n"
                                + "M01                        ( OPTIONAL STOP )\r\n"
                                + "\r\n"
                                + "( ----- SEMI-FINISH CUT --- )\r\n"
                                + "\r\n"
                                + "#550 = #550 + #505   ( RETRACT TOOL BY FREEFORM DEPARTURE DISTANCE  )\r\n"
                                + "\r\n"
                                + "( CALL SUB ROUTINE - SEMI-FINISH CUT )\r\n"
                                + "IF[#500EQ#531] GOTO31\r\n"
                                + "\r\n"
                                + "WHILE[#500LT#531]DO2\r\n"
                                + hcs.CutCpst + "               ( ACTIVATE TOOL OFFSET )\r\n"
                                + "#507 = " + hcs.JetNo.Replace("M", "") + "           ( MIST NUMBER )\r\n"
                                + "#552 = #532      ( ASSIGN DEPTH OF CUT )\r\n"
                                + "#553 = #533      ( ASSIGN FEED RATE )\r\n"
                                + "#554 = #534      ( SET NFTS TO OFF )\r\n"
                                + "M98(" + hcs.NCFilePath + ")\r\n"
                                + "#500 = #500 + 1\r\n"
                                + "END 2\r\n"
                                + "\r\n"
                                + "N31\r\n"
                                + "\r\n"
                                + "#500 = 0                   ( RESET PASS COUNTER )\r\n"
                                + "#554 = 0\r\n"
                                + "G52                        ( RESET G52 OFFSETS )\r\n"
                                + "G04 P.4                    ( DWELL )\r\n"
                                + "T0000                      ( DEACTIVATE ALL TOOL OFFSETS )\r\n"
                                + "M01                        ( OPTIONAL STOP )\r\n"
                                + "\r\n"
                                + "( ----- FINISH CUT -------- )\r\n"
                                + "\r\n"
                                + "( CALL SUB ROUTINE - FINISH CUT )\r\n"
                                + "IF[#500EQ#541] GOTO41\r\n"
                                + "\r\n"
                                + "WHILE[#500LT#541]DO3\r\n"
                                + cs.CutCpst + "               ( ACTIVATE TOOL OFFSET  )\r\n"
                                + "#507 = " + cs.JetNo.Replace("M", "") + "           ( MIST NUMBER )\r\n"
                                + "#552 = #542      ( ASSIGN DEPTH OF CUT )\r\n"
                                + "#553 = #543      ( ASSIGN FEED RATE )\r\n"
                                + "#554 = #544      ( SET NFTS TO OFF )\r\n"
                                + "M98(" + cs.NCFilePath + ")\r\n"
                                + "#500 = #500 + 1\r\n"
                                + "END 3\r\n"
                                + "\r\n"
                                + "N41\r\n"
                                + "\r\n"
                                + "#500 = 0                   ( RESET PASS COUNTER )\r\n"
                                + "#554 = 0 \r\n"
                                + "G52                        ( RESET G52 OFFSETS )\r\n"
                                + "G04 P.4                    ( DWELL )\r\n"
                                + "T0000                      ( DEACTIVATE ALL TOOL OFFSETS )\r\n"
                                + "M01                        ( OPTIONAL STOP )\r\n"
                                + "( ============ SECTION - FOOTER ====================== )\r\n"
                                + "\r\n"
                                + "T0000      ( CANCEL ALL TOOL OFFSETS )\r\n"
                                + "M30      ( RESET PROGRAM )\r\n";
            System.IO.FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            sw.Write(MainTxt);
            sw.Close();
            fs.Close();
            return true;
        }
        public static bool MakeRecDataNCHE(RecCutSegments sg, double firstX, double Y, double firstZ, ref string headTxt, ref string endTxt)
        {
            headTxt = "G94 	   		( FEED RATE IN mm/min & NFTS OFF )\r\n"
                      + "#550 = #550 - #552					( CURRENT CUTTING OFFSET )\r\n"
                      + "G52 Z[#550]							( SET COORDINATE SYSTEM OFFSET )\r\n"
                      + "G01 X" + firstX.ToString("F8") + " Y" + Y.ToString("F8") + " Z" + (firstZ + sg.RPOfs).ToString("F8") + " F400				( PARKING POSITION - X )\r\n"
                      + "( LEAD IN BLOCKS )\r\n"
                      + "M[#507]							( MIST ON )\r\n";
            endTxt = "G04 P1             ( DWELL )\r\n"
                      + "M29                ( MIST OFF )\r\n"
                      + "( PARKING POSITION )\r\n"
                      + "G94 G01 Z5 F200\r\n"
                      + "X" + firstX.ToString("F8") + "\r\n"
                      + "( =============== SECTION - FOOTER =================== )\r\n"
                      + "M99 									( RETURN TO MAIN )\r\n";
            return true;
        }
        public static bool MakeRecDataNc(RecCutSegments ccs, RecCutSegments hcs, RecCutSegments cs,
            string ccDataStr, string hcDataStr, string cDataStr, double[] firstX, double[] firstZ, double Y, string filePath)
        {
            int findTag = filePath.LastIndexOf("\\");
            if (findTag == -1)
            {
                return false;
            }
            string ccsFileP = filePath.Substring(0, findTag);
            string hcsFileP = ccsFileP, csFileP = ccsFileP;
            string headTxt = "", endTxt = "";//前后STR
            if (!Directory.Exists(ccsFileP))
            {
                return false;
            }
            ccsFileP += ("\\" + ccs.NCFilePath);
            hcsFileP += ("\\" + hcs.NCFilePath);
            csFileP += ("\\" + cs.NCFilePath);

            //初车数据文件
            MakeRecDataNCHE(ccs, firstX[0], Y, firstZ[0], ref headTxt, ref endTxt);
            StreamWriter sw = new StreamWriter(ccsFileP, false, Encoding.Default);
            sw.Write(headTxt);
            //中间数据段
            //将中间数据段最后以回车结束
            ComFunc.MakeEndOneEnter(ccDataStr);
            sw.Write(ccDataStr);
            //结尾
            sw.Write(endTxt);
            sw.Close();

            //半精车数据文件
            MakeRecDataNCHE(hcs, firstX[1], Y, firstZ[1], ref headTxt, ref endTxt);
            sw = new StreamWriter(hcsFileP, false, Encoding.Default);
            sw.Write(headTxt);
            //中间数据段
            //将中间数据段最后以回车结束
            ComFunc.MakeEndOneEnter(hcDataStr);
            sw.Write(hcDataStr);
            //结尾
            sw.Write(endTxt);
            sw.Close();

            //精车数据文件
            MakeRecDataNCHE(cs, firstX[2], Y, firstZ[2], ref headTxt, ref endTxt);
            sw = new StreamWriter(csFileP, false, Encoding.Default);
            sw.Write(headTxt);
            //中间数据段
            //将中间数据段最后以回车结束
            ComFunc.MakeEndOneEnter(cDataStr);
            sw.Write(cDataStr);
            //结尾
            sw.Write(endTxt);
            sw.Close();
            return true;
        }
        public static bool MakeMainSSSNcFile(SSSCutSegments ccs, SSSCutSegments hcs, SSSCutSegments cs, string filePath)
        {
            int findTag = filePath.LastIndexOf("\\");
            if (findTag == -1)
            {
                return false;
            }
            string fileP = filePath.Substring(0, findTag);
            string MainTxt = "";
            if (!Directory.Exists(fileP))
            {
                return false;
            }
            MainTxt = "( ============ SECTION - HEADER ====================== )\r\n"
                                + "( Cigit Version : 2.7 Build : 55 )\r\n"
                                + "( CREATED : "
                                + DateTime.Now.DayOfWeek.ToString() + " " + DateTime.Now.ToString() + " )\r\n"//时间日期                               
                                + "( OPERATOR : Cigit Operator ZhuGuoDong)\r\n"
                                + "( SCRIPT: SSS Post.mpyx - MODIFIED: 2015/5/14 )\r\n\r\n"
                                + "( ============ SECTION - INITIALIZATION ============== )\r\n"
                                + "( ***** RESET ALL 5XX VARIABLES ******* )\r\n"
                                + "#500 = 500\r\n"
                                + "WHILE [#500LT599] DO 1\r\n"
                                + "#500 = #500 + 1\r\n"
                                + "#[#500] = 0\r\n"
                                + "END1\r\n"
                                + "#599 = 0\r\n"
                                + "#500 = 0\r\n"
                                + "M79							( DISABLE C AXIS MODE )\r\n"
                                + "\r\n"
                                + "( ============ SECTION - VARIABLE DECLARATION ======== )\r\n"
                                + "\r\n"
                                + "( ***** PROGRAM VARIABLES ******* )\r\n"
                                + "#500 = 0                        ( PASS COUNTER )\r\n"
                                + "#501 = 0.00                     ( USER INPUT - OVERALL LENGTH )\r\n"
                                + "#503 = 0.00                     ( USER INPUT - ZORIGIN=FLAT )\r\n"
                                + "#504 = 0.0                      ( USER INPUT - TOOL NOSE RADIUS FOR MACHINE COMP )\r\n"
                                + "#505 = 0.03514524              ( POSITIVE FREEFORM DEPARTURE )\r\n"
                                + "#507 = " + cs.JetNo.Replace("M", "") + "                       ( MIST NUMBER )\r\n"//喷气号
                                + "#588 = -0.01218929              ( NEGATIVE FREEFORM DEPARTURE )\r\n"
                                + "\r\n"
                                + "( ***** ROUGH CUT CONSTANTS ***** )\r\n"
                                + "#521 = " + ccs.TotalLoops.ToString() + "					( # OF CUTS )\r\n"
                                + "#522 = " + ccs.DepthOfCut.ToString() + "			( DOC )\r\n"
                                + "#523 = 10.0					( FEEDRATE-G94 )\r\n"
                                + "#524 = 0					( 0 = NFTS OFF, 1 = NFTS ON )\r\n"
                                + "\r\n"
                                + "( ***** SEMI-FINISH CONSTANTS *** )\r\n"
                                + "#531 = " + hcs.TotalLoops.ToString() + "						( # OF CUTS )\r\n"
                                + "#532 = " + hcs.DepthOfCut.ToString() + "			( DOC )\r\n"
                                + "#533 = 10					( FEEDRATE-G94 USER INPUT )\r\n"
                                + "#534 = 0					( 0 = NFTS OFF, 1 = NFTS ON )\r\n"
                                + "\r\n"
                                + "( ***** FINISH CUT CONSTANTS **** )\r\n"
                                + "#541 = " + cs.TotalLoops.ToString() + "						( # OF CUTS )\r\n"
                                + "#542 = " + cs.DepthOfCut.ToString() + "			( DOC )\r\n"
                                + "#543 = 10					( FEEDRATE-G94 USER INPUT )\r\n"
                                + "#544 = 0					( 0 = NFTS OFF, 1 = NFTS ON )\r\n"
                                + "\r\n"
                                + "( #550 = TOTAL Z-STOCK REMOVAL )\r\n"
                                + "( #551 = TOTAL X-STOCK REMOVAL )\r\n"
                                + "( #552 = CURRENT DOC )\r\n"
                                + "( #553 = CURRENT FEEDRATE )\r\n"
                                + "( #554 = CURRENT NFTS STATUS )\r\n"
                                + "\r\n"
                                + "( ============ SECTION - COMMANDS ==================== )\r\n"
                                + "\r\n"
                                + "G01 G71 G90 G40 G18 " + cs.WorkCoord + "\r\n"
                                + "T0000					( DEACTIVATE ALL TOOL OFFSETS )\r\n"
                                + "G94					( FEED PER MINUTE )\r\n"
                                + "G04 P.4					( DWELL )              ( PARKING POSITION - NO TOOL ACTIVE )\r\n"
                                + "M01 						( OPTIONAL STOP )\r\n"
                                + "\r\n"
                                + "#550 = #501 + [#521*#522] + [#531*#532] + [#541*#542] + #503 + #504 - #505     ( TOTAL STOCK REMOVAL )\r\n"
                                + "\r\n"
                                + "( ----- ROUGH CUT --------- )\r\n"
                                + "\r\n"
                                + "G04 P.4					( DWELL )\r\n"
                                + "M64					    	( NFTS OFF )\r\n"
                                + "\r\n"
                                + "#550 = #550 + #505   ( RETRACT TOOL BY FREEFORM DEPARTURE DISTANCE  )\r\n"
                                + "( CALL SUB ROUTINE - ROUGH CUT )\r\n"
                                + "IF[#500EQ#521] GOTO21\r\n"
                                + "M80                                 ( C AXIS ORIENTATION )\r\n"
                                + "G09 C0.0                            ( USER INPUT )\r\n"
                                + "G92 C0.0\r\n"
                                + "WHILE[#500LT#521]DO2\r\n"
                                + cs.CutCpst + "               ( ACTIVATE TOOL OFFSET )\r\n"
                                + "#507 = " + cs.JetNo.Replace("M", "") + "                      ( MIST NUMBER )\r\n"//喷气号\r\n"
                                + "#552 = #522			( ASSIGN DEPTH OF CUT )\r\n"
                                + "#553 = #523			( ASSIGN FEED RATE  )\r\n"
                                + "#554 = #524			( SET NFTS TO OFF  )\r\n"
                                + "M98(" + ccs.NCFilePath + ")\r\n"
                                + "#500 = #500 + 1\r\n"
                                + "END 2\r\n"
                                + "\r\n"
                                + "N21\r\n"
                                + "\r\n"
                                + "#500 = 0                   ( RESET PASS COUNTER )\r\n"
                                + "G52                        ( RESET G52 OFFSETS )\r\n"
                                + "G04 P.4                    ( DWELL )\r\n"
                                + "T0000                      ( DEACTIVATE ALL TOOL OFFSETS )              ( PARKING POSITION - NO TOOL ACTIVE )\r\n"
                                + "M01                        ( OPTIONAL STOP )\r\n"
                                + "\r\n"
                                + "( ----- SEMI-FINISH CUT --- )\r\n"
                                + "\r\n"
                                + "\r\n"
                                + "( CALL SUB ROUTINE - SEMI-FINISH CUT )\r\n"
                                + "IF[#500EQ#531] GOTO31\r\n"
                                + "\r\n"
                                + "M80                                 ( C AXIS ORIENTATION )\r\n"
                                + "G09 C0.0                            ( USER INPUT )\r\n"
                                + "G92 C0.0                            ( SET COORD SYS )\r\n"
                                + "WHILE[#500LT#531]DO3\r\n"
                                + cs.CutCpst + "               ( ACTIVATE TOOL OFFSET )\r\n"
                                + "#507 = " + cs.JetNo.Replace("M", "") + "           ( MIST NUMBER )\r\n"//喷气号
                                + "#552 = #532			( ASSIGN DEPTH OF CUT )\r\n"
                                + "#553 = #533			( ASSIGN FEED RATE )\r\n"
                                + "#554 = #534			( SET NFTS TO OFF )\r\n"
                                + "M98(" + hcs.NCFilePath + ")\r\n"//半精车
                                + "#500 = #500 + 1\r\n"
                                + "END 3\r\n"
                                + "\r\n"
                                + "N31\r\n"
                                + "\r\n"
                                + "#500 = 0                   ( RESET PASS COUNTER )\r\n"
                                + "#554 = 0\r\n"
                                + "G52                        ( RESET G52 OFFSETS )\r\n"
                                + "M79                        ( DISABLE C AXIS MODE )\r\n"
                                + "G04 P.4                    ( DWELL )\r\n"
                                + "T0000                      ( DEACTIVATE ALL TOOL OFFSETS )              ( PARKING POSITION - NO TOOL ACTIVE )\r\n"
                                + "M01                        ( OPTIONAL STOP )\r\n"
                                + "\r\n"
                                + "( ----- FINISH CUT -------- )\r\n"
                                + "\r\n"
                                + "( CALL SUB ROUTINE - FINISH CUT )\r\n"
                                + "IF[#500EQ#541] GOTO41\r\n"
                                + "\r\n"
                                + "M80                                 ( C AXIS ORIENTATION )\r\n"
                                + "G09 C0.0                            ( USER INPUT )\r\n"
                                + "G92 C0.0                            ( SET COORD SYS )\r\n"
                                + "WHILE[#500LT#541]DO1\r\n"
                                + cs.CutCpst + "               ( ACTIVATE TOOL OFFSET  )\r\n"
                                + "#507 = " + cs.JetNo.Replace("M", "") + "           ( MIST NUMBER )\r\n"//喷气号
                                + "#552 = #542			( ASSIGN DEPTH OF CUT )\r\n"
                                + "#553 = #543			( ASSIGN FEED RATE )\r\n"
                                + "#554 = #544			( SET NFTS TO OFF )\r\n"
                                + "M98(" + cs.NCFilePath + ")\r\n"//精车
                                + "#500 = #500 + 1\r\n"
                                + "END 1\r\n"
                                + "\r\n"
                                + "N41\r\n"
                                + "\r\n"
                                + "#500 = 0                   ( RESET PASS COUNTER )\r\n"
                                + "#554 = 0 \r\n"
                                + "G52                        ( RESET G52 OFFSETS )\r\n"
                                + "M79                        ( DISABLE C AXIS MODE )\r\n"
                                + "G04 P.4                    ( DWELL )\r\n"
                                + "T0000                      ( DEACTIVATE ALL TOOL OFFSETS )\r\n"
                                + "G53 Z0.0 F200              ( PARKING POSITION - NO TOOL ACTIVE )\r\n"
                                + "M01                        ( OPTIONAL STOP )\r\n"
                                + "\r\n"
                                + "( ============ SECTION - FOOTER ====================== )\r\n"
                                + "\r\n"
                                + "T0000					( CANCEL ALL TOOL OFFSETS )\r\n"
                                + "M05						( STOP SPINDLE )\r\n"
                                + "M30						( RESET PROGRAM )\r\n"
                                + "\r\n";
            System.IO.FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            sw.Write(MainTxt);
            sw.Close();
            fs.Close();
            return true;
        }
        public static void MakeSSSFileHE(SSSCutSegments sg, double firstZ, ref string headStr, ref string endStr)
        {
            headStr = "G94 	   		( FEED RATE IN mm/min & NFTS OFF )\r\n"
                                + "#550 = #550 - #552					( CURRENT CUTTING OFFSET )\r\n"
                                + "G52 Z[#550]							( SET COORDINATE SYSTEM OFFSET )\r\n"
                                + "G01 X" + sg.CutXStart.ToString("F6") + " F200				( PARKING POSITION - X )\r\n"
                                + "Z" + sg.RPOfs.ToString("F6") + "							( PARKING POSITION - Z )\r\n"
                                + "\r\n"
                                + "( LEAD IN BLOCKS )\r\n"
                                + "G01 C0.0 X18.1 F200\r\n"
                                + "Z" + (0.1 + firstZ).ToString("F8") + "\r\n"
                                + "Y0.0							( SET Y AXIS TO 0 )\r\n"
                                + "M[#507]							( MIST ON )\r\n"
                                + "G01 G93 F" + sg.CutTime.ToString("F6") + "\r\n";

            endStr = "C0.0\r\n"
                    + "G04 P1             ( DWELL )\r\n"
                    + "M29                ( MIST OFF )\r\n"
                    + "( PARKING POSITION )\r\n"
                    + "G94 G01 Z" + sg.RPOfs.ToString("F8") + " F200\r\n"
                    + "X" + sg.CutXStart.ToString("F8") + "\r\n"
                    + "( =============== SECTION - FOOTER =================== )\r\n"
                    + "M99\r\n";
        }
        public static bool MakeSSSDataNc(SSSCutSegments ccs, SSSCutSegments hcs, SSSCutSegments cs,
            string ccDataPath, string hcDataPath, string cDataPath, double firstZ, string filePath)
        {
            int findTag = filePath.LastIndexOf("\\");
            if (findTag == -1)
            {
                return false;
            }
            string ccsFileP = filePath.Substring(0, findTag);
            string hcsFileP = ccsFileP, csFileP = ccsFileP;
            string headTxt = "", endTxt = "";//前后STR
            if (!Directory.Exists(ccsFileP))
            {
                return false;
            }
            ccsFileP += ("\\" + ccs.NCFilePath);
            hcsFileP += ("\\" + hcs.NCFilePath);
            csFileP += ("\\" + cs.NCFilePath);

            //初车数据文件
            MakeSSSFileHE(ccs, firstZ, ref headTxt, ref endTxt);
            StreamWriter sw = new StreamWriter(ccsFileP, false, Encoding.Default);
            sw.Write(headTxt);
            //读取中间数据段,写入文件
            StreamReader sr = new StreamReader(ccDataPath, Encoding.Default);
            string tpOneLine = "";
            while ((tpOneLine = sr.ReadLine()) != null)
            {
                sw.WriteLine(tpOneLine);
            }
            sr.Close();
            //结尾
            sw.Write(endTxt);
            sw.Close();

            //半精车数据文件
            MakeSSSFileHE(hcs, firstZ, ref headTxt, ref endTxt);
            sw = new StreamWriter(hcsFileP, false, Encoding.Default);
            sw.Write(headTxt);
            //读取中间数据段,写入文件
            sr = new StreamReader(hcDataPath, Encoding.Default);
            tpOneLine = "";
            while ((tpOneLine = sr.ReadLine()) != null)
            {
                sw.WriteLine(tpOneLine);
            }
            sr.Close();
            //结尾
            sw.Write(endTxt);
            sw.Close();

            //精车数据文件
            MakeSSSFileHE(cs, firstZ, ref headTxt, ref endTxt);
            sw = new StreamWriter(csFileP, false, Encoding.Default);
            sw.Write(headTxt);
            //读取中间数据段,写入文件
            sr = new StreamReader(cDataPath, Encoding.Default);
            tpOneLine = "";
            while ((tpOneLine = sr.ReadLine()) != null)
            {
                sw.WriteLine(tpOneLine);
            }
            sr.Close();
            //结尾
            sw.Write(endTxt);
            sw.Close();
            return true;
        }
        
        public static bool MakeSFNc(CutSegments hcs, string filePath, ref string SFTxt)
        {
            int findTag = filePath.LastIndexOf("\\");
            if (findTag == -1)
            {
                return false;
            }
            string csFileP = filePath.Substring(0, findTag);
            if (!Directory.Exists(csFileP))
            {
                return false;
            }
            //csFileP += ("\\" + hcs.NCFilePath.Replace("-FtsSF", "-FtsFC"));
            string SpdDir = "";
            if (hcs.SpdDir == "逆时针")
            {
                SpdDir = "M04";
            }
            else
            {
                SpdDir = "M03";
            }
            SFTxt = "( Cigit Version : 2.7 Build : 55 )\r\n( CREATED : "
                                + DateTime.Now.DayOfWeek.ToString() + " " + DateTime.Now.ToString() + " )\r\n"//时间日期                               
                                + "( OPERATOR : Cigit Operator )\r\n"
                                + "( SCRIPT: FTS Post.mpyx - MODIFIED: 2016/5/12 )\r\n"
                                + "( Aperture Type : Circle )\r\n"
                                + "( Outer Aperture : " + (hcs.CutXStart * 2).ToString() + "\r\n"
                                + "( X Start and X End : " + hcs.CutXStart.ToString() + " mm, " + hcs.CutXEnd.ToString() + " mm )\r\n"
                                + "( LeadIn and LeadOut Distance, Increment : " + hcs.LeadInDis.ToString() + " mm, " + hcs.LeadOutDis.ToString() + " mm )\r\n"
                                + "( ========== SECTION - COMMANDS ========== )\r\n"
                                + "G94 M64				( FEED RATE IN mm/min & NFTS OFF )\r\n"
                                + "#550 = #550 - #552					( CURRENT CUTTING OFFSET )\r\n"
                                + "G52 Z[#550]							( SET COORDINATE SYSTEM OFFSET )\r\n"
                                + "G01 X" + (hcs.CutXStart + hcs.LeadInDis).ToString("F8") + " F200				( PARKING POSITION - X )\r\n"
                                + "Z" + hcs.RPOfs.ToString() + "						( PARKING POSITION - Z )\r\n"
                                + "\r\n"
                                + "M64         ( FTS ON, NONTRACKING        BIT 2 OFF, BIT 3 ON           )\r\n"
                                + "G04 P1        ( DWELL                                                    )\r\n"
                                + SpdDir + " S[#555]\r\n"
                                + "G01 X" + (hcs.CutXStart + hcs.LeadInDis).ToString("F8") + " F200				( PARKING POSITION - X )\r\n"
                                + "Z" + hcs.RPOfs.ToString("F8") + "						( PARKING POSITION - Z )\r\n"
                                + "( LEAD IN BLOCKS )\r\n"
                                + "X" + (hcs.CutXStart + hcs.LeadInDis).ToString("F8") +　" F200\r\n"
                                + "Z" + hcs.ZDepth.ToString() + "\r\n"
                                + "Y0.0							( SET Y AXIS TO 0 )\r\n"
                                + "M[#507]							( MIST ON )\r\n"
                                + "M65		( TRACKING ON )\r\n"
                                + "( CUTTING BLOCKS )\r\n"
                                + "X" + hcs.CutXStart.ToString("F8") + "Z0.00000000 F[#553]\r\n";
            //中间数据段
            string dataStr = "";
            double XSt = hcs.CutXStart;
            double deltaX = hcs.SurIncre;
            if (hcs.SpdDir == "逆时针")
            {
                XSt = -XSt;
                while (XSt < hcs.CutXEnd && Math.Abs(XSt - hcs.CutXEnd) > 1e-9)
                {
                    dataStr += ("X" + XSt.ToString("F8") + " Z0.00000000\r\n");
                    XSt += deltaX;
                }
                //最后一个点
                XSt = hcs.CutXEnd;
                dataStr += ("X" + XSt.ToString("F8") + " Z0.00000000\r\n");
            }
            else
            {
                deltaX = -deltaX;
                while (XSt > hcs.CutXEnd && Math.Abs(XSt - hcs.CutXEnd) > 1e-9)
                {
                    dataStr += ("X" + XSt.ToString("F8") + " Z0.00000000\r\n");
                    XSt += deltaX;
                }
                //最后一个点
                XSt = hcs.CutXEnd;
                dataStr += ("X" + XSt.ToString("F8") + " Z0.00000000\r\n");
            }
            string endStr =     "( LEAD OUT BLOCKS )\r\n"
                                + "X" + hcs.CutXEnd.ToString("F8") + " Z" + hcs.ZDepth.ToString() + "\r\n"
                                + "M62		( TRACKING OFF )\r\n"
                                + "X" + hcs.CutXEnd.ToString("F8") + " Z" + hcs.ZDepth.ToString() + " F[#553]\r\n"
                                + "G04P.4                   ( DWELL )\r\n"
                                + "M29                      ( MIST OFF )\r\n"
                                + "( PARKING POSITION )\r\n"
                                + "G94 G01 Z" + hcs.RPOfs.ToString("F8") + " F200\r\n"
                                + "X" + (hcs.CutXStart + hcs.LeadOutDis).ToString("F8") + "\r\n"
                                + "( =============== SECTION - FOOTER =================== )\r\n"
                                + "M64\r\n"
                                + "M99 									( RETURN TO MAIN )\r\n";
            //SFTxt += dataStr;
            //SFTxt += endStr;
            //System.IO.FileStream fs = new FileStream(filePath, FileMode.Truncate, FileAccess.ReadWrite);
            //System.IO.FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(filePath, false);
            sw.Write(SFTxt);
            sw.Write(dataStr);
            sw.Write(endStr);
            sw.Close();
            return true;
        }

        public static bool GetZValue(Point pt1, Point pt2, double xValue, ref double zValue)
        {
            zValue = 0;
            //斜率不存在
            if (pt2.X == pt1.X)
            {
                if (xValue != pt1.X)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //计算斜率
            double k = (pt2.Y - pt1.Y) / (pt2.X - pt1.X);
            zValue = k * (xValue - pt1.X) + pt1.Y;
            return true;
        }
    }
    public class FitFunc
    {
        public static bool FittingCircle(List<Vertex> ptList, ref Vertex circleInfo)
        {
            Vertex resCircle = new Vertex(0, 0, 0);
            if (ptList.Count < 3)
            {
                return false;
            }
            double X1 = 0, X2 = 0, X3 = 0;
            double Y1 = 0, Y2 = 0, Y3 = 0;
            double X1Y1 = 0, X1Y2 = 0, X2Y1 = 0;
            int listLen = ptList.Count;
            for (int i = 0; i < listLen; i++)
            {
                X1 = X1 + ptList[i].x;
                Y1 = Y1 + ptList[i].y;
                X2 = X2 + ptList[i].x * ptList[i].x;
                Y2 = Y2 + ptList[i].y * ptList[i].y;
                X3 = X3 + ptList[i].x * ptList[i].x * ptList[i].x;
                Y3 = Y3 + ptList[i].y * ptList[i].y * ptList[i].y;
                X1Y1 = X1Y1 + ptList[i].x * ptList[i].y;
                X1Y2 = X1Y2 + ptList[i].x * ptList[i].y * ptList[i].y;
                X2Y1 = X2Y1 + ptList[i].x * ptList[i].x * ptList[i].y;
            }
            double C, D, E, G, H, N;
            double a, b, c;
            N = ptList.Count;
            C = N * X2 - X1 * X1;
            D = N * X1Y1 - X1 * Y1;
            E = N * X3 + N * X1Y2 - (X2 + Y2) * X1;
            G = N * Y2 - Y1 * Y1;
            H = N * X2Y1 + N * Y3 - (X2 + Y2) * Y1;
            a = (H * D - E * G) / (C * G - D * D);
            b = (H * C - E * D) / (D * D - G * C);
            c = -(a * X1 + b * Y1 + X2 + Y2) / N;
            circleInfo.x = a / (-2);
            circleInfo.y = b / (-2);
            circleInfo.z = Math.Sqrt(a * a + b * b - 4 * c) / 2;
            return true;
        }
        public static bool FittingCircle(Queue<Vertex> ptList, ref Vertex circleInfo)
        {
            Vertex resCircle = new Vertex(0, 0, 0);
            if (ptList.Count < 3)
            {
                return false;
            }
            double X1 = 0, X2 = 0, X3 = 0;
            double Y1 = 0, Y2 = 0, Y3 = 0;
            double X1Y1 = 0, X1Y2 = 0, X2Y1 = 0;
            int listLen = ptList.Count;
            foreach(Vertex tp in ptList)
            {
                X1 = X1 + tp.x;
                Y1 = Y1 + tp.y;
                X2 = X2 + tp.x * tp.x;
                Y2 = Y2 + tp.y * tp.y;
                X3 = X3 + tp.x * tp.x * tp.x;
                Y3 = Y3 + tp.y * tp.y * tp.y;
                X1Y1 = X1Y1 + tp.x * tp.y;
                X1Y2 = X1Y2 + tp.x * tp.y * tp.y;
                X2Y1 = X2Y1 + tp.x * tp.x * tp.y;
            }
            //for (int i = 0; i < listLen; i++)
            //{
            //    X1 = X1 + ptList[i].x;
            //    Y1 = Y1 + ptList[i].y;
            //    X2 = X2 + ptList[i].x * ptList[i].x;
            //    Y2 = Y2 + ptList[i].y * ptList[i].y;
            //    X3 = X3 + ptList[i].x * ptList[i].x * ptList[i].x;
            //    Y3 = Y3 + ptList[i].y * ptList[i].y * ptList[i].y;
            //    X1Y1 = X1Y1 + ptList[i].x * ptList[i].y;
            //    X1Y2 = X1Y2 + ptList[i].x * ptList[i].y * ptList[i].y;
            //    X2Y1 = X2Y1 + ptList[i].x * ptList[i].x * ptList[i].y;
            //}
            double C, D, E, G, H, N;
            double a, b, c;
            N = ptList.Count;
            C = N * X2 - X1 * X1;
            D = N * X1Y1 - X1 * Y1;
            E = N * X3 + N * X1Y2 - (X2 + Y2) * X1;
            G = N * Y2 - Y1 * Y1;
            H = N * X2Y1 + N * Y3 - (X2 + Y2) * Y1;
            a = (H * D - E * G) / (C * G - D * D);
            b = (H * C - E * D) / (D * D - G * C);
            c = -(a * X1 + b * Y1 + X2 + Y2) / N;
            circleInfo.x = a / (-2);
            circleInfo.y = b / (-2);
            circleInfo.z = Math.Sqrt(a * a + b * b - 4 * c) / 2;
            return true;
        }
    }
    public class portsFunc
    {
        //private SerialPort sp = null;
        public static bool CheckCom(List<string> comList)
        {
            comList.Clear();
            //查找10个串口
            for (int i = 0; i < 15; i++)
            {
                try
                {
                    SerialPort spCheck = new SerialPort("COM" + (i + 1).ToString());
                    spCheck.Open();
                    spCheck.Close();
                    comList.Add("COM" + (i + 1).ToString());
                }
                catch (Exception)
                {
                    continue;
                }
            }
            if (comList.Count != 0)
            {
                return true;                
            }
            return false;
        }
        public static bool GetUSBPort(ref string portName)
        {
            string[] ports = SerialPort.GetPortNames();
            if (ports.Length <= 1)
            {
                return false;
            }
            //string registName = @"\Device\Serial3";
            string registName = @"\Device\Serial2";
            RegistryKey hkml = Registry.LocalMachine;
            //RegistryKey hardware = hkml.OpenSubKey("HARDWARE", true);
            RegistryKey hardware = hkml.OpenSubKey("HARDWARE");
            RegistryKey divicemap = hardware.OpenSubKey("DEVICEMAP");
            RegistryKey serialcomM = divicemap.OpenSubKey("SERIALCOMM");
            try
            {
                portName = serialcomM.GetValue(registName).ToString();

            }
            catch (Exception)
            {
                return false;
            }
            if (portName.IndexOf("COM") != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool OpenCom(SerialPort sp)
        {
            try
            {
                //SerialPort sp = new SerialPort(comName);
                sp.Open();
                return true;
            }
            catch (Exception err)
            {
                string errType = err.GetType().ToString();
                switch (errType)
                {
                    case "System.UnauthorizedAccessException":
                        MessageBox.Show("端口已被别的应用程序所占", "错误");
                        break;
                    case "System.ArgumentOutOfRangeException":
                        MessageBox.Show("ArgumentOutOfRangeException", "错误");
                        break;
                    case "System.ArgumentException":
                        MessageBox.Show("ArgumentException", "错误");
                        break;
                    case "System.IO.IOException":
                        MessageBox.Show("该端口无效", "错误");
                        break;
                    case "System.InvalidOperationException":
                        MessageBox.Show("InvalidOperationException", "错误");
                        break;
                    default:
                        break;
                }
                return false;
            }
        }
        public static bool GetComData(string comName, byte[] getData)
        {
            SerialPort sp = new SerialPort(comName);
            bool test = sp.IsOpen;
            if(sp.IsOpen)
            {
                getData = new byte[sp.BytesToRead];
                sp.Read(getData, 0, getData.Length);//读取所接收到的数据
                return true;
            }
            else
            {
                try
                {
                    SerialPort port =new SerialPort(comName);
                    port.Open();
                    port.Close();
                    MessageBox.Show("串口存在但是没打开");
                }
                catch (Exception)
                {
                    MessageBox.Show("串口不存在");
                }
                return false;
            }
        }
    }
    public class DBInfo
    {
        private string _DBServer;//数据库所在服务器名称
        private string _UserId;//用户ID
        private string _Psw;//用户密码
        private string _DBName;//数据库
        private string _TBName;
        public DBInfo(string dbServer, string user, string psw, string dbName, string dbTableName)
        {
            _DBServer = dbServer;
            _UserId = user;
            _Psw = psw;
            _DBName = dbName;
            _TBName = dbTableName;
        }
        public DBInfo()
        {
            _DBServer = "";
            _UserId = "";
            _Psw = "";
            _DBName = "";
            _TBName = "";
        }
        public string DBServer
        {
            get
            {
                return _DBServer;
            }
            set
            {
                if (value != this._DBServer)
                {
                    this._DBServer = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string UserId
        {
            get
            {
                return _UserId;
            }
            set
            {
                if (value != this._UserId)
                {
                    this._UserId = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Psw
        {
            get
            {
                return _Psw;
            }
            set
            {
                if (value != this._Psw)
                {
                    this._Psw = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string DBName
        {
            get
            {
                return _DBName;
            }
            set
            {
                if (value != this._DBName)
                {
                    this._DBName = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string TBName
        {
            get
            {
                return _TBName;
            }
            set
            {
                if (value != this._TBName)
                {
                    this._TBName = value;
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
    public class DBFunc
    {
        //初始化连接数据库字符串
        public static string InitConString(DBInfo dInfo)
        {
            string constr = "server=" + dInfo.DBServer + ";User Id=" + dInfo.UserId + ";password=" + dInfo.Psw + ";Database=" + dInfo.DBName;
            return constr;
        }
        public static bool AddFCToDB(DBInfo dinfo, CutSegments hsSeg, CutSegments sSeg)
        {
            //连接数据库
            string constr = "server=" + dinfo.DBServer + ";User Id=" + dinfo.UserId + ";password=" + dinfo.Psw + ";Database=" + dinfo.DBName;
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();
            //先查询到所有字段名
            //string sqlstr = "insert into " + dinfo.TBName + "";
            string sqlstr = "select column_name from information_schema.columns where table_name='" + dinfo.TBName + "' and COLUMN_NAME not like '%id%' and COLUMN_NAME not like '%date%';";
            MySqlCommand cmd = new MySqlCommand(sqlstr, mycon);
            List<string> names = new List<string>();
            try 
            {	        
                //调用 MySqlCommand  的 ExecuteReader 方法
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int len = reader.FieldCount;
                    names.Add(reader.GetString(0));
                    //string s2 = reader.GetString(1);
                    //Console.WriteLine(reader.GetInt32(0) + "," + myreader.GetString(1) + "," + myreader.GetString(2));
                    Trace.WriteLine("1");
                }
                //将DataReader关闭
                reader.Close();
                Trace.WriteLine("2");
            }
            catch
            {
                //关闭连接，抛出异常
                mycon.Close();
                throw;
            }
            //写入参数
            //得到所有字段名称串起来
            string colAll = "";
            for (int i = 0; i < names.Count; i++)
            {
                colAll += (names[i] + ",");
            }
            colAll = colAll.Substring(0, colAll.Length - 1);
            //初始化本次插入操作信息
            string initStr = "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 新记录', '说明:', '熊欣',"
                                + "'" + hsSeg.WorkCoord + "','" + hsSeg.CutCpst + "','" + hsSeg.SpdDir + "','" + hsSeg.JetNo //此行是半精车和精车通用的属性
                                 + "','" + hsSeg.SpdSpeed + "','" + hsSeg.FeedRate + "','" + hsSeg.SurIncre //半精车单独属性
                                 + "','" + hsSeg.CutXStart + "','" + hsSeg.CutXEnd + "','" + hsSeg.LeadInDis + "','" + hsSeg.LeadOutDis + "','" + hsSeg.RPOfs + "','" + hsSeg.RPFdr // 又是通用属性
                                  + "','" + hsSeg.TotalLoops + "','" + hsSeg.DepthOfCut//单独属性
                                   + "','" + hsSeg.ZDepth + "','" + hsSeg.CutR//共同属性
                                    + "','" + hsSeg.NCFilePath//单独属性
                                    + "','" + sSeg.SpdSpeed + "','" + sSeg.FeedRate + "','" + sSeg.SurIncre + "','" + sSeg.TotalLoops
                                    + "','" + sSeg.DepthOfCut + "','" + sSeg.NCFilePath + "'";
            sqlstr = "insert into " + dinfo.TBName + "(" + colAll + ") VALUES (" + initStr + ")";
            //cmd.Parameters.Clear();
            //cmd.CommandText = sqlstr;
            //cmd.Connection = mycon;
            MySqlCommand cmd1 = new MySqlCommand(sqlstr, mycon);
            int rowsReturned = cmd1.ExecuteNonQuery();
            mycon.Close();
            return true;
        }
    /// <summary>
    /// 准备执行一个命令
    /// </summary>
    /// <param name="cmd">sql命令</param>
    /// <param name="conn">OleDb连接</param>
    /// <param name="trans">OleDb事务</param>
    /// <param name="cmdType">命令类型例如 存储过程或者文本</param>
    /// <param name="cmdText">命令文本,例如:Select * from Products</param>
    /// <param name="cmdParms">执行命令的参数</param>
    private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText, MySqlParameter[] cmdParms)
    {

        if (conn.State != ConnectionState.Open)
            conn.Open();
        cmd.Connection = conn;
        cmd.CommandText = cmdText;

        if (trans != null)
            cmd.Transaction = trans;

        cmd.CommandType = cmdType;

        if (cmdParms != null)
        {
            foreach (MySqlParameter parm in cmdParms)
                cmd.Parameters.Add(parm);
        }
    }

}

    //金刚刀车床的加工点类
    [Serializable]
    public class DiaCutPoint
    {
        public Vertex dcPt { get; set; }
        //加工速度
        public string cutSpd { get; set; } 
    }
    //表达式类型
    public enum ExpType
    {
        num = 0,//数字
        oper = 1,//操作符
        par = 2,//括号
        neg = 3,//负号
        func = 4,//三角函数
        vari = 5,//变量
        unknow = -1
    }
    //表达式元素
    public class ExpElem
    {
        //数值
        public double num { get; set; }
        //操作符
        public char oper { get; set; }
        public char vari { get; set; }
        //函数名称
        public string func { get; set; }
        //元素类型
        public ExpType etype { get; set; }
        private bool _isValid;
        public bool isValid
        {
            get { return _isValid; }
            set
            {
                if (value != this._isValid)
                {
                    this._isValid = value;
                }
            }
        }
        public ExpElem(double loadVary)
        {
            num = loadVary;
            etype = ExpType.num;
            _isValid = true;
        }
        public ExpElem(ExpNode estr)
        {
            etype = estr.eType;
            _isValid = true;
            num = 0;
            oper = '\0';
            vari = '\0';
            func = "";
            switch (etype)
            {
                case ExpType.num://数字，小数之类
                    try
                    {
                        num = Convert.ToDouble(estr.eData);
                    }
                    catch (Exception)
                    {
                        _isValid = false;
                        //throw;
                    }
                    break;
                case ExpType.oper://+-*/^
                    try
                    {
                        oper = estr.eData[0];
                    }
                    catch (Exception)
                    {
                        _isValid = false;                        
                        //throw;
                    }
                    break;
                case ExpType.par:
                    try
                    {
                        oper = estr.eData[0];
                        if (oper != '(' && oper != ')')
                        {
                            _isValid = false;
                        }
                    }
                    catch (Exception)
                    {
                        _isValid = false;
                        //throw;
                    }
                    break;
                case ExpType.neg://-号
                    try
                    {
                        oper = estr.eData[0];
                        if (oper != '-')
                        {
                            _isValid = false;
                        }
                    }
                    catch (Exception)
                    {
                        _isValid = false;
                        //throw;
                    }
                    break;
                case ExpType.func://函数名称
                    func = estr.eData;
                    break;
                case ExpType.vari:
                    try
                    {
                        vari = estr.eData[0];
                        if (vari != 'x' && vari != 'y' && vari != 'z')
                        {
                            _isValid = false;
                        }
                    }
                    catch (Exception)
                    {
                        _isValid = false;
                        //throw;
                    }

                    break;
                case ExpType.unknow:
                    _isValid = false;
                    break;
                default:
                    break;
            }
        }
    }
    [Serializable]
    public class ExpNode
    {
        public string eData { get; set; }
        public ExpType eType { get; set; }
        public ExpNode()
        {
            eData = "";
            eType = ExpType.unknow;
        }
        public ExpNode(string _eData)
        {
            eData = _eData;
            eType = ExpType.unknow;
        }
        public ExpNode(string _eData, ExpType _eType)
        {
            eData = _eData;
            eType = _eType;
        }
        public ExpNode(string _eData, int _eType)
        {
            eData = _eData;
            eType = (ExpType)_eType;
        }
        public ExpNode(ExpNode _en)
        {
            eData = _en.eData;
            eType = _en.eType;
        }
    }
    [Serializable]
    public class Vertex
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }

        public Vertex()
        {
            x = 0;
            y = 0;
            z = 0;
        }

        public Vertex(double _x, double _y, double _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }
        public Vertex(Vertex _pt)
        {
            x = _pt.x;
            y = _pt.y;
            z = _pt.z;
        }
        public Vertex(double _x, double _y)
        {
            x = _x;
            y = _y;
            z = 0;
        }
        public static bool operator ==(Vertex v1, Vertex v2)
        {
            bool x1 = Math.Abs(v1.x - v2.x) < 1e-9;
            bool y1 = Math.Abs(v1.y - v2.y) < 1e-9;
            bool z1 = Math.Abs(v1.z - v2.z) < 1e-9;
            return (x1 && y1 && z1);
        }
        public static bool operator !=(Vertex v1, Vertex v2)
        {
            bool x1 = Math.Abs(v1.x - v2.x) < 1e-9;
            bool y1 = Math.Abs(v1.y - v2.y) < 1e-9;
            bool z1 = Math.Abs(v1.z - v2.z) < 1e-9;
            return !(x1 && y1 && z1);
        }
        public Vertex assign()
        {
            Vertex dstPt = new Vertex();
            dstPt.x = this.x;
            dstPt.y = this.y;
            dstPt.z = this.z;
            return dstPt;
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if ((obj.GetType().Equals(this.GetType())) == false)
            {
                return false;
            }
            Vertex tmp = (Vertex)obj;
            bool x1 = Math.Abs(this.x - tmp.x) < 1e-9;
            bool y1 = Math.Abs(this.y - tmp.y) < 1e-9;
            bool z1 = Math.Abs(this.z - tmp.z) < 1e-9;
            return (x1 && y1 && z1);
        }
        public override int GetHashCode()
        {
            return this.x.GetHashCode();
        }
    }

    public class CsvInfo
    {
        public int ptNum { get; set; }
        public double xResolution { get; set; }
        public double yResolution { get; set; }
        public double zResolution { get; set; }
        public double zMin { get; set; }
        public double zMax { get; set; }
        public Vertex midPt { get; set; }
        public CsvInfo()
        {
            ptNum = 0;
            xResolution = 0;
            yResolution = 0;
            zResolution = 0;
            zMin = 0;
            zMax = 0;
            midPt = new Vertex();
        }
        public CsvInfo(int _ptNum, double _xRsl, double _yRsl, double _zRsl, double _zMin, double _zMax, Vertex _midPt)
        {
            ptNum = _ptNum;
            xResolution = _xRsl;
            yResolution = _yRsl;
            zResolution = _zRsl;
            midPt = new Vertex();
            midPt.x = _midPt.x;
            midPt.y = _midPt.y;
            midPt.z = _midPt.z;
            zMin = _zMin;
            zMax = _zMax;
        }
    }
    [Serializable]
    //平行四边形类
    //成员变量m_lftBtAngle表示平行四边形左下角的角度，以弧度表示
    public class Parallelogram
    {
        public double m_len1;
        public double m_len2;
        public double m_lftBtAngle;
        public complex m_centPt;
        //以弧度表示角度
        public Parallelogram(double len1, double len2, double lftBtAngle)
        {
            m_len1 = len1;
            m_len2 = len2;
            m_lftBtAngle = lftBtAngle;
            m_centPt = new complex(0, 0);
        }
        public Parallelogram(double len1, double len2, double lftBtAngle, double centx, double centy)
        {
            m_len1 = len1;
            m_len2 = len2;
            m_lftBtAngle = lftBtAngle;
            m_centPt = new complex(centx, centy);
        }
        //以弧度表示角度
        public Parallelogram(double len1, double len2, double lftBtAngle, complex centpt)
        {
            m_len1 = len1;
            m_len2 = len2;
            m_lftBtAngle = lftBtAngle;
            m_centPt = centpt;
        }

        public List<complex> get4VertexCrd()
        {
            	//先求平行四边形的对角线长度
            double diaLine1 = Math.Sqrt(m_len1 * m_len1 + m_len2 * m_len2 - 2 * m_len1 * m_len2 * Math.Cos(m_lftBtAngle));
            double diaLine2 = Math.Sqrt(m_len1 * m_len1 + m_len2 * m_len2 - 2 * m_len1 * m_len2 * Math.Cos(Math.PI - m_lftBtAngle));
	        //计算四个角的向量，从左下角开始，逆时针
	        double theta1 = Math.Acos((m_len1 * m_len1 + diaLine2 * diaLine2 - m_len2 * m_len2) / m_len1 / diaLine2 / 2);
	        double theta2 = Math.Acos((m_len1 * m_len1 + diaLine1 * diaLine1 - m_len2 * m_len2) / m_len1 / diaLine1 / 2);
	        ////转化为角度看看
            List<complex> ptList = new List<complex>();
	        complex pt1 = complex.polar(diaLine2 / 2, Math.PI + theta1);
	        complex pt2 = complex.polar(diaLine1 / 2, 2 * Math.PI - theta2);
	        complex pt3 = complex.polar(diaLine2 / 2, theta1);
	        complex pt4 = complex.polar(diaLine1 / 2, Math.PI - theta2);
	        ptList.Add(pt3);
	        ptList.Add(pt4);
	        ptList.Add(pt1);
	        ptList.Add(pt2);
	        for (int i = 0; i < 4; i++)
	        {
                ptList[i].x += m_centPt.x;
                ptList[i].y += m_centPt.y;
	        }
            return ptList;
        }
        public static List<complex> ArrayPrl(complex basePt, int arrX, int arrY, double Tx, double Ty, double theta)
        {
            List<complex> resList = new List<complex>();
            complex cx = new complex(Tx, 0);
            complex cy = complex.polar(Ty, theta);
            for (int i = 0; i < arrY; i++)
            {
                for (int j = 0; j < arrX; j++)
                {
                    complex curTp = basePt + cx * j + cy * i;
                    resList.Add(curTp);
                }
            }
            return resList;
        }

    }

    [Serializable]
    public class complex
    {
        public double x;
        public double y;

        public complex()
        {
            x = 0;
            y = 0;
        }
        public complex(double _x)
        {
            x = _x;
            y = 0;
        }
        public complex(double _x, double _y)
        {
            x = _x;
            y = _y;
        }
        public static implicit operator complex(double _x)
        {
            return new complex(_x);
        }
        public static explicit operator double(complex _x)
        {
            return _x.x;
        }
        public static bool operator ==(complex lhs, complex rhs)
        {
            return ((double)lhs.x == (double)rhs.x) & ((double)lhs.y == (double)rhs.y);
        }
        public static bool operator !=(complex lhs, complex rhs)
        {
            return ((double)lhs.x != (double)rhs.x) | ((double)lhs.y != (double)rhs.y);
        }
        public static complex operator +(complex lhs)
        {
            return lhs;
        }
        public static complex operator -(complex lhs)
        {
            return new complex(-lhs.x, -lhs.y);
        }
        public static complex operator +(complex lhs, complex rhs)
        {
            return new complex(lhs.x + rhs.x, lhs.y + rhs.y);
        }
        public static complex operator -(complex lhs, complex rhs)
        {
            return new complex(lhs.x - rhs.x, lhs.y - rhs.y);
        }
        public static complex operator *(complex lhs, complex rhs)
        {
            return new complex(lhs.x * rhs.x - lhs.y * rhs.y, lhs.x * rhs.y + lhs.y * rhs.x);
        }
        public static complex operator /(complex lhs, complex rhs)
        {
            complex result = new complex();
            double e;
            double f;
            if (System.Math.Abs(rhs.y) < System.Math.Abs(rhs.x))
            {
                e = rhs.y / rhs.x;
                f = rhs.x + rhs.y * e;
                result.x = (lhs.x + lhs.y * e) / f;
                result.y = (lhs.y - lhs.x * e) / f;
            }
            else
            {
                e = rhs.x / rhs.y;
                f = rhs.y + rhs.x * e;
                result.x = (lhs.y + lhs.x * e) / f;
                result.y = (-lhs.x + lhs.y * e) / f;
            }
            return result;
        }
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj is byte)
                return Equals(new complex((byte)obj));
            if (obj is sbyte)
                return Equals(new complex((sbyte)obj));
            if (obj is short)
                return Equals(new complex((short)obj));
            if (obj is ushort)
                return Equals(new complex((ushort)obj));
            if (obj is int)
                return Equals(new complex((int)obj));
            if (obj is uint)
                return Equals(new complex((uint)obj));
            if (obj is long)
                return Equals(new complex((long)obj));
            if (obj is ulong)
                return Equals(new complex((ulong)obj));
            if (obj is float)
                return Equals(new complex((float)obj));
            if (obj is double)
                return Equals(new complex((double)obj));
            if (obj is decimal)
                return Equals(new complex((double)(decimal)obj));
            return base.Equals(obj);
        }
        //计算复数的模
        public double GetModulus()
        {
            return Math.Sqrt(this.x * this.x + this.y * this.y);
        }
        //计算复数的辐角
        public double GetArg()
        {
            if (Math.Abs(this.x) <1e-9 && Math.Abs(this.y) < 1e-9)
            {
                return 0;
            }
            double theta = Math.Acos(this.x / this.GetModulus());
            //这个是0度
            if (Math.Abs(theta) < 1e-9)
            {
                return 0;
            }
            if (this.y > 0)
            {
                return theta;
            }
            else 
            {
                return Math.PI * 2 - theta;
            }
        }
        //计算旋转一定角度之后的复数
        public complex RotateAngle(double theta)
        {
            complex cTheta = new complex(Math.Sin(theta), Math.Cos(theta));
            return this * cTheta;
        }
        //按照模、幅角的形式返回复数
        public static complex polar(double rho, double theta)
        {
            complex res = new complex(0, 0);
            //先处理theta值,当theta==0时
            if (Math.Abs(theta)<1e-9)
            {
                res.x = rho;
                return res;
            }
            double angle = theta - (int)(theta / Math.PI / 2) * Math.PI * 2;
            if (theta < 0)
            {
                angle += 2 * Math.PI;               
            }
            //幅角为90度
            if (Math.Abs(Math.PI / 2 - angle) < 1e-9)
            {
                res.x = 0;
                res.y = rho;
                return res;
            }
            //幅角为180度
            if (Math.Abs(Math.PI - angle) < 1e-9)
            {
                res.x = -rho;
                res.y = 0;
                return res;
            }
            //幅角为270度
            if (Math.Abs(Math.PI * 3 / 2 - angle) < 1e-9)
            {
                res.x = 0;
                res.y = -rho;
                return res;
            }
            //幅角为360度
            if (Math.Abs(Math.PI * 2 - angle) < 1e-9)
            {
                res.x = rho;
                res.y = 0;
                return res;
            }
            res.x = rho * Math.Cos(angle);
            res.y = rho * Math.Sin(angle);
            return res;
        }
    }    

    public sealed class Arrow : Shape
    {
        #region Dependency Properties

        public static readonly DependencyProperty X1Property = DependencyProperty.Register("X1", typeof(double), typeof(Arrow), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty Y1Property = DependencyProperty.Register("Y1", typeof(double), typeof(Arrow), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty X2Property = DependencyProperty.Register("X2", typeof(double), typeof(Arrow), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty Y2Property = DependencyProperty.Register("Y2", typeof(double), typeof(Arrow), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty HeadWidthProperty = DependencyProperty.Register("HeadWidth", typeof(double), typeof(Arrow), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty HeadHeightProperty = DependencyProperty.Register("HeadHeight", typeof(double), typeof(Arrow), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        #endregion

        #region CLR Properties

        [TypeConverter(typeof(LengthConverter))]
        public double X1
        {
            get { return (double)base.GetValue(X1Property); }
            set { base.SetValue(X1Property, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double Y1
        {
            get { return (double)base.GetValue(Y1Property); }
            set { base.SetValue(Y1Property, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double X2
        {
            get { return (double)base.GetValue(X2Property); }
            set { base.SetValue(X2Property, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double Y2
        {
            get { return (double)base.GetValue(Y2Property); }
            set { base.SetValue(Y2Property, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double HeadWidth
        {
            get { return (double)base.GetValue(HeadWidthProperty); }
            set { base.SetValue(HeadWidthProperty, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double HeadHeight
        {
            get { return (double)base.GetValue(HeadHeightProperty); }
            set { base.SetValue(HeadHeightProperty, value); }
        }

        #endregion

        #region Overrides

        protected override Geometry DefiningGeometry
        {
            get
            {
                // Create a StreamGeometry for describing the shape
                StreamGeometry geometry = new StreamGeometry();
                geometry.FillRule = FillRule.EvenOdd;

                using (StreamGeometryContext context = geometry.Open())
                {
                    InternalDrawArrowGeometry(context);
                }

                // Freeze the geometry for performance benefits
                geometry.Freeze();

                return geometry;
            }
        }

        #endregion

        #region Privates

        private void InternalDrawArrowGeometry(StreamGeometryContext context)
        {
            double theta = Math.Atan2(Y1 - Y2, X1 - X2);
            double sint = Math.Sin(theta);
            double cost = Math.Cos(theta);

            Point pt1 = new Point(X1, this.Y1);
            Point pt2 = new Point(X2, this.Y2);

            Point pt3 = new Point(
                X2 + (HeadWidth * cost - HeadHeight * sint),
                Y2 + (HeadWidth * sint + HeadHeight * cost));

            Point pt4 = new Point(
                X2 + (HeadWidth * cost + HeadHeight * sint),
                Y2 - (HeadHeight * cost - HeadWidth * sint));

            context.BeginFigure(pt1, true, false);
            context.LineTo(pt2, true, true);
            context.LineTo(pt3, true, true);
            context.LineTo(pt2, true, true);
            context.LineTo(pt4, true, true);
        }

        #endregion
    }
    public class fnlSegments : INotifyPropertyChanged
    {
        private string _WorkCoord;//工作坐标系
        private string _CutCpst;//刀补号
        private string _SpdDir;//主轴旋转方向
        private string _JetNo;//喷气号
        private double _PartSize;//工件半径
        private int _TotalLoops;//循环次数
        private double _CutDepth;//切深
        private double _FeedRate;//进给速度
        private double _SpindleSpd;//主轴转速
        private double _LeadInX;//Xleadin
        private double _LeadInZ;//Zleadin
        private double _LeadOutZ;//Zleadout
        private double _HZ;//等等Z
        private string _NCFilePath;
        
        public double PartSize
        {
            get
            {
                return _PartSize;
            }
            set
            {
                if (value != this._PartSize)
                {
                    this._PartSize = value;
                    NotifyPropertyChanged();
                }
            }
        }
        
        public int TotalLoops
        {
            get
            {
                return _TotalLoops;
            }
            set
            {
                if (value != this._TotalLoops)
                {
                    this._TotalLoops = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double SpindleSpd
        {
            get
            {
                return _SpindleSpd;
            }
            set
            {
                if (value != this._SpindleSpd)
                {
                    this._SpindleSpd = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double FeedRate
        {
            get
            {
                return _FeedRate;
            }
            set
            {
                if (value != this._FeedRate)
                {
                    this._FeedRate = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double CutDepth
        {
            get
            {
                return _CutDepth;
            }
            set
            {
                if (value != this._CutDepth)
                {
                    this._CutDepth = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string WorkCoord
        {
            get
            {
                return _WorkCoord;
            }
            set
            {
                if (value != this._WorkCoord)
                {
                    this._WorkCoord = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string CutCpst
        {
            get
            {
                return _CutCpst;
            }
            set
            {
                if (value != this._CutCpst)
                {
                    this._CutCpst = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string SpdDir
        {
            get
            {
                return _SpdDir;
            }
            set
            {
                if (value != this._SpdDir)
                {
                    this._SpdDir = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string JetNo
        {
            get
            {
                return _JetNo;
            }
            set
            {
                if (value != this._JetNo)
                {
                    this._JetNo = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double LeadInX
        {
            get
            {
                return _LeadInX;
            }
            set
            {
                if (value != this._LeadInX)
                {
                    this._LeadInX = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double LeadInZ
        {
            get
            {
                return _LeadInZ;
            }
            set
            {
                if (value != this._LeadInZ)
                {
                    this._LeadInZ = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double LeadOutZ
        {
            get
            {
                return _LeadOutZ;
            }
            set
            {
                if (value != this._LeadOutZ)
                {
                    this._LeadOutZ = value;
                    NotifyPropertyChanged();
                }
            }
        }
        
        public double HZ
        {
            get
            {
                return _HZ;
            }
            set
            {
                if (value != this._HZ)
                {
                    this._HZ = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string NCFilePath
        {
            get
            {
                return _NCFilePath;
            }
            set
            {
                if (value != this._NCFilePath)
                {
                    this._NCFilePath = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public fnlSegments()
        {
            _WorkCoord = "G59";
            _CutCpst = "T0101";
            _SpdDir = "顺时针";
            _JetNo = "M26";
            _PartSize = 40.0;
            _TotalLoops = 1;
            _CutDepth = 0.0;
            _FeedRate = 10.0;
            _SpindleSpd = 2000.0;
            _LeadInX = 1.0;
            _LeadInZ = 5.0;
            _LeadOutZ = 0.5;
            _HZ = 0.0043999;
            _NCFilePath = "Untitled.NC";
        }
        public fnlSegments(string _workCoord, string _cutCpst, string _spdDir, string _jetNo,
                            double _portSize, int _totalLoops, double _depthOfCut, double _feedRate, double _spindleSpd, double _leadInX, double _leadInZ,
                            double _leadOutZ, double _hs, string _ncFilePath)
        {
            _WorkCoord = _workCoord;
            _CutCpst = _cutCpst;
            _SpdDir = _spdDir;
            _JetNo = _jetNo;
            _PartSize = _portSize;
            _TotalLoops = _totalLoops;
            _CutDepth = _depthOfCut;
            _FeedRate = _feedRate;
            _SpindleSpd = _spindleSpd;
            _LeadInX = _leadInX;
            _LeadInZ = _leadInZ;
            _LeadOutZ = _leadOutZ;
            _HZ = _hs;
            _NCFilePath = _ncFilePath;
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
    public class CutSegments : INotifyPropertyChanged
    {
        private string _WorkCoord;
        private string _CutCpst;
        private string _SpdDir;
        private string _JetNo;
        private int _SpdSpeed;
        private double _FeedRate;
        private double _SurIncre;
        private double _CutXStart;
        private double _CutXEnd;
        private double _LeadInDis;
        private double _LeadOutDis;
        private int _RPOfs;
        private int _RPFdr;
        private int _TotalLoops;
        private double _DepthOfCut;
        private double _ZDepth;
        private double _CutR;
        private string _NCFilePath;
        public string WorkCoord
        {
            get
            {
                return _WorkCoord;
            }
            set
            {
                if (value != this._WorkCoord)
                {
                    this._WorkCoord = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string CutCpst
        {
            get
            {
                return _CutCpst;
            }
            set
            {
                if (value != this._CutCpst)
                {
                    this._CutCpst = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string SpdDir
        {
            get
            {
                return _SpdDir;
            }
            set
            {
                if (value != this._SpdDir)
                {
                    this._SpdDir = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string JetNo
        {
            get
            {
                return _JetNo;
            }
            set
            {
                if (value != this._JetNo)
                {
                    this._JetNo = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public int SpdSpeed
        {
            get
            {
                return _SpdSpeed;
            }
            set
            {
                if (value != this._SpdSpeed)
                {
                    this._SpdSpeed = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double FeedRate
        {
            get
            {
                return _FeedRate;
            }
            set
            {
                if (value != this._FeedRate)
                {
                    this._FeedRate = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double SurIncre
        {
            get
            {
                return _SurIncre;
            }
            set
            {
                if (value != this._SurIncre)
                {
                    this._SurIncre = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double CutXStart
        {
            get
            {
                return _CutXStart;
            }
            set
            {
                if (value != this._CutXStart)
                {
                    this._CutXStart = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double CutXEnd
        {
            get
            {
                return _CutXEnd;
            }
            set
            {
                if (value != this._CutXEnd)
                {
                    this._CutXEnd = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double LeadInDis
        {
            get
            {
                return _LeadInDis;
            }
            set
            {
                if (value != this._LeadInDis)
                {
                    this._LeadInDis = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double LeadOutDis
        {
            get
            {
                return _LeadOutDis;
            }
            set
            {
                if (value != this._LeadOutDis)
                {
                    this._LeadOutDis = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public int RPOfs
        {
            get
            {
                return _RPOfs;
            }
            set
            {
                if (value != this._RPOfs)
                {
                    this._RPOfs = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public int RPFdr
        {
            get
            {
                return _RPFdr;
            }
            set
            {
                if (value != this._RPFdr)
                {
                    this._RPFdr = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public int TotalLoops
        {
            get
            {
                return _TotalLoops;
            }
            set
            {
                if (value != this._TotalLoops)
                {
                    this._TotalLoops = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double DepthOfCut
        {
            get
            {
                return _DepthOfCut;
            }
            set
            {
                if (value != this._DepthOfCut)
                {
                    this._DepthOfCut = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public double ZDepth
        {
            get
            {
                return _ZDepth;
            }
            set
            {
                if (value != this._ZDepth)
                {
                    this._ZDepth = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public double CutR
        {
            get
            {
                return _CutR;
            }
            set
            {
                if (value != this._CutR)
                {
                    this._CutR = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string NCFilePath
        {
            get
            {
                return _NCFilePath;
            }
            set
            {
                if (value != this._NCFilePath)
                {
                    this._NCFilePath = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public CutSegments()
        {
            _WorkCoord = "G59";
            //_WorkCoord = 0;
            _CutCpst = "T0101";
            _SpdDir = "顺时针";
            _JetNo = "M26";
            _SpdSpeed = 2000;
            _FeedRate = 10.0;
            _SurIncre = 0.005;
            _CutXStart = 25;
            _CutXEnd = 0;
            _LeadInDis = 0.1;
            _LeadOutDis = 0.1;
            _RPOfs = 9;
            _RPFdr = 200;
            _TotalLoops = 1;
            _DepthOfCut = 0;
            _ZDepth = 0.25;
            _CutR = 0.4;
            _NCFilePath = "Untitled-FtsSF.NC";
        }
        public CutSegments(string _workCoord, string _cutCpst, string _spdDir, string _jetNo,
                            int _spdSpeed, double _feedRate, double _surIncre, double _cutXStart, double _cutXEnd,
                            double _leadInDis, double _leadOutDis, int _rpofs, int _rpfdr, int _totalLoops, double _depthOfCut, double _zdepth, double _cutr, string _ncFilePath)
        {
            _WorkCoord = _workCoord;
            _CutCpst = _cutCpst;
            _SpdDir = _spdDir;
            _JetNo = _jetNo;
            _SpdSpeed = _spdSpeed;
            _FeedRate = _feedRate;
            _SurIncre = _surIncre;
            _CutXStart = _cutXStart;
            _CutXEnd = _cutXEnd;
            _LeadInDis = _leadInDis;
            _LeadOutDis = _leadOutDis;
            _RPOfs = _rpofs;
            _RPFdr = _rpfdr;
            _TotalLoops = _totalLoops;
            _DepthOfCut = _depthOfCut;
            _ZDepth = _zdepth;
            _CutR = _cutr;
            _NCFilePath = _ncFilePath;
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
    public class SSSCutSegments : INotifyPropertyChanged
    {
        private string _WorkCoord;//主轴旋转方向
        private string _CutCpst;//刀补号
        private string _JetNo;//喷气号
        private double _CutXStart;//起始位置
        private int _RPOfs;//安全距离
        private int _TotalLoops;//加工循环次数
        private double _DepthOfCut;//每次切深
        private double _CutTime;//每次加工时间
        private string _NCFilePath;//NC文件
        public string WorkCoord
        {
            get
            {
                return _WorkCoord;
            }
            set
            {
                if (value != this._WorkCoord)
                {
                    this._WorkCoord = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string CutCpst
        {
            get
            {
                return _CutCpst;
            }
            set
            {
                if (value != this._CutCpst)
                {
                    this._CutCpst = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string JetNo
        {
            get
            {
                return _JetNo;
            }
            set
            {
                if (value != this._JetNo)
                {
                    this._JetNo = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public double CutXStart
        {
            get
            {
                return _CutXStart;
            }
            set
            {
                if (value != this._CutXStart)
                {
                    this._CutXStart = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public int RPOfs
        {
            get
            {
                return _RPOfs;
            }
            set
            {
                if (value != this._RPOfs)
                {
                    this._RPOfs = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public int TotalLoops
        {
            get
            {
                return _TotalLoops;
            }
            set
            {
                if (value != this._TotalLoops)
                {
                    this._TotalLoops = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double DepthOfCut
        {
            get
            {
                return _DepthOfCut;
            }
            set
            {
                if (value != this._DepthOfCut)
                {
                    this._DepthOfCut = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double CutTime
        {
            get
            {
                return _CutTime;
            }
            set
            {
                if (value != this._CutTime)
                {
                    this._CutTime = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string NCFilePath
        {
            get
            {
                return _NCFilePath;
            }
            set
            {
                if (value != this._NCFilePath)
                {
                    this._NCFilePath = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public SSSCutSegments()
        {
            _WorkCoord = "G59";
            _CutCpst = "T0101";
            _JetNo = "M26";
            _CutXStart = 25;
            _RPOfs = 9;
            _TotalLoops = 1;
            _DepthOfCut = 0.1;
            _CutTime = 0.001;
            _NCFilePath = "Untitled-SSSSF.NC";
        }
        public SSSCutSegments(string _workCoord, string _cutCpst, string _jetNo, double _cutXStart, 
                               int _rpofs, int _totalLoops, double _depthOfCut, double _cutTime, string _ncFilePath)
        {
            _WorkCoord = _workCoord;
            _CutCpst = _cutCpst;
            _JetNo = _jetNo;
            _CutXStart = _cutXStart;
            _RPOfs = _rpofs;
            _TotalLoops = _totalLoops;
            _DepthOfCut = _depthOfCut;
            _CutTime = _cutTime;
            _NCFilePath = _ncFilePath;
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
    public class RecCutSegments : INotifyPropertyChanged
    {
        private string _WorkCoord;//工作坐标系
        private string _CutCpst;//刀补号
        private string _JetNo;//喷气号
        private double _RPOfs;//进刀安全距离
        private int _TotalLoops;//加工循环次数
        private double _DepthOfCut;//每次切深
        //private double _CutTime;//每次加工时间
        private string _NCFilePath;//NC文件
        public string WorkCoord
        {
            get
            {
                return _WorkCoord;
            }
            set
            {
                if (value != this._WorkCoord)
                {
                    this._WorkCoord = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string CutCpst
        {
            get
            {
                return _CutCpst;
            }
            set
            {
                if (value != this._CutCpst)
                {
                    this._CutCpst = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string JetNo
        {
            get
            {
                return _JetNo;
            }
            set
            {
                if (value != this._JetNo)
                {
                    this._JetNo = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public double RPOfs
        {
            get
            {
                return _RPOfs;
            }
            set
            {
                if (value != this._RPOfs)
                {
                    this._RPOfs = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public int TotalLoops
        {
            get
            {
                return _TotalLoops;
            }
            set
            {
                if (value != this._TotalLoops)
                {
                    this._TotalLoops = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double DepthOfCut
        {
            get
            {
                return _DepthOfCut;
            }
            set
            {
                if (value != this._DepthOfCut)
                {
                    this._DepthOfCut = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string NCFilePath
        {
            get
            {
                return _NCFilePath;
            }
            set
            {
                if (value != this._NCFilePath)
                {
                    this._NCFilePath = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public RecCutSegments()
        {
            _WorkCoord = "G59";
            _CutCpst = "T0101";
            _JetNo = "M26";
            _RPOfs = 2;
            _TotalLoops = 1;
            _DepthOfCut = 0.1;
            //_CutTime = 0.001;
            _NCFilePath = "Untitled-RecSF.NC";
        }
        public RecCutSegments(string _workCoord, string _cutCpst, string _jetNo,
                               double _rpofs, int _totalLoops, double _depthOfCut, string _ncFilePath)
        {
            _WorkCoord = _workCoord;
            _CutCpst = _cutCpst;
            _JetNo = _jetNo;
            _RPOfs = _rpofs;
            //_CutTime = _cutTime;
            _TotalLoops = _totalLoops;
            _DepthOfCut = _depthOfCut;
            _NCFilePath = _ncFilePath;
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
    public class NetFunc
    {
        public static bool IsStrIpAddress(string ipStr)
        {
            IPAddress ip;
            if (IPAddress.TryParse(ipStr, out ip))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
