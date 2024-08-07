using Dal.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Interface; 

public interface IOpeningHoursDao
{
    Task<int> InsertOpeningHoursAsync(OpeningHours newOpeningHours);
    Task<IEnumerable<OpeningHours>> GetOpeningHoursForRestaurantAsync(int restaurantId);
    Task<bool> DeleteOpeningHoursAsync(int openingHoursId);
    Task<bool> UpdateOpeningHoursAsync(OpeningHours openingHours);
}
