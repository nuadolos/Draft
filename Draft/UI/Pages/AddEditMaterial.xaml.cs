using Draft.Entities;
using Draft.UI.Windows;
using Draft.Utilities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Draft.UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditMaterial.xaml
    /// </summary>
    public partial class AddEditMaterial : Page
    {
        #region Состояние и свойства страницы AddEditMaterial

        private Material addMaterial;

        private List<MaterialSupplier> lMatSupplier { get => Transition.Context.MaterialSupplier.Where(p => p.MaterialID == addMaterial.ID).ToList(); }

        public string Path { get { return System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "../../UI")); } }

        #endregion

        #region Конструктор страницы AddEditMaterial

        public AddEditMaterial(Material transferProd)
        {
            InitializeComponent();

            addMaterial = transferProd ?? new Material();
            HeaderBlock.Text = transferProd != null ? "Редактирование материла" : "Добавление материла";
            this.Title = HeaderBlock.Text;

            if (transferProd != null)
            {
                DeleteMatBtn.Visibility = Visibility.Visible;

                OperationsMatSupp.Visibility = Visibility.Visible;
                ViewMatSupp.ItemsSource = lMatSupplier;
            }

            if (!string.IsNullOrWhiteSpace(transferProd?.Image))
            {
                ImageMaterial.Source = (ImageSource)new ImageSourceConverter().ConvertFromString($@"{Path}\{transferProd.Image}");
            }

            var allTypes = Transition.Context.MaterialType.ToList();
            MatTypeCBox.ItemsSource = allTypes;

            DataContext = addMaterial;
        }

        #endregion

        #region Сохранение новых данных, сформированные пользователем

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder error = new StringBuilder();

            if (string.IsNullOrWhiteSpace(addMaterial.Title))
                error.AppendLine("Укажите наименование");
            if (addMaterial.MaterialType == null)
                error.AppendLine("Выберите тип материала");
            if (!double.TryParse(addMaterial.CountInStock.ToString(), out _))
                error.AppendLine("Укажите кол-во материалов на складе");
            if (string.IsNullOrWhiteSpace(addMaterial.Unit))
                error.AppendLine("Укажите единицу измерения");
            if (!int.TryParse(addMaterial.CountInPack.ToString(), out _))
                error.AppendLine("Укажите кол-во материалов в упаковке");
            if (!int.TryParse(addMaterial.MinCount.ToString(), out _))
                error.AppendLine("Укажите мин. кол-во материалов");
            else if (addMaterial.MinCount.ToString().StartsWith("-"))
                error.AppendLine("Мин. кол-во не может быть отрицательной");
            if (!decimal.TryParse(addMaterial.Cost.ToString(), out _))
                error.AppendLine("Укажите стоимость");
            else if (addMaterial.Cost.ToString().StartsWith("-"))
                error.AppendLine("Стоимость не может быть отрицательной");

            if (string.IsNullOrWhiteSpace(addMaterial.Description))
                addMaterial.Description = "";

            if (error.Length > 0)
            {
                MessageBox.Show($"Данные не соотвествуют следующим критериям:\n{error}", "Сохранение материала", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (addMaterial.ID == 0)
                Transition.Context.Material.Add(addMaterial);

            try
            {
                Transition.Context.SaveChanges();
                MessageBox.Show($"Данные успешно сохранены", "Сохранение материала", MessageBoxButton.OK, MessageBoxImage.Information);
                Transition.MainFrame.GoBack();
            }
            catch (Exception er)
            {
                MessageBox.Show($"При сохранении продукта произошла ошибка:\n{er.Message}", "Сохранение материала", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Добавление фото

        private void DownloadImageBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog downloadImage = new OpenFileDialog();
            downloadImage.Filter = "Image Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
            downloadImage.InitialDirectory = $@"{Path}\materials";

            if ((bool)downloadImage.ShowDialog())
            {
                if (!File.Exists(Path + "\\materials\\" + downloadImage.SafeFileName))
                    File.Copy(downloadImage.FileName, Path + "\\materials\\" + downloadImage.SafeFileName);

                addMaterial.Image = $@"\materials\{downloadImage.SafeFileName}";
                ImageMaterial.Source = (ImageSource)new ImageSourceConverter().ConvertFromString(downloadImage.FileName);
            }
        }

        #endregion

        #region Удаление материала из базы данных

        private void DeleteMatBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in Transition.Context.ProductMaterial.ToList())
            {
                if (item.MaterialID == addMaterial.ID)
                {
                    MessageBox.Show("Материал используется при производстве какой-либо продукции",
                        "Удаление материала", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            if (MessageBox.Show($"Вы хотите удалить материал {addMaterial.Title}?",
                "Удаление материала", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Transition.Context.Material.Remove(addMaterial);
                    Transition.Context.SaveChanges();
                    MessageBox.Show("Материал успешно удален",
                        "Удаление материала", MessageBoxButton.OK, MessageBoxImage.Information);
                    Transition.MainFrame.GoBack();
                }
                catch (Exception er)
                {
                    MessageBox.Show($"При удалении продукта возникла ошибка:\n{er.Message}",
                        "Удаление материала", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        #endregion

        #region Добавление, редактирование и удаление поставщиков, прикрепленных к материалу

        private void OpenMatSupp(MaterialSupplier tempPrMat = null, int id = 0)
        {
            MatSupp supplair = new MatSupp(tempPrMat, id);

            if (supplair.ShowDialog() == true)
            {
                ViewMatSupp.ItemsSource = lMatSupplier;
            }
        }

        private void AddMatSupp_Click(object sender, RoutedEventArgs e)
        {
            OpenMatSupp(id: addMaterial.ID);
        }

        private void EditMatSupp_Click(object sender, RoutedEventArgs e)
        {
            OpenMatSupp(ViewMatSupp.SelectedItem as MaterialSupplier);
        }

        private void DeleteMatSupp_Click(object sender, RoutedEventArgs e)
        {
            var deleteProdMat = ViewMatSupp.SelectedItems.Cast<MaterialSupplier>().ToList();

            if (deleteProdMat.Count > 0)
            {
                if (MessageBox.Show($"Вы хотите удалить {deleteProdMat.Count} элементов?", "Удаление материала",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Transition.Context.MaterialSupplier.RemoveRange(deleteProdMat);
                    Transition.Context.SaveChanges();
                    Transition.Context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                    ViewMatSupp.ItemsSource = lMatSupplier;
                    MessageBox.Show("Данные успешно удалены",
                        "Удаление материала", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
                MessageBox.Show("Необходимо выбрать хотя бы один элемент",
                        "Удаление материала", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        #endregion
    }
}
