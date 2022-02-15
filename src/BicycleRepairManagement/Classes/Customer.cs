using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BicycleRepairManagement
{
    [Serializable]
    public class Customer
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public string FullName { get { return $"{this.FirstName} {this.LastName}"; } }
        public string EMailTel { get; set; } = String.Empty;
        public uint Gender { get; set; } = 0;
        public ObservableCollection<Repair> Repairs { get; set; } = new ObservableCollection<Repair>();

    }
    [Serializable]
    public class Repair
    {
        public uint BicycleCategory { get; set; } = 0;
        public bool IsEBike { get; set; } = false;
        public ObservableCollection<Area> Areas { get; set; } = new ObservableCollection<Area>();
        public DateTime TargetDate { get; set; } = DateTime.Now;
        public uint UsedHours { get; set; } = 0;
        public ObservableCollection<Material> Materials { get; set; } = new ObservableCollection<Material>();
        public string CreatedAt { get; set; } = DateTime.Now.ToString("dd'/'MM'/'yyyy");
    }
    [Serializable]
    public class Area
    {
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
    }
    [Serializable]
    public class Material
    {
        public string Name { get; set; } = String.Empty;
        public string Price { get; set; } = String.Empty;
    }
}