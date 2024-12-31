using Auradent.core;
using Auradent.Data;
using Microsoft.Extensions.DependencyInjection;
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
using System.Windows.Shapes;

namespace Auradent.Windows
{
    /// <summary>
    /// Interaction logic for newdoctordashboard.xaml
    /// </summary>
    public partial class newdoctordashboard : Window
    {
        private readonly IdataHelper<Patient> dataHelperPatient;
        private readonly IdataHelper<Appointment> dataHelperAppointment;
        private List<OntologyArticle> allOntologyArticles;

        public newdoctordashboard()
        {
            InitializeComponent();
            var services = ((App)Application.Current).Services;
            if (services == null)
            {
                throw new InvalidOperationException("Service provider is not initialized.");
            }
            dataHelperPatient = services.GetService<IdataHelper<Patient>>() ?? throw new InvalidOperationException("Patient data helper service is not available.");
            dataHelperAppointment = services.GetService<IdataHelper<Appointment>>() ?? throw new InvalidOperationException("Appointment data helper service is not available.");
            LoadTodaysAppointments();
            LoadOntologyData();
        }

        private void LoadTodaysAppointments()
        {
            var today = DateTime.Today;
            var todaysAppointments = dataHelperAppointment.GetAllData()
                .Where(a => a.AppointmentDate.Date == today && a.AppointmentStatus != "Completed")
                .OrderBy(a => a.AppointmentTime)
                .Take(3)
                .ToList();

            // Clear existing text
            First_patient.Text = string.Empty;
            time_patient_1.Text = string.Empty;

            // Display appointments in cards
            if (todaysAppointments.Count > 0)
            {
                var firstPatient = dataHelperPatient.GetAllData().FirstOrDefault(p => p.PatientID == todaysAppointments[0].PatientID_FK);
                if (firstPatient != null)
                {
                    First_patient.Text = firstPatient.PatientName;
                    time_patient_1.Text = todaysAppointments[0].AppointmentTime;
                }
            }

            if (todaysAppointments.Count > 1)
            {
                var secondPatient = dataHelperPatient.GetAllData().FirstOrDefault(p => p.PatientID == todaysAppointments[1].PatientID_FK);
                if (secondPatient != null)
                {
                    Second_patient.Text = secondPatient.PatientName;
                    time_patient_2.Text = todaysAppointments[1].AppointmentTime;
                }
            }

            if (todaysAppointments.Count > 2)
            {
                var thirdPatient = dataHelperPatient.GetAllData().FirstOrDefault(p => p.PatientID == todaysAppointments[2].PatientID_FK);
                if (thirdPatient != null)
                {
                    Third_patient.Text = thirdPatient.PatientName;
                    time_patient_3.Text = todaysAppointments[2].AppointmentTime;
                }
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void log_out(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Upcoming_Patient_1(object sender, RoutedEventArgs e)
        {
            var today = DateTime.Today;
            var todaysAppointments = dataHelperAppointment.GetAllData()
                .Where(a => a.AppointmentDate.Date == today)
                .OrderBy(a => a.AppointmentTime)
                .FirstOrDefault();

            if (todaysAppointments != null)
            {
                var patient = dataHelperPatient.GetAllData().FirstOrDefault(p => p.PatientID == todaysAppointments.PatientID_FK);
                if (patient != null)
                {
                    IntegRatedPatient integRatedPatient = new IntegRatedPatient(patient)
                    {
                        Title = "Patient Details - " + patient.PatientName,
                        WindowState = WindowState.Maximized
                    };
                    integRatedPatient.Show();
                    this.Close();
                }
            }
        }

        private void Calender_btn(object sender, RoutedEventArgs e)
        {
            Nurse_s_dashboard nurse_S_Dashboard = new Nurse_s_dashboard
            {
                Title = "Calender",
                WindowState = WindowState.Maximized
            };
            nurse_S_Dashboard.Show();
            this.Close();
        }

        private void Upcoming_Patient_2(object sender, RoutedEventArgs e)
        {
            var today = DateTime.Today;
            var todaysAppointments = dataHelperAppointment.GetAllData()
                .Where(a => a.AppointmentDate.Date == today)
                .OrderBy(a => a.AppointmentTime)
                .Skip(1)
                .FirstOrDefault();

            if (todaysAppointments != null)
            {
                var patient = dataHelperPatient.GetAllData().FirstOrDefault(p => p.PatientID == todaysAppointments.PatientID_FK);
                if (patient != null)
                {
                    IntegRatedPatient integRatedPatient = new IntegRatedPatient(patient)
                    {
                        Title = "Patient Details - " + patient.PatientName,
                        WindowState = WindowState.Maximized
                    };
                    integRatedPatient.Show();
                    this.Close();
                }
            }
        }

        private void Upcoming_Patient_3(object sender, RoutedEventArgs e)
        {
            var today = DateTime.Today;
            var todaysAppointments = dataHelperAppointment.GetAllData()
                .Where(a => a.AppointmentDate.Date == today)
                .OrderBy(a => a.AppointmentTime)
                .Skip(2)
                .FirstOrDefault();

            if (todaysAppointments != null)
            {
                var patient = dataHelperPatient.GetAllData().FirstOrDefault(p => p.PatientID == todaysAppointments.PatientID_FK);
                if (patient != null)
                {
                    IntegRatedPatient integRatedPatient = new IntegRatedPatient(patient)
                    {
                        Title = "Patient Details - " + patient.PatientName,
                        WindowState = WindowState.Maximized
                    };
                    integRatedPatient.Show();
                    this.Close();
                }
            }
        }

        private void SearchBtn(object sender, RoutedEventArgs e)
        {
            Search search = new Search
            {
                Title = "Search",
                WindowState = WindowState.Maximized
            };
            search.Show();
            this.Close();

        }

        private void ReportBtn(object sender, RoutedEventArgs e)
        {
            Report report = new Report
            {
                Title = "Report",
                WindowState = WindowState.Normal
            };
            report.Show();
        }

        private async void LoadOntologyData()
        {
            try
            {
                using (var client = new System.Net.Http.HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    // Get initial set of common diseases to show by default
                    var commonDiseases = new[]
                    {
                        "DOID:10534", // Gastric ulcer
                        "DOID:1498",  // Diabetes mellitus
                        "DOID:2841",  // Asthma
                        "DOID:684",   // Hepatitis
                        "DOID:1287"   // Cardiovascular disease
                    };

                    var diseases = new List<OntologyArticle>();
                    foreach (var id in commonDiseases)
                    {
                        var response = await client.GetAsync($"https://www.ebi.ac.uk/ols/api/ontologies/doid/terms/http%253A%252F%252Fpurl.obolibrary.org%252Fobo%252F{id.Replace(":", "_")}");
                        if (response.IsSuccessStatusCode)
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            var options = new System.Text.Json.JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            };

                            var ontologyArticle = System.Text.Json.JsonSerializer.Deserialize<OntologyArticle>(json, options);
                            if (ontologyArticle != null)
                            {
                                diseases.Add(ontologyArticle);
                            }
                        }
                    }

                    allOntologyArticles = diseases;
                    UpdateOntologyList(diseases);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading ontology data: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateOntologyList(IEnumerable<OntologyArticle> articles)
        {
            if (articles == null) return;

            var displayArticles = articles.Select(a => new
            {
                Name = a?.Label ?? "Unknown Disease",
                Id = a?.Id?.Replace("DOID:", "") ?? "No ID",
                Definition = a?.Meta != null && a.Meta.ContainsKey("definition") && a.Meta["definition"]?.Any() == true
                    ? a.Meta["definition"][0].Value
                    : "No definition available",
                Synonyms = a?.Meta != null && a.Meta.ContainsKey("exact_synonym") && a.Meta["exact_synonym"] != null
                    ? a.Meta["exact_synonym"].Select(s => s.Value).Where(v => v != null).ToList()
                    : new List<string>(),
                DetailsUrl = $"https://disease-ontology.org/?id=DOID:{a?.Id?.Replace("DOID:", "")}"
            })
            .OrderBy(a => a.Name)
            .ToList();

            OntologyList.ItemsSource = displayArticles;
        }

        private async void OntologySearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = OntologySearchBox.Text?.Trim();
            if (string.IsNullOrWhiteSpace(searchText))
            {
                if (allOntologyArticles != null)
                {
                    UpdateOntologyList(allOntologyArticles);
                }
                return;
            }

            try
            {
                using (var client = new System.Net.Http.HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    // Search the Disease Ontology through EBI's OLS API
                    var response = await client.GetAsync($"https://www.ebi.ac.uk/ols/api/search?q={Uri.EscapeDataString(searchText)}&ontology=doid&rows=10");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var searchResult = System.Text.Json.JsonDocument.Parse(json);
                        var responses = searchResult.RootElement.GetProperty("response");
                        var docs = responses.GetProperty("docs");

                        var searchResults = new List<OntologyArticle>();
                        foreach (var doc in docs.EnumerateArray())
                        {
                            var id = doc.GetProperty("obo_id").GetString();
                            var label = doc.GetProperty("label").GetString();
                            var description = doc.TryGetProperty("description", out var desc) ? desc[0].GetString() : null;

                            searchResults.Add(new OntologyArticle
                            {
                                Id = id,
                                Label = label,
                                Meta = new Dictionary<string, List<MetaValue>>
                                {
                                    {
                                        "definition",
                                        new List<MetaValue>
                                        {
                                            new MetaValue { Value = description ?? "No definition available" }
                                        }
                                    }
                                }
                            });
                        }

                        UpdateOntologyList(searchResults);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Search error: {ex.Message}");
                // If search fails, fall back to local filtering
                if (allOntologyArticles != null)
                {
                    var filteredArticles = allOntologyArticles.Where(a =>
                        (a?.Label?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true) ||
                        (a?.Meta != null &&
                         a.Meta.ContainsKey("definition") &&
                         a.Meta["definition"]?.Any() == true &&
                         a.Meta["definition"][0].Value?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true)
                    ).ToList();

                    UpdateOntologyList(filteredArticles);
                }
            }
        }

        private void ViewOntologyDetails_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string url)
            {
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening URL: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}