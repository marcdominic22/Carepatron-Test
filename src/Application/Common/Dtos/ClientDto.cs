using Application.Common.Mappings;

using Domain;

namespace Application.Common.Dtos
{
    public class ClientDto : IMapFrom<Client>
    {
        /// <summary>
        /// Id of Client
        /// </summary>
        /// <example>1</example>
        public long Id { get; set; }

        /// <summary>
        /// First Name of Client
        /// </summary>
        /// <example>John</example>
        public string FirstName { get; set; }

        /// <summary>
        /// Last Name of Client
        /// </summary>
        /// <example>Doe</example>
        public string LastName { get; set; }

        /// <summary>
        /// Email of an Client
        /// </summary>
        /// <example>John.doe@example.com</example>
        public string Email { get; set; }

        /// <summary>
        /// Phone Number of Client
        /// </summary>
        /// <example>+63954781112</example>
        public string PhoneNumber { get; set; }
    }
}