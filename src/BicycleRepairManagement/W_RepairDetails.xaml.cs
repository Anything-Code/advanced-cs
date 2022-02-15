using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

namespace BicycleRepairManagement
{
    /// <summary>
    /// Interaktionslogik für W_Appointments.xaml
    /// </summary>
    public partial class W_RepairDetails : Window
    {
        public W_RepairDetails()
        {
            InitializeComponent();
            CbxBikeCategory.ItemsSource = new List<string> { "City bike", "Mountain bike", "Road bike", "Gravel bike" }.ToImmutableList();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Owner.Visibility = Visibility.Visible;
        }

        private void BtnAddArea_Click(object sender, RoutedEventArgs e)
        {
            var repair = this.DataContext as Repair;
            if (repair == null) return;

            repair.Areas.Add(new Area());
        }

        private void BtnAddMaterial_Click(object sender, RoutedEventArgs e)
        {
            var repair = this.DataContext as Repair;
            if (repair == null) return;

            repair.Materials.Add(new Material());
        }

        private void BtnRemoveMaterial_Click(object sender, RoutedEventArgs e)
        {
            var repair = this.DataContext as Repair;
            if (repair == null) return;
            if (repair.Materials.Count == 1)
            {
                MessageBox.Show(
                    $"You cannot remove the first material.",
                    "Warning",
                    MessageBoxButton.OK,
                    MessageBoxImage.Question
                );
                return;
            }
            var material = (Material)((StackPanel)((StackPanel)((Button)(e.Source)).Parent).Parent).DataContext;
            repair.Materials.Remove(material);
        }

        private void BtnRemoveArea_Click(object sender, RoutedEventArgs e)
        {
            var repair = this.DataContext as Repair;
            if (repair == null) return;
            if (repair.Areas.Count == 1)
            {
                MessageBox.Show(
                    $"You cannot remove the first area.",
                    "Warning",
                    MessageBoxButton.OK,
                    MessageBoxImage.Question
                );
                return;
            }
            var area = (Area)((StackPanel)((StackPanel)((Button)(e.Source)).Parent).Parent).DataContext;
            repair.Areas.Remove(area);
        }
    }
}
