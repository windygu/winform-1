﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.Utils;
using System.IO;
using DevExpress.XtraEditors;

namespace JCodes.Framework.WinFormUI
{
	public partial class frmSettings : DevExpress.XtraEditors.XtraForm {
        public frmSettings()
        {
			InitializeComponent();
		}

        private void buttonOK_Click(object sender, EventArgs e)
        {
            XtraMessageBox.Show("此功能待完善");
        }
	}
}

