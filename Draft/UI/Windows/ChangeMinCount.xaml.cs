using Draft.Entities;
using Draft.Utilities;
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
        #region Состояние окна ChangeMinCount

        private int maxValue;
        private List<Material> lEditMinCount;

        #endregion

        #region Конструктор окна ChangeMinCount

        public ChangeMinCount(List<Material> transferEditMinCount)
        {
            InitializeComponent();

            lEditMinCount = transferEditMinCount;

            foreach (var item in lEditMinCount)
            {
                if (maxValue < item.MinCount)
                    maxValue = (int)item.MinCount;
            }

            EditMinCount.Text = maxValue.ToString();
        }

        #endregion

        #region Изменение минимального количество

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            int tempCount;

            if (int.TryParse(EditMinCount.Text, out tempCount))
            {
                foreach (var item in lEditMinCount)
                {
                    item.MinCount = tempCount;
                }

                try
                {
                    Transition.Context.SaveChanges();
                    Transition.Context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                    MessageBox.Show($"Минимальное количество выбранных материалов изменено на {tempCount}",
                        "Изменение мин. количества", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    this.Close();
                }
                catch (Exception er)
                {
                    MessageBox.Show($"При изменении мин. количества возникла ошибка:\n{er.Message}",
                        "Изменение мин. количества", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
                MessageBox.Show("В поле для ввода должно быть число",
                    "Изменение мин. количества", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        #endregion

        #region Закрытие окна без изменений данных

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}
