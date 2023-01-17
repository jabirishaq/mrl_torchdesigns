using System;
using System.IO;

namespace Nop.Core.Domain.SystemDesignHelp
{
    /// <summary>
    /// Represents a best sellers report line
    /// </summary>
    [Serializable]
    public partial class SystemDesignHelp
    {

        public string PictureUrl { get; set; }

        public Stream filestream { get; set; }

        public string FirstName { get; set; }
              
        public string LastName { get; set; }

        public string Email { get; set; }

        public string PSI { get; set; }

        public string Second { get; set; }

        public string CountryId { get; set; }
     
        public string SoilTypeId { get; set; }
    
        public string CityWellId { get; set; }
     
        //public string WaterPressureId { get; set; }
        //public IList<SelectListItem> AvailableWaterPressure { get; set; }

        public string HaveFaucetsId { get; set; } 
      
        public string UseFaucetsId { get; set; }
       
        public string DrippersId { get; set; }
       

    }
}
