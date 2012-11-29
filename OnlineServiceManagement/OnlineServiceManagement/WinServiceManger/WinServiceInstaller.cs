using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace WinServiceManger
{
    [RunInstaller(true)]
    public partial class WinServiceMangerInstaller : System.Configuration.Install.Installer
    {
        public WinServiceMangerInstaller()
        {
            InitializeComponent();
        }
    }
}
