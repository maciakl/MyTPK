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
    public partial class ReceiveMessage : Form
    {

        private MyTPKRSA Crypto;

        public ReceiveMessage(MyTPKUI parent)
        {
            InitializeComponent();
            Crypto = new MyTPKRSA();

            this.key = Crypto.PublicKey;

            this.StartPosition = FormStartPosition.CenterParent;

        }

        public string key
        {
            set { textBox1.Text = value; }
        }
             

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "mytpk files (*.mytpk)|*.mytpk|All files (*.*)|*.*";
            dialog.Title = "Select a file to decrypt";
            string file = (dialog.ShowDialog() == DialogResult.OK) ? dialog.FileName : null;

            if (file != null)
            {
                try
                {
                    progressBar1.PerformStep();
                    label2.Text = "Decrypting file";
                    Crypto.DecryptFile(file);

                    progressBar1.Value = 100;
                    label2.Text = "Done";

                    MessageBox.Show("File created: " + file.Substring(0, file.Length - 6));
                }
                catch (Exception x)
                {
                    MessageBox.Show(this, x.Message, "Decryption Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                this.Hide();
                this.Dispose();
                
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox1.Text);
        }

    }
}
