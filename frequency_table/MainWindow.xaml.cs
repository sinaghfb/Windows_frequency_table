using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Media;


namespace frequency_table
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           MessageBox.Show("The purpose of writing this program is to implement mathematical algorithms in academic textbooks, " +
               "and in some places may not be optimal in terms of time complexity and memory." 
               +"\nMore detailed descriptions of code implementation are written in the project files." +
                "\n\n\nMade by Sina Ghasemzadeh Farde Bidgoli under MIT license (2022)", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            MessageBox.Show("Please enter any new data in the new line and the data should only be in numbers.","Warning" 
                ,MessageBoxButton.OK, MessageBoxImage.Warning);

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(Input_tb.Text))
            {
                //here we change columns header 
                Function function = new(Input_tb.Text);
                if (function.Output() != null)
                {
                    DataGrid.ItemsSource = function.Output();
                    DataGrid.Columns[0].Header = "RowNumber";
                    DataGrid.Columns[1].Header = "Down Range";
                    DataGrid.Columns[2].Header = "Up Range";
                    DataGrid.Columns[3].Header = "xi";
                    DataGrid.Columns[4].Header = "fi";
                    DataGrid.Columns[5].Header = "Fi";
                    DataGrid.Columns[6].Header = "Relative Frequency";
                    DataGrid.Columns[7].Header = "Cumulative Relative Frequency";
                    DataGrid.Columns[8].Header = "fi*xi";
                    DataGrid.Columns[9].Header = "fi*(xi-average)^2";
                    DataGrid.Columns[10].Header = "fi*(xi-average)^3";
                    DataGrid.Columns[11].Header = "fi*(xi-average)^4";
                    Average_tb.Text = function.AVG.ToString();
                    Skewness_tb.Text = function.Skewness.ToString();
                    Kurtosis_tb.Text = function.Kurtosis.ToString();
                    Median_tb.Text = function.Med.ToString();
                    SD_tb.Text = function.SD.ToString();
                    Variance_tb.Text = function.variance.ToString();
                }
                else
                {
                    MessageBox.Show("Somrthing wrong with input pls check and try agian",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Input_tb.Text))
            {
                if (!Input_tb.Text.Any(char.IsLetter))
                {
                    InpuCheck_label.Content = "Valid";
                    InpuCheck_label.Foreground = new SolidColorBrush(Colors.Green);
                    Caculate_btn.IsEnabled = true;
                }
                else
                {
                    InpuCheck_label.Content = "Error";
                    InpuCheck_label.Foreground = new SolidColorBrush(Colors.Red);
                    Caculate_btn.IsEnabled = false;
                }
            }
        }

    }
}

