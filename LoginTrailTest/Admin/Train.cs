using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginTrailTest.Admin
{
    public class Train
    {
        public int Train_ID { get; set; }
        public string Train_Number { get; set; }
        public string Departure_Station { get; set; }
        public string Destination_Station { get; set; }
        public int Number_of_Cars { get; set; }
        public string Car_Types { get; set; }
        public int Number_of_Coupe { get; set; }
        public int Number_of_Plac { get; set; }
    }
    public class Route
    {
        public int Route_ID { get; set; }
        public int Train_ID { get; set; }
        public string Train_Number { get; set; } 
        public string Station_Name { get; set; }
        public decimal Route_Length { get; set; }
        public decimal Price_coupe { get; set; }
        public decimal Price_plac { get; set; }
    }

    public class Schedule
    {
        public int Schedule_ID { get; set; }
        public string Train_Number { get; set; }
        public string Platform { get; set; }
        public DateTime Departure_Date { get; set; }
        public TimeSpan Departure_Time { get; set; }
        public int Busy_Coupe { get; set; }
        public int Busy_Plac { get; set; }
    }

    public class Station
    {
        public int Station_ID { get; set; }
        public string Station_Name { get; set; }
        public string City { get; set; }
    }
    public class TicketSale
    {
        public int Sale_ID { get; set; }
        public string Ticket_Code { get; set; }
        public int Route_ID { get; set; }
        public string Station_Name { get; set; }
        public string Platform { get; set; }
        public string Customer_Name { get; set; }
        public string Train_Number { get; set; }
        public int Car_Number { get; set; }
        public string Seat_Type { get; set; }
        public int Seat_Number { get; set; }
        public DateTime Departure_Date { get; set; }
        public TimeSpan Departure_Time { get; set; }
        public decimal Price { get; set; }

    }
    public class Seats
    {
        public int Seats_ID { get; set; }
        public string Train_Number { get; set; }
        public int Car_Number { get; set; }
        public string Car_Type { get; set; }
        public string Seat_Code { get; set; }
    }
}
