﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Auradent.core;
using Auradent.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Auradent.Windows
{
    public partial class Search : Window
    {
        private readonly IdataHelper<Patient> _patientHelper;
        private ObservableCollection<Member> members;

        public Search()
        {
            InitializeComponent();
            var services = ((App)Application.Current).Services;
            _patientHelper = services.GetService<IdataHelper<Patient>>();

            LoadPatients();
        }

        private void LoadPatients()
        {
            var converter = new BrushConverter();
            members = new ObservableCollection<Member>();
            var patients = _patientHelper.GetAllData().OrderBy(p => p.PatientID);
            int number = 1;

            foreach (var patient in patients)
            {
                // Get first character of patient's name for the circle
                string character = !string.IsNullOrEmpty(patient.PatientName)
                    ? patient.PatientName[0].ToString().ToUpper()
                    : "?";

                // Rotate through colors for visual variety
                string bgColor = GetColorForIndex(number);

                members.Add(new Member
                {
                    Number = number.ToString(),
                    Character = character,
                    BgColor = (Brush)converter.ConvertFromString(bgColor),
                    Name = patient.PatientName,
                    Position = patient.PatientID.ToString(),
                    Phone = patient.PatientPhone ?? "N/A"
                });

                number++;
            }

            membersDataGrid.ItemsSource = members;
        }

        private string GetColorForIndex(int index)
        {
            // Rotate through these colors
            string[] colors = new[]
            {
                "#1098AD", // Blue
                "#1E88E5", // Light Blue
                "#FF8F00", // Orange
                "#FF5252", // Red
                "#0CA678", // Green
                "#6741D9", // Purple
                "#FF6D00"  // Dark Orange
            };

            return colors[(index - 1) % colors.Length];
        }

        private void PatientSearchComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string searchType = selectedItem.Content.ToString();
                // Update UI to reflect search type (ID or Name)
                txtSearch.Text = string.Empty;
                txtSearch.TextChanged += (s, args) =>
                {
                    FilterPatients(searchType, txtSearch.Text);
                };
            }
        }

        private void FilterPatients(string searchType, string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                membersDataGrid.ItemsSource = members;
                return;
            }

            var filteredMembers = members.Where(m =>
            {
                if (searchType == "ID")
                    return m.Position.Contains(searchText, StringComparison.OrdinalIgnoreCase);
                else // Name
                    return m.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase);
            });

            membersDataGrid.ItemsSource = filteredMembers;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private bool isMaximized = false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (isMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;
                    isMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                    isMaximized = true;
                }
            }
        }

        private void home_btn(object sender, RoutedEventArgs e)
        {
            newdoctordashboard doctorDashboard = new newdoctordashboard
            {
                WindowState = WindowState.Maximized,
                Title = "Doctor Dashboard"
            };
            doctorDashboard.Show();
            this.Close();
        }
    }

    public class Member
    {
        public string Number { get; set; }
        public string Character { get; set; }
        public Brush BgColor { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}