using System;
using System.Collections.Generic;

namespace Aufgabe_2.ExportManagers
{
    public class CSVMemberModel
    {
        public CSVMemberModel()
        {
            Users = new List<User>();
        }

        public List<User> Users { get; set; }
    }

    public class User
    {
        public String PreName { get; set; }
        public String LastName { get; set; }
        public String Postal { get; set; }
        public String City { get; set; }
        public String Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public double Contribution { get; set; }
    }
}
