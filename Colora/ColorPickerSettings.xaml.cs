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

namespace Colora
{
    /// <summary>
    /// Interaction logic for ColorPickerSettings.xaml
    /// </summary>
    public partial class ColorPickerSettings : Window
    {
        public ColorPickerSettings(int interval, int colors)
        {
            InitializeComponent();
            TxtInterval.Text = interval.ToString();
            TxtNumberColors.Text = colors.ToString();
        }

        private void butOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public int Colors   => int.TryParse(TxtNumberColors.Text, out var colors  ) ? colors   : 16 ;
        public int Interval => int.TryParse(TxtInterval.Text    , out var interval) ? interval : 100;
    }
}
