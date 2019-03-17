using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Transport.Model;
using Transport.Repository;
using Transport.Service;

namespace Transport.Controller
{
    public partial class TransportWindow : Form, Util.IObserver<Ride>, Util.IObserver<Booking>
    {
        private readonly RideService _rideService;

        private readonly BookingService _bookingService;

        private List<Ride> _rides;

        public TransportWindow()
        {
            InitializeComponent();
            _rides = new List<Ride>();
            _rideService = new RideService(new RideDBRepository(DBUtils.GetProperties()));
            _bookingService = new BookingService(new BookingDBRepository(DBUtils.GetProperties()));
            _bookingService.AddObserver(this);
            _rideService.AddObserver(this);
            ridesView.ReadOnly = true;
            bookingsView.ReadOnly = true;
            bookingsView.Columns.Cast<DataGridViewColumn>().ToList().ForEach(f => f.SortMode = DataGridViewColumnSortMode.NotSortable);
            NotifyOnEvent();
        }

        public void NotifyOnEvent()
        {
            ridesView.Rows.Clear();
            bookingsView.Rows.Clear();
            foreach (Ride ride in _rideService.GetAll()) {
                ridesView.Rows.Add(ride.Destination, ride.Date.ToShortDateString(), ride.Hour.ToString(), 
                    _rideService.AvailableSeatsRide(ride));
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            if (searchByDestination.Text.Trim().Length == 0)
            {
                MessageBox.Show("Destination cannot be empty.");
                return;
            }

            if (!DateTime.TryParse(searchByDate.Text, out DateTime parsedDate))
            {
                MessageBox.Show("Not a valid date.");
                return;
            }

            if (!DateTime.TryParse(searchByHour.Text, out DateTime parsedHour))
            {
                MessageBox.Show("Not a valid hour.");
                return;
            }

            bookingsView.Rows.Clear();
            _rides = new List<Ride>();
            List<int> seats = new List<int>();
            _rideService.FilterDestinationDateHour(searchByDestination.Text.Trim(), searchByDate.Text.Trim(),
                searchByHour.Text.Trim()).ForEach(
                ride =>
                {
                    _rides.Add(ride);
                    var clientNameSeat = _bookingService.FilterByClient(ride.Id);
                
                    foreach (var tuple in clientNameSeat)
                    {
                        bookingsView.Rows.Add(tuple.Item1, tuple.Item2, "Already booked");
                        seats.Add(tuple.Item2);
                    }
                    for (int index = 1; index <= RideService.MaxAvailablePlaces; index++)
                    {
                        if (seats.IndexOf(index) == -1)
                        {
                            bookingsView.Rows.Add("-", index, "Book");
                        }
                    }
                }
                );
            bookingsView.Sort(bookingsView.Columns["SeatNo"], ListSortDirection.Ascending);

            if (!_rides.Any())
            {
                MessageBox.Show("No such ride found.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!_rides.Any())
            {
                MessageBox.Show("Please search for a ride.");
                return;
            }
            var rezervare = new Bookings(_rides[0]);
            Hide();
            rezervare.Show();
            rezervare.FormClosed += (a, b) =>
            {
                button1_Click(this, new EventArgs());
                Show();
            };
        }
    }
}
