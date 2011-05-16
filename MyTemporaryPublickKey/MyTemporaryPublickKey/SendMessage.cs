// ReceiveMessage (c) 2011 Lukasz Grzegorz Maciak

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
    public partial class SendMessage : Form
    {

        private MyTPKRSA Crypto;
                
        public SendMessage()
        {
            InitializeComponent();
            Crypto = new MyTPKRSA();

            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "All files (*.*)|*.*";
            dialog.Title = "Select a file to encrypt";
            string file = (dialog.ShowDialog() == DialogResult.OK) ? dialog.FileName : null;

            if (file != null)
            {
                progressBar1.PerformStep();
                label2.Text = "Encrypting a file";
                Crypto.EncryptFile(file, textBox1.Text);

                progressBar1.Value = 100;
                label2.Text = "Done";

                MessageBox.Show("File created: " + file + ".mytpk");
                this.Hide();
                this.Dispose();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
