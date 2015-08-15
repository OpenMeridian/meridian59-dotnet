﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Meridian59.Data.Models.AdminData;

namespace Meridian59.DebugUI
{
    public partial class ObjectEditor : Form
    {
        public ObjectEditor(AdminObject obj)
        {
            InitializeComponent();
            Text = String.Format("{0} {1}", obj.ObjectNumber, obj.ClassName);
            dataGridView1.DataSource = obj.Properties;
        }
    }
}