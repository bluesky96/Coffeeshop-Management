﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CoffeeManagement.Utilities;
using CoffeeManagement.DTOs;

namespace CoffeeManagement.Views.Popups
{
    public partial class JoinTable : Form
    {
        Bill _bill;
        List<Table> _tables;

        public delegate void UpdateTableAndBill(Bill bill, List<Table> tables);
        public UpdateTableAndBill UpdateDelegate;

        public JoinTable(Bill currentBill, List<Table> tables)
        {
            InitializeComponent();
            _bill = currentBill;
            _tables = tables;

            // In danh sách bàn hiện tại
            try
            {
                this.tableListTB.Text = string.Join(";", _bill.Tables.Select(t => t.Name));
            }
            catch (NullReferenceException e)
            {
                MessageHelper.CreateErrorMessage("Dữ liệu bàn chưa có!");
                //DialogResult = DialogResult.Abort;
                this.Close();
                return;
            }
            // Danh sách bàn có thể ghép
            foreach (Table t in _tables)
            {
                this.newTableCB.Items.Add(t.Name);
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            if (MessageHelper.CreateYesNoQuestion("Bạn thực sự muốn gộp bàn?") == DialogResult.Yes)
            {
                // Do joining Table List, Bill Data.
                Table selectedTable = _tables[this.newTableCB.SelectedIndex];
                _bill.Tables.Add(selectedTable);
                selectedTable.Bills.Add(_bill);
                //_tables.Find(_bill.Tables.First()).Name = "";// Ghép tên bàn mới vào bàn cũ trong danh sách bàn
                _tables.Remove(_bill.Tables.Last<Table>()); // Loại bỏ bàn vừa ghép ra khỏi danh sách
                //_bill.Tables.First<Table>().Name
                if (UpdateDelegate != null)
                {// tại đây gọi nó
                    UpdateDelegate(_bill, _tables);
                }
                MessageHelper.CreateMessage("Đã ghép vào bàn " + _bill.Tables.First<Table>().Name);
            }
        }
    }
}
