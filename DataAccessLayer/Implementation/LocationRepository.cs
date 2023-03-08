﻿using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Common;

namespace DataAccessLayer.Implementation
{
    public class LocationRepository : ILocationRepository
    {
        private readonly InternshipContext _internshipContext;

        public LocationRepository(InternshipContext internshipContext)
        {
            _internshipContext = internshipContext;
        }
        public async Task AddLocationAsync(Location location)
        {
            if (location == null)
            {
                return;
            }
            await _internshipContext.Locations.AddAsync(location);
            await _internshipContext.SaveChangesAsync();
        }

        public bool IsLocationNameUnique(int id, string name)
        {
            return !_internshipContext.Locations.Any(x => x.LocationId != id && x.Name == name);
        }

        public async Task<Location> GetLocationByIdAsync(int locationId)
        {
            var location = await _internshipContext.Locations
                .Where(l => l.LocationId == locationId)
                .FirstOrDefaultAsync();

            return location;
        }

        public async Task UpdateLocationAsync(Location location)
        {
            if (location == null)
            {
                throw new ArgumentNullException(nameof(location));
            }
            _internshipContext.Locations.Update(location);
            await _internshipContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Location>> GetAllLocationsAsync()
        {
            var locations = await _internshipContext.Locations.ToListAsync();

            return locations;
        }

        public async Task DeleteLocationByIdAsync(int locationId)
        {
            var locationToDelete = await GetLocationByIdAsync(locationId);

            if (locationToDelete != null)
            {
                _internshipContext.Locations.Remove(locationToDelete);
                await _internshipContext.SaveChangesAsync();
            }
        }

        public async Task<PagedResponse<Location>> GetLocationsFilteredPagedAsync(PagingFilteringParameters pagingFilteringParameters, Location location)
        {
            var filteredLocations = _internshipContext.Locations
              .Where(l => location.Address == null || l.Address.Contains(location.Address))
              .Where(l => location.Name == null || l.Name.Contains(location.Name))
              .Where(l => location.ContactEmail == null || l.ContactEmail.Contains(location.ContactEmail))
              .Where(l => location.City == null || l.City.Contains(location.City))
              .Where(l => location.Country == null || l.Country.Contains(location.Country));

            var count = filteredLocations.Count();

            IQueryable<Location> orderedLocations = null;

            switch (pagingFilteringParameters.OrderBy)
            {
                case "Name": 
                    orderedLocations = pagingFilteringParameters.OrderDescending 
                        ? filteredLocations.OrderByDescending(l => l.Name) 
                        : filteredLocations.OrderBy(l => l.Name);
                    break;
                case "Country":
                    orderedLocations = pagingFilteringParameters.OrderDescending 
                        ? filteredLocations.OrderByDescending(l => l.Country) 
                        : filteredLocations.OrderBy(l => l.Country);
                    break;
                case "City":
                    orderedLocations = pagingFilteringParameters.OrderDescending 
                        ? filteredLocations.OrderByDescending(l => l.City) 
                        : filteredLocations.OrderBy(l => l.City);
                    break;
                case "ContactEmail":
                    orderedLocations = pagingFilteringParameters.OrderDescending 
                        ? filteredLocations.OrderByDescending(l => l.ContactEmail) 
                        : filteredLocations.OrderBy(l => l.ContactEmail);
                    break;
                case "Address":
                    orderedLocations = pagingFilteringParameters.OrderDescending 
                        ? filteredLocations.OrderByDescending(l => l.Address) 
                        : filteredLocations.OrderBy(l => l.Address);
                    break;
            }

            IQueryable<Location> pagedLocations = orderedLocations.Skip((pagingFilteringParameters.PageNumber) * pagingFilteringParameters.PageSize)
              .Take(pagingFilteringParameters.PageSize);

            List<Location> result = await pagedLocations.ToListAsync();

            return new PagedResponse<Location>(result, pagingFilteringParameters.PageNumber,
                pagingFilteringParameters.PageSize, count);
        }
    }
}
