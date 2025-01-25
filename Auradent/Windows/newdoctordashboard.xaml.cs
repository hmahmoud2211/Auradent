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
using System.Xml;
using System.Net.Http;
using System.Timers;
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
        private List<ResearchPaper> allResearchPapers;
        private System.Timers.Timer searchDebounceTimer;

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
            
            // Initialize research papers asynchronously
            _ = InitializeResearchPapers();

            // Initialize the debounce timer
            searchDebounceTimer = new System.Timers.Timer(1000); // 1 second delay
            searchDebounceTimer.AutoReset = false;
            searchDebounceTimer.Elapsed += async (s, e) =>
            {
                await Application.Current.Dispatcher.InvokeAsync(async () =>
                {
                    string searchText = ResearchSearchBox.Text?.Trim();
                    if (!string.IsNullOrWhiteSpace(searchText))
                    {
                        await LoadResearchPapers($"dental AND {searchText}");
                    }
                });
            };
        }

        private async Task InitializeResearchPapers()
        {
            await LoadResearchPapers("dental research latest");
        }

        private void LoadTodaysAppointments()
        {
            var today = DateTime.Today;
            var todaysAppointments = dataHelperAppointment.GetAllData()
                .Where(a => a.AppointmentDate.Date == today && a.AppointmentStatus != "Completed")
                .OrderBy(a => a.AppointmentTime)
                .ToList();

            // Update total count
            TotalPatientsCount.Text = todaysAppointments.Count.ToString();

            // Display first 3 appointments in cards
            var firstThreeAppointments = todaysAppointments.Take(3).ToList();

            // Clear existing text
            First_patient.Text = string.Empty;
            time_patient_1.Text = string.Empty;

            // Display appointments in cards
            if (firstThreeAppointments.Count > 0)
            {
                var firstPatient = dataHelperPatient.GetAllData().FirstOrDefault(p => p.PatientID == firstThreeAppointments[0].PatientID_FK);
                if (firstPatient != null)
                {
                    First_patient.Text = firstPatient.PatientName;
                    time_patient_1.Text = firstThreeAppointments[0].AppointmentTime;
                }
            }

            if (firstThreeAppointments.Count > 1)
            {
                var secondPatient = dataHelperPatient.GetAllData().FirstOrDefault(p => p.PatientID == firstThreeAppointments[1].PatientID_FK);
                if (secondPatient != null)
                {
                    Second_patient.Text = secondPatient.PatientName;
                    time_patient_2.Text = firstThreeAppointments[1].AppointmentTime;
                }
            }

            if (firstThreeAppointments.Count > 2)
            {
                var thirdPatient = dataHelperPatient.GetAllData().FirstOrDefault(p => p.PatientID == firstThreeAppointments[2].PatientID_FK);
                if (thirdPatient != null)
                {
                    Third_patient.Text = thirdPatient.PatientName;
                    time_patient_3.Text = firstThreeAppointments[2].AppointmentTime;
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
                        Title = $"Patient Details - {patient.PatientName}",
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
                        Title = $"Patient Details - {patient.PatientName}",
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
                        Title = $"Patient Details - {patient.PatientName}",
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
                        var response = await client.GetAsync($"https://www.ebi.ac.uk/ols/api/search?q={id}&exact=true&queryFields=obo_id");
                        if (response.IsSuccessStatusCode)
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            var searchResult = System.Text.Json.JsonDocument.Parse(json);
                            var docs = searchResult.RootElement.GetProperty("response").GetProperty("docs");

                            if (docs.EnumerateArray().Any())
                            {
                                var doc = docs.EnumerateArray().First();
                                var label = doc.GetProperty("label").GetString();
                                var description = doc.TryGetProperty("description", out var desc) && desc.GetArrayLength() > 0
                                    ? desc[0].GetString()
                                    : "No definition available";

                                var ontologyArticle = new OntologyArticle
                                {
                                    Id = id,
                                    Label = label,
                                    Meta = new Dictionary<string, List<MetaValue>>
                                    {
                                        {
                                            "definition",
                                            new List<MetaValue>
                                            {
                                                new MetaValue { Value = description }
                                            }
                                        }
                                    }
                                };

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

        private async void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearch.Text?.Trim();
            
            if (string.IsNullOrEmpty(searchText))
            {
                lstSearchResults.ItemsSource = null;
                return;
            }

            try
            {
                var patients = dataHelperPatient.GetAllData()
                    .Where(p => 
                        (p.PatientName != null && p.PatientName.Contains(searchText, StringComparison.OrdinalIgnoreCase)) || 
                        p.PatientID.ToString().Contains(searchText) || 
                        (p.PatientPhone != null && p.PatientPhone.Contains(searchText)))
                    .Take(10)
                    .ToList();

                // For debugging
                Console.WriteLine($"Found {patients.Count} matching patients");
                foreach (var patient in patients)
                {
                    Console.WriteLine($"Patient: {patient.PatientID} - {patient.PatientName} - {patient.PatientPhone}");
                }

                lstSearchResults.ItemsSource = patients;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching patients: {ex.Message}");
                Console.WriteLine($"Search error: {ex}");
            }
        }

        private void lstSearchResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstSearchResults.SelectedItem is Patient selectedPatient)
            {
                try
                {
                    // Debug logging
                    Console.WriteLine($"Selected patient: ID={selectedPatient.PatientID}, Name={selectedPatient.PatientName}");
                    
                    // Clear the selection and search
                    lstSearchResults.SelectedItem = null;
                    txtSearch.Text = string.Empty;
                    
                    // Create and show the patient details window with the selected patient
                    IntegRatedPatient integRatedPatient = new IntegRatedPatient(selectedPatient)
                    {
                        Title = $"Patient Details - {selectedPatient.PatientName}",
                        WindowState = WindowState.Maximized
                    };
                    integRatedPatient.Show();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening patient details: {ex.Message}");
                    Console.WriteLine($"Error in selection changed: {ex.StackTrace}");
                }
            }
            else
            {
                Console.WriteLine("No patient selected or invalid selection type");
            }
        }

        private void chatbot_btn(object sender, RoutedEventArgs e)
        {
            chatbot chatbot = new chatbot
            {
                Title = "Chatbot",
                WindowState = WindowState.Maximized
            };
            chatbot.Show();
            this.Close();
        }

        public class ResearchPaper
        {
            public string Title { get; set; }
            public string Authors { get; set; }
            public string Journal { get; set; }
            public string PublishDate { get; set; }
            public string Url { get; set; }
        }

        private async Task LoadResearchPapers(string searchQuery = "dental")
        {
            try
            {
                using (var client = new System.Net.Http.HttpClient())
                {
                    string baseUrl = "https://eutils.ncbi.nlm.nih.gov/entrez/eutils/";
                    
                    // Increase delay to 1 second
                    await Task.Delay(1000);

                    string searchUrl = $"{baseUrl}esearch.fcgi?" +
                        $"db=pubmed&" +
                        $"term={Uri.EscapeDataString(searchQuery)}&" +
                        "retmax=5&" +
                        "sort=date";

                    var searchResponse = await client.GetStringAsync(searchUrl);
                    var xmlDoc = new System.Xml.XmlDocument();
                    xmlDoc.LoadXml(searchResponse);
                    var idNodes = xmlDoc.SelectNodes("//Id");

                    var papers = new List<ResearchPaper>();
                    
                    if (idNodes != null)
                    {
                        foreach (System.Xml.XmlNode idNode in idNodes)
                        {
                            if (idNode?.InnerText == null) continue;

                            // Increase delay between article requests to 1 second
                            await Task.Delay(1000);

                            string detailUrl = $"{baseUrl}esummary.fcgi?" +
                                $"db=pubmed&" +
                                $"id={idNode.InnerText}";

                            try
                            {
                                var detailResponse = await client.GetStringAsync(detailUrl);
                                var detailXml = new System.Xml.XmlDocument();
                                detailXml.LoadXml(detailResponse);

                                var docSum = detailXml.SelectSingleNode("//DocSum");
                                if (docSum != null)
                                {
                                    var paper = new ResearchPaper
                                    {
                                        Title = GetXmlItemContent(docSum, "Title") ?? "No Title",
                                        Authors = GetXmlItemContent(docSum, "Author") ?? "No Authors",
                                        Journal = GetXmlItemContent(docSum, "Source") ?? "No Journal",
                                        PublishDate = GetXmlItemContent(docSum, "PubDate") ?? "No Date",
                                        Url = $"https://pubmed.ncbi.nlm.nih.gov/{idNode.InnerText}/"
                                    };
                                    
                                    papers.Add(paper);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error processing article {idNode.InnerText}: {ex.Message}");
                                continue;
                            }
                        }
                    }

                    allResearchPapers = papers;
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        UpdateResearchList(papers);
                    });
                }
            }
            catch (Exception ex)
            {
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    MessageBox.Show($"Error loading research papers: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                });
            }
        }

        // Helper method to get content from XML
        private string GetXmlItemContent(System.Xml.XmlNode docSum, string itemName)
        {
            var node = docSum.SelectSingleNode($".//Item[@Name='{itemName}']");
            return node?.InnerText;
        }

        private void UpdateResearchList(IEnumerable<ResearchPaper> papers)
        {
            ResearchList.ItemsSource = papers;
        }

        private void ResearchSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchDebounceTimer.Stop(); // Reset the timer
            
            string searchText = ResearchSearchBox.Text?.Trim();
            if (string.IsNullOrWhiteSpace(searchText))
            {
                if (allResearchPapers != null)
                {
                    UpdateResearchList(allResearchPapers);
                }
                return;
            }

            searchDebounceTimer.Start(); // Start the timer
        }

        private void ViewResearchPaper_Click(object sender, RoutedEventArgs e)
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

        private void ViewPatient_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is Patient patient)
            {
                try
                {
                    // Keep the current window open
                    IntegRatedPatient integRatedPatient = new IntegRatedPatient(patient)
                    {
                        Title = $"Patient Details - {patient.PatientName}",
                        WindowState = WindowState.Maximized
                    };
                    integRatedPatient.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening patient details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}