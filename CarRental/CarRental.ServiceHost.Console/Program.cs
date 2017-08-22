using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using CarRental.Business.Managers.Managers;
using System.Timers;
using CarRental.Business.Entities;
using System.Transactions;
using System.Security.Principal;
using System.Threading;
using Core.Common.Cores;
using CarRental.Business.Bootstrapper;

namespace CarRental.ServiceHosts
{
    class Program
    {
        static void Main(string[] args)
        {

            // Add security credentials 
            GenericPrincipal principal = new GenericPrincipal(
                new GenericIdentity("Khang"), new string[] { "CarRentalAdmin" });
            Thread.CurrentPrincipal = principal;


            // When RentalManager instanciate in OnTimerElapsed, because it derived from ManagerBase, in ManagerBase, there is a call to 
            // MEF to resolve dependencies so bootstrapper have to be initialized.
            ObjectBase.Container = MEFLoader.Init();

            Console.WriteLine("Starting service..");
            Console.WriteLine();
            ServiceHost hostInventory = new ServiceHost(typeof(InventoryManager));
            ServiceHost hostRental = new ServiceHost(typeof(RentalManager));
            ServiceHost hostAccount = new ServiceHost(typeof(AccountManager));
            StartService(hostInventory, "InventoryService");
            StartService(hostRental, "RentalService");
            StartService(hostAccount, "AccountService");

            System.Timers.Timer timer = new System.Timers.Timer(10000);
            timer.Elapsed += OnTimerElaped;
            timer.Start();
            Console.WriteLine("Reservation monitor started..");

            Console.WriteLine();
            Console.WriteLine("Press Enter to end.");
            Console.ReadLine();

            timer.Stop();
            Console.WriteLine("Reservation monitor stopped.");

            StopService(hostInventory, "InventoryService");
            StopService(hostRental, "RentalService");
            StopService(hostAccount, "AccountService");
        
        }

        // This method keep track of dead reservation and cancel them
        static void OnTimerElaped(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Looking for dead reservation at {0}.", DateTime.Now.ToString());
            RentalManager rentalManager = new RentalManager();

            Reservation[] reservations = rentalManager.GetDeadReservations();
            if (reservations != null)
            {
                foreach(Reservation reservation in reservations)
                {
                    // Transaction will roll back if error takes place
                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            rentalManager.CancelReservation(reservation.ReservationID);
                            Console.WriteLine("Cancel reservation with ID '{0}'", reservation.ReservationID);
                            scope.Complete();
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine("There was an exception when ettemping to cancel reservation {0}", reservation.ReservationID);
                        }
                    }
                }
            }

        }

        static void StartService(ServiceHost host, string ServiceDescription)
        {
            host.Open();
            Console.WriteLine("Service {0} started", ServiceDescription);
            foreach(var endpoint in host.Description.Endpoints)
            {
                Console.WriteLine("Listening in endpoint:");
                Console.WriteLine("Address is {0}", endpoint.Address);
                Console.WriteLine("Binding is {0}", endpoint.Binding);
                Console.WriteLine("Contract is {0}", endpoint.Contract);
            }
            Console.WriteLine();
        }

        static void StopService(ServiceHost host, string ServiceDescription)
        {
            // Close will wait for service to finish up, abort will force service to stop
            host.Close();
            Console.WriteLine("Service {0} is stopped", ServiceDescription);
        }
    }
}
