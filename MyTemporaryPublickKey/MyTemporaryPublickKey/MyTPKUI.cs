// MyTPKUI (c) 2011 Lukasz Grzegorz Maciak

/*
* License: This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU General Public License as published by
* the Free Software Foundation; either version 3 of the License, or (at your
* option) any later version. This program is distributed in the hope that it
* will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty
* of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General
* Public License for more details.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyTemporaryPublickKey
{
    public partial class MyTPKUI : Form
    {
        
        public MyTPKUI()
        {
            InitializeComponent();      
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            ReceiveMessage receiveWindow = new ReceiveMessage(this);
            receiveWindow.ShowDialog(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SendMessage sendWindow = new SendMessage();
            sendWindow.ShowDialog(this);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            progressBar1.PerformStep();
            MyTPKRSA temp = new MyTPKRSA(); 
            progressBar1.PerformStep();

            progressBar1.PerformStep();
            temp.deletePersistentKey();
            progressBar1.PerformStep();

            progressBar1.PerformStep();
            temp.newSymmetricKeys();
            progressBar1.PerformStep();

            progressBar1.PerformStep();
            temp.getPersistentKey();
            progressBar1.PerformStep();

            progressBar1.Value = 100;

            MessageBox.Show(this, "New key pair generated", "Keys Generated", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

            progressBar1.Value = 0;
        }
    }
}
