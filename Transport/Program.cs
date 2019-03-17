using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Transport.Controller;
using Transport.Model;
using Transport.Repository;
using Transport.Service;

namespace Transport
{
    static class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
            EmployeeDBRepository db = new EmployeeDBRepository(DBUtils.GetProperties());
            Console.WriteLine(db.Size());

            foreach (Employee employee in db.FindAll())
            {
                Console.WriteLine(employee.LastName);
            }
            var empl = new Employee
            {
                Id = 5,
                FirstName = "Daniel",
                LastName = "Toader",
                UserName = "tdn_dani",
                Password = "cheie",
                Office = "Sighisoara"
            };
            db.Update(5, empl);
            RideDBRepository rideDB = new RideDBRepository(DBUtils.GetProperties());
            Console.WriteLine(rideDB.FindRide("Cancun", "2019-10-27", "08:45").First().Value);
            RideService rideservice = new RideService(new RideDBRepository(DBUtils.GetProperties()));
            BookingDBRepository bookingDB = new BookingDBRepository(DBUtils.GetProperties());
            List<Ride> _rides = new List<Ride>();
            _rides = rideservice.FilterAndSorter(rideDB.FindAll().ToList(), ride => ride.Destination.ToLower().Contains("cancun") &&
                                                     ride.Date.ToString("yyyy-MM-dd").Contains("2019-10-27") &&
                                                     ride.Hour.ToString(@"HH\:mm").Contains("08:45"),
                ride => ride.Id);
            foreach (Ride ride in _rides)
            {
                Console.WriteLine(ride.Date);
            }
        }
    }
}
