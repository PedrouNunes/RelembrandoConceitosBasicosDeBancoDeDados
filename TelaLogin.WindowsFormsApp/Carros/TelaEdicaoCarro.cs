﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TelaLogin.WindowsFormsApp.Carros
{
    public partial class TelaEdicaoCarro : Form
    {
        string placaCarro;
        public TelaEdicaoCarro(string placa)
        {
            InitializeComponent();
            this.placaCarro = placa;
        }
    }
}
