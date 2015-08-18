using System;
using System.ComponentModel;
using System.Windows.Forms;
using Meridian59.Data.Models.AdminData;

namespace Meridian59.DebugUI
{
    public partial class ObjectEditor : Form
    {
        private BindingSource dataBindingSource;
        private AdminObject trackedObject;
  
        public ObjectEditor(AdminObject obj)
        {
            InitializeComponent();
            Text = String.Format("{0} {1}", obj.ObjectNumber, obj.ClassName); 
            dataBindingSource = new BindingSource();
            dataBindingSource.DataSource = obj.Properties;
            dataGridView1.DataSource = dataBindingSource;
            dataGridView1.Columns[0].ReadOnly = true;
            obj.PropertyChanged += TrackedObject_OnPropertyChanged;
            trackedObject = obj;
        }

        private void TrackedObject_OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            RefreshData();
            ResetBindings();
        }

        public void RefreshData()
        {
            dataBindingSource.Clear();
            dataBindingSource.DataSource = null;
            dataGridView1.DataSource = null;
            dataBindingSource.DataSource = trackedObject.Properties;
            dataGridView1.DataSource = dataBindingSource;
            dataGridView1.Refresh();
        }

        public AdminObject GetTrackedObject()
        {
            return trackedObject;
        }
    }
}
