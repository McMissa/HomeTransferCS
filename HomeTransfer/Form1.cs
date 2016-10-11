using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HomeTransfer.controller;
using HomeTransfer.model;

namespace HomeTransfer
{
    public partial class Form1 : Form
    {
        private static Form1 form1;

        public static Form getInstance()
        {
            return form1;
        }

        public Form1()
        {
            InitializeComponent();
            form1 = this;
            HomeTransferServerData local = HomeTransferModel.getInstance().getLocalHomeTransferData();
            this.Text = local.name + "@" + local.IP;
            this.listBox1.AllowDrop = true;
            this.listBox1.DragDrop += listBox1_DragDrop;
            this.listBox1.DragEnter += listBox1_DragEnter;
        }

        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            HomeTransferModel.getInstance().addFiles(files);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            String path = "";
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                path = file.FileName;
            }
            String[] paths = new string[1];
            paths[0] = path;
            HomeTransferModel.getInstance().addFiles(paths);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HomeTransferController.getInstance().deleteFiles();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String server = "";
            if (listBox2.Items.Count > 1)
            {
                server = listBox2.GetItemText(listBox2.SelectedItem);
                if (server.Equals(""))
                {
                    MessageBox.Show("Please select a remote instance first");
                    return;
                }
            } else if (listBox2.Items.Count == 1)
            {
                server = listBox2.Items[0].ToString();
            } else
            {
                return;
            }
            HomeTransferController.sendFiles(server, HomeTransferModel.getInstance().getFiles());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            HomeTransferController.getInstance().refreshList();
        }

        public void updateGUI()
        {
            listBox1.Invoke((MethodInvoker)(() => {
                listBox1.Items.Clear();
                String[] files = HomeTransferModel.getInstance().getFiles();
                foreach(String file in files)
                {
                    listBox1.Items.Add(file);
                }
            }));

            listBox2.Invoke((MethodInvoker)(() => {
                listBox2.Items.Clear();
                String[] servers = HomeTransferModel.getInstance().getServers();
                foreach (String server in servers)
                {
                    listBox2.Items.Add(server);
                }
            }));

        }
    }
}
