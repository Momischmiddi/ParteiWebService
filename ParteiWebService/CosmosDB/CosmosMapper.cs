using DataAccessLibrary.DataAccess;
using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParteiWebService
{
    public class CosmosMapper
    {
        public static List<DataAccessLibrary.Models.Organization> MapOrganizations(List<CosmosDB.DBModels.Organization> organizations, ParteiDbContext parteiDbContext)
        {
            var result = new List<DataAccessLibrary.Models.Organization>();
            var admins = FindAdminsOfOrganization(organizations, parteiDbContext);

            foreach (CosmosDB.DBModels.Organization o in organizations) {
                var toAdd = new DataAccessLibrary.Models.Organization();

                toAdd.AdminId = o.AdminId;
                toAdd.Admin = admins.Where(a => a.Id == o.AdminId).First();
                toAdd.Name = o.Name;
                toAdd.OrganizationImage = o.OrganizationImageUrl;

                result.Add(toAdd);
            }

            return result;
        }

        private static List<ApplicationUser> FindAdminsOfOrganization(List<CosmosDB.DBModels.Organization> organizations, ParteiDbContext parteiDbContext)
        {
            var result = new List<ApplicationUser>();
            var users = parteiDbContext.ApplicationUsers.ToList();
            
            foreach (var org in organizations)
            {
                foreach (var user in users)
                {
                    if (user.Id == org.AdminId)
                    {
                        result.Add(user);
                        break;
                    }
                }
            }

            return result;
        }
    }
}
