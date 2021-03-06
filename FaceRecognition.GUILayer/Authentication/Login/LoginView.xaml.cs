﻿using FaceRecognition.GUILayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FaceRecognition.GUILayer.Authentication.Login
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        
        public LoginView()
        {
            InitializeComponent();
        }

        private void PasswordBoxFieldChanged(object sender, RoutedEventArgs e)
        {
            ((LoginViewModel)DataContext).User.Password = PasswordBoxField.Password;
        }
    }
}
