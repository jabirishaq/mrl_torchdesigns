using Nop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.AuthorizeNet.Domain
{
    public class Td_AuthorizeNet_DeclinedCard_Log : BaseEntity
    {
        /// <summary>
        /// Gets or sets customer identifier
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets info of unsuccessful payment using credit card
        /// </summary>
        public string CreditCardDeclinedDueTo { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of declined payment via credit card
        /// </summary>
        public DateTime CreatedOn { get; set; }
    }
}
