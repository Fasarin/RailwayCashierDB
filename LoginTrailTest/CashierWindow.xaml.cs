using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using ConnectionToSQL.Helper;
using iTextSharp.text.pdf;
using LoginTrailTest.Admin;
using LoginTrailTest.CashierPanel;
using iTextSharp.text;
using Microsoft.Win32;


namespace LoginTrailTest
{
    public partial class CashierWindow : Window
    {
        private List<Seats> Seats;
        private List<TicketSale> TicketSales;
        private CreateTicketWindow createTicketWindow;
        public CashierWindow()
        {
            InitializeComponent();
            TicketSales = DBHelper.GetTicketSalesFromDatabase();
            Seats = DBHelper.GetSeatsFromDatabase();
            YourData.ItemsSource = TicketSales;
        }
        private void SearchTicket(object sender, RoutedEventArgs e)
        {
            List<TicketSale> filteredTicketSales = TicketSales;

            if (DatePicker.SelectedDate.HasValue)
            {
                filteredTicketSales = filteredTicketSales.Where(ts => ts.Departure_Date.Date == DatePicker.SelectedDate.Value.Date).ToList();
            }

            string searchText = SearchTextBox.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(searchText))
            {
                filteredTicketSales = filteredTicketSales.Where(ts =>
                    ts.Ticket_Code.ToLower().Contains(searchText) ||
                    ts.Train_Number.ToLower().Contains(searchText) ||
                    ts.Seat_Type.ToLower().Contains(searchText)
                ).ToList();
            }

