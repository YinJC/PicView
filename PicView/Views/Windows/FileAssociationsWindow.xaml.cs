﻿using PicView.UILogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using static PicView.ConfigureSettings.ConfigColors;
using static PicView.SystemIntegration.Wallpaper;
using static PicView.UILogic.Animations.MouseOverAnimations;
using static PicView.SystemIntegration.NativeMethods;
using PicView.UILogic.Animations;

namespace PicView.Views.Windows
{
    /// <summary>
    /// Interaction logic for FileAssociationsWindow.xaml
    /// </summary>
    public partial class FileAssociationsWindow : Window
    {
        public FileAssociationsWindow()
        {
            Title = Application.Current.Resources["SetAsDefualt"] + " - PicView";
            Width = ConfigureWindows.GetSettingsWindow.ActualWidth;
            Height = ConfigureWindows.GetSettingsWindow.ActualHeight;
            InitializeComponent();

            ContentRendered += delegate
            {
                // Hide from alt tab
                var helper = new WindowInteropHelper(this).Handle;
                _ = SetWindowLong(helper, GWL_EX_STYLE, (GetWindowLong(helper, GWL_EX_STYLE) | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);

                TitleBar.MouseLeftButtonDown += (_, _) => DragMove();
                LocationChanged += delegate // Move parent window as well
                {
                    ConfigureWindows.GetSettingsWindow.Top = Top;
                    ConfigureWindows.GetSettingsWindow.Left = Left;
                };
                CloseButton.TheButton.Click += (_, _) => HideLogic();             

                RasterFormatsCheck.Checked += delegate 
                {
                    var list = RasterFormatsContainer.Children.OfType<CheckBox>();
                    foreach (var item in list)
                    {
                        item.IsChecked = true;
                    }
                };
                RasterFormatsCheck.Unchecked += delegate
                {
                    var list = RasterFormatsContainer.Children.OfType<CheckBox>();
                    foreach (var item in list)
                    {
                        item.IsChecked = false;
                    }
                };

                PhotoshopCheck.Checked += delegate
                {
                    var list = PhotoshopContainer.Children.OfType<CheckBox>();
                    foreach (var item in list)
                    {
                        item.IsChecked = true;
                    }
                };
                PhotoshopCheck.Unchecked += delegate
                {
                    var list = PhotoshopContainer.Children.OfType<CheckBox>();
                    foreach (var item in list)
                    {
                        item.IsChecked = false;
                    }
                };

                VectorCheck.Checked += delegate
                {
                    var list = VectorContainer.Children.OfType<CheckBox>();
                    foreach (var item in list)
                    {
                        item.IsChecked = true;
                    }
                };
                VectorCheck.Unchecked += delegate
                {
                    var list = VectorContainer.Children.OfType<CheckBox>();
                    foreach (var item in list)
                    {
                        item.IsChecked = false;
                    }
                };

                RawCameraCheck.Checked += delegate
                {
                    var list = CameraFormatsContainer.Children.OfType<CheckBox>();
                    foreach (var item in list)
                    {
                        item.IsChecked = true;
                    }
                };
                RawCameraCheck.Unchecked += delegate
                {
                    var list = CameraFormatsContainer.Children.OfType<CheckBox>();
                    foreach (var item in list)
                    {
                        item.IsChecked = false;
                    }
                };

                OtherCheck.Checked += delegate
                {
                    var list = OtherFormatsContainer.Children.OfType<CheckBox>();
                    foreach (var item in list)
                    {
                        item.IsChecked = true;
                    }
                };
                OtherCheck.Unchecked += delegate
                {
                    var list = OtherFormatsContainer.Children.OfType<CheckBox>();
                    foreach (var item in list)
                    {
                        item.IsChecked = false;
                    }
                };
            };      

            KeyDown += (_, e) => 
            {
                if (e.Key == System.Windows.Input.Key.Escape)
                {
                    HideLogic();
                }
            };

            ApplyButton.Click += delegate 
            {
                var rasterFormats = RasterFormatsContainer.Children.OfType<CheckBox>().Where(x => x.IsChecked == true);
                var photoshopFormats = PhotoshopContainer.Children.OfType<CheckBox>().Where(x => x.IsChecked == true);
                var vectorFormats = VectorContainer.Children.OfType<CheckBox>().Where(x => x.IsChecked == true);
                var cameraFormats = CameraFormatsContainer.Children.OfType<CheckBox>().Where(x => x.IsChecked == true);
                var otherFormats = OtherFormatsContainer.Children.OfType<CheckBox>().Where(x => x.IsChecked == true);

                var list = rasterFormats.Concat(photoshopFormats).Concat(vectorFormats).Concat(cameraFormats).Concat(otherFormats);
                var sb = new StringBuilder();

                for (int i = 0; i < list.Count(); i++)
                {
                    sb.Append(list.ElementAt(i).Content);

                    if (i != list.Count() - 1)
                    {
                        sb.Append(',');
                    }
                }
                ConfigureSettings.GeneralSettings.ElevateProcess(sb.ToString());
            };

            // ApplyButton
            ApplyButton.PreviewMouseLeftButtonDown += delegate { PreviewMouseButtonDownAnim(ApplyText); };
            ApplyButton.MouseEnter += delegate { ButtonMouseOverAnim(ApplyText); };
            ApplyButton.MouseEnter += delegate { AnimationHelper.MouseEnterBgTexColor(ApplyBrush); };
            ApplyButton.MouseLeave += delegate { ButtonMouseLeaveAnim(ApplyText); };
            ApplyButton.MouseLeave += delegate { AnimationHelper.MouseLeaveBgTexColor(ApplyBrush); };
        }

        internal void HideLogic()
        {
            var da = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(.3)
            };

            da.Completed += delegate
            {
                Hide();
            };

            ConfigureWindows.GetSettingsWindow.Effect = null;
            BeginAnimation(OpacityProperty, da);
            ConfigureWindows.GetSettingsWindow.Focus();
        }
    }
}
