﻿namespace CodeFirst.Dtos
{
    public class AddressDto
    {
        public required string Street { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public required string PostalCode { get; set; }
    }
}
