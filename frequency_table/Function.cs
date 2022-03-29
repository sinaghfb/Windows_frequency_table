using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace frequency_table
{
    internal class Function
    {
        private double Xmin, Xmax, L, R, K;
        public double Med, AVG, variance, Skewness, Kurtosis, SD;
        private int Frication;
        private List<double> Input_Data = new List<double>();
        private List<OutputTable> Output_Data = new();
        private bool Validate = true;
        //this method gets data and converts it too a list
        public Function(string Input)
        {
            string[] str = Input.Split(new Char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            double Temp = 0;
            //if we have wrong input validate will be set to false
            for (int i = 0; i < str.Length; i++)
            {
               Validate= double.TryParse(str[i],out Temp);
                Input_Data.Add(Temp);
                
            }
            //according to paper data musb be sorted
            Input_Data.Sort();
        }
        //in this method we caculate number of fraction digits for caculating rounded data unit
        private int Friction_Caculator()
        {
            int _count = 0;
            string _temp = "";
            for (int i = 0; i < Input_Data.Count; i++)
            {
                _temp = Input_Data[i].ToString();
                if (_temp.IndexOf('.', 0) > 0)
                {

                    _temp = _temp.Remove(0, _temp.IndexOf('.') + 1);
                    if (_temp.Length > _count)
                    {
                        _count = _temp.Length;
                    }
                }
            }
            return _count;
        }
        //this method caculate basics for initialize the table
        private void Basics()
        {
            Frication = Friction_Caculator();
            double Temp = 0;
            Temp = Math.Pow(0.1, Frication);
            //minimum data 
            Xmin = Input_Data[0] - (Temp / 2);
            //maximum data
            Xmax = Input_Data.Last() + (Temp / 2);
            //caculating variation range
            R = Xmax - Xmin;
            //Number of rows
            K = Math.Round((1 + (3.32 + Math.Log10(Input_Data.Count))), Frication, MidpointRounding.ToPositiveInfinity);
            //Length of row
            L = Math.Round((R / K), Frication, MidpointRounding.ToPositiveInfinity);
            Temp = Xmin;
            //this loop checks if last matches the maximum number
            for (int i = 0; i < K; i++)
            {
                Temp += L;
                if (i == K - 1 & Temp < Xmax)
                {
                    K = K - 1;
                    i = 0;
                    L = Math.Round((R / K), Frication, MidpointRounding.ToPositiveInfinity);
                    Temp = Xmin;
                }
            }
        }
        private void Table_Maker()
        {
            double xi, dr, ur;
            int fi, Fi;
            dr = Xmin;
            AVG = 0;
            Fi = 0;
            fi = 0;
            //this loop caculates the average or you can use average method of list but the numbers are not match 
            for (int i = 0; i < K; i++)
            {
                ur = dr + L;
                for (int j = 0; j < Input_Data.Count; j++)
                {
                    if (Input_Data[j] < ur & Input_Data[j] >= dr)
                    {
                        fi++;
                    }
                }
                xi = (dr + ur) / 2;
                AVG += (fi * xi);
                fi = 0;
                dr = ur;
            }
            AVG = AVG / Input_Data.Count;
            xi = 0;
            fi = 0;
            dr = Xmin;
            ur = 0;
            //in this loop we make each row and put it on table
            for (int i = 0; i < K; i++)
            {
                OutputTable ot = new();
                ot.RowNumber = i;
                ot.DownRange = dr;
                ur = dr + L;
                ot.Uprange = ur;
                for (int j = 0; j < Input_Data.Count; j++)
                {
                    if (Input_Data[j] < ur & Input_Data[j] >= dr)
                    {
                        fi++;
                        Fi++;
                    }
                }
                ot.fi = fi;
                ot.Fi = Fi;
                xi = (dr + ur) / 2;
                ot.xi = xi;
                ot.fixi = xi * fi;
                //this is for caculating variance
                ot.xbar2 = Math.Round(fi * Math.Pow(xi - AVG, 2), 4);
                //this is for caculating skewness
                ot.xbar3 = Math.Round(fi * Math.Pow(xi - AVG, 3), 4);
                //this is for caculating Kurtosis
                ot.xbar4 = Math.Round(fi * Math.Pow(xi - AVG, 4), 4);
                ot.fipercent = Math.Round((Convert.ToDouble(fi) / Input_Data.Count) , 4);
                ot.Fipercent = Math.Round((Convert.ToDouble(Fi) / Input_Data.Count) , 4);
                Output_Data.Add(ot);
                dr = ur;
                fi = 0;
            }
        }
        private bool Table_Caculator()
        {
            variance = 0;
            Skewness = 0;
            Kurtosis = 0;
            SD = 0;
            Med = 0;
            //in this loop we caculate variance ,skewness ,Standard deviation and Kurtosis
            for (int i = 0; i < K; i++)
            {
                OutputTable ot = new();
                ot = Output_Data[i];
                if (ot.Fi >= (Input_Data.Count / 2) & Med == 0)
                {

                    if (i == 0)
                    {
                        Med = ot.DownRange + ((Input_Data.Count / 2.0) * L) / ot.fi;
                        break;
                    }
                    else
                    {
                        Med = ot.DownRange + (((Input_Data.Count / 2.0) - Output_Data[i - 1].Fi) * L) / ot.fi;
                    }

                }
                variance += ot.xbar2;
                Skewness += ot.xbar3;
                Kurtosis += ot.xbar4;
            }
            variance = variance / Input_Data.Count;
            SD = Math.Pow(variance, 0.5);
            Skewness = Skewness / (Input_Data.Count * Math.Pow(SD, 3));
            Kurtosis = (Kurtosis / (Input_Data.Count * Math.Pow(SD, 4))) - 3;
            return true;
        }
        public List<OutputTable> Output()
        {
            //if validate be false this mean we have problem too parsing data too list 
            if (Validate)
            {
                Basics();
                Table_Maker();
                Table_Caculator();
                return Output_Data;
            }
            return null;    
        }
    }
}
