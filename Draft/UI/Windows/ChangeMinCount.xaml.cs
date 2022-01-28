using Draft.Entities;
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

namespace Draft.UI.Windows
{
    /// <summary>
    /// Логика взаимодействия для ChangeMinCount.xaml
    /// </summary>
    public partial class ChangeMinCount : Window
    {
        public ChangeMinCount(List<Material> transferLMat)
        {
            InitializeComponent();
        }
    }
}
