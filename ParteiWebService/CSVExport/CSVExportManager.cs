using ParteiWebService.ExportManagers;
using CsvHelper;
using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ParteiWebService.CSV_Export
{
    public class CSVExportManager
    {
        public static void CreateMemberCSV(CSVMemberModel model, string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (var writer = new StreamWriter(path))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(model.Users);
            }
        }

        public static CSVMemberModel ReadMemberCSV(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("CSV Datei existiert nicht.");
            }

            List<User> records = null;

            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                records = csv.GetRecords<User>().ToList();
            }
            if (records == null)
            {
                throw new Exception("CSV Datei kann nicht gelesen werden.");
            }

            CSVMemberModel mb = new CSVMemberModel
            {
                Users = records
            };

            return mb;
        }

        public static Member MapModelMemberToMember(User modelMember,string id, int organisationId)
        {
            Member member = new Member();
            member.ID = id;
            member.PreName = modelMember.PreName;
            member.LastName = modelMember.LastName;
            member.Home = modelMember.City;
            member.DateOfBirth = modelMember.DateOfBirth;
            member.Contribution = modelMember.Contribution;
            member.Adress = modelMember.Address;
            member.PostCode = modelMember.Postal;
            member.OrganizationId = organisationId;
            return member;
        }

        public static CSVMemberModel MapMemberToModelMember(List<Member> memberList)
        {
            CSVMemberModel modelMember = new CSVMemberModel();
            List<User> list = new List<User>();

            foreach (var member in memberList ){

                list.Add(new User
                {
                    PreName = member.PreName,
                    LastName = member.LastName,
                    Address = member.Adress,
                    DateOfBirth = member.DateOfBirth,
                    City = member.Home,
                    Contribution = member.Contribution,
                    Postal = member.PostCode
                });
            }
            modelMember.Users = list;
            return modelMember;
        }
    }
}
