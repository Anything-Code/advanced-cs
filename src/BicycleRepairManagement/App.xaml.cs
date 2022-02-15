using BicycleRepairManagement.Properties;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BicycleRepairManagement
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Visibility IsDebug
        {
            #if DEBUG
            get { return Visibility.Visible; }
            #else
            get { return Visibility.Collapsed; }
            #endif
        }
        public static bool IsDebugBool
        {
            #if DEBUG
            get { return true; }
            #else
            get { return false; }
            #endif
        }

        public static ObservableCollection<Customer> _state = new ObservableCollection<Customer>();
        public static W_Appointments? W_Appointments;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _state = MyStorage.ReadBin<ObservableCollection<Customer>>("AppState.bin");
            if (_state == null) _state = new ObservableCollection<Customer>();

            refreshAppointments();
        }

        public static ObservableCollection<object> refreshAppointments ()
        {
            var customers = _state.ToList().ToImmutableList();
            var appointments = customers.Select(customer => customer.Repairs.Select(repair => new {
                TargetDate = repair.TargetDate,
                BicycleCategory = repair.BicycleCategory,
                Customer = customer,
            }))
                .SelectMany(i => i)
                .OrderBy(item => item.TargetDate);

            return new ObservableCollection<object>(appointments);
        } 

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            MyStorage.WriteBin(_state, "AppState.bin");
            Settings.Default.Save();
        }
    }
}
