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
    /// Логика взаимодействия для MatSupp.xaml
    /// </summary>
    public partial class MatSupp : Window
    {

        #region Поля окна MatSupp

        private MaterialSupplier addSupplier;
        private int idMaterial;

        #endregion

        #region Коструктор окна MatSupp

        public MatSupp(int id)
        {
            InitializeComponent();

            addSupplier = new MaterialSupplier();
            idMaterial = id;

            SupplierCmb.ItemsSource = Transition.Context.Supplier.ToList();

            DataContext = addSupplier;
        }

        #endregion

        #region Сохранение данных, связанных с поставщиком материала

        private void AddSupplierBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder error = new StringBuilder();

            if (addSupplier.Supplier == null)
                error.AppendLine("Выберите поставщика");

            if (error.Length > 0)
            {
                MessageBox.Show($"Данные не соотвествуют следующим критериям:\n{error}",
                    "Сохранение поставщика", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            addSupplier.MaterialID = idMaterial;
            Transition.Context.MaterialSupplier.Add(addSupplier);

            try
            {
                Transition.Context.SaveChanges();
                Transition.Context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                MessageBox.Show($"Поставщик успешно прикреплен к материалу",
                    "Сохранение поставщика", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                this.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show($"При сохранении поставщика возникла ошибка:\n{er.Message}",
                    "Сохранение поставщика", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