            if (filteredTicketSales.Any())
            {
                YourData.ItemsSource = filteredTicketSales;
            }
            else
            {
                MessageBox.Show("No results found", "Search results", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void TicketButton_Click(object sender, RoutedEventArgs e)
        {
            CreateTicketWindow createTicketWindow = new CreateTicketWindow();
            createTicketWindow.TicketCreated += CreateTicketWindow_TicketCreated;
            createTicketWindow.Show();
        }
        private void CreateTicketWindow_TicketCreated(object sender, EventArgs e)
        {
            TicketSales = DBHelper.GetTicketSalesFromDatabase();
            YourData.ItemsSource = TicketSales;
        }
        private void DeleteTicketButton_Click(object sender, RoutedEventArgs e)
        {
            if (YourData.SelectedItem is TicketSale selectedTicketSale)
            {
                if (MessageBox.Show("Are you sure you want to delete this ticket?", "Delete ticket", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    DBHelper.DeleteTicketSale(selectedTicketSale.Sale_ID);
                    TicketSales = DBHelper.GetTicketSalesFromDatabase();
                    YourData.ItemsSource = TicketSales;
                }
            }
            else
            {
                MessageBox.Show("Please select a ticket to delete.", "No ticket selected", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void SaveTicketAsPdfButton_Click(object sender, RoutedEventArgs e)
        {
            if (YourData.SelectedItem is TicketSale selectedTicketSale)
            {
                using (var doc = new Document())
                {
                    var saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        using (var writer = PdfWriter.GetInstance(doc, new FileStream(saveFileDialog.FileName, FileMode.Create)))
                        {
                            doc.Open();

                            var cb = writer.DirectContent;
                            cb.BeginText();
                            var baseFont = BaseFont.CreateFont("c:/windows/fonts/arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                            cb.SetFontAndSize(baseFont, 20);
                            cb.SetTextMatrix(50, 800);
                            cb.ShowText("Ticket information");
                            cb.EndText();

                            cb.BeginText();
                            cb.SetFontAndSize(baseFont, 18);
                            int yPosition = 780;

                            cb.SetTextMatrix(50, yPosition); yPosition -= 20;
                            cb.ShowText("Ticket code: " + selectedTicketSale.Ticket_Code);
                            cb.SetTextMatrix(50, yPosition); yPosition -= 20;
                            cb.ShowText("Train number: " + selectedTicketSale.Train_Number);
                            cb.SetTextMatrix(50, yPosition); yPosition -= 20;
                            cb.ShowText("Platform: " + selectedTicketSale.Platform);
                            cb.SetTextMatrix(50, yPosition); yPosition -= 20;
                            cb.ShowText("Car number: " + selectedTicketSale.Car_Number);
                            cb.SetTextMatrix(50, yPosition); yPosition -= 20;
                            cb.ShowText("Car type: " + selectedTicketSale.Seat_Type);
                            cb.SetTextMatrix(50, yPosition); yPosition -= 20;
                            cb.ShowText("Seat number: " + selectedTicketSale.Seat_Number);
                            cb.SetTextMatrix(50, yPosition); yPosition -= 20;
                            cb.ShowText("Profit Station: " + selectedTicketSale.Station_Name);
                            cb.SetTextMatrix(50, yPosition); yPosition -= 20;
                            cb.ShowText("Date of dispatch: " + selectedTicketSale.Departure_Date.ToString("dd/MM/yyyy"));
                            cb.SetTextMatrix(50, yPosition); yPosition -= 20;
                            cb.ShowText("Departure time: " + selectedTicketSale.Departure_Time);
                            cb.SetTextMatrix(50, yPosition); yPosition -= 20;
                            cb.ShowText("Price: " + selectedTicketSale.Price);
                            cb.SetTextMatrix(50, yPosition); yPosition -= 20;
                            cb.ShowText("Full name: " + selectedTicketSale.Customer_Name);
                            cb.EndText();

                            doc.Close();

                            MessageBox.Show("The PDF file was saved as " + saveFileDialog.FileName, "PDF saved", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a ticket to save in PDF format.", "No ticket selected", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void StatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            var trainSeats = Seats.GroupBy(s => new { s.Train_Number, s.Car_Number, s.Car_Type })
                                  .Select(g => new
                                  {
                                      TrainNumber = g.Key.Train_Number,
                                      CarNumber = g.Key.Car_Number,
                                      CarType = g.Key.Car_Type,
                                      TotalSeats = g.Count()
                                  }).ToList();

            var occupiedSeats = TicketSales.GroupBy(ts => new { ts.Train_Number, ts.Car_Number, ts.Seat_Type, ts.Departure_Date, ts.Departure_Time, ts.Station_Name })
                                           .Select(g => new
                                           {
                                               TrainNumber = g.Key.Train_Number,
                                               CarNumber = g.Key.Car_Number,
                                               CarType = g.Key.Seat_Type,
                                               DepartureDate = g.Key.Departure_Date,
                                               DepartureTime = g.Key.Departure_Time,
                                               DestinationStation = g.Key.Station_Name,
                                               OccupiedSeats = g.Count()
                                           }).ToList();

            var statistics = from seat in trainSeats
                             join occupied in occupiedSeats
                             on new { seat.TrainNumber, seat.CarNumber, seat.CarType } equals new { occupied.TrainNumber, occupied.CarNumber, CarType = occupied.CarType } into gj
                             from subOccupied in gj.DefaultIfEmpty()
                             select new
                             {
                                 seat.TrainNumber,
                                 seat.CarNumber,
                                 seat.CarType,
                                 seat.TotalSeats,
                                 DepartureDate = subOccupied?.DepartureDate ?? DateTime.MinValue,
                                 DepartureTime = subOccupied?.DepartureTime ?? TimeSpan.Zero,
                                 DestinationStation = subOccupied?.DestinationStation ?? "Unknown",
                                 OccupiedSeats = subOccupied?.OccupiedSeats ?? 0,
                                 FreeSeats = seat.TotalSeats - (subOccupied?.OccupiedSeats ?? 0)
                             };

            string statisticsReport = "Train statistics:\n\n";
            foreach (var stat in statistics)
            {
                statisticsReport += $"Train number: {stat.TrainNumber}\n";
                statisticsReport += $"Date of dispatch: {stat.DepartureDate:dd/MM/yyyy}\n";
                statisticsReport += $"Departure time: {stat.DepartureTime}\n";
                statisticsReport += $"Destination station: {stat.DestinationStation}\n";
                statisticsReport += $"Wagon {stat.CarNumber} ({stat.CarType}): seats occupied {stat.OccupiedSeats}, vacancies {stat.FreeSeats}\n\n";
            }

            MessageBox.Show(statisticsReport, "Train statistics", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
