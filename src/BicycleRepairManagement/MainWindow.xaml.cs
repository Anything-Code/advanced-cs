using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
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
using System.Xml.Serialization;

namespace BicycleRepairManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private string FirstCharToUpper (string input) =>
            input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => input[0].ToString().ToUpper() + input.Substring(1)
            };

        private string randomWord () => FirstCharToUpper(Faker.Lorem.Words(1).ToList().First());

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CbxGender.ItemsSource = new List<string> { "Female", "Male", "Diverse" }.ToImmutableList();
            CbxBikeCategory.ItemsSource = new List<string> { "City bike", "Mountain bike", "Road bike", "Gravel bike" }.ToImmutableList();
            LBxCustomers.ItemsSource = App._state;
            IcAppointments.ItemsSource = App.refreshAppointments();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = new Customer();
            App._state.Add(customer);
            LBxCustomers.SelectedItem = customer;
            LBxCustomers.ScrollIntoView(customer);
        }

        private DateTime RandomDate(int from, int to) => DateTime.Now.AddDays(new Random().Next(from, to));

        private void BtnSeed_Click(object sender, RoutedEventArgs e)
        {
            Customer member = new Customer
            {
                FirstName = Faker.Name.First(),
                LastName = Faker.Name.Last(),
                EMailTel = Faker.Internet.Email(),
                Gender = (uint) new Random().Next(0, 2),
                Repairs = new ObservableCollection<Repair> {
                    new Repair {
                        TargetDate = RandomDate(10, 30),
                        Areas = new ObservableCollection<Area> { new Area() },
                        Materials = new ObservableCollection<Material> { new Material() },
                    },
                    new Repair {
                        TargetDate = RandomDate(10, 30),
                        Areas = new ObservableCollection<Area> { new Area() },
                        Materials = new ObservableCollection<Material> { new Material() },
                    },
                    new Repair {
                        TargetDate = RandomDate(10, 30),
                        Areas = new ObservableCollection<Area> { new Area() },
                        Materials = new ObservableCollection<Material> { new Material() },
                    },
                    new Repair {
                        TargetDate = RandomDate(10, 30),
                        Areas = new ObservableCollection<Area> { new Area() },
                        Materials = new ObservableCollection<Material> { new Material() },
                    },
                    new Repair {
                        TargetDate = RandomDate(10, 30),
                        Areas = new ObservableCollection<Area> { new Area() },
                        Materials = new ObservableCollection<Material> { new Material() },
                    },
                }
            };
            App._state.Add(member);
            LBxCustomers.SelectedItem = member;
            LBxCustomers.ScrollIntoView(member);
            IcAppointments.ItemsSource = App.refreshAppointments();
            if (App.W_Appointments != null)
                App.W_Appointments.refreshIcItemsSource();
        }

        private void BtnDeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            var customers = LBxCustomers.SelectedItems.Cast<Customer>().ToImmutableList();
            var deleteMessageBox = () => MessageBox.Show("Please select an item to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);

            if (customers == null)
            {
                deleteMessageBox();
                return;
            } else if (customers.Count() == 0)
            {
                deleteMessageBox();
                return;
            }
            var res = MessageBox.Show(
                $"Are you sure you want to remove {customers.FirstOrDefault().FirstName} {customers.FirstOrDefault().LastName} from the List of customers?",
                "Warning",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (res == MessageBoxResult.Yes)
            {
                customers.ForEach((customer) => App._state.Remove(customer));
                LBxCustomers.ItemsSource = filteredCustomers(TbxFilter.Text);
            }
            IcAppointments.ItemsSource = App.refreshAppointments();
            if (App.W_Appointments != null)
                App.W_Appointments.refreshIcItemsSource();
        }

        private void TbxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filter = TbxFilter.Text.ToLower();
            if (filter == "")
            {
                LBxCustomers.ItemsSource = App._state;
            }
            else
            {
                var list = filteredCustomers(filter);

                LBxCustomers.ItemsSource = list.ToList();
            }
        }

        private IEnumerable<Customer> filteredCustomers(string filterInput) => from m in App._state where m.FullName.ToLower().Contains(filterInput) orderby m.LastName.IndexOf(filterInput) select m;

        private void BtnAddRepair_Click(object sender, RoutedEventArgs e)
        {
            if (LBxCustomers.SelectedItem == null)
            {
                return;
            }

            var selectedCustomer = LBxCustomers.SelectedItem as Customer;
            var newRepair = new Repair {
                Areas = new ObservableCollection<Area> { new Area() },
                Materials = new ObservableCollection<Material> { new Material() }
            };

            selectedCustomer.Repairs.Add(newRepair);
            LbxRepairs.SelectedItem = newRepair;
            LbxRepairs.ScrollIntoView(newRepair);
            IcAppointments.ItemsSource = App.refreshAppointments();
            if (App.W_Appointments != null)
                App.W_Appointments.refreshIcItemsSource();
        }

        private void BtnDeleteRepair_Click(object sender, RoutedEventArgs e)
        {
            var repairs = LbxRepairs.SelectedItems.Cast<Repair>().ToImmutableList();
            var selectedCustomer = LBxCustomers.SelectedItem as Customer;
            var deleteMessageBox = () => MessageBox.Show("Please select an item to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);

            if (selectedCustomer == null)
            {
                return;
            }

            if (repairs == null)
            {
                deleteMessageBox();
                return;
            }
            else if (repairs.Count() == 0)
            {
                deleteMessageBox();
                return;
            }
            var res = MessageBox.Show(
                $"Are you sure you want to remove \"{ConvertIntToBicycle(repairs.FirstOrDefault().BicycleCategory)}\" from the repairs-list of {(LBxCustomers.SelectedItem as Customer).FullName}?",
                "Warning",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (res == MessageBoxResult.Yes)
            {
                repairs.ForEach((repair) => selectedCustomer.Repairs.Remove(repair));

                var lastRepair = selectedCustomer.Repairs.FirstOrDefault();
                //LbxRepairs.SelectedItem = lastRepair;
                //LbxRepairs.ScrollIntoView(lastRepair);
            }
        }
        private object ConvertIntToBicycle(uint value)
        {
            switch (value)
            {
                case 0: return "City bike";
                case 1: return "Mountain bike";
                case 2: return "Road bike";
                case 3: return "Gravel bike";
            }
            return null;
        }

        private void LBxCustomers_Selected(object sender, RoutedEventArgs e)
        {
            //LbxRepairs.SelectedIndex = 0;
        }

        private void Grid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var grid = (Grid)sender;
            if (grid.Visibility == Visibility.Visible)
            {
                GrdAppointments.Visibility = Visibility.Collapsed;
            }
            else
            {
                GrdAppointments.Visibility = Visibility.Visible;
            }
        }

        private void BtnUnselectRepair_Click(object sender, RoutedEventArgs e)
        {
            LbxRepairs.SelectedItem = null;
        }

        private void BtnUnselectCustomer_Click(object sender, RoutedEventArgs e)
        {
            LBxCustomers.SelectedItem = null;
            IcAppointments.ItemsSource = App.refreshAppointments();
            if (App.W_Appointments != null)
                App.W_Appointments.refreshIcItemsSource();
        }

        private void BtnToAppointments_Click(object sender, RoutedEventArgs e)
        {
            App.W_Appointments = new W_Appointments { DataContext=App.refreshAppointments() };
            App.W_Appointments.Owner = this;
            App.W_Appointments.Show();
        }

        private void BtnToRepairDetails_Click(object sender, RoutedEventArgs e)
        {
            var win = new W_RepairDetails { DataContext = LbxRepairs.SelectedItem as Repair };
            win.Owner = this;
            win.Show();
        }

        private void MItExport_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "BicycleRepairManagementState";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog.Filter = "XML (*.xml)|*.xml";
            if (saveFileDialog.ShowDialog() == true)
            {
                MyStorage.WriteObjectAsXMLStringToFS(saveFileDialog.FileName, App._state);
            }
        }

        private void BtnAddArea_Click(object sender, RoutedEventArgs e)
        {
            var repair = LbxRepairs.SelectedItem as Repair;
            if (repair == null) return;

            repair.Areas.Add(new Area());
        }

        private void BtnAddMaterial_Click(object sender, RoutedEventArgs e)
        {
            var repair = LbxRepairs.SelectedItem as Repair;
            if (repair == null) return;

            repair.Materials.Add(new Material());
        }

        private void BtnRemoveMaterial_Click(object sender, RoutedEventArgs e)
        {
            var repair = LbxRepairs.SelectedItem as Repair;
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
            var repair = LbxRepairs.SelectedItem as Repair;
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

        private void MItSettings_Click(object sender, RoutedEventArgs e)
        {
            var win = new W_Settings();
            win.Owner = this;
            win.Show();
        }

        private void DaPiTargetDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            IcAppointments.ItemsSource = App.refreshAppointments();
            if (App.W_Appointments != null)
                App.W_Appointments.refreshIcItemsSource();
        }
    }
}
